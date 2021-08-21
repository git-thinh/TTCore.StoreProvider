using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Razor.Extensions;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using TTCore.StoreProvider.Middleware;
using TTCore.StoreProvider.TagHelpers;

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

    public class CustomTemplateEngine : MvcRazorTemplateEngine
    {
        public CustomTemplateEngine(RazorEngine engine, RazorProject project) : base(engine, project) { }
        public override RazorCSharpDocument GenerateCode(RazorCodeDocument codeDocument)
        {
            var csharpDocument = base.GenerateCode(codeDocument);
            var generatedCode = csharpDocument.GeneratedCode;
            // Look at generatedCode
            return csharpDocument;
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
            //services.AddSingleton<RazorTemplateEngine, CustomTemplateEngine>();

            services.AddTransient<ITagHelperComponent, BodyScriptTagHelperComponent>();
            services.AddTransient<ITagHelperComponent, HeaderStyleTagHelperComponent>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "TTCore API",
                    Description = "TTCore API Swagger Surface",
                    Contact = new OpenApiContact
                    {
                        Name = "Mr.Thinh",
                        Email = "chuphucben@gmail.com",
                        Url = new Uri("http://thinh.iot.vn")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "MIT",
                        Url = new Uri("http://thinh.iot.vn/LICENSE")
                    }
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement() {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            }); 
            //services.AddGrpcSwagger();


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
