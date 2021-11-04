using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Logging;

using _99phantram.Helpers;
using _99phantram.Services;

namespace _99phantram
{
  public class Startup
  {
    readonly string AllowSpecificOrigins = "_AllowSpecificOrigins";

    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddCors(options =>
      {
        options.AddPolicy(name: AllowSpecificOrigins, builder =>
        {
          builder.WithOrigins("http://localhost:3000").AllowAnyMethod().AllowCredentials().AllowAnyHeader();
          builder.WithOrigins("https://*.99phantram.com").SetIsOriginAllowedToAllowWildcardSubdomains().AllowAnyMethod().AllowCredentials().AllowAnyHeader();
        });
      });
      services.AddFluentValidation();

      services.Add99PhantramServices();

      services.AddControllers().AddNewtonsoftJson();
      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "99phantram_backend", Version = "v1" });
        // Bearer token authentication
        OpenApiSecurityScheme securityDefinition = new OpenApiSecurityScheme()
        {
          Name = "Bearer",
          BearerFormat = "JWT",
          Scheme = "bearer",
          Description = "Specify the authorization token.",
          In = ParameterLocation.Header,
          Type = SecuritySchemeType.Http,
        };
        c.AddSecurityDefinition("jwt_auth", securityDefinition);

        // Make sure swagger UI requires a Bearer token specified
        OpenApiSecurityScheme securityScheme = new OpenApiSecurityScheme()
        {
          Reference = new OpenApiReference()
          {
            Id = "jwt_auth",
            Type = ReferenceType.SecurityScheme
          }
        };
        OpenApiSecurityRequirement securityRequirements = new OpenApiSecurityRequirement()
        {
          {securityScheme, new string[] { }},
        };
        c.AddSecurityRequirement(securityRequirements);
      });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
    {
      MongoDBContext.InitMongoDB(Configuration, logger);

      if (env.IsDevelopment())
      {
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "99phantram_backend v1"));
      }

      app.UseForwardedHeaders(new ForwardedHeadersOptions
      {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
      });

      app.UseCors(AllowSpecificOrigins);
      app.UseRouting();
      app.UseMiddleware<JwtMiddleWare>();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}
