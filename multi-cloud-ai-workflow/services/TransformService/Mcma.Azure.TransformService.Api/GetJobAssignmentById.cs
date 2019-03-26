using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Mcma.Azure.TransformService.Api
{
    public static class GetJobAssignmentById
    {
        [FunctionName("GetJobAssignmentById")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "job-assignments/{id}")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("GetJobAssignmentsById triggered.");

            // TODO: Do something...

            return (ActionResult)new OkObjectResult($"");
        }
    }
}