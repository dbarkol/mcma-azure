using System;
using System.IO;
using System.Threading.Tasks;
using Mcma.Common.Core.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.ServiceBus.Core;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Mcma.Azure.TransformService.Api
{
    public static class AddJobAssignment
    {
        [FunctionName("AddJobAssignment")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "job-assignments")] HttpRequest req,
            [ServiceBus("%QueueName%", Connection = "ServiceBusConnectionString")] IAsyncCollector<string> messages,
            ILogger log)
        {
            log.LogInformation("AddJobAssignment triggered.");

            // Retrieve the message body
            var requestBody = new StreamReader(req.Body).ReadToEnd();
            if (string.IsNullOrEmpty(requestBody))
            {
                return new BadRequestObjectResult("Not a valid request");
            }

            // Create a job assignment request and send it to the 
            // queue for further processing. 
            var msg = new JobAssignmentRequest
            {
                ActionType = JobAssignmentRequestType.AddJobAssignment,
                Details = requestBody

            };
            await messages.AddAsync(JsonConvert.SerializeObject(msg));
            
            return (ActionResult)new OkObjectResult($"");
        }
    }
}