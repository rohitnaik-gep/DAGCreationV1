using System;
using System.Collections.Generic;
using System.Text;

namespace DAGCreationV1
{
    public class NotebookHelper
    {
        private string _template = "@task \n" +
                                    "def notebookReturnCheck(taskToCheck, **context): \n" +
                                    "   databricks_hook = DatabricksHook(databricks_conn_id = azureDatabrickConnID) \n" +
                                    "   ADB_jobRunId = context['ti'].xcom_pull(task_ids = taskToCheck, key = 'run_id') \n" +
                                    "   responds = databricks_hook._do_api_call( \n" +
                                    "       ('GET', f'api/2.0/jobs/runs/get-output?run_id={ADB_jobRunId}'), {} \n" +
                                    "   ) \n" +
                                    "   result = responds['notebook_output']['result'] \n \n" +
                                    "   print(f'result of {taskToCheck} : {result}') \n" +
                                    "   if(result.lower() == 'error') : \n" +
                                    "       raise AirflowFailException('Task %s failed' %taskToCheck) \n \n";

        public string GetNotebookReturnCheckTemplate()
        {
            return _template;
        }

        public string CallNotebookReturn(string taskToCheck)
        {
            return "      task_" + taskToCheck + "ReturnCheck = notebookReturnCheck('" + taskToCheck + "') \n \n";
        }
    }
}
