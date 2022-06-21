using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;

namespace CallApi
{
    class Program
    {
        static async Task Main()
        {
            const string baseUrl = "https://www.shop.madeiraurbana.com.br/api/v1/products/";
            const string authToken = "53c9c1ff5612f12a5068e0a564c67370a2b2ed20";

            using var client = HttpClientFactory.Create();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var data = ReadJsonData();
            for (int i = 0; i < data.Count; i++)
            {
                Console.WriteLine($"Posting item {i}...");

                HttpResponseMessage response = await HttpClientExtensions.PostAsJsonAsync(client, baseUrl, data[i]);
                response.EnsureSuccessStatusCode();
            }
        }

        static IReadOnlyList<dynamic> ReadJsonData()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "CallApi.Resources.data.json";

            using var stream = assembly.GetManifestResourceStream(resourceName);
            using var reader = new StreamReader(stream);
            string fileContent = reader.ReadToEnd();

            dynamic data = JsonConvert.DeserializeObject<List<dynamic>>(fileContent.ToString());
            return data.AsReadOnly();
        }
    }
}
