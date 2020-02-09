using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Ofl.Net.Http.ApiClient.Json
{
    public abstract class JsonApiClient : ApiClient
    {
        #region Constructor

        protected JsonApiClient(HttpClient httpClient) :
            base(httpClient)
        { }

        #endregion

        #region Overrides.

        protected virtual JsonSerializerOptions CreateJsonSerializerOptions() =>
            JsonSerializerOptionsExtensions.CreateDefaultJsonSerializerOptions();

        protected override async Task<HttpResponseMessage> ProcessHttpResponseMessageAsync(
            HttpResponseMessage httpResponseMessage,
            CancellationToken cancellationToken
        )
        {
            // Validate parameters.
            if (httpResponseMessage == null) throw new ArgumentNullException(nameof(httpResponseMessage));

            // Get the serializer settings.
            JsonSerializerOptions options = CreateJsonSerializerOptions();

            // Call the overload.
            return await ProcessHttpResponseMessageAsync(httpResponseMessage, options, cancellationToken).
                ConfigureAwait(false);
        }

        protected virtual Task<HttpResponseMessage> ProcessHttpResponseMessageAsync(
            HttpResponseMessage httpResponseMessage,
            JsonSerializerOptions jsonSerializerOptions,
            CancellationToken cancellationToken
        )
        {
            // Validate parameters.
            if (httpResponseMessage == null) throw new ArgumentNullException(nameof(httpResponseMessage));
            if (jsonSerializerOptions == null) throw new ArgumentNullException(nameof(jsonSerializerOptions));

            // Ensure the status code.
            httpResponseMessage.EnsureSuccessStatusCode();

            // Return the response.
            return Task.FromResult(httpResponseMessage);
        }

        protected override async Task<TReturn> GetAsync<TResponse, TReturn>(
            string url, 
            Func<HttpResponseMessage, TResponse, TReturn> transformer,
            CancellationToken cancellationToken
        )
        {
            // Validate parameters.
            if (string.IsNullOrWhiteSpace(url)) throw new ArgumentNullException(nameof(url));
            if (transformer == null) throw new ArgumentNullException(nameof(transformer));

            // Format the url.
            url = await FormatUrlAsync(url, cancellationToken).ConfigureAwait(false);

            // Get the options.
            var options = CreateJsonSerializerOptions();

            // Get the response.
            using HttpResponseMessage response = await HttpClient.GetAsync(url, cancellationToken).ConfigureAwait(false);

            // Process the response.
            return await ProcessResponseAsync(
                response, 
                options, 
                transformer,
                cancellationToken
            ).ConfigureAwait(false);
        }

        protected override async Task PostAsync<TRequest>(
            string url, 
            TRequest request,
            CancellationToken cancellationToken
        )
        {
            // Validate parameters.
            if (string.IsNullOrWhiteSpace(url)) throw new ArgumentNullException(nameof(url));
            if (request == null) throw new ArgumentNullException(nameof(request));

            // Format the url.
            url = await FormatUrlAsync(url, cancellationToken).ConfigureAwait(false);

            // Create the serializer.
            var options = CreateJsonSerializerOptions();

            // Get the response.
            using HttpResponseMessage response = await HttpClient.PostJsonForHttpResponseMessageAsync(
                url, options, request, cancellationToken).ConfigureAwait(false);

            // Process the response.
            await ProcessResponseAsync(response, options, cancellationToken).ConfigureAwait(false);
        }

        protected override async Task<TReturn> PostAsync<TRequest, TResponse, TReturn>(
            string url,
            TRequest request,
            Func<HttpResponseMessage, TResponse, TReturn> transformer,
            CancellationToken cancellationToken
        )
        {
            // Validate parameters.
            if (string.IsNullOrWhiteSpace(url)) throw new ArgumentNullException(nameof(url));
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (transformer == null) throw new ArgumentNullException(nameof(transformer));

            // Format the url.
            url = await FormatUrlAsync(url, cancellationToken).ConfigureAwait(false);

            // Create the serializer.
            var options = CreateJsonSerializerOptions();

            // Get the response.
            using HttpResponseMessage response = await HttpClient
                .PostJsonForHttpResponseMessageAsync(
                    url, options, request, cancellationToken
                ).ConfigureAwait(false);

            // Process the response.
            return await ProcessResponseAsync<TResponse, TReturn>(
                response, 
                options, 
                transformer,
                cancellationToken
            ).ConfigureAwait(false);
        }

        protected override async Task<TReturn> DeleteAsync<TResponse, TReturn>(
            string url, 
            Func<HttpResponseMessage, TResponse, TReturn> transformer,
            CancellationToken cancellationToken
        )
        {
            // Validate parameters.
            if (string.IsNullOrWhiteSpace(url)) throw new ArgumentNullException(nameof(url));
            if (transformer == null) throw new ArgumentNullException(nameof(transformer));

            // Format the url.
            url = await FormatUrlAsync(url, cancellationToken).ConfigureAwait(false);

            // Create the serializer.
            var options = CreateJsonSerializerOptions();

            // Get the response.
            using HttpResponseMessage message = await HttpClient.DeleteAsync(url, cancellationToken).ConfigureAwait(false);

            // Process the response.
            TResponse response = await ProcessResponseAsync<TResponse>(
                message, options, cancellationToken).ConfigureAwait(false);

            // Transform and return.
            return transformer(message, response);
        }

        #endregion

        #region Helpers

        private async Task ProcessResponseAsync(
            HttpResponseMessage httpResponseMessage,
            JsonSerializerOptions jsonSerializerOptions, 
            CancellationToken cancellationToken
        )
        {
            // Validate parameters.
            if (httpResponseMessage == null) throw new ArgumentNullException(nameof(httpResponseMessage));
            if (jsonSerializerOptions == null) throw new ArgumentNullException(nameof(jsonSerializerOptions));

            // Process the response.
            using var _ = await 
                ProcessHttpResponseMessageAsync(httpResponseMessage, jsonSerializerOptions, cancellationToken)
                .ConfigureAwait(false);
        }

        protected virtual Task<TResponse> ProcessResponseAsync<TResponse>(
            HttpResponseMessage httpResponseMessage,
            JsonSerializerOptions jsonSerializerOptions,
            CancellationToken cancellationToken
        )
        {
            // Validate parameters.
            if (httpResponseMessage == null) throw new ArgumentNullException(nameof(httpResponseMessage));
            if (jsonSerializerOptions == null) throw new ArgumentNullException(nameof(jsonSerializerOptions));

            // Call the overload.
            return ProcessResponseAsync<TResponse, TResponse>(
                httpResponseMessage,
                jsonSerializerOptions,
                (m, r) => r,
                cancellationToken
            );
        }

        protected virtual async Task<TReturn> ProcessResponseAsync<TResponse, TReturn>(
            HttpResponseMessage httpResponseMessage,
            JsonSerializerOptions jsonSerializerOptions,
            Func<HttpResponseMessage, TResponse, TReturn> transformer,
            CancellationToken cancellationToken
        )
        {
            // Validate parameters.
            if (httpResponseMessage == null) throw new ArgumentNullException(nameof(httpResponseMessage));
            if (jsonSerializerOptions == null) throw new ArgumentNullException(nameof(jsonSerializerOptions));
            if (transformer == null) throw new ArgumentNullException(nameof(transformer));

            // Process the response.
            using HttpResponseMessage message = await
                ProcessHttpResponseMessageAsync(httpResponseMessage, jsonSerializerOptions, cancellationToken)
                .ConfigureAwait(false);

            // Deserialize.
            TResponse response = await message.ToObjectAsync<TResponse>(
                jsonSerializerOptions, 
                cancellationToken
            ).ConfigureAwait(false);

            // Transform.
            return transformer(message, response);
        }

        #endregion
    }
}
