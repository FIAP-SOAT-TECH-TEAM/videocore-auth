using VideoCore.Auth.DTO;
using VideoCore.Auth.Model;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Newtonsoft.Json.Serialization;

namespace VideoCore.Auth.Config
{
  public class ErrorDtoExample : OpenApiExample<ErrorDto>
  {
    public override IOpenApiExample<ErrorDto> Build(NamingStrategy? namingStrategy = null)
    {
      Examples.Add(
          OpenApiExampleResolver.Resolve(
              "ErrorDTOExample",
              new ErrorDto
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

  public class UserDetailsDtoExample : OpenApiExample<UserDetailsDto>
  {
    public override IOpenApiExample<UserDetailsDto> Build(NamingStrategy? namingStrategy = null)
    {
      Examples.Add(
          OpenApiExampleResolver.Resolve(
              "UserDetailsDTOExample",
              new UserDetailsDto
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
