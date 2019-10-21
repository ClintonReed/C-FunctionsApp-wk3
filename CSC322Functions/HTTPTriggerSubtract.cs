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

            // Values

            int num1 = Convert.ToInt32(req.Query["num1"]);  // num1 input from user
            int num2 = Convert.ToInt32(req.Query["num2"]);  // num2 input from user
            int total = num1 - num2;                        // subtracts num1 from num2 and returns the value as an INT
            string num3 = num1.ToString();                  // num3 - converts num1 from an INT to a STRING
            string num4 = num2.ToString();                  // num4 - converts num2 from an INT to a STRING
            string result = total.ToString();               // result - converts total from an INT to a STRING

            // Operation

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();     // voodoo mumbo jumbo - has something to do with the reqbody?
            dynamic data = JsonConvert.DeserializeObject(requestBody);                  // converts something using Json to the request body?
            result = result ?? data?.result;                // requests the result string data and prints the message
            num3 = result ?? data?.num1;                    // requests the num1 input and returns it as num3 to the result string
            num4 = result ?? data?.num2;                    // requests the num2 input and returns it as num4 to the result string

            // Output

            return result != null
                ? (ActionResult)new OkObjectResult($"Enter two numbers you would like to subtract into the query string labeled num1 and num2. You entered {num1} and {num2}. {num1} - {num2} = {total}") // final page output message
                : new BadRequestObjectResult("Please pass a name on the query string or in the request body");   // should display when no inputs are entered on page load, but doesn't. Maybe has something to do with the required requests of strings on page load?
        }
    }
}
