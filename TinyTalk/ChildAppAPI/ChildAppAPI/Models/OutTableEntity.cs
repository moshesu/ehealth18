using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChildAppAPI.Models
{
    class OutTableEntity : TableEntity
    {
        public string PartitionKey;
        public string RowKey;
        public string Timestamp;
        public string TimeStamp;
        public string Message;
        public string Message_Send;
        public string Message_Recive;

        public OutTableEntity() { }
    }
}