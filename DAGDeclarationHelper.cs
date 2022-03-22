using System;
using System.Collections.Generic;
using System.Text;

namespace DAGCreationV1
{
    public class DAGDeclarationHelper
    {
        public string GetDefaultArgs(int year, int month, int day)
        {
            return "default_args = { \n" +
                   "   'start_date': datetime(year = " + year + ", month = " + month + ", day = " + day + ") \n" +
                   "} \n\n";
        }

        public string GetDAGDeclaration(/*int year, int month, int day, */string dagId, string interval, string desc, string catchup, string args, string tags)
        {
            //var defaultArgs = GetDefaultArgs(year, month, day);
            return  "with DAG( \n" +
                    "   dag_id = '" + dagId + "' ,\n" +
                    "   default_args = default_args,\n" +
                    "   schedule_interval = '" + interval + "', \n" +
                    "   description = '" + desc + "', \n"+
                    "   catchup=" + catchup + ", \n" +
                    "   params={" + args + "}, \n" +
                    "   tags=[" + tags + "], \n" +
                    ") as dag: \n\n";
        }
    }
}
