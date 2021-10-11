using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Threading;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace ClApp
{
    public static class QnApp
    {
        [FunctionName("QnApp")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            var constring = Environment.GetEnvironmentVariable("CosmosDbConnectionString");
            //var client = new MongoClient(constring);
            //var db = client.GetDatabase("ClAppStorage");
            //var issues = db.GetCollection<Issue>("Issues");

            log.LogInformation("C# HTTP trigger function processed a request.");

            if (req.Method.ToLower().Equals("get"))
            {
                //var allIssues = await issues.FindAsync(FilterDefinition<Issue>.Empty);
                //var result = allIssues.ToList().Select(i => new {i.CustomerName, i.ProblemDescription});
                var result = $"Hallå eller! Frågan är: ";
                return new OkObjectResult(result);
            }
            else if (req.Method.ToLower().Equals("post"))
            {
                var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                string answer = JsonConvert.DeserializeObject<string>(requestBody);
                if (answer.ToLower() != "blue")
                {
                    return new BadRequestObjectResult("Wrong answer. Please try again.");
                }
              
                var message = $"{answer} is the right answer! Congratulations!";
                return new OkObjectResult(message);
            }
            return new BadRequestObjectResult("Invalid request. Please try again.");
        }
    }
}
