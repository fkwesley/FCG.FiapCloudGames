using FCG.Application.Interfaces;
using FCG.FiapCloudGames.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FCG.FiapCloudGames.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GamesController : ControllerBase
    {
        private readonly IGameService _gameService;


        public GamesController(IGameService gameService)
        {
            _gameService = gameService;
        }

        [HttpGet]
        public IActionResult GetAllGames()
        {
            var games = _gameService.GetAllGames();
            return Ok(games);
        }

        [HttpGet("{id}")]
        public IActionResult GetGameById(int id)
        {
            try
            {
                var game = _gameService.GetGameById(id);
                return Ok(game);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost(Name = "Games")]
        public IActionResult Add([FromBody] Game game)
        {
            var created = _gameService.AddGame(game);
            return CreatedAtAction(nameof(GetGameById), new { id = created.GameId }, created);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Game game)
        {
            if (id != game.GameId)
                return BadRequest("ID mismatch");

            var updated = _gameService.UpdateGame(game);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var deleted = _gameService.DeleteGame(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
