using FileKeeper.Domain.Exceptions.UserExceptions;

namespace FileKeeper.Domain.Entities;

public class UserEntity
{
    public Guid Id { get; private set; }
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string Username { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PhoneNumber { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }
    public bool IsDeleted { get; private set; }
    
    private UserEntity() { }
    
    private static void ValidateUserDetails(string firstName, string lastName, string username, string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(firstName))
        {
            throw new InvalidUserDetailsException("First name is invalid");
        }

        if (string.IsNullOrWhiteSpace(lastName))
        {
            throw new InvalidUserDetailsException("Last name is invalid");
        }

        if (string.IsNullOrWhiteSpace(username))
        {
            throw new InvalidUserDetailsException("Username is invalid");
        }
        
        if (string.IsNullOrWhiteSpace(phoneNumber))
        {
            throw new InvalidUserDetailsException("Phone number is invalid");
        }
    }

    public static UserEntity Create(string firstName, string lastName, string username, string email,
        string phoneNumber)
    {
        ValidateUserDetails(firstName, lastName, username, phoneNumber);
        
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new InvalidUserDetailsException("Email is invalid");
        }

        return new UserEntity
        {
            Id = Guid.NewGuid(),
            FirstName = firstName,
            LastName = lastName,
            Username = username,
            Email = email,
            PhoneNumber = phoneNumber,
            CreatedAt = DateTime.UtcNow,
            IsDeleted = false
        };
    }
    
    public void UpdateProfile(string firstName, string lastName, string username, string phoneNumber)
    {
        ValidateUserDetails(firstName, lastName, username, phoneNumber);

        FirstName = firstName;
        LastName = lastName;
        Username = username;
        PhoneNumber = phoneNumber;
    }
    
    public void SoftDelete()
    {
        if (IsDeleted)
        {
            throw new UserAlreadyDeletedException();
        }
        IsDeleted = true;
    }

}