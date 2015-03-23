using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Http.Hosting;
using System.Web.Http.Routing;
using AutofacOwinAuth.AuthorizationServer;
using Moq;

namespace AecCloud.WebAPI.Tests
{
    //http://stackoverflow.com/questions/11851558/testing-route-configuration-in-asp-net-webapi
    static class WebAPIRouteFinder
    {
        /// <summary>
        ///     Routes the request.
        /// </summary>
        /// <param name="config">The config.</param>
        /// <param name="request">The request.</param>
        /// <returns>Inbformation about the route.</returns>
        public static RouteInfo RouteRequest(HttpConfiguration config, HttpRequestMessage request)
        {
            // create context
            var controllerContext = new HttpControllerContext(config, new Mock<IHttpRouteData>().Object, request);

            // get route data
            var routeData = config.Routes.GetRouteData(request);
            RemoveOptionalRoutingParameters(routeData.Values);

            HttpActionDescriptor actionDescriptor = null;
            HttpControllerDescriptor controllerDescriptor = null;

            // Handle web api 2 attribute routes
            if (routeData.Values.ContainsKey("MS_SubRoutes"))
            {
                var subroutes = (IEnumerable<IHttpRouteData>)routeData.Values["MS_SubRoutes"];
                routeData = subroutes.First();
                actionDescriptor = ((HttpActionDescriptor[])routeData.Route.DataTokens.First(token => token.Key == "actions").Value).First();
                controllerDescriptor = actionDescriptor.ControllerDescriptor;
            }
            else
            {
                request.Properties[HttpPropertyKeys.HttpRouteDataKey] = routeData;
                controllerContext.RouteData = routeData;

                // get controller type
                controllerDescriptor = new DefaultHttpControllerSelector(config).SelectController(request);
                controllerContext.ControllerDescriptor = controllerDescriptor;

                // get action name
                actionDescriptor = new ApiControllerActionSelector().SelectAction(controllerContext);

            }

            return new RouteInfo
            {
                Controller = controllerDescriptor.ControllerType,
                Action = actionDescriptor.ActionName,
                RouteData = routeData
            };
        }


        #region | Extensions |

        public static bool ShouldMapTo<TController>(this string fullDummyUrl, string action, Dictionary<string, object> parameters = null)
        {
            return ShouldMapTo<TController>(fullDummyUrl, action, HttpMethod.Get, parameters);
        }

        public static bool ShouldMapTo<TController>(this string fullDummyUrl,
            string action, HttpMethod httpMethod, Dictionary<string, object> parameters = null)
        {
            var request = new HttpRequestMessage(httpMethod, fullDummyUrl);
            var config = new HttpConfiguration();
            WebApiConfig.Register(config);
            config.EnsureInitialized();

            var route = RouteRequest(config, request);

            var controllerName = typeof(TController).Name;
            if (route.Controller.Name != controllerName)
                throw new Exception(String.Format(
                    "The specified route '{0}' does not match the expected controller '{1}'", fullDummyUrl, controllerName));

            if (route.Action.ToLowerInvariant() != action.ToLowerInvariant())
                throw new Exception(String.Format(
                    "The specified route '{0}' does not match the expected action '{1}'", fullDummyUrl, action));

            if (parameters != null && parameters.Any())
            {
                foreach (var param in parameters)
                {
                    if (route.RouteData.Values.All(kvp => kvp.Key != param.Key))
                        throw new Exception(String.Format(
                            "The specified route '{0}' does not contain the expected parameter '{1}'", fullDummyUrl, param));
                    var paramValue = String.Empty;
                    if (param.Value != null)
                    {
                        paramValue = param.Value.ToString();
                    }
                    if (!route.RouteData.Values[param.Key].Equals(paramValue))
                        throw new Exception(String.Format(
                            "The specified route '{0}' with parameter '{1}' and value '{2}' does not equal does not match supplied value of '{3}'",
                            fullDummyUrl, param.Key, route.RouteData.Values[param.Key], param.Value));
                }
            }

            return true;
        }

        #endregion


        #region | Private Methods |

        /// <summary>
        ///     Removes the optional routing parameters.
        /// </summary>
        /// <param name="routeValues">The route values.</param>
        private static void RemoveOptionalRoutingParameters(IDictionary<string, object> routeValues)
        {
            var optionalParams = routeValues
                .Where(x => x.Value == RouteParameter.Optional)
                .Select(x => x.Key)
                .ToList();

            foreach (var key in optionalParams)
            {
                routeValues.Remove(key);
            }
        }

        #endregion
    }

    /// <summary>
    ///     Route information
    /// </summary>
    public class RouteInfo
    {
        public Type Controller { get; set; }
        public string Action { get; set; }
        public IHttpRouteData RouteData { get; set; }
    }
}
