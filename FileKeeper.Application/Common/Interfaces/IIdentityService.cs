using FileKeeper.Domain.Entities;

namespace FileKeeper.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<Guid> CreateUserAsync(UserEntity user, string password);
    Task<bool> UsernameExistsAsync(string username);
    Task<bool> EmailExistsAsync(string email);
    Task<UserEntity?> GetByIdAsync(Guid id);
    Task UpdateProfileAsync(UserEntity user);
    Task DeleteUserAsync(UserEntity user);

}