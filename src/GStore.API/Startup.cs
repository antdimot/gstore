using GStore.API.Common;
using GStore.Core.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;

namespace GStore.API
{
    public class Startup
    {
        public Startup( IConfiguration configuration )
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private string _corsPolicy = "gstore_cors_policy";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices( IServiceCollection services )
        {
            services.AddCors( options =>
            {
                options.AddPolicy( _corsPolicy,
                builder =>
                {
                    // Not a permanent solution, but just trying to isolate the problem
                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                } );
            } );

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

            services.AddControllers();

            services.AddApiVersioning();           

            services.AddScoped<DataContext>();

            services.AddScoped<SecurityService>();

            services.AddSwaggerGen( c =>
            {
                c.SwaggerDoc( "v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "GSTore API",
                    Description = "A service for storing and retrieving data by latitude and longitude. ",
                    TermsOfService = new Uri( "https://github.com/antdimot/gstore" ),
                    Contact = new OpenApiContact
                    {
                        Name = "Antonio Di Motta",
                        Email = string.Empty,
                        Url = new Uri( "https://github.com/antdimot" ),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under Apache License",
                        Url = new Uri( "https://github.com/antdimot/gstore/blob/master/LICENSE.txt" ),
                    }
                } );
            } );
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env )
        {
            if( env.IsDevelopment() )
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors( _corsPolicy );

            app.UseSwagger();

            app.UseSwaggerUI( c =>
            {
                c.SwaggerEndpoint( "/swagger/v1/swagger.json", "GSTore API V1" );
                c.RoutePrefix = string.Empty;
            } );

            //app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints( endpoints =>
            {
                endpoints.MapControllers();
            } );
        }
    }
}
