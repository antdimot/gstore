﻿using GStore.API.Common;
using GStore.Core.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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

            services.AddControllers();

            services.AddApiVersioning();           

            services.AddScoped<DataContext>();

            services.AddScoped<SecurityService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env )
        {
            if( env.IsDevelopment() )
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            //app.UseCors( builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader() );

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints( endpoints =>
            {
                endpoints.MapControllers();
            } );
        }
    }
}
