using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using TTCore.StoreProvider.Middleware;

namespace TTCore.StoreProvider
{
    public class MvcAreaRouteBase : Attribute, IRouteTemplateProvider
    {
        public string Template => "[area]/[controller]";
        public int? Order => 2;
        //public string Name { get; set; }
        public string Name { get; set; } //= "[area]_[controller]_[action]";
    }

    public class MvcRouteBase : Attribute, IRouteTemplateProvider
    {
        public string Template => "[controller]";
        public int? Order => 2;
        //public string Name { get; set; }
        public string Name { get; set; } //= "[controller]_[action]";
    }

    public class ApiRouteBaseAttribute : Attribute, IRouteTemplateProvider
    {
        public string Template => "api/[controller]";
        public int? Order => 2;
        //public string Name { get; set; }
        public string Name { get; set; } //= "Api_[controller]_[action]";
    }

    public class MyCustomConstraint : IRouteConstraint
    {
        private Regex _regex;

        public MyCustomConstraint()
        {
            _regex = new Regex(@"^[1-9]*$",
                                RegexOptions.CultureInvariant | RegexOptions.IgnoreCase,
                                TimeSpan.FromMilliseconds(100));
        }
        public bool Match(HttpContext httpContext, IRouter route, string routeKey,
                          RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (values.TryGetValue(routeKey, out object value))
            {
                var parameterValueString = Convert.ToString(value, CultureInfo.InvariantCulture);
                if (parameterValueString == null)
                    return false;
                bool ok = _regex.IsMatch(parameterValueString);
                return ok;
            }
            return false;
        }
    }

    public class CustomLocationExpander : IViewLocationExpander
    {
        private const string _CustomViewPath = "CustomViewPath";
        private const string _CustomController = "CustomController";
        public void PopulateValues(ViewLocationExpanderContext context)
        {
            Regex DefaultComponentDetector = new Regex(@"^((?:[Cc]omponents))+\/+([\w\.]+)\/+(.*)");


            /*
             * If successful, 
             * Group 0 = FullMatch (ex "Components/MyComponent/Default")
             * Group 1 = Components (ex "Component")
             * Group 2 = Component Name (ex "MyComponent")
             * Group 3 = View Name (ex "Default")
             * */
            var DefaultComponentMatch = DefaultComponentDetector.Match(context.ViewName);

            if (DefaultComponentMatch.Success)
            {
                // Will render Components/ComponentName as the new view name
                context.Values.Add(_CustomViewPath, string.Format("{0}/{1}", DefaultComponentMatch.Groups[1].Value, DefaultComponentMatch.Groups[2].Value));
                context.Values.Add(_CustomController, context.ControllerName);
            }

        }

        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            /* Parameters:
             * {2} - Area Name
             * {1} - Controller Name
             * {0} - View Name
             */
            List<string> Paths = new List<string> { 
            // Default View Locations to support imported / legacy paths
            "/Views/{1}/{0}.cshtml",
            "/Views/Shared/{0}.cshtml",

            // Adds Feature Folder Rendering
            "/Features/{1}/{0}.cshtml",
            "/Features/Shared/{0}.cshtml",

            // Handles My Custom rendered views
            "/{0}.cshtml"
            };

            // Add "Hard Coded" custom view paths to checks, along with the normal default view paths for backward compatability
            if (context.Values.ContainsKey(_CustomViewPath))
            {
                // Generate full View Paths with my custom View Name and Controller Name

                var CombinedPaths = new List<string>(Paths.Select(x => string.Format(x, context.Values[_CustomViewPath], context.Values[_CustomController], "")));
                // Add in original paths for backward compatability
                CombinedPaths.AddRange(Paths);

                return CombinedPaths;
            }

