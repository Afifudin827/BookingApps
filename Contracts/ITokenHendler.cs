using System.Security.Claims;

namespace Server.Contracts;

public interface ITokenHendler
{
    string Generate(IEnumerable<Claim> claims);
}
