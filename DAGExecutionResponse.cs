using System;
using System.Collections.Generic;
using System.Text;

namespace DAGCreationV1
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);    

    public class DAGExecutionResponse
    {
        public Conf conf { get; set; }
        public string dag_id { get; set; }
        public string dag_run_id { get; set; }
        public object end_date { get; set; }
        public DateTime execution_date { get; set; }
        public bool external_trigger { get; set; }
        public DateTime logical_date { get; set; }
        public object start_date { get; set; }
        public string state { get; set; }
    }


}
