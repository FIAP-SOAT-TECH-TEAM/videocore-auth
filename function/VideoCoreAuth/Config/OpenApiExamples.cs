using VideoCore.Auth.DTO;
using VideoCore.Auth.Model;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Newtonsoft.Json.Serialization;

namespace VideoCore.Auth.Config
{
  public class ErrorDTOExample : OpenApiExample<ErrorDTO>
  {
    public override IOpenApiExample<ErrorDTO> Build(NamingStrategy namingStrategy)
    {
      Examples.Add(
          OpenApiExampleResolver.Resolve(
              "ErrorDTOExample",
              new ErrorDTO
              {
                Timestamp = DateTime.UtcNow,
                Status = 500,
                Message = "Ocorreu um erro inesperado.",
                Path = "/auth/login"
              },
              namingStrategy
          )
      );

      return this;
    }
  }

  public class UserDetailsDTOExample : OpenApiExample<UserDetailsDTO>
  {
    public override IOpenApiExample<UserDetailsDTO> Build(NamingStrategy namingStrategy)
    {
      Examples.Add(
          OpenApiExampleResolver.Resolve(
              "UserDetailsDTOExample",
              new UserDetailsDTO
              {
                Subject = "c1a2b3c4-d5e6-7890-abcd-ef1234567890",
                Name = "João da Silva",
                Email = "joao.silva@exemplo.com"
              },
              namingStrategy
          )
      );

      return this;
    }
  }
}
