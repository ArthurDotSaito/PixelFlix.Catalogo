using System.Text.Json;
using FC.Pixelflix.Catalogo.Api.Extensions.String;

namespace FC.Pixelflix.Catalogo.Api.Configurations.Policies;

public class JsonSnakeCasePolicy : JsonNamingPolicy
{
    public override string ConvertName(string name)
    {
        return name.ToSnakeCase();
    }
}