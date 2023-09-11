using Moq;
using PadelApp.Application.Abstractions;
using PadelApp.Application.Abstractions.Authentication;
using PadelApp.Application.Abstractions.Repositories;
using PadelApp.Application.Handlers;
using PadelApp.Domain.ErrorHandling;
using Xunit;

namespace PadelApp.Test.Player;

public class PlayerRegisterRequestHandlerTests
{
    private readonly Mock<IPlayerRepository> _mockPlayerRepository;
    private readonly PlayerRegisterRequestHandler _handler;

    public PlayerRegisterRequestHandlerTests()
    {
        Mock<IUnitOfWork> mockUnitOfWork = new();
        _mockPlayerRepository = new Mock<IPlayerRepository>();
        Mock<IJwtProvider> mockJwtProvider = new();
        
        _handler = new PlayerRegisterRequestHandler(
            mockUnitOfWork.Object,
            _mockPlayerRepository.Object,
            mockJwtProvider.Object);
    }

    [Fact]
    public async Task Handle_WhenUserAlreadyExists_ReturnsFailResult()
    {
        // Arrange
        _mockPlayerRepository.Setup(x =>
                x.GetByUsernameOrEmail(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(new Domain.Aggregates.Player());

        // Act
        var result = await _handler.Handle(new PlayerRegisterCommand("username", "password", "email"), default);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.UserAlreadyExists().Message, result.Error.Message);
    }
}