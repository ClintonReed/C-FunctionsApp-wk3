using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace csc322functions
{
    public static class HTTPTriggerADD
    {
        [FunctionName("HTTPTriggerADD")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            int num1 = Convert.ToInt32(req.Query["num1"]);
            int num2 = Convert.ToInt32(req.Query["num2"]);
            int total = num1 + num2;
            string num3 = num1.ToString();
            string num4 = num2.ToString();
            string solution = total.ToString();

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            solution = solution ?? data?.solution;
            num3 = solution ?? data?.num1;
            num4 = solution ?? data?.num2;

            return solution != null
                ? (ActionResult)new OkObjectResult($"Enter 2 numbers to the query string labeled num1 and num2 that you would like to add togethor. You entered {num1} and {num2}. {num1} + {num2} = {total}")
                : new BadRequestObjectResult("Please pass two numbers on the query string or in the request body, labeled num1 and num2");
        }
    }
}
