using LibraryAPI.Models;

namespace LibraryAPI.Services;
public interface ITokenService
{
    string CreateToken(User user);
}
