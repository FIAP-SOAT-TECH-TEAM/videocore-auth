using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;

namespace VideoCore.Auth.Config
{
  
  /// <summary>
  /// OpenAPI configuration options
  /// </summary>
  public class OpenApiConfigurationOptions : DefaultOpenApiConfigurationOptions
  {
    public override OpenApiInfo Info { get; set; } = new OpenApiInfo()
    {
      Version = "1.0.0",
      Title = "VideoCore Auth API",
      Description = "API para autenticação e autorização de usuários utilizando AWS Cognito.",
      Contact = new OpenApiContact()
      {
        Name = "SOAT Team 8",
        Url = new Uri("https://github.com/FIAP-SOAT-TECH-TEAM/videocore-auth"),
      },
      License = new OpenApiLicense()
      {
        Name = "MIT",
        Url = new Uri("https://opensource.org/licenses/MIT"),
      }
    };

    public override OpenApiVersionType OpenApiVersion { get; set; } = OpenApiVersionType.V3;
  }

}