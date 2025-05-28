using FCG.Application.DTO.Game;
using FCG.Application.DTO.User;
using FCG.Application.Interfaces;
using FCG.FiapCloudGames.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FCG.FiapCloudGames.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        #region GETS
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetAll()
        {
            var games = _userService.GetAllUsers();
            return Ok(games);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetById(int id)
        {
            try
            {
                var game = _userService.GetUserById(id);
                return Ok(game);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
        #endregion

        #region POST
        [HttpPost(Name = "Users")]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Add([FromBody] UserRequest user)
        {
            var created = _userService.AddUser(user);
            return CreatedAtAction(nameof(GetById), new { id = created.UserId }, created);
        }
        #endregion

        #region PUT
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Update(int id, [FromBody] UserRequest user)
        {
            user.UserId = id;
            var updated = _userService.UpdateUser(user);
            return Ok(updated);
        }
        #endregion

        #region DELETE
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Delete(int id)
        {
            var deleted = _userService.DeleteUser(id);
            return deleted ? NoContent() : NotFound();
        }
        #endregion
    }
}
