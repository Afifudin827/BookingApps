using System.Security.Claims;

namespace Server.Contracts;

public interface ITokenHendler
{
    //menambahkan atribut generate agar data yang di dapat berdasarkan generate yang ada di repositori
    string Generate(IEnumerable<Claim> claims);
}
