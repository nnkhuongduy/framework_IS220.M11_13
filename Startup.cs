using _99phantram.Interfaces;
using _99phantram.Helpers;
using _99phantram.Options;
using _99phantram.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using FluentValidation.AspNetCore;
using _99phantram.Models;
using FluentValidation;

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
        });
      });
      services.AddFluentValidation();

      services.AddTransient<IValidator<PostUserBody>, PostUserBodyValidator>();
      services.AddSingleton<IDatabaseContextOptions, DatabaseContextOptions>();
      services.AddSingleton<IDatabaseContext, DatabaseContext>();
      services.AddSingleton<IAuthService, AuthService>();
      services.AddSingleton<IRoleService, RoleService>();
      services.AddSingleton<IUserService, UserService>();

      services.AddControllers().AddNewtonsoftJson();
      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "99phantram_backend", Version = "v1" });
      });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "99phantram_backend v1"));
      }

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
