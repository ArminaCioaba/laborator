using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Models;

namespace Company.Function
{
    public static class CreateStudents
    {
        [Function("CreateStudents")]
        [TableOutput("studenti")]
        public static StudentEntity Run([QueueTrigger("students-queue", Connection = "tema4azure_STORAGE")] string myQueueItem,
            FunctionContext context)
        {
            var logger = context.GetLogger("CreateStudents");
            logger.LogInformation($"C# Queue trigger function processed: {myQueueItem}");

            var student =JsonConvert.DeserializeObject<StudentEntity>(myQueueItem);

            return student;
        }
    }
}
