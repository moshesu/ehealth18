
using Microsoft.WindowsAzure.Storage.Table;

namespace HBApp
{
    public class AzureTableEntity : TableEntity
    {
            public AzureTableEntity(string valueString, string date_day, string date_hr)
        {
            this.PartitionKey = date_day;
            this.RowKey = date_hr;
            //this.Value = valueString;
        }

        public AzureTableEntity() { }

        public string Value { get; set; }
        

    }
}