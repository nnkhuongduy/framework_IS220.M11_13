using FluentValidation;

using _99phantram.Interfaces;
using _99phantram.Models;
using _99phantram.Services;
using _99phantram.Options;

namespace Microsoft.Extensions.DependencyInjection
{
  public static class _99phantramServiceCollectionExtensions
  {
    public static IServiceCollection Add99PhantramServices(this IServiceCollection services)
    {
      services.AddTransient<IValidator<PostUserBody>, PostUserBodyValidator>();
      services.AddTransient<IValidator<PutUserBody>, PutUserBodyValidator>();

      services.AddSingleton<IAmazonS3Options, AmazonS3Options>();
      services.AddSingleton<IAmazonS3Context, AmazonS3Context>();

      services.AddSingleton<IAuthService, AuthService>();
      services.AddSingleton<IRoleService, RoleService>();
      services.AddSingleton<IUserService, UserService>();
      services.AddSingleton<ICategoryService, CategoryService>();

      return services;
    }
  }
}