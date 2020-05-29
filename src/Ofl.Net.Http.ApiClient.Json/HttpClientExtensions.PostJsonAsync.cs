using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Ofl.Net.Http.ApiClient.Json
{
    public static partial class HttpClientExtensions
    {
        public static Task<TResponse> PostJsonAsync<TRequest, TResponse>(
            this HttpClient httpClient,
            string uri,
            TRequest request,
            CancellationToken cancellationToken
        ) => httpClient.PostJsonAsync<TRequest, TResponse>(
            uri,
            false,
            request, 
            cancellationToken
        );

        public static Task<TResponse> PostJsonAsync<TResponse>(
            this HttpClient httpClient,
            string uri,
            object request,
            CancellationToken cancellationToken
        ) => httpClient.PostJsonAsync<TResponse>(
            uri,
            false,
            request,
            cancellationToken
        );

        public static Task<TResponse> PostJsonAsync<TRequest, TResponse>(
            this HttpClient httpClient,
            string uri,
            bool includeCharset,
            TRequest request,
            CancellationToken cancellationToken
        ) => httpClient.PostJsonAsync<TRequest, TResponse>(
            uri,
            includeCharset,
            JsonSerializerOptionsExtensions.DefaultJsonSerializerOptions,
            request, 
            cancellationToken
        );

        public static Task<TResponse> PostJsonAsync<TResponse>(
            this HttpClient httpClient,
            string uri,
            bool includeCharset,
            object request,
            CancellationToken cancellationToken
        ) => httpClient.PostJsonAsync<TResponse>(
            uri,
            includeCharset,
            JsonSerializerOptionsExtensions.DefaultJsonSerializerOptions,
            request,
            cancellationToken
        );

        public static Task<TResponse> PostJsonAsync<TRequest, TResponse>(
            this HttpClient httpClient,
            string uri,
            JsonSerializerOptions jsonSerializerOptions,
            TRequest request,
            CancellationToken cancellationToken
        ) => httpClient.PostJsonAsync<TRequest, TResponse>(
            uri,
            false,
            jsonSerializerOptions,
            request,
            cancellationToken
        );

        public static Task<TResponse> PostJsonAsync<TResponse>(
            this HttpClient httpClient,
            string uri,
            JsonSerializerOptions jsonSerializerOptions,
            object request,
            CancellationToken cancellationToken
        ) => httpClient.PostJsonAsync<TResponse>(
            uri,
            false,
            jsonSerializerOptions,
            request,
            cancellationToken
        );

        public static async Task<TResponse> PostJsonAsync<TRequest, TResponse>(
            this HttpClient httpClient,
            string uri,
            bool includeCharset,
            JsonSerializerOptions jsonSerializerOptions,
            TRequest request, 
            CancellationToken cancellationToken
        )
        {
            // Validate parameters.
            if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));
            if (string.IsNullOrWhiteSpace(uri)) throw new ArgumentNullException(nameof(uri));
            if (request == null) throw new ArgumentNullException(nameof(request));

            // Get the message.
            using HttpResponseMessage httpResponseMessage = await httpClient.
                PostJsonForHttpResponseMessageAsync(
                    uri,
                    includeCharset,
                    jsonSerializerOptions,
                    request, 
                    cancellationToken
                )
                .ConfigureAwait(false);

            // Process
            return await httpResponseMessage.ProcessHttpResponseMessageAsync<TResponse>(
                jsonSerializerOptions,
                cancellationToken
            )
            .ConfigureAwait(false);
        }

        public static async Task<TResponse> PostJsonAsync<TResponse>(
            this HttpClient httpClient,
            string uri,
            bool includeCharset,
            JsonSerializerOptions jsonSerializerOptions,
            object request,
            CancellationToken cancellationToken
        )
        {
            // Validate parameters.
            if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));
            if (string.IsNullOrWhiteSpace(uri)) throw new ArgumentNullException(nameof(uri));
            if (request == null) throw new ArgumentNullException(nameof(request));

            // Get the message.
            using HttpResponseMessage httpResponseMessage = await httpClient.
                PostJsonForHttpResponseMessageAsync(
                    uri,
                    includeCharset,
                    jsonSerializerOptions,
                    request,
                    cancellationToken
                )
                .ConfigureAwait(false);

            // Process
            return await httpResponseMessage.ProcessHttpResponseMessageAsync<TResponse>(
                jsonSerializerOptions,
                cancellationToken
            )
            .ConfigureAwait(false);
        }

        private static async Task<TResponse> ProcessHttpResponseMessageAsync<TResponse>(
            this HttpResponseMessage httpResponseMessage,
            JsonSerializerOptions jsonSerializerOptions,
            CancellationToken cancellationToken
        )
        {
            // Ensure the success code.
            httpResponseMessage.EnsureSuccessStatusCode();

            // Return the object.
            return await httpResponseMessage
                .ToObjectAsync<TResponse>(jsonSerializerOptions, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}