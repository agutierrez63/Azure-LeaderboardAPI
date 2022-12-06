using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;

namespace LeaderboardFunction
{
    public static class AddScoreNonFunc
    {
        [FunctionName("AddScoreNonFunc")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("AddScore");

            string connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");

            string containerName = "leaderboardcontainer";

            BlobContainerClient blobContainerClient = new BlobContainerClient(connectionString, containerName);
            BlobClient blobClient = blobContainerClient.GetBlobClient("leaderboard.json");

            Leaderboard leaderboard;

            BlobDownloadInfo blobDownloadInfo = blobClient.Download();
            using (StreamReader streamReader = new StreamReader(blobDownloadInfo.Content))
            {
                leaderboard = JsonConvert.DeserializeObject<Leaderboard>(streamReader.ReadToEnd());
            }

            leaderboard.leaderboardSingleList.Add(new LeaderboardSingle
            {
                name = "JohnDoe",
                score = 100
            });

            string saveBlobData = JsonConvert.SerializeObject(leaderboard);
            MemoryStream memoryStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(saveBlobData));
            var blobClientInfo = blobClient.Upload(memoryStream, true);
            memoryStream.Dispose();

            string responseMessage = blobClientInfo.GetRawResponse().ToString();

            return new OkObjectResult(responseMessage);
        }
    }
}
