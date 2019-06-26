using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage.Table;

namespace Azure_DevOps_Build_Release_Status.Tables
{
    public class DevOps : TableEntity
    {
        public DevOps()
        {
        }

        public DevOps(int ID, string ParttionKey)
        {
            this.RowKey = ID.ToString();
            this.PartitionKey = ParttionKey;
        }

        public string finishtime { get; set; }
        public string lastchangeddate { get; set; }
        public string type { get; set; }
        public string projectname { get; set; }
        public string number { get; set; }
        public string starttime { get; set; }
        public string result { get; set; }
        public string requestedby { get; set; }
        public string status { get; set; }
        public string definitionid { get; set; }
        public string definitionname { get; set; }
        public string qeueuedon { get; set; }
        public string environment { get; set; }
        public string releasepipeline { get; set; }

    }
}