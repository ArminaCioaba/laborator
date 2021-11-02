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

            await AddNewStudent();
            //await EditStudent();
            //await GetAllStudents();

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
    }
}
