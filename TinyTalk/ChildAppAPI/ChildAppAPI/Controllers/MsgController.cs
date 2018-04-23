using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using ChildAppAPI.Models;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;

namespace ChildAppAPI.Controllers
{

    public class MsgController : ApiController
    {
        //noy added need to check
        [HttpGet]
        //public  async Task<TableQuerySegment<OutTableEntity>> Get_msg([FromUri]string email)
        public async Task<bool> Get_msg([FromUri]string email)
        {
            // Retrieve the storage account from the connection string.
            //CloudStorageAccount storageAccount = CloudStorageAccount.Parse(Microsoft.WindowsAzure.CloudConfigurationManager.GetSetting("https://functionqueuecob099.table.core.windows.net/outTable"));

            var storageAccount = CloudStorageAccount.Parse("functionqueuecob099_" +
            "https://functionqueuecob099.table.core.windows.net/outTable");
            // Create the table client.
            var tableClient = storageAccount.CreateCloudTableClient();
            // Get a reference to a table named "outTable"
            CloudTable table = tableClient.GetTableReference("outTable");

            // Construct the query operation for all customer entities where PartitionKey="Smith".
            TableQuery<OutTableEntity> query = new TableQuery<OutTableEntity>().Where(TableQuery.GenerateFilterCondition("Message_Recive", QueryComparisons.Equal, email));

            // Initialize the continuation token to null to start from the beginning of the table.
            TableContinuationToken continuationToken = null;
            TableQuerySegment<OutTableEntity> tableQueryResult = null;
            do
            {
                // Retrieve a segment (up to 1,000 entities).
                tableQueryResult = await table.ExecuteQuerySegmentedAsync(query, continuationToken);

                //ExecuteQuerySegmentedAsync
                // Assign the new continuation token to tell the service where to
                // continue on the next iteration (or null if it has reached the end).
                continuationToken = tableQueryResult.ContinuationToken;

                // Print the number of rows retrieved.
                Console.WriteLine("Rows retrieved {0}", tableQueryResult.Results.Count);

                // Loop until a null continuation token is received, indicating the end of the table.
            }
            while (continuationToken != null);
            return true;

        }
        /*
            [HttpGet]
        public static async Task<bool> GetMsgAsync1()
        {
            // Retrieve the storage account from the connection string.
            //DefaultEndpointsProtocol=https;AccountName=functionqueuecob099;AccountKey=0/gynCIsiLI5IxzrQUAgAUtPiAzVrfetdrrXCvHPWM972iT9fdA5JbBqgZkbJIY37AgzMiwMpdBdB/Jcvu+6aQ==
            Microsoft.WindowsAzure.Storage.CloudStorageAccount storageAccount = Microsoft.WindowsAzure.Storage.CloudStorageAccount.Parse(
               CloudConfigurationManager.GetSetting("https://functionqueuecob099.table.core.windows.net/outTable"));

            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Create the CloudTable object that represents the "people" table.
            CloudTable table = tableClient.GetTableReference("outTable");

            // Construct the query operation for all customer entities where PartitionKey="Smith".
            TableQuery<OutTableEntity> query = new TableQuery<OutTableEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "Smith"));

            // Print the fields for each customer.
            foreach (OutTableEntity entity in table.ExecuteQuery(query))
            {
                Console.WriteLine("{0}, {1}\t{2}\t{3}", entity.PartitionKey, entity.RowKey);
            }
            return true;
        }
    }
    */
    }
}