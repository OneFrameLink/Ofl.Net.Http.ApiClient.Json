using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Ofl.Net.Http.ApiClient.Json
{
    public static class HttpResponseMessageExtensions
    {
        public static Task<T> ToObjectAsync<T>(
            this HttpResponseMessage httpResponseMessage,
            CancellationToken cancellationToken
        )
        {
            // Validate parameters.
            if (httpResponseMessage == null) throw new ArgumentNullException(nameof(httpResponseMessage));

            // Call the overload,
            return httpResponseMessage.ToObjectAsync<T>(
                JsonSerializerOptionsExtensions.DefaultJsonSerializerOptions,
                cancellationToken
            );
        }

        public static async Task<T> ToObjectAsync<T>(
            this HttpResponseMessage httpResponseMessage,
            JsonSerializerOptions jsonSerializerOptions, 
            CancellationToken cancellationToken
        )
        {
            // Validate parameters.
            if (httpResponseMessage == null) throw new ArgumentNullException(nameof(httpResponseMessage));
            if (jsonSerializerOptions == null) throw new ArgumentNullException(nameof(jsonSerializerOptions));

            // Get the stream.
            using Stream stream = await httpResponseMessage.Content.ReadAsStreamAsync().ConfigureAwait(false);

            // Deserialize and return.
            return await JsonSerializer
                .DeserializeAsync<T>(stream, jsonSerializerOptions, cancellationToken)
                .ConfigureAwait(false);
       }
    }
}