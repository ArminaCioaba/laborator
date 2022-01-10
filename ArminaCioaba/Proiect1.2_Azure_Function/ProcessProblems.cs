using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Models1;

namespace Company.Function
{
    public static class ProcessProblems
    {
        [Function("ProcessProblems")]
        [TableOutput("problemstabel")]
        public static ProblemEntity Run([QueueTrigger("problems-queue", Connection = "datcproiect_STORAGE")] string myQueueItem,
            FunctionContext context)
        {
            var logger = context.GetLogger("ProcessProblems");
            logger.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
            var problem =JsonConvert.DeserializeObject<ProblemEntity>(myQueueItem);

            return problem;
        }
        
    }
}
