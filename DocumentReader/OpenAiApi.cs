using DocumentReader.Models;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DocumentReader
{
    public class OpenAiApi
    {
        public static string BaseUrl = "https://api.openai.com/v1/";

        private string apiKey;
        private HttpClient client;

        public OpenAiApi(string apiKey, HttpClient client)
        {
            this.apiKey = apiKey;
            this.client = client;
        }

        public OpenAiApi(string apiKey)
        {
            this.apiKey = apiKey;
            client = new HttpClient();
        }

        /// <summary>
        /// Will complete based on a completion parameter, usually you want to use a conversation and complete on that
        /// but if you know what you are doing you can use this method directly
        /// </summary>
        /// <param name="completionParameter">The parameter to complete on</param>
        /// <returns>A completion result of the type CompletionResult</returns>
        /// <exception cref="NullReferenceException">If the deserialized completion result is null</exception>
        /// <exception cref="CompletionException">If there was something wrong with completing</exception>
        public async Task<CompletionResult> CompleteAsync(CompletionParameter completionParameter)
        {
            HttpRequestMessage request = CreateAuthenticatedRequestMessage(HttpMethod.Post, BaseUrl + "chat/completions");

            string jsonContent = completionParameter.ToJson();
            request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    CompletionResult? result = JsonSerializer.Deserialize<CompletionResult>(await response.Content.ReadAsStringAsync());

                    if (result == null)
                        throw new NullReferenceException($"Deserialized CompletionResult was unexpectedly null when completing based on the completion parameter {completionParameter}");

                    return result;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable)
                {
                    Thread.Sleep(1000);
                    return await CompleteAsync(completionParameter);
                }

                throw new Exception($"({response.StatusCode}) Error when completing based on the parameter {completionParameter}.\n{await response.Content.ReadAsStringAsync()}");
            }
            catch (TimeoutException)
            {
                throw new Exception("Error when creating an answer, the connection to OpenAi timed out.", null);
            }
        }

        /// <summary>
        /// Will get the embeddings for a text input using a given model (default is text-embedding-ada-002)
        /// </summary>
        /// <param name="textInput">The input to get the embeddings for</param>
        /// <param name="model"></param>
        /// <returns>A GetEmbeddingsResult object</returns>
        public async Task<GetEmbeddingsResult> GetEmbeddingsAsync(string textInput, string model = "text-embedding-ada-002")
        {
            HttpRequestMessage request = CreateAuthenticatedRequestMessage(HttpMethod.Post, BaseUrl + "embeddings");

            string json = JsonSerializer.Serialize(new { input = textInput, model });
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            return GetEmbeddingsResult.FromJson(await (await client.SendAsync(request)).Content.ReadAsStringAsync()).SetInputText(textInput);
        }

        private HttpRequestMessage CreateAuthenticatedRequestMessage(HttpMethod method, string url)
        {
            HttpRequestMessage request = new HttpRequestMessage(method, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            return request;
        }
    }
}
