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
        public static Task<HttpResponseMessage> PostJsonForHttpResponseMessageAsync<TRequest>(
            this HttpClient httpClient,
            string uri,
            TRequest request,
            CancellationToken cancellationToken
        ) => httpClient.PostJsonForHttpResponseMessageAsync(
            uri,
            JsonSerializerOptionsExtensions.DefaultJsonSerializerOptions,
            request,
            cancellationToken
        );

        public static Task<HttpResponseMessage> PostJsonForHttpResponseMessageAsync(
            this HttpClient httpClient,
            string uri,
            object request,
            CancellationToken cancellationToken
        ) => httpClient.PostJsonForHttpResponseMessageAsync(
            uri,
            JsonSerializerOptionsExtensions.DefaultJsonSerializerOptions,
            request,
            cancellationToken
        );

        public static Task<HttpResponseMessage> PostJsonForHttpResponseMessageAsync<TRequest>(
            this HttpClient httpClient,
            string uri,
            JsonSerializerOptions jsonSerializerOptions,
            TRequest request,
            CancellationToken cancellationToken
        ) => httpClient.PostJsonForHttpResponseMessageAsync(
            uri,
            false,
            jsonSerializerOptions,
            request,
            cancellationToken
        );

        public static Task<HttpResponseMessage> PostJsonForHttpResponseMessageAsync(
            this HttpClient httpClient,
            string uri,
            JsonSerializerOptions jsonSerializerOptions,
            object request,
            CancellationToken cancellationToken
        ) => httpClient.PostJsonForHttpResponseMessageAsync(
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

            // Post the bytes.
            return await httpClient.PostBytesAsJsonForHttpResponseMessageAsync(
                uri,
                includeCharset,
                bytes,
                cancellationToken
            )
            .ConfigureAwait(false);
        }

        public static async Task<HttpResponseMessage> PostJsonForHttpResponseMessageAsync(
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

            // Get the bytes.
            byte[] bytes = JsonSerializer.SerializeToUtf8Bytes(request, request.GetType(), jsonSerializerOptions);

            // Post the bytes.
            return await httpClient.PostBytesAsJsonForHttpResponseMessageAsync(
                uri,
                includeCharset,
                bytes,
                cancellationToken
            )
            .ConfigureAwait(false);
        }

        private static async Task<HttpResponseMessage> PostBytesAsJsonForHttpResponseMessageAsync(
            this HttpClient httpClient,
            string uri,
            bool includeCharset,
            byte[] bytes,
            CancellationToken cancellationToken
        )
        {
            // Validate parameters.
            if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));
            if (string.IsNullOrWhiteSpace(uri)) throw new ArgumentNullException(nameof(uri));
            if (bytes == null) throw new ArgumentNullException(nameof(bytes));

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
    }
}