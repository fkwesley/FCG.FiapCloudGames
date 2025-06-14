﻿using Xunit;
using Moq;
using FluentAssertions;
using FCG.Application.Services;
using FCG.Domain.Repositories;
using FCG.Application.DTO.Game;
using FCG.Domain.Entities;
using FCG.Application.Exceptions;
using FCG.Application.Interfaces;
using FCG.FiapCloudGames.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FCG.Tests.UnitTests.FCG.Tests.Application.Services
{
    public class GameServiceTests
    {
        private readonly Mock<IGameRepository> _gameRepositoryMock;
        private readonly Mock<ILoggerService> _loggerServiceMock;
        private readonly GameService _gameService;

        public GameServiceTests()
        {
            _gameRepositoryMock = new Mock<IGameRepository>();
            _loggerServiceMock = new Mock<ILoggerService>();
            _gameService = new GameService(_gameRepositoryMock.Object, _loggerServiceMock.Object);
        }

        [Fact]
        public void GetAllGames_ShouldReturnAllGames()
        {
            // Arrange
            var games = new List<Game> { new Game { GameId = 1, Name = "GTA IV", Description = "O mais aguardado", Genre = "Action", CreatedAt = DateTime.UtcNow.AddMinutes(50) } };
            _gameRepositoryMock.Setup(r => r.GetAllGames()).Returns(games); 

            // Act
            var result = _gameService.GetAllGames();

            // Assert
            result.Should().HaveCount(1);
            result.First().Name.Should().Be("GTA IV");
        }

        [Theory]
        [InlineData("GTA IV", "O Mais aguardado", "Action")]
        [InlineData("GTA VICE CITY", "Classic game", "Adventure")]
        public void AddGame_ShouldThrow_WhenGameAlreadyExists(string gameName, string description, string genre)
        {
            // Arrange
            var existingGames = new List<Game>
        {
            new Game
            {
                Name = gameName,
                Description = description,
                Genre = genre
            }
        };
            _gameRepositoryMock.Setup(r => r.GetAllGames()).Returns(existingGames);

            var request = new GameRequest { Name = gameName, Description = description, Genre = genre };

            // Act
            Action act = () => _gameService.AddGame(request);

            // Assert
            act.Should().Throw<ValidationException>()
                .WithMessage($"Game {gameName} already exists.");
        }

        [Fact]
        public void AddGame_ShouldAddSuccessfully_WhenNameIsUnique()
        {
            // Arrange
            var request = new GameRequest
            {
                Name = "Max Payne 3",
                Description = "Very good game",
                Genre = "Action"
            };

            _gameRepositoryMock.Setup(r => r.GetAllGames())
                .Returns(new List<Game>()); // Nenhum jogo existente

            _gameRepositoryMock.Setup(r => r.AddGame(It.IsAny<Game>()))
                .Returns((Game g) =>
                {
                    g.GameId = 1;
                    g.CreatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
                    return g;
                });

            // Act
            var result = _gameService.AddGame(request);

            // Assert
            result.Name.Should().Be("Max Payne 3");
            result.GameId.Should().Be(1);
        }

        [Fact]
        public void UpdateGame_ShouldThrow_WhenDuplicateNameExists()
        {
            // Arrange
            var request = new GameRequest { GameId = 2, Name = "Max Payne 3", Description = "Very good game", Genre = "Action" };
            var games = new List<Game> { new Game { GameId = 1, Name = "Max Payne 3", Description = "other description", Genre = "Action" } };
            _gameRepositoryMock.Setup(r => r.GetAllGames()).Returns(games);

            // Act
            Action act = () => _gameService.UpdateGame(request);

            // Assert
            act.Should().Throw<ValidationException>()
                .WithMessage("Game Max Payne 3 already exists.");
        }

        [Fact]
        public void DeleteGame_ShouldReturnTrue_WhenGameDeleted()
        {
            // Arrange
            _gameRepositoryMock.Setup(r => r.DeleteGame(1)).Returns(true);

            // Act
            var result = _gameService.DeleteGame(1);

            // Assert
            result.Should().BeTrue();
        }
    }
}