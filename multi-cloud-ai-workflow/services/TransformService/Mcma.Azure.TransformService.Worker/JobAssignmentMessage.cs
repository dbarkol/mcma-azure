using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Mcma.Azure.TransformService.Worker
{
    public static class JobAssignmentMessage
    {
        [FunctionName("JobAssignmentMessage")]
        public static void Run([ServiceBusTrigger("%QueueName%", Connection = "ServiceBusConnectionString")]string myQueueItem, 
            ILogger log)
        {
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
        }
    }
}
