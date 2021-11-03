using System;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Threading.Tasks;
using Models;

namespace tema4_console
{
    class Program
    {
        private static CloudTableClient tableClient;
        private static CloudTable studentsTable;
        static void Main(string[] args)
        {
            Task.Run(async ()=> {await Initialize();})
            .GetAwaiter()
            .GetResult();
        }

        static async Task Initialize()
        {
            string storageConnectionString="DefaultEndpointsProtocol=https;"
            + "AccountName=tema4azure"
            + ";AccountKey=fTjCDgICKtG0Q34Cqy33h1ouekzyBGFZVL65r7aDyz/9HQ0Aedhm4sy3XNeMwkHVFxUMBWbzAG2g4DCtRZzqHw=="
            + ";EndpointSuffix=core.windows.net";

            var account=CloudStorageAccount.Parse(storageConnectionString);
            tableClient=account.CreateCloudTableClient();

            studentsTable=tableClient.GetTableReference("studenti");

            await studentsTable.CreateIfNotExistsAsync();

            //await AddNewStudent();
            //await GetAllStudents();
            await EditOneStudent("UPT","1234567891014");
            await GetOneStudent("UPT", "1234567891014");
            await DeleteOneStudent("UPT", "1234567891014");
            await GetAllStudents();

        }
        
        private static async Task AddNewStudent()
        {
            var student =new StudentEntity("UPT", "1234567891014");
            student.FirstName="George";
            student.LastName="Paulescu";
            student.Email="george_paulescu@gmail.com";
            student.PhoneNumber="0723456677";
            student.Faculty="AC";
            student.Year=3;

            var insertOperation=TableOperation.Insert(student);

            await studentsTable.ExecuteAsync(insertOperation);
        }

        private static async Task GetAllStudents()
        {
            Console.WriteLine("University\tCNP\tFirstName\tLastName\tEmail\tPhoneNumber\tFaculty\nYear");
            TableQuery<StudentEntity>query=new TableQuery<StudentEntity>();
            TableContinuationToken token=null;

            do{
                TableQuerySegment<StudentEntity> resultSegment=await studentsTable.ExecuteQuerySegmentedAsync(query,token);
                token=resultSegment.ContinuationToken;
                foreach(StudentEntity entity in resultSegment.Results)
                {
                    Console.WriteLine("{0} | {1} | {2} | {3} | {4} | {5} | {6} | {7}", entity.PartitionKey, entity.RowKey, entity.FirstName, entity.LastName, entity.Email, entity.PhoneNumber, entity.Faculty, entity.Year);
                }
            }while(token!=null);
        }

        private static async Task GetOneStudent(string Partkey, string rwkey)
        {
            Console.WriteLine("University\tCNP\tFirstName\tLastName\tEmail\tPhoneNumber\tFaculty\nYear");
             TableQuery<StudentEntity>query=new TableQuery<StudentEntity>();
            TableContinuationToken token=null;

            do{
                TableQuerySegment<StudentEntity> resultSegment=await studentsTable.ExecuteQuerySegmentedAsync(query,token);
                token=resultSegment.ContinuationToken;
                foreach(StudentEntity entity in resultSegment.Results)
                {
                    if((entity.PartitionKey==Partkey)&(entity.RowKey==rwkey))
                    Console.WriteLine("{0} | {1} | {2} | {3} | {4} | {5} | {6} | {7}", entity.PartitionKey, entity.RowKey, entity.FirstName, entity.LastName, entity.Email, entity.PhoneNumber, entity.Faculty, entity.Year);
                }
            }while(token!=null);
        }
        private static async Task EditOneStudent(string Partkey, string rwkey)
        {
             TableQuery<StudentEntity>query=new TableQuery<StudentEntity>();
            TableContinuationToken token=null;

            do{
                TableQuerySegment<StudentEntity> resultSegment=await studentsTable.ExecuteQuerySegmentedAsync(query,token);
                token=resultSegment.ContinuationToken;
                foreach(StudentEntity entity in resultSegment.Results)
                {
                    if((entity.PartitionKey==Partkey)&(entity.RowKey==rwkey))
                    {
                        entity.Email="george.paulescu@student.upt.ro";
                        entity.PhoneNumber="0712345612";
                        entity.Year=4;
                         var insertOperation=TableOperation.Merge(entity);

                         await studentsTable.ExecuteAsync(insertOperation);
                    }
                   
                }
            }while(token!=null);
        }
        private static async Task DeleteOneStudent(string Partkey, string rwkey)
        {
             TableQuery<StudentEntity>query=new TableQuery<StudentEntity>();
            TableContinuationToken token=null;

            do{
                TableQuerySegment<StudentEntity> resultSegment=await studentsTable.ExecuteQuerySegmentedAsync(query,token);
                token=resultSegment.ContinuationToken;
                foreach(StudentEntity entity in resultSegment.Results)
                {
                    if((entity.PartitionKey==Partkey)&(entity.RowKey==rwkey))
                    {
                        
                         var insertOperation=TableOperation.Delete(entity);

                         await studentsTable.ExecuteAsync(insertOperation);
                    }
                   
                }
            }while(token!=null);
        }
    }
}
