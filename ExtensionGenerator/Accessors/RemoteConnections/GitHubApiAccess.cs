
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ExtensionGenerator.Accessors.RemoteConnections
{

    public class GitHubApiService
    {
        
        private static readonly HttpClient httpClient = new HttpClient();

        public static async Task<List<Commit>> GetLatestCommitsAsync(string fullRepositoryUrlWithCommitsPath)
        {
            try
            {
                string apiUrl = fullRepositoryUrlWithCommitsPath;
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    var latestCommits = JsonConvert.DeserializeObject<List<Commit>>(json);
                    return latestCommits;
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }

            return null;
        }
    }
}
