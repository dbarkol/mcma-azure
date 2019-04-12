
# Worker

This Worker sample is responsible for processing a unit of work. It is invoked when a new message is placed on a queue. The sample also demonstrates how to communicate with a Cosmos DB collection.

Trigger: Azure Service Bus queue

Sample local.settings.json:
```json
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet",
    "QueueName": "{{service-bus-queue-name}}",
    "ServiceBusConnectionString": "{{{service-bus-connection-string}}",
    "DocumentDbAuthKey": "{{cosmos-db-auth-key}}",
    "DocumentDbCollectionId": "jobCollection",
    "DocumentDbEndpoint": "https://{{cosmos-db-name}}.documents.azure.com:443/",
    "DocumentDbDatabaseId": "jobDatabase"
  }
}
```

# API 

The API project demonstrates how to handle HTTP requests with an Azure Function. It is triggered each time a HTTP request is received. The sample also showcases how to place a message on a Service Bus queue using an output binding. 

Trigger: HTTP

Output binding: Azure Service Bus queue

Sample local.settings.json:
```json
{
    "IsEncrypted": false,
    "Values": {
        "AzureWebJobsStorage": "UseDevelopmentStorage=true",
        "FUNCTIONS_WORKER_RUNTIME": "dotnet",
        "ServiceBusConnectionString": "{{service-bus-connection-string}}",
        "QueueName": "{{service-bus-queue-name}}"
  }
}
```