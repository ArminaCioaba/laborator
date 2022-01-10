using Microsoft.WindowsAzure.Storage.Table;

namespace Models1
{
    public class ProblemEntity:TableEntity
    {
        public ProblemEntity(string ID_user, string ID_problem)
        {
            this.PartitionKey=ID_user;
            this.RowKey=ID_problem;
        }

        public ProblemEntity(){ }
        public string Title{get;set;}
        public string Description{get; set;}
        public string Location {get;set;}
        public string Status{get;set;}
        public string Severity {get;set;}
        public int Category {get;set;}
    }
}