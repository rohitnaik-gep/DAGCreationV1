using System;
using System.Collections.Generic;
using System.Text;

namespace DAGCreationV1
{
    public class Conf
    {
        public string fileLocation { get; set; }
        public string dagFileName { get; set; }
    }

    public class Root
    {
        public Root()
        {
            conf = new Conf() { fileLocation = "", dagFileName = "" };
            dag_run_id = "";
        }
        public string dag_run_id { get; set; }
        public Conf conf { get; set; }
    }

    public class Pause
    {
        public bool is_paused { get; set; }
    }

    public class JRoot
    {
        public int fileIndexColumn { get; set; }
        public double invoice_number { get; set; }
        public string invoice_date { get; set; }
        public double supplier_number { get; set; }
        public string supplier_name { get; set; }
        public double invoice_amount { get; set; }
    }


}
