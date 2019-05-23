using GStore.API.Common;
using GStore.Core.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace GStore.API
{
    public class Startup
    {
        public Startup( IConfiguration configuration )
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices( IServiceCollection services )
        {
            services.AddAuthentication( options => {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            } )
            .AddJwtBearer( options => {
                options.Audience = SecurityService.Audience;
                options.TokenValidationParameters = new SecurityService( Configuration ).CreateValidationParams();
            } );

            // set authorization policy for api accessing
            services.AddAuthorization( options =>
            {
                options.AddPolicy( "AdminApi", policy =>
                    policy.RequireAssertion( context =>
                         context.User.HasClaim( c =>
                              c.Type == "UserAuthz" && c.Value.Contains("admin") ) ) );
            } );

            services.AddCors();

            services.AddMvc( options =>
            {
                //options.RequireHttpsPermanent = true;
            } ).SetCompatibilityVersion( CompatibilityVersion.Version_2_1 );

            services.AddApiVersioning();

            services.AddSwaggerGen( c =>
            {
                c.SwaggerDoc( "v1", new Info
                {
                    Title = "GStore API",
                    Description = "A service for storing and retrieving data by latitude and longitude.",
                    Version = "1",
                    Contact = new Contact
                    {
                        Name = "Antonio Di Motta",
                        Email = string.Empty,
                        Url = "https://twitter.com/dimotta"
                    },
                } );
                c.OperationFilter<VersionOperationFilter>();
                c.DocumentFilter<ApplyDocumentVersionExtensions>();
                            // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            } );

            services.AddScoped<DataContext>();
            services.AddScoped<SecurityService>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //if( env.IsDevelopment() )
            //{
            //    app.UseDeveloperExceptionPage();
            //}

            app.UseCors( builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader() );

            app.UseAuthentication();
            app.UseMvc();

            app.UseSwagger( c => { c.RouteTemplate = "api/swagger/{documentName}/swagger.json"; } );
            app.UseSwaggerUI( c => {
                c.SwaggerEndpoint( "v1/swagger.json", "GStore API" );
                c.RoutePrefix = "api/swagger";
            } );
        }
    }

    public class ApiExplorerGroupPerVersionConvention : IControllerModelConvention
    {
        public void Apply( ControllerModel controller )
        {
            var controllerNamespace = controller.ControllerType.Namespace;
            var apiVersion = controllerNamespace.Split('.').Last().ToLower();

            controller.ApiExplorer.GroupName = apiVersion;
        }
    }

    internal class ApplyDocumentVersionExtensions : IDocumentFilter
    {
        public void Apply( SwaggerDocument swaggerDoc, DocumentFilterContext context )
        {
            swaggerDoc.Paths = swaggerDoc.Paths
            .ToDictionary(
                path => path.Key.Replace( "{version}", swaggerDoc.Info.Version ),
                path => path.Value
            );
        }
    }

    internal class VersionOperationFilter : IOperationFilter
    {
        public void Apply( Operation operation, OperationFilterContext context )
        {
            var version = operation.Parameters?.FirstOrDefault(p => p.Name == "version");
            if( version != null )
            {
                operation.Parameters.Remove( version );
            }
        }
    }
}
