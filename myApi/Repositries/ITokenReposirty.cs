using Microsoft.AspNetCore.Identity;

namespace myApi.Repositries
{
    public interface ITokenReposirty
    {
        String CreateJWTToken(IdentityUser user, List<string> roles);
    }
}
