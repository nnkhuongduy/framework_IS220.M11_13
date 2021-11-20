using FluentValidation;

using _99phantram.Interfaces;
using _99phantram.Models;
using _99phantram.Services;
using _99phantram.Options;
using _99phantram.Helpers;

namespace Microsoft.Extensions.DependencyInjection
{
  public static class _99phantramServiceCollectionExtensions
  {
    public static IServiceCollection Add99PhantramServices(this IServiceCollection services)
    {
      services.AddTransient<IValidator<PostUserBody>, PostUserBodyValidator>();
      services.AddTransient<IValidator<PutUserBody>, PutUserBodyValidator>();
      services.AddTransient<IValidator<LocationBody>, LocationBodyValidator>();

      services.AddSingleton<IAmazonS3Options, AmazonS3Options>();
      services.AddSingleton<IAmazonS3Context, AmazonS3Context>();

      services.AddSingleton<IAuthService, AuthService>();
      services.AddSingleton<IRoleService, RoleService>();
      services.AddSingleton<IUserService, UserService>();
      services.AddSingleton<ICategoryService, CategoryService>();
      services.AddSingleton<ISpecService, SpecService>();
      services.AddSingleton<ILocationService, LocationService>();

      services.AddScoped<AppAuthorize>();
      services.AddScoped<JwtHolder>();

      return services;
    }
  }
}