using Microsoft.WindowsAzure.Storage.Table;

namespace Models
{
    public class UserEntity:TableEntity
    {
        public UserEntity(string role, string ID)
        {
            this.PartitionKey=role;
            this.RowKey=ID;
        }

        public UserEntity(){ }
        public string FirstName{get;set;}
        public string LastName{get; set;}
        public string Email {get;set;}
        public string Password{get;set;}
        public string PhoneNumber {get;set;}
        public int Points{get;set;}
        public string[] Problems {get; set;}
        public string Location{get; set;}
    }
}