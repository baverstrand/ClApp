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

//using Microsoft.Azure.Documents;
//using Microsoft.Azure.Documents.Client;

namespace ClApp
{
    public static class ClAppIt
    {
        [FunctionName("ClAppIt")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            var constring = Environment.GetEnvironmentVariable("CosmosDbConnectionString");
            var client = new MongoClient(constring);
            var db = client.GetDatabase("ClAppStorage");
            var issues = db.GetCollection<Issue>("Issues");

            log.LogInformation("C# HTTP trigger function processed a request.");

            if (req.Method.ToLower().Equals("get"))
            {
                var allIssues = await issues.FindAsync(FilterDefinition<Issue>.Empty);
                var result = allIssues.ToList().Select(i => new {i.CustomerName, i.ProblemDescription});
                return new OkObjectResult(result);
            }
            else if (req.Method.ToLower().Equals("post"))
            {
                var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var issue = JsonConvert.DeserializeObject<Issue>(requestBody);

                await issues.InsertOneAsync(issue);
                return new CreatedResult("", issue.customerName);
            }

            return new OkObjectResult(null);
        }

        public class Issue
        {
            [BsonIgnoreIfDefault]
            [BsonRepresentation(BsonType.ObjectId)]
            public string _id { get; set; }
            //[JsonProperty("customerId")]
            public int CustomerId { get; set; }
            //[JsonProperty("customerName")]
            public string CustomerName { get; set; }
            //[JsonProperty("problemDescription")]
            public string ProblemDescription { get; set; }
        }

        public static Issue CreateIssue(int customerId, string customerName, string problemDescription) =>
            new Issue
            {
                CustomerId = customerId, 
                CustomerName = customerName, 
                ProblemDescription = problemDescription
            };
    }
}
