using Amazon.CognitoIdentityProvider.Model;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using VideoCore.Auth.Model;
using Amazon.CognitoIdentityProvider;

namespace VideoCore.Auth.Services
{
  /// <summary>
  /// Serviço utilitário para operações no Amazon Cognito.
  /// </summary>
  public class CognitoService(IAmazonCognitoIdentityProvider cognito, CognitoSettings settings) : ICognitoService
  {
    private readonly IAmazonCognitoIdentityProvider _cognito = cognito;
    private readonly CognitoSettings _settings = settings;

    /// <summary>
    /// Obtém usuário pelo sub (ID único) no User Pool.
    /// </summary>
    /// <param name="sub">Sub (ID único) do usuário.</param>
    /// <returns>Usuário encontrado ou nulo.</returns>
    public async Task<UserType?> GetUserBySubAsync(string sub)
    {
      var listUsersRequest = new ListUsersRequest
      {
        UserPoolId = _settings.UserPoolId,
        Filter = $"sub = \"{sub}\""
      };
      var listUsers = await _cognito.ListUsersAsync(listUsersRequest);
      var user = listUsers.Users.FirstOrDefault();

      return user;
    }

    /// <summary>
    /// Valida um access token JWT emitido pelo Amazon Cognito, utilizando JWKS (JSON Web Key Set).
    /// </summary>
    /// <param name="accessToken">JWT (access_token) a ser validado.</param>
    /// <returns>Instância de <see cref="JwtSecurityToken"/> já validada.</returns>
    /// <exception cref="HttpRequestException">
    /// Lançada quando não é possível obter as chaves públicas (JWKS) do Cognito.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Lançada quando o token é nulo, vazio ou possui formato inválido.
    /// </exception>
    /// <exception cref="SecurityTokenException">
    /// Lançada quando a validação do token falha (assinatura, emissor, audiência ou expiração).
    /// </exception>
    public async Task<JwtSecurityToken> ValidateToken(string accessToken)
    {
      // Baixa as chaves públicas do Cognito (JWKS)
      using var http = new HttpClient();
      var jwksUri = $"https://cognito-idp.{_settings.Region}.amazonaws.com/{_settings.UserPoolId}/.well-known/jwks.json";
      var jwksJson = await http.GetStringAsync(jwksUri);
      var keys = new JsonWebKeySet(jwksJson).GetSigningKeys();

      var tokenHandler = new JwtSecurityTokenHandler();
      var validationParameters = new TokenValidationParameters
      {
        ValidateIssuer = true,
        ValidIssuer = $"https://cognito-idp.{_settings.Region}.amazonaws.com/{_settings.UserPoolId}",
        ValidateAudience = false,
        // ValidAudience = settings.AppClientId,
        ValidateLifetime = true,
        RequireSignedTokens = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKeys = keys
      };

      tokenHandler.ValidateToken(accessToken, validationParameters, out var validatedToken);
      var jwtValidatedToken = (JwtSecurityToken)validatedToken;
      var clientId = jwtValidatedToken.Claims.FirstOrDefault(c => c.Type == "client_id")?.Value;

      // Access Tokens do Cognito não possuem o claim "aud", então validamos a audiência manualmente
      // https://stackoverflow.com/questions/53148711/why-doesnt-amazon-cognito-return-an-audience-field-in-its-access-tokens
      if (clientId != _settings.AppClientId)
        throw new SecurityTokenException("Audience inválido.");

      return jwtValidatedToken;
    }

  }
}