using FCG.Application.Services;
using FCG.Domain.Entities;
using FCG.Domain.Enums;
using FCG.Domain.Repositories;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;

namespace FCG.Tests.UnitTests.FCG.Tests.Application.Services
{
    public class LoggerServiceTests
    {
        private readonly Mock<ILoggerRepository> _loggerRepoMock;
        private readonly Mock<IHttpContextAccessor> _httpContextMock;
        private readonly LoggerService _loggerService;

        public LoggerServiceTests()
        {
            _loggerRepoMock = new Mock<ILoggerRepository>();
            _httpContextMock = new Mock<IHttpContextAccessor>();

            _loggerService = new LoggerService(_loggerRepoMock.Object, _httpContextMock.Object);
        }

        [Fact]
        public async Task LogTraceAsync_ShouldUseRequestId_WhenLogIdIsNull()
        {
            // Arrange
            var expectedId = Guid.NewGuid();
            var trace = new Trace { Message = "Test", Level = LogLevel.Info, LogId = null };

            var context = new DefaultHttpContext();
            context.Items["RequestId"] = expectedId;
            _httpContextMock.Setup(h => h.HttpContext).Returns(context);

            _loggerRepoMock.Setup(r => r.LogTraceAsync(It.IsAny<Trace>()))
                .Returns(Task.CompletedTask)
                .Callback<Trace>(t => t.LogId.Should().Be(expectedId));

            // Act
            await _loggerService.LogTraceAsync(trace);

            // Assert
            _loggerRepoMock.Verify(r => r.LogTraceAsync(It.IsAny<Trace>()), Times.Once);
        }

        [Fact]
        public async Task LogTraceAsync_ShouldKeepProvidedLogId()
        {
            var providedId = Guid.NewGuid();
            var trace = new Trace { LogId = providedId, Level = LogLevel.Info };

            _loggerRepoMock.Setup(r => r.LogTraceAsync(It.IsAny<Trace>()))
                .Returns(Task.CompletedTask)
                .Callback<Trace>(t => t.LogId.Should().Be(providedId));

            await _loggerService.LogTraceAsync(trace);

            _loggerRepoMock.Verify(r => r.LogTraceAsync(It.IsAny<Trace>()), Times.Once);
        }

        [Fact]
        public async Task LogTraceAsync_ShouldThrow_WhenTraceIsNull()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _loggerService.LogTraceAsync(null));
        }

        [Fact]
        public async Task LogRequestAsync_ShouldCallRepository()
        {
            var log = new RequestLog { LogId = Guid.NewGuid(), Path = "/test" };

            _loggerRepoMock.Setup(r => r.LogRequestAsync(log)).Returns(Task.CompletedTask);

            await _loggerService.LogRequestAsync(log);

            _loggerRepoMock.Verify(r => r.LogRequestAsync(log), Times.Once);
        }

        [Fact]
        public async Task UpdateRequestLogAsync_ShouldThrow_WhenLogIsNull()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _loggerService.UpdateRequestLogAsync(null));
        }
    }
}
