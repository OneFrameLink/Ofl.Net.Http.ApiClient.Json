using System.Text.Json;

namespace Ofl.Net.Http.ApiClient.Json
{
    public static class JsonSerializerOptionsExtensions
    {
        internal static readonly JsonSerializerOptions DefaultJsonSerializerOptions = 
            CreateDefaultJsonSerializerOptions();

        // This is a method because the JsonSerializerOptions type is mutable.
        public static JsonSerializerOptions CreateDefaultJsonSerializerOptions() => new JsonSerializerOptions {
            // Camel case.
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,

            // Don't send null values.
            IgnoreNullValues = true
        };
    }
}
