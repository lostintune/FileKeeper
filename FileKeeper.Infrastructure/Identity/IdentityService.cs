using System.Security.Authentication;
using FileKeeper.Application.Common.Exceptions;
using FileKeeper.Application.Common.Interfaces.Users;
using FileKeeper.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace FileKeeper.Infrastructure.Identity;

public class IdentityService: IIdentityService
{
    private readonly UserManager<AppIdentityUser> _userManager;
    
    public IdentityService(UserManager<AppIdentityUser> userManager)
    {
        _userManager = userManager;
    }
    
    public async Task<Guid> CreateUserAsync(UserEntity user, string password)
    {
        var identityUser = new AppIdentityUser
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            UserName = user.Username,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            CreatedAt = user.CreatedAt,
            IsDeleted = user.IsDeleted
        };
        
        var result = await _userManager.CreateAsync(identityUser, password);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new IdentityOperationException($"Failed to create user: {errors}");
        }
        
        return identityUser.Id;
    }

    public async Task<bool> UsernameExistsAsync(string username)
    {
        var user = await _userManager.FindByNameAsync(username);
        return user != null;
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        return user != null;
    }

    public async Task<UserEntity?> GetByIdAsync(Guid id)
    {
        var identityUser = await _userManager.FindByIdAsync(id.ToString());
        
        if (identityUser == null)
        {
            return null;
        }
        
        return UserEntity.Reconstitute(
            identityUser.Id,
            identityUser.FirstName,
            identityUser.LastName,
            identityUser.UserName!,
            identityUser.Email!,
            identityUser.PhoneNumber!,
            identityUser.CreatedAt,
            identityUser.IsDeleted);
    }

    public async Task UpdateProfileAsync(UserEntity user)
    {
        var identityUser = await _userManager.FindByIdAsync(user.Id.ToString());
    
        if (identityUser == null)
        {
            throw new UserNotFoundException(user.Id);
        }
    
        identityUser.FirstName = user.FirstName;
        identityUser.LastName = user.LastName;
        identityUser.UserName = user.Username;
        identityUser.PhoneNumber = user.PhoneNumber;
        
        var result = await _userManager.UpdateAsync(identityUser);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new IdentityOperationException($"Failed to update user: {errors}");
        }
    }

    public async Task DeleteUserAsync(UserEntity user)
    {
        var identityUser = await _userManager.FindByIdAsync(user.Id.ToString());
        if (identityUser == null)
        {
            throw new UserNotFoundException(user.Id);
        }
        
        identityUser.IsDeleted = user.IsDeleted;
        
        var result = await _userManager.UpdateAsync(identityUser);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new IdentityOperationException($"Failed to update user: {errors}");
        }
    }

    public async Task<UserEntity?> ValidateCredentialsAsync(string email, string password)
    {
        var identityUser = await _userManager.FindByEmailAsync(email);
        if (identityUser == null)
        {
            return null;
        }
        
        var result = await _userManager.CheckPasswordAsync(identityUser, password);

        if (!result)
        {
            return null;
        }
        
        return UserEntity.Reconstitute(
            identityUser.Id,
            identityUser.FirstName,
            identityUser.LastName,
            identityUser.UserName!,
            identityUser.Email!,
            identityUser.PhoneNumber!,
            identityUser.CreatedAt,
            identityUser.IsDeleted);
    }
}