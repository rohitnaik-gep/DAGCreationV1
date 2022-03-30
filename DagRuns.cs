using System;
using System.Collections.Generic;
using System.Text;

namespace DAGCreationV1
{

    public class DagRun
    {
        public Conf conf { get; set; }
        public string dag_id { get; set; }
        public string dag_run_id { get; set; }
        public object end_date { get; set; }
        public DateTime execution_date { get; set; }
        public bool external_trigger { get; set; }
        public DateTime logical_date { get; set; }
        public DateTime start_date { get; set; }
        public string state { get; set; }
    }

    public class DagRuns
    {
        public List<DagRun> dag_runs { get; set; }
        public int total_entries { get; set; }
    }


}
