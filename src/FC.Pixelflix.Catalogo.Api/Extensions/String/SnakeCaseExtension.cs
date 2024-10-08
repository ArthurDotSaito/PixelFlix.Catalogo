﻿using Newtonsoft.Json.Serialization;

namespace FC.Pixelflix.Catalogo.Api.Extensions.String;

public static class SnakeCaseExtension
{
    private static readonly NamingStrategy _snakeCaseNamingStrategy = new SnakeCaseNamingStrategy();
    
    public static string ToSnakeCase(this string stringToConvert)
    {
        ArgumentNullException.ThrowIfNull(stringToConvert, nameof(stringToConvert));
        return _snakeCaseNamingStrategy.GetPropertyName(stringToConvert, false);
    }
}