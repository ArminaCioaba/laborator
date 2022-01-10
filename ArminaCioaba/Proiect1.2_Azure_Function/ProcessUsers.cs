using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Models;

namespace Company.Function
{
    public static class ProcessUsers
    {
        [Function("ProcessUsers")]
        [TableOutput("userstabel")]
        public static UserEntity Run([QueueTrigger("users-queue", Connection = "datcproiect_STORAGE")] string myQueueItem,
            FunctionContext context)
        {
            var logger = context.GetLogger("ProcessUsers");
            logger.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
            var user =JsonConvert.DeserializeObject<UserEntity>(myQueueItem);

            return user;
        }
        
    }
}
