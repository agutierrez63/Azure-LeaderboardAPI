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
    public static class AddScore
    {
        [FunctionName("AddScore")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            [Blob("leaderboardcontainer/leaderboard.json", FileAccess.Read, Connection = "AzureWebJobsStorage")] string leaderboardBlobString,
            [Blob("leaderboardcontainer/leaderboard.json", FileAccess.Write, Connection = "AzureWebJobsStorage")] TextWriter leaderboardBlobTextWriter,
            ILogger log)
        {
            log.LogInformation("AddScore");

            Leaderboard leaderboard = JsonConvert.DeserializeObject<Leaderboard>(leaderboardBlobString);

            string requestBody = new StreamReader(req.Body).ReadToEnd();
            LeaderboardSingle leaderboardSingle = JsonConvert.DeserializeObject<LeaderboardSingle>(requestBody);

            leaderboard.leaderboardSingleList.Add(leaderboardSingle);

            string saveBlobData = JsonConvert.SerializeObject(leaderboard);

            leaderboardBlobTextWriter.Write(saveBlobData);

            return new OkObjectResult(saveBlobData);
        }
    }
}
