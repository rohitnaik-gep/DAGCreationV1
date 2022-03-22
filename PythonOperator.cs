using System;
using System.Collections.Generic;
using System.Text;

namespace DAGCreationV1
{
    public class PythonOperator
    {
        string _operatorTemplateWithParams = "task_replace_taskid = PythonOperator( \n" +
            "   task_id='replace_taskid', \n" +
            "   python_callable=replace_method_name, \n" +
            "   op_kwargs=replace_args \n" +
            ") \n \n";

        string _operatorTemplateWithoutParams = "task_replace_taskid = PythonOperator( \n" +
            "   task_id='replace_taskid', \n" +
            "   python_callable=replace_method_name, \n" +
            ") \n \n";

        public string GetTemplateWithArgs(string taskId, string callableMethod, string args)
        {
            return _operatorTemplateWithParams.Replace("replace_taskid", taskId).Replace("replace_method_name", callableMethod).Replace("replace_args", args);
        }

        public string GetTemplateWithoutArgs(string taskId, string callableMethod)
        {
            return _operatorTemplateWithoutParams.Replace("replace_taskid", taskId).Replace("replace_method_name", callableMethod);
        }
    }
}
