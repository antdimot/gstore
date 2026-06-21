using GStore.API.Common;
using GStore.Core.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Asp.Versioning;
using Scalar.AspNetCore;

namespace GStore.API
{
    public class Startup(IConfiguration configuration)
    {
        public IConfiguration Configuration { get; } = configuration;

        private readonly string _corsPolicy = "gstore_cors_policy";

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

            services.AddAuthorizationBuilder()
                .AddPolicy( "AdminApi", policy =>
                    policy.RequireAssertion( context =>
                         context.User.HasClaim( c =>
                              c.Type == "UserAuthz" && c.Value.Contains("admin") ) ) );

            services.AddControllers();

            services.AddApiVersioning( options =>
            {
                options.DefaultApiVersion = new ApiVersion( 1, 0 );
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            } ).AddMvc();

            services.AddScoped<DataContext>();

            services.AddScoped<SecurityService>();

            services.AddOpenApi();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env )
        {
            if( env.IsDevelopment() )
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors( _corsPolicy );

            //app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints( endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapOpenApi();
                endpoints.MapScalarApiReference();
            } );
        }
    }
}
