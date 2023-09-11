using Moq;
using PadelApp.Application.Abstractions;
using PadelApp.Application.Abstractions.Authentication;
using PadelApp.Application.Abstractions.Repositories;
using PadelApp.Application.Handlers;
using PadelApp.Domain.ErrorHandling;

namespace PadelApp.UnitTest.Player
{
    public class PlayerRegisterRequestHandlerTests
    {
        private readonly Mock<IPlayerRepository> _mockPlayerRepository;
        private readonly Mock<IJwtProvider> _mockJwtProvider;
        private readonly PlayerRegisterRequestHandler _handler;

        public PlayerRegisterRequestHandlerTests()
        {
            Mock<IUnitOfWork> mockUnitOfWork = new();
            _mockPlayerRepository = new Mock<IPlayerRepository>();
            _mockJwtProvider = new Mock<IJwtProvider>();
            
            _handler = new PlayerRegisterRequestHandler(
                mockUnitOfWork.Object,
                _mockPlayerRepository.Object,
                _mockJwtProvider.Object);
        }

        [Fact]
        public async Task Handle_HappyPath_ReturnsSuccessResult()
        {
            // Arrange
            string token = "someToken";
            _mockPlayerRepository.Setup(x => x.GetByUsernameOrEmail(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((Domain.Aggregates.Player)null!);
            _mockJwtProvider.Setup(x => x.GeneratePlayerToken(It.IsAny<Domain.Aggregates.Player>()))
                .Returns(token);

            // Act
            var result = await _handler.Handle(new PlayerRegisterCommand("username", "password", "email"), default);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(token, result.Value);
        }

        [Fact]
        public async Task Handle_WhenEmailAndUsernameAlreadyExists_ReturnsFailResult()
        {
            // Arrange
            _mockPlayerRepository.Setup(x => x.GetByUsernameOrEmail("username", "email"))
                .ReturnsAsync(new Domain.Aggregates.Player());

            // Act
            var result = await _handler.Handle(new PlayerRegisterCommand("username", "password", "email"), default);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(DomainErrors.UserAlreadyExists().Message, result.Error.Message);
        }

        [Fact]
        public async Task Handle_WhenOnlyEmailAlreadyExists_ReturnsFailResult()
        {
            // Arrange
            _mockPlayerRepository.Setup(x => x.GetByUsernameOrEmail(It.IsAny<string>(), "email"))
                .ReturnsAsync(new Domain.Aggregates.Player());

            // Act
            var result = await _handler.Handle(new PlayerRegisterCommand("new_username", "password", "email"), default);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(DomainErrors.UserAlreadyExists().Message, result.Error.Message);
        }

        [Fact]
        public async Task Handle_WhenOnlyUsernameAlreadyExists_ReturnsFailResult()
        {
            // Arrange
            _mockPlayerRepository.Setup(x => x.GetByUsernameOrEmail("username", It.IsAny<string>()))
                .ReturnsAsync(new Domain.Aggregates.Player());

            // Act
            var result = await _handler.Handle(new PlayerRegisterCommand("username", "password", "new_email"), default);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(DomainErrors.UserAlreadyExists().Message, result.Error.Message);
        }
    }
}
