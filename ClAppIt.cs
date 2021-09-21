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
            log.LogInformation("C# HTTP trigger function processed a request.");

            if (req.Method.ToLower().Equals("get"))
            {
                // hämta alla complaints och skriv ut objektet
                Console.WriteLine("Hej på dig du");

            }
            else if (req.Method.ToLower().Equals("post"))
            {
                // spara ny post med complaints i Db
                Console.WriteLine("Hejdå");
            }

            Console.WriteLine(req.Method);
            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }
    }
}
