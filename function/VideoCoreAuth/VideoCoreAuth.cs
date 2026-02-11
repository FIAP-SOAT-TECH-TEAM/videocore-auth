using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using VideoCore.Auth.DTO;
using VideoCore.Auth.Services;
using VideoCore.Auth.Presenter;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Azure.Functions.Worker.Http;
using Amazon.CognitoIdentityProvider.Model;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.OpenApi.Models;
using System.Net;

namespace VideoCore.Auth
{
    public class VideoCoreAuth(ILogger<VideoCoreAuth> logger, ICognitoService cognitoService)
    {
        private readonly ILogger<VideoCoreAuth> _logger = logger;
        private readonly ICognitoService _cognitoService = cognitoService;

        [Function("ValidateToken")]
        [OpenApiIgnore]
        [OpenApiOperation(operationId: "ValidateToken", tags: "Auth")]
        [OpenApiParameter(name: "access_token", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "Token JWT de acesso")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(UserDetailsDto), Description = "Token validado com sucesso")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Unauthorized, contentType: "application/json", bodyType: typeof(ErrorDto), Description = "Token inválido ou inexistente")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.InternalServerError, contentType: "application/json", bodyType: typeof(ErrorDto), Description = "Erro interno")]
        public async Task<IActionResult> ValidateToken(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "validate")]
        HttpRequestData httpRequestData)
        {
            try
            {
                var accessToken = httpRequestData.Query["access_token"];
                if (string.IsNullOrWhiteSpace(accessToken))
                    throw new NotAuthorizedException("Access token não fornecido.");

                var url = httpRequestData.Query["url"];
                if (string.IsNullOrWhiteSpace(url))
                    throw new NotAuthorizedException("URL não fornecida.");

                var httpMethod = httpRequestData.Query["http_method"];
                if (string.IsNullOrWhiteSpace(httpMethod))
                    throw new NotAuthorizedException("HTTP method não fornecido.");
                httpMethod = httpMethod.ToUpperInvariant();
                    
                var jwtToken = await _cognitoService.ValidateToken(accessToken);
                var jwtTokenSubject = jwtToken.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
                if (string.IsNullOrWhiteSpace(jwtTokenSubject))
                    throw new SecurityTokenException("Token inválido: 'sub' claim não encontrado.");

                var user = await _cognitoService.GetUserBySubAsync(jwtTokenSubject) ?? throw new NotAuthorizedException("Usuário não encontrado.");

                var response = UserPresenter.ToUserDetailsDTO(user, jwtToken.Claims);
                return new OkObjectResult(response);
            }
            catch (SecurityTokenException ex)
            {
                _logger.LogWarning(ex, "Token inválido.");

                var responseStatusCode = (int)HttpStatusCode.Unauthorized;
                var url = httpRequestData.Query["url"]!;
                var errorDto = CommonPresenter.ToErrorDTO(ex, responseStatusCode, url);

                return new UnauthorizedObjectResult(errorDto);
            }
            catch (NotAuthorizedException ex)
            {
                _logger.LogWarning(ex, "Usuário não autorizado.");

                var responseStatusCode = (int)HttpStatusCode.Unauthorized;
                var url = httpRequestData.Query["url"]!;
                var errorDto = CommonPresenter.ToErrorDTO(ex, responseStatusCode, url);

                return new UnauthorizedObjectResult(errorDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao validar token.");

                var responseStatusCode = (int)HttpStatusCode.InternalServerError;
                var url = httpRequestData.Query["url"]!;
                var errorDto = CommonPresenter.ToErrorDTO(ex, responseStatusCode, url);

                return new ObjectResult(errorDto) { StatusCode = responseStatusCode };
            }
        }

    }
}