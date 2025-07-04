using FCG.API.Models;
using FCG.Application.DTO.Game;
using FCG.Application.DTO.User;
using FCG.Application.Interfaces;
using FCG.FiapCloudGames.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FCG.FiapCloudGames.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class GamesController : ControllerBase
    {
        private readonly IGameService _gameService;

        public GamesController(IGameService gameService)
        {
            _gameService = gameService;
        }

        #region GETS
        /// <summary>
        /// Returns all users registered.
        /// </summary>
        /// <returns>List of Users</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<GameResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public IActionResult GetAll()
        {
            var games = _gameService.GetAllGames();
            return Ok(games);
        }

        /// <summary>
        /// Returms a game by id.
        /// </summary>
        /// <returns>Object User</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GameResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public IActionResult GetById(int id)
        {
            var game = _gameService.GetGameById(id);
            return Ok(game);
        }
        #endregion

        #region POST
        /// <summary>
        /// Add a game.
        /// </summary>
        /// <returns>Object User added</returns>
        [HttpPost(Name = "Games")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(GameResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public IActionResult Add([FromBody] GameRequest gameRequest)
        {
            var createdGame = _gameService.AddGame(gameRequest);
            return CreatedAtAction(nameof(GetById), new { id = createdGame.GameId }, createdGame);
        }
        #endregion

        #region PUT
        /// <summary>
        /// Update a game.
        /// </summary>
        /// <returns>Object User updated</returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(GameResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public IActionResult Update(int id, [FromBody] GameRequest gameRequest)
        {
            gameRequest.GameId = id; 

            var updated = _gameService.UpdateGame(gameRequest);
            return Ok(updated);
        }
        #endregion

        #region DELETE
        /// <summary>
        /// Delete a game.
        /// </summary>
        /// <returns>No content</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public IActionResult Delete(int id)
        {
            _gameService.DeleteGame(id);
            return NoContent();
        }
        #endregion
    }
}
