using Amazon.CognitoIdentityProvider.Model;
using System.IdentityModel.Tokens.Jwt;

namespace VideoCore.Auth.Services
{
  /// <summary>
  /// Contrato para operações utilitárias no Amazon Cognito.
  /// </summary>
  public interface ICognitoService
  {
    /// <summary>
    /// Obtém usuário pelo sub (ID único) no User Pool.
    /// </summary>
    /// <param name="sub">Sub (ID único) do usuário.</param>
    /// <returns>Usuário encontrado ou nulo.</returns>
    Task<UserType?> GetUserBySubAsync(string sub);

    /// <summary>
    /// Valida um access token JWT emitido pelo Amazon Cognito, utilizando JWKS (JSON Web Key Set).
    /// </summary>
    /// <param name="accessToken">JWT (access_token) a ser validado.</param>
    /// <returns>Instância de <see cref="JwtSecurityToken"/> já validada.</returns>
    Task<JwtSecurityToken> ValidateToken(string accessToken);
  }
}