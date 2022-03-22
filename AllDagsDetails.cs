using System;
using System.Collections.Generic;
using System.Text;

namespace DAGCreationV1
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class ScheduleInterval
    {
        public string __type { get; set; }
        public string value { get; set; }
        public int? days { get; set; }
        public int? microseconds { get; set; }
        public int? seconds { get; set; }
    }

    public class Tag
    {
        public string name { get; set; }
    }

    public class Dag
    {
        public string dag_id { get; set; }
        public string description { get; set; }
        public string file_token { get; set; }
        public string fileloc { get; set; }
        public bool is_active { get; set; }
        public bool is_paused { get; set; }
        public bool is_subdag { get; set; }
        public List<string> owners { get; set; }
        public object root_dag_id { get; set; }
        public ScheduleInterval schedule_interval { get; set; }
        public List<Tag> tags { get; set; }
    }

    public class AllDagsDetails
    {
        public List<Dag> dags { get; set; }
        public int total_entries { get; set; }
    }


}