            // Returns the normal view paths
            return Paths;
        }
    }

    public static class ApiRazorMvcRoutingMiddleware
    {
        public static ViewResult View2(this Controller controller)
        {
            string pathView = string.Empty;
            var context = controller.ControllerContext;
            string controllerName = context.ActionDescriptor.ControllerName;
            if (controllerName == "_Index")
                pathView = "/UI/Home/Index.cshtml";
            else
            {
                string area = context.ActionDescriptor.RouteValues["area"];
                string actionName = context.ActionDescriptor.ActionName;
                string template = context.ActionDescriptor.AttributeRouteInfo?.Template;
                string file = actionName + ".cshtml";

                int pos = template.IndexOf(controllerName + "/");
                if (pos > 0)
                {
                    pos = pos + controllerName.Length + 1;
                    file = template.Substring(pos, template.Length - pos) + ".cshtml";
                }

                if (string.IsNullOrEmpty(area)) area = "Shared";
                pathView = string.Format("/UI/{0}/{1}/{2}", area, controllerName, file);
            }

            return controller.View(pathView);
        }

        public static ViewResult View2<T>(this Controller controller, T model = default(T))
        {
            string pathView = string.Empty;

            var context = controller.ControllerContext;
            var area = context.ActionDescriptor.RouteValues["area"];
            var controllerName = context.ActionDescriptor.ControllerName;
            var actionName = context.ActionDescriptor.ActionName;
            var template = context.ActionDescriptor.AttributeRouteInfo?.Template;

            return controller.View(pathView, model);
        }

        public static void AddApiRazorMvcService(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "Swagger.Api", Version = "v1" }); });

            services.AddRazorPages().WithRazorPagesRoot("/UI/Page");
            //services.Configure<RazorPagesOptions>(options => options.RootDirectory = "/UI/Page");

            services.AddMvc(option =>
            {
                option.SetMvcResponseCaching();
            }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddControllersWithViews();
            services.Configure<RazorViewEngineOptions>(o =>
            {
                o.ViewLocationFormats.Clear();
                o.ViewLocationFormats.Add("/UI/Header/{0}" + RazorViewEngine.ViewExtension);
                o.ViewLocationFormats.Add("/UI/Component/{0}" + RazorViewEngine.ViewExtension);
                o.ViewLocationFormats.Add("/UI/Layout/{0}" + RazorViewEngine.ViewExtension);

                o.AreaViewLocationFormats.Clear();
                o.AreaViewLocationFormats.Add("/UI/Header/{0}" + RazorViewEngine.ViewExtension);
                o.AreaViewLocationFormats.Add("/UI/Component/{0}" + RazorViewEngine.ViewExtension);
                o.AreaViewLocationFormats.Add("/UI/Layout/{0}" + RazorViewEngine.ViewExtension);

                //o.ViewLocationFormats.Clear();
                //o.ViewLocationFormats.Add("/UI/{1}/{0}" + RazorViewEngine.ViewExtension);
                //o.ViewLocationFormats.Add("/UI/Shared/{0}" + RazorViewEngine.ViewExtension);

                //o.ViewLocationExpanders.Clear();
                //o.ViewLocationExpanders.Add(new CustomLocationExpander());

                //// {2} is area, {1} is controller,{0} is the action    
                //o.ViewLocationFormats.Clear();
                //o.ViewLocationFormats.Add("/Controllers/{1}/Views/{0}" + RazorViewEngine.ViewExtension);
                //o.ViewLocationFormats.Add("/Controllers/Shared/Views/{0}" + RazorViewEngine.ViewExtension);

                //// Untested. You could remove this if you don't care about areas.
                //o.AreaViewLocationFormats.Clear();
                //o.AreaViewLocationFormats.Add("/Areas/{2}/Controllers/{1}/Views/{0}" + RazorViewEngine.ViewExtension);
                //o.AreaViewLocationFormats.Add("/Areas/{2}/Controllers/Shared/Views/{0}" + RazorViewEngine.ViewExtension);
                //o.AreaViewLocationFormats.Add("/Areas/Shared/Views/{0}" + RazorViewEngine.ViewExtension);
            });

            services.AddRouting(option =>
            {
                option.ConstraintMap.Add("customName", typeof(MyCustomConstraint));
            });

        }

        public static void MapApiRazorMvcMiddleware(this IEndpointRouteBuilder endpoints, IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AIT.UI.Api v1"));

            //endpoints.MapControllers();
            //endpoints.MapDefaultControllerRoute();
            //endpoints.MapRazorPages();


            //endpoints.MapControllerRoute(name: "blog", pattern: "blog/{*article}", defaults: new { controller = "Blog", action = "Article" });

            //endpoints.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

            //endpoints.MapAreaControllerRoute("zebra_route", "Zebra", "Manage/{controller}/{action}/{ id ?}");
            //endpoints.MapControllerRoute("default_route", "{controller}/{action}/{id?}");

            //endpoints.MapControllerRoute("Zebra_route", "Manage/{controller}/{action}/{id?}",
            //    defaults: new { area = "Zebra" }, constraints: new { area = "Zebra" });
            //endpoints.MapControllerRoute("default_route", "{controller}/{action}/{id?}");

            //endpoints.MapControllerRoute("blog_route", "Manage/{controller}/{action}/{id?}", 
            //    defaults: new { area = "Blog" }, constraints: new { area = "Blog" });
            //endpoints.MapControllerRoute("default_route", "{controller}/{action}/{id?}");

            //endpoints.MapAreaControllerRoute(name: "duck_route", areaName: "Duck", pattern: "Manage/{controller}/{action}/{id?}");
            //endpoints.MapControllerRoute(name: "default", pattern: "Manage/{controller=Home}/{action=Index}/{id?}");

            //endpoints.MapAreaControllerRoute(name: "duck_route", areaName: "Duck", pattern: "{area}/{controller}/{action}/{id?}");
            //endpoints.MapControllerRoute(name: "default", pattern: "{area=Duck}/{controller=Home}/{action=Index}/{id?}");
            //endpoints.MapControllerRoute("default_route", "{controller}/{action}/{id?}");

            //endpoints.MapControllerRoute(name: "areaRoute", pattern: "{area}/{controller}/{action}/{id?}");
            endpoints.MapControllers();
            endpoints.MapDefaultControllerRoute();
            endpoints.MapRazorPages();
        }
    }

}
