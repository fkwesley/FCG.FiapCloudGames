using FCG.FiapCloudGames.Core.Entities;

namespace FCG.Application.Interfaces
{
    public interface IAuthService
    {
        string GenerateToken(User user);
    }
}
