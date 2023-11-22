namespace DrealStudio.Application.Services.Interface;

public interface IUserAwareRequest
{
    Guid UserId { get; set; }
}