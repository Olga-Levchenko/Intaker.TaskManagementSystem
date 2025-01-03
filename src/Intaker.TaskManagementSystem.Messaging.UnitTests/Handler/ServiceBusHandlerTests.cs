using Intaker.TaskManagementSystem.Messaging.Handler;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using RabbitMQ.Client;

namespace Intaker.TaskManagementSystem.Messaging.UnitTests.Handler
{
    [TestFixture]
    public class ServiceBusHandlerTests
    {
        private readonly Mock<ILogger<ServiceBusHandler>> _loggerMock;
        private readonly Mock<IConnection> _connectionMock;
        private readonly Mock<IChannel> _channelMock;
        private readonly ServiceBusHandler _serviceBusHandler;

        public ServiceBusHandlerTests()
        {
            _loggerMock = new Mock<ILogger<ServiceBusHandler>>();
            _connectionMock = new Mock<IConnection>();
            _channelMock = new Mock<IChannel>();

            var connectionFactoryMock = new Mock<IConnectionFactory>();
            connectionFactoryMock.Setup(f => f.CreateConnectionAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(_connectionMock.Object);
            _connectionMock.Setup(c => c.CreateChannelAsync(It.IsAny<CreateChannelOptions>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_channelMock.Object);
            _serviceBusHandler = new ServiceBusHandler(_loggerMock.Object, connectionFactoryMock.Object);
        }

        [Test]
        public async Task Configure_ShouldSetupConnectionAndChannel()
        {
            // Act
            await _serviceBusHandler.Configure();

            // Assert
            _connectionMock.Verify(c => c.CreateChannelAsync(It.IsAny<CreateChannelOptions>(), default), Times.Once);
            _channelMock.Verify(c => c.QueueDeclareAsync(It.IsAny<string>(), false, false, false, null, false, false, default), Times.Once);
        }
    }
}
