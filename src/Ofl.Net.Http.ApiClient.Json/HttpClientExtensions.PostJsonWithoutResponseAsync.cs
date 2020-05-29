using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Ofl.Net.Http.ApiClient.Json
{
    public static partial class HttpClientExtensions
    {
        public static Task PostJsonWithoutResponseAsync<TRequest>(
            this HttpClient httpClient,
            string uri,
            TRequest request,
            CancellationToken cancellationToken
        ) => httpClient.PostJsonWithoutResponseAsync(
            uri, 
            false,
            request, 
            cancellationToken
        );

        public static Task PostJsonWithoutResponseAsync(
            this HttpClient httpClient,
            string uri,
            object request,
            CancellationToken cancellationToken
        ) => httpClient.PostJsonWithoutResponseAsync(
            uri,
            false,
            request,
            cancellationToken
        );

        public static Task PostJsonWithoutResponseAsync<TRequest>(
            this HttpClient httpClient,
            string uri,
            bool includeCharset,
            TRequest request,
            CancellationToken cancellationToken
        ) => httpClient.PostJsonWithoutResponseAsync(
            uri,
            includeCharset,
            JsonSerializerOptionsExtensions.DefaultJsonSerializerOptions,
            request,
            cancellationToken
        );

        public static Task PostJsonWithoutResponseAsync(
            this HttpClient httpClient,
            string uri,
            bool includeCharset,
            object request,
            CancellationToken cancellationToken
        ) => httpClient.PostJsonWithoutResponseAsync(
            uri,
            includeCharset,
            JsonSerializerOptionsExtensions.DefaultJsonSerializerOptions,
            request,
            cancellationToken
        );

        public static Task PostJsonWithoutResponseAsync<TRequest>(
            this HttpClient httpClient,
            string uri,
            JsonSerializerOptions jsonSerializerOptions,
            TRequest request,
            CancellationToken cancellationToken
        ) => httpClient.PostJsonWithoutResponseAsync(
            uri,
            false,
            jsonSerializerOptions,
            request,
            cancellationToken
        );

        public static Task PostJsonWithoutResponseAsync(
            this HttpClient httpClient,
            string uri,
            JsonSerializerOptions jsonSerializerOptions,
            object request,
            CancellationToken cancellationToken
        ) => httpClient.PostJsonWithoutResponseAsync(
            uri,
            false,
            jsonSerializerOptions,
            request,
            cancellationToken
        );

        public static async Task PostJsonWithoutResponseAsync<TRequest>(
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
            if (jsonSerializerOptions == null) throw new ArgumentNullException(nameof(jsonSerializerOptions));
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

            // Ensure the success code.
            httpResponseMessage.EnsureSuccessStatusCode();
        }

        public static async Task PostJsonWithoutResponseAsync(
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
            if (jsonSerializerOptions == null) throw new ArgumentNullException(nameof(jsonSerializerOptions));
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

            // Ensure the success code.
            httpResponseMessage.EnsureSuccessStatusCode();
        }
    }
}