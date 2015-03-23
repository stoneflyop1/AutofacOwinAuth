# 在Asp.Net Web API中使用Autofac注入Owin的认证
## 数据库，使用EntityFramework自定义DbContext
这里，我们没有使用Asp.Net自带的IdentityContext，而是使用了领域模型中的`EntityFramework`生成的数据库；为了方便，此处领域模型，我们仅给了一张用户表。所有的代码独立为一个类库(`AutofacOwinAuth.Core`)。对于领域模型的操作，我们采用了比较通用的`数据`加`服务`的方式。主要的类包括：

* **User**           —— 用户模型，对应到用户表
* **UserMap**        —— 使用`FluentAPI`进行表结构的映射
* **EntityContext**  —— 派生自`DbContext`，而不是`IdentityContext`
* **IReposotory&lt;T&gt;** —— 模板化的数据仓库，内部操作DbContext
* **IUserService**   —— 用户操作的服务接口，提供给WebAPI层调用（即：WebAPI通过服务间接的使用数据库）
## Asp.Net Identity的使用
这部分位于Web API的项目中，根据VS的模板生成的代码修改而来。

* 我们去掉了`ApplicationUser`和`ApplicationDbContext`类，在Core类库中以**User**和**EntityContext**类代替。
* 为了使用Autofac的依赖注入，我们对ApplicationUserManager进行了一些调整，去掉了静态的Create方法，其中的属性设置直接写入了构造函数中。（DataProtectionProvider在Startup.ConfigureAuth中通过**IAppBuilder**实例化）
* 因为我们的用户表不是在IdentityContext中，所以我们重新实现了`IUserStor`e接口；根据实际的应用场景，可能还需要实现`IUserPasswordStore`(带密码的用户必须)、`IUserEmailStore`(带Email的用户必须)、`IUserLockoutStore`(用户多次尝试需要锁定账户必须)、`IUserTwoFactorStore`(多次验证必须)。
* 我们重新实现的IUserStore为**ApplicationUserStore**，以**IUserService**作为构造函数操作使用Autofac进行注入。
* 一开始我们尝试使用用户名和密码Login的方式进行登录认证，所以还模仿Asp.Net MVC派生了一个`ApplicationSignInManager`类进行有密码的账户登录的尝试，结果发现无法保持登录的认证信息到HttpContext的User对象上。这是一次失败的尝试。
## Autofac注入到Owin
需要引入的包(package)很多，包括`Autofac`，`Autofac.Owin`, `Autofac.WebAPI2`, `Autofac.WebAPI2.Owin`。
关于如何使用Autofac进行依赖注入，此处就不多谈了。只谈一下要跟注入Owin需要注意的地方，即：根据我们的尝试，可以成功使用需要做的事情。代码集中在AutofacConfig和Startup类中。

* 以前我们使用Autofac注册类型时，倾向于使用`InstancePerRequest`的方式实例化注册的类型实例，但为了照顾Owin的上下文，此处我们需要使用**InstancePerLifetimeScope**的方式。具体代码见：**AutofacConfig.RegisterTypes**方法。
* 为了实例化ApplicationUserManager，在Startup类中添加一个类型为**IDataProtectionProvider**的静态域(Field)；并在`ConfigureAuth`方法中，调用IAppBuilder的**GetDataProtectionProvider**方法进行实例化。
* 在Startup.并在ConfigureAuth方法中，在全局的HttpConfiguration上初始化Autofac的配置，并在Owin上使用Autofac中间件，以及Autofac的WebAPI注入。
* 根据VS模板的代码配置OAuth认证，这里使用默认的bearer方式认证
* 最后，告诉Owin使用WebAPI。
## 测试
至此，WebAPI的WebApplication就可以使用了。如何使用在`AutofacOwinAuth.WebAPI.Tests`的测试项目中体现(使用NUnit+VS的适配器)。

* 第一步就是注册账户，路由为：`api/Account/Register`，提供邮箱和密码即可，见**TestRegister**测试方法。（此处，邮箱作为了账户的UserName）
* 第二步为认证。前文提到，我们模仿MVC项目添加的Login方法没有办法保存认证，是因为按照现在Owin的配置，只能通过`BearerToken`和外部链接账户才能保存认证信息。具体使用方法见**TestToken**测试方法。
* 第三步测试认证后调用其他API。TestToken方法已给出了一个使用`TokenAuth`进行其他读请求(Get)的示例，**TestPasswords**方法给出了使用TokenAuth进行Post的示例。
* 注：使用提供邮箱和密码的方式注册的账户，无法使用`SetPassword`的Action重设密码，而只能通过`ChangePassword`设置密码。**SetPassword**是留给没有密码的账户设置密码的方法。

## 参考资料
* [http://leastprivilege.com/2013/11/25/dissecting-the-web-api-individual-accounts-templatepart-1-overview/](http://leastprivilege.com/2013/11/25/dissecting-the-web-api-individual-accounts-templatepart-1-overview/)
* [http://leastprivilege.com/2013/11/26/dissecting-the-web-api-individual-accounts-templatepart-2-local-accounts/](http://leastprivilege.com/2013/11/26/dissecting-the-web-api-individual-accounts-templatepart-2-local-accounts/)
* [http://leastprivilege.com/2013/11/26/dissecting-the-web-api-individual-accounts-templatepart-3-external-accounts/](http://leastprivilege.com/2013/11/26/dissecting-the-web-api-individual-accounts-templatepart-3-external-accounts/)
* [http://www.asp.net/web-api/overview/security/individual-accounts-in-web-api](http://www.asp.net/web-api/overview/security/individual-accounts-in-web-api)
* [http://tech.trailmax.info/2014/09/aspnet-identity-and-ioc-container-registration/](http://tech.trailmax.info/2014/09/aspnet-identity-and-ioc-container-registration/)
* [http://www.asp.net/aspnet/overview/owin-and-katana/owin-oauth-20-authorization-server](http://www.asp.net/aspnet/overview/owin-and-katana/owin-oauth-20-authorization-server)