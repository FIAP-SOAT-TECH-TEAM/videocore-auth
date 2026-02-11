using System.Security.Claims;
using Amazon.CognitoIdentityProvider.Model;
using VideoCore.Auth.DTO;
    
namespace VideoCore.Auth.Presenter
{
    /// <summary>
    /// Fornece mapeamentos de respostas do Amazon Cognito para DTOs de usuário.
    /// </summary>
    public static class UserPresenter
    {
        /// <summary>
        /// Converte a resposta de obtenção de usuário do Amazon Cognito em um <see cref="UserDetailsDto"/>.
        /// </summary>
        /// <param name="user">Resposta do Cognito retornada por <c>AdminGetUser</c>.</param>
        /// <param name="claims">Coleção de claims extraídas do token JWT do usuário.</param>
        /// <returns>Um <see cref="UserDetailsDto"/> com os detalhes do usuário.</returns>
        /// <exception cref="InvalidOperationException">Lançada quando <paramref name="user"/> é nulo.</exception>
        public static UserDetailsDto ToUserDetailsDTO(UserType? user = null, IEnumerable<Claim>? claims = null)
        {
            return new UserDetailsDto
            {
                Subject = claims?.FirstOrDefault(c => c.Type == "sub")?.Value ?? "",
                Name = user?.Attributes.Find(attr => attr.Name == "name")?.Value ?? "",
                Email = user?.Attributes.Find(attr => attr.Name == "email")?.Value ?? ""
            };
        }
    }
}