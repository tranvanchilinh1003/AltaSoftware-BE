
using ISC_ELIB_SERVER.DTOs.Requests;
using System.IdentityModel.Tokens.Jwt;

namespace ISC_ELIB_SERVER.Utils
{
    public class ExtractAccessTokenJWT
    {
        public static Dictionary<string, object> DecodeJWT(AccessTokenReq token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token.AccessToken);

            return jwtToken.Payload.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }
    }
}
