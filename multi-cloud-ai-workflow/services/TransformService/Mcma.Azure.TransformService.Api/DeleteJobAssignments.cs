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
    public static class DeleteJobAssignments
    {
        [FunctionName("DeleteJobAssignments")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "job-assignments")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("DeleteJobAssignments triggered.");

            // Do something here...

            return (ActionResult) new OkObjectResult($"");
        }
    }
}
