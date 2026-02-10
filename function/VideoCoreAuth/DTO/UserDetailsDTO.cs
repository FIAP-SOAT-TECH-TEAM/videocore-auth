using VideoCore.Auth.Config;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

namespace VideoCore.Auth.DTO
{
  /// <summary>
  /// Data Transfer Object (DTO) para detalhes do usuário.
  /// </summary>
  [OpenApiExample(typeof(UserDetailsDTOExample))]
  public class UserDetailsDTO
  {
    /// <summary>
    /// Identificador único do usuário.
    /// </summary>
    [OpenApiProperty(Description = "Identificador único do usuário.")]
    public required string Subject { get; set; }

    /// <summary>
    /// Nome completo do usuário.
    /// </summary>
    [OpenApiProperty(Description = "Nome completo do usuário.")]
    public required string Name { get; set; }

    /// <summary>
    /// E-mail do usuário.
    /// </summary>
    [OpenApiProperty(Description = "E-mail do usuário.")]
    public required string Email { get; set; }
  }
}