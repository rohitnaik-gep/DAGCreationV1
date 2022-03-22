using System;
using System.Collections.Generic;
using System.Text;

namespace DAGCreationV1
{
    public class DatabricksOperatorHelper
    {
        private string _databricksOperator = "      replace_taskid = DatabricksRunNowOperator( \n" +
                                             "          task_id='replace_taskid', \n" +
                                             //"          dag=replace_dagid, \n" +
                                             "          databricks_conn_id = azureDatabrickConnID, \n" +
                                             "          notebook_params= replace_notebookParams, \n" +
                                             "          job_id=replace_databricksjobid, \n" +
                                             "          email_on_failure=replace_emailonfailure, \n" +
                                             "          do_xcom_push=replace_doxcompush \n" +
                                             "          ) \n \n"; 

        //string taskId, string dagId, string notebookParams, string bricksJobId, string emailAlert, string doXcomPush
        public string GetTemplate(DataBricksOperator bricksOp)
        {
            return _databricksOperator.Replace("replace_taskid", bricksOp.TaskId).Replace("replace_dagid", bricksOp.DAGId).Replace("replace_doxcompush", bricksOp.DoXcomPush)
                .Replace("replace_notebookParams", bricksOp.NotebookParams).Replace("replace_databricksjobid", bricksOp.BricksJobId).Replace("replace_emailonfailure", bricksOp.EmailAlert);
        }
    }
}
