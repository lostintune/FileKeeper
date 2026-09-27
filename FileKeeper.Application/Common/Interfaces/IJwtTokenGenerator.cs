using FileKeeper.Domain.Entities;

namespace FileKeeper.Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(UserEntity user);
}