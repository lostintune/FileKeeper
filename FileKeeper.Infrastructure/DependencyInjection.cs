using FileKeeper.Application.Common.Interfaces;
using FileKeeper.Application.Common.Interfaces.Files;
using FileKeeper.Application.Common.Interfaces.Users;
using FileKeeper.Infrastructure.Auth;
using FileKeeper.Infrastructure.Identity;
using FileKeeper.Infrastructure.Persistence;
using FileKeeper.Infrastructure.Repositories;
using FileKeeper.Infrastructure.Storage;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace FileKeeper.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options => 
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddIdentityCore<AppIdentityUser>()
            .AddRoles<IdentityRole<Guid>>()
            .AddEntityFrameworkStores<AppDbContext>();
        
        services.AddScoped<IFileRepository, FileRepository>();
        services.AddScoped<IFileAccessRepository, FileAccessRepository>();
        services.AddScoped<IFileStorage, LocalFileStorage>();
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        
        return services;
    }
}