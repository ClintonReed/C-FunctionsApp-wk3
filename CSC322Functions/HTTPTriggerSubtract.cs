using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CSC322Functions
{
    public static class HTTPTriggerSubtract
    {
        [FunctionName("HTTPTriggerSubtract")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            int num1 = Convert.ToInt32(req.Query["num1"]);
            int num2 = Convert.ToInt32(req.Query["num2"]);
            int total = num1 - num2;
            string num3 = num1.ToString();   
            string num4 = num2.ToString();   
            string result = total.ToString();

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            result = result ?? data?.result;
            num3 = result ?? data?.num1;
            num4 = result ?? data?.num2;

            return result != null
                ? (ActionResult)new OkObjectResult($"Enter two numbers you would like to subtract into the query string labeled num1 and num2. You entered {num1} and {num2}. {num1} - {num2} = {total}")
                : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
        }
    }
}
