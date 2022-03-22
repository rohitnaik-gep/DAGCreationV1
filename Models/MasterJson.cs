using System;
using System.Collections.Generic;
using System.Text;

namespace DAGCreationV1.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class ConnectionDetails
    {
    }

    public class DagNode
    {
        public int nodeId { get; set; }
        public int prevNodeId { get; set; }
        public int nextNodeId { get; set; }
        public string type { get; set; }
        public string name { get; set; }
        public ConnectionDetails connectionDetails { get; set; }
        public string operation { get; set; }
        public List<string> columns { get; set; }
        public string joinType { get; set; }
        public int? leftDataSourceNodeId { get; set; }
        public string leftDataSourceField { get; set; }
        public int? rightDataSourceNodeId { get; set; }
        public string rightDataSourceField { get; set; }
    }

    public class Operator
    {
        public int order { get; set; }
        public string task_id { get; set; }
        public string python_callable { get; set; }
        public string op_kwargs { get; set; }
    }

    public class MasterJson
    {
        public List<DagNode> dagNodes { get; set; }
        public List<Operator> operators { get; set; }
    }


}
