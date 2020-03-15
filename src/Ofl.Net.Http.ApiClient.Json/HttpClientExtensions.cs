using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Ofl.Net.Http.ApiClient.Json
{
    public static partial class HttpClientExtensions
    {
        #region PostJsonAsync

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

        public static Task PostJsonWithoutResponseAsync<TRequest>(
            this HttpClient httpClient,
            string uri,
            JsonSerializerOptions jsonSerializerOptions,
            TRequest request,
            CancellationToken cancellationToken
        ) => httpClient.PostJsonWithoutResponseAsync<TRequest>(
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

            // Ensure the success code.
            httpResponseMessage.EnsureSuccessStatusCode();

            // Return the object.
            return await httpResponseMessage
                .ToObjectAsync<TResponse>(jsonSerializerOptions, cancellationToken)
                .ConfigureAwait(false);
        }

        public static Task<HttpResponseMessage> PostJsonForHttpResponseMessageAsync<TRequest>(
            this HttpClient httpClient,
            string uri,
            TRequest request,
            CancellationToken cancellationToken
        ) => httpClient.PostJsonForHttpResponseMessageAsync(uri, request, cancellationToken);

        public static Task<HttpResponseMessage> PostJsonForHttpResponseMessageAsync<TRequest>(
            this HttpClient httpClient,
            string uri,
            JsonSerializerOptions jsonSerializerOptions,
            TRequest request,
            CancellationToken cancellationToken
        ) => httpClient.PostJsonForHttpResponseMessageAsync<TRequest>(
            uri,
            false,
            jsonSerializerOptions,
            request,
            cancellationToken
        );

        public static async Task<HttpResponseMessage> PostJsonForHttpResponseMessageAsync<TRequest>(
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

            // Get the bytes.
            byte[] bytes = JsonSerializer.SerializeToUtf8Bytes(request, jsonSerializerOptions);

            // Create the byte content.
            using var content = new ByteArrayContent(bytes);

            // Get the content type.
            string contentType = "application/json" + (
                includeCharset ? "; charset=utf-8" : null
            );

            // Set the headers.
            content.Headers.ContentType = MediaTypeHeaderValue.Parse(contentType);

            // Post and return the message.
            return await httpClient.PostAsync(uri, content, cancellationToken).ConfigureAwait(false);
        }

        #endregion
    }
}