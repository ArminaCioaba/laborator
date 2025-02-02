using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Storage.Queues;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Models1;
using Newtonsoft.Json;

namespace problems.api
{
    public class ProblemsRepository : IProblemsRepository
    {
        private string _connectionString;

        private CloudTableClient _tableClient;

        private CloudTable _problemsTable;

        public ProblemsRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetValue(typeof(string), "AzureStorageAccountConnectionString").ToString();

            Task.Run(async () => { await InitializeTable(); }).GetAwaiter().GetResult();
        }

        public async Task<List<ProblemEntity>> GetAllProblems()
        {
            var problems = new List<ProblemEntity>();

            TableQuery<ProblemEntity> query = new TableQuery<ProblemEntity>(); //.Where(TableQuery.GenerateFilterCondition("FirstName", QueryComparisons.Equal, "Istvan"));

            TableContinuationToken token = null;
            do
            {
                TableQuerySegment<ProblemEntity> resultSegment = await _problemsTable.ExecuteQuerySegmentedAsync(query, token);
                token = resultSegment.ContinuationToken;

                problems.AddRange(resultSegment.Results);

            } while (token != null);

            return problems;
        }

        public async Task<ProblemEntity> GetProblem(string id)
        {
            var parsedId = ParseProblemId(id);

            var partitionKey = parsedId.Item1;
            var rowKey = parsedId.Item2;

            var query = TableOperation.Retrieve<ProblemEntity>(partitionKey, rowKey);

            var result = await _problemsTable.ExecuteAsync(query);

            return (ProblemEntity)result.Result;
        }

        public async Task InsertNewProblem(ProblemEntity user)
        {
            // var insertOperation = TableOperation.Insert(student);

            // await _studentsTable.ExecuteAsync(insertOperation);

            var jsonStudent = JsonConvert.SerializeObject(user);
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(jsonStudent);
            var base64String = System.Convert.ToBase64String(plainTextBytes);

            QueueClient queueClient = new QueueClient(
                _connectionString,
                "problems-queue"
                );
            queueClient.CreateIfNotExists();

            await queueClient.SendMessageAsync(base64String);
        }

        public async Task DeleteProblem(string id)
        {
            var parsedId = ParseProblemId(id);

            var partitionKey = parsedId.Item1;
            var rowKey = parsedId.Item2;

            var entity = new DynamicTableEntity(partitionKey, rowKey) { ETag = "*" };

            await _problemsTable.ExecuteAsync(TableOperation.Delete(entity));
        }

        public async Task EditProblem(ProblemEntity problem)
        {
            var editOperation = TableOperation.Merge(problem);

            // Implemented using optimistic concurrency
            try
            {
                await _problemsTable.ExecuteAsync(editOperation);
            }
            catch (StorageException e)
            {
                if (e.RequestInformation.HttpStatusCode == (int)HttpStatusCode.PreconditionFailed)
                    throw new System.Exception("Entitatea a fost deja modificata. Te rog sa reincarci entitatea!");
            }
        }

        private async Task InitializeTable()
        {
            var account = CloudStorageAccount.Parse(_connectionString);
            _tableClient = account.CreateCloudTableClient();

            _problemsTable = _tableClient.GetTableReference("problemstabel");

            await _problemsTable.CreateIfNotExistsAsync();

        }

        // Used for extracting PartitionKey and RowKey from student id, assuming that id's format is "PartitionKey-RowKey", e.g "UPT-1994014200982"
        private (string, string) ParseProblemId(string id)
        {
            var elements = id.Split('-');

            return (elements[0], elements[1]);
        }
    }
}