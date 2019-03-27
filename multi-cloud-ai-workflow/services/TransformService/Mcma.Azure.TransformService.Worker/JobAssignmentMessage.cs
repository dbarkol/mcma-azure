using System;
using System.Threading.Tasks;
using Mcma.Common.Core.Model;
using Mcma.Common.Repositories;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Mcma.Azure.TransformService.Worker
{
    public static class JobAssignmentMessage
    {
        [FunctionName("JobAssignmentMessage")]
        public static async Task Run([ServiceBusTrigger("%QueueName%", Connection = "ServiceBusConnectionString")]string queueItem, 
            ILogger log)
        {
            log.LogInformation($"JobAssignmentMessage queue triggered.");

            // Deserialize the message into the request object.
            var msg = JsonConvert.DeserializeObject<JobAssignmentRequest>(queueItem);

            // Add the job assignment 
            if (msg.ActionType == JobAssignmentRequestType.AddJobAssignment)
            {
                var repository = new JobAssignmentRepository();
                var jobAssignment = new JobAssignment() { Description = msg.Details, Id = Guid.NewGuid().ToString() };
                await repository.CreateJobAssignment(jobAssignment);
            }

        }

        
    }
}
