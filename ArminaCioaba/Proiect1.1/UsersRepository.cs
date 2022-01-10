using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Storage.Queues;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Models;
using Newtonsoft.Json;

namespace users.api
{
    public class UsersRepository : IUsersRepository
    {
        private string _connectionString;

        private CloudTableClient _tableClient;

        private CloudTable _usersTable;

        public UsersRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetValue(typeof(string), "AzureStorageAccountConnectionString").ToString();

            Task.Run(async () => { await InitializeTable(); }).GetAwaiter().GetResult();
        }

        public async Task<List<UserEntity>> GetAllUsers()
        {
            var users = new List<UserEntity>();

            TableQuery<UserEntity> query = new TableQuery<UserEntity>(); 
            
            TableContinuationToken token = null;
            do
            {
                TableQuerySegment<UserEntity> resultSegment = await _usersTable.ExecuteQuerySegmentedAsync(query, token);
                token = resultSegment.ContinuationToken;

                users.AddRange(resultSegment.Results);

            } while (token != null);

            return users;
        }

        public async Task<UserEntity> GetUser(string id)
        {
            var parsedId = ParseUserId(id);

            var partitionKey = parsedId.Item1;
            var rowKey = parsedId.Item2;

            var query = TableOperation.Retrieve<UserEntity>(partitionKey, rowKey);

            var result = await _usersTable.ExecuteAsync(query);

            return (UserEntity)result.Result;
        }

        public async Task InsertNewUser(UserEntity user)
        {
             //var insertOperation = TableOperation.Insert(user);

             //await _usersTable.ExecuteAsync(insertOperation);

            var jsonUser = JsonConvert.SerializeObject(user);
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(jsonUser);
            var base64String = System.Convert.ToBase64String(plainTextBytes);

            QueueClient queueClient = new QueueClient(
                _connectionString,
                "users-queue"
                );
            queueClient.CreateIfNotExists();

            await queueClient.SendMessageAsync(base64String);
        }

        public async Task DeleteUser(string id)
        {
            var parsedId = ParseUserId(id);

            var partitionKey = parsedId.Item1;
            var rowKey = parsedId.Item2;

            var entity = new DynamicTableEntity(partitionKey, rowKey) { ETag = "*" };

            await _usersTable.ExecuteAsync(TableOperation.Delete(entity));
        }

        public async Task EditUser(UserEntity user)
        {
            var editOperation = TableOperation.Merge(user);

            // Implemented using optimistic concurrency
            try
            {
                await _usersTable.ExecuteAsync(editOperation);
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

            _usersTable = _tableClient.GetTableReference("userstabel");

            await _usersTable.CreateIfNotExistsAsync();
            await GetAllUsers();

        }

        // Used for extracting PartitionKey and RowKey from student id, assuming that id's format is "PartitionKey-RowKey", e.g "UPT-1994014200982"
        private (string, string) ParseUserId(string id)
        {
            var elements = id.Split('-');

            return (elements[0], elements[1]);
        }
    }
}