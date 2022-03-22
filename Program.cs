using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DAGCreationV1
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var workflowDagId = "UAP_Rohit_20";
            var generatedDagInfo = new Program().WriteBricksDAG(workflowDagId);
            var airflowHelper = new AirflowRESTApiHelper();

            await airflowHelper.MoveWorkflowDAG(generatedDagInfo.DagFileLocation, generatedDagInfo.DagFileName, "sftp_operators_workflow");

            var dagInfo = await airflowHelper.GetDagInfo(workflowDagId);

            var pauseResp = airflowHelper.UnPauseWorkflowDAG(workflowDagId);

            //var dagResp = await airflowHelper.ExecuteWorkflowDAG(workflowDagId);

            Thread.Sleep(30000);

            var dagStatus = await airflowHelper.CheckWorkflowDAGStatus(workflowDagId, pauseResp.Result.dag_run_id);

            Console.WriteLine(JsonConvert.SerializeObject(dagStatus));
            Console.ReadKey();
            //check DAG run status

        }

        //public async Task Do()
        //{
            //string moverDagId = "sftp_operators_workflow";
          
            //await new AirflowRESTApiHelper().MoveWorkflowDAG();
        //}

        public void WritePythonDAGFile()
        {
            var declHelper = new DAGDeclarationHelper();
            var pyOperator = new PythonOperator();
            var bricksOperator = new DatabricksOperatorHelper();
            var imports = new ImportStatementsHelper().GetBasicImportStatements();
            var defaultArgs = declHelper.GetDefaultArgs(2022, 03, 7);
            var tags = new List<string>() { "UAP", "ExternalDataLoad", "Cleansing", "AI_Publish" };
            var dagDecl = declHelper.GetDAGDeclaration("etl_users2", "@daily", "extract and load users", "False", "", string.Join(",", tags.Select(r => "'" + r + "'")));

            var operator1 = pyOperator.GetTemplateWithoutArgs("extract_users", "_extract_users");
            var param1 = "{'path': '/home/airflow/airflow/data/users.csv'}";
            var operator2 = pyOperator.GetTemplateWithArgs("load_users", "_load_users", param1);

            var notebookParams = "{ 'location': 'fileInfo / 132785250691065455 / load / PIPE QC.json' }";
            //var bricksOperator1 = bricksOperator.GetTemplate("ExternalDataLoad", "etl_users2", notebookParams, "13473", "False", "True");

            var taskSequenceList = new List<string>() { "extract_users", "load_users" };
            var taskSeq = string.Join(" >> ", taskSequenceList);
            var finalString = imports + defaultArgs + dagDecl + operator1 + operator2 + taskSeq;

            var dagPath = @"D:\Handy Docs\Airflow related\";
            var fileName = "etl_users.py";
            File.WriteAllText(dagPath + fileName, finalString);
        }

        public DagInfo WriteBricksDAG(string dagId)
        {
            var declHelper = new DAGDeclarationHelper();
            var bricksOperatorHelper = new DatabricksOperatorHelper();
            var bricksOperators = new List<DataBricksOperator>();
            var notebookHelper = new NotebookHelper();

            //1
            var imports = new ImportStatementsHelper().GetBasicImportStatements();

            //2
            var notebookReturnCheck = notebookHelper.GetNotebookReturnCheckTemplate();

            //3
            var defaultArgs = declHelper.GetDefaultArgs(2022, 03, 7);

            //4
            var tags = new List<string>() { "UAP", "ExternalDataLoad", "Cleansing", "AI_Publish" }; //what are these
            var dagDecl = declHelper.GetDAGDeclaration(dagId, "@daily", "desc", "False", "", string.Join(",", tags.Select(r => "'" + r + "'")));

            bricksOperators = FillBricksDetails();

            //5
            StringBuilder bricksSB = new StringBuilder();
            foreach (var op in bricksOperators)
            {
                bricksSB.Append(bricksOperatorHelper.GetTemplate(op));
                if (op.IsNotebookReturnCheck)
                    bricksSB.Append(notebookHelper.CallNotebookReturn(op.TaskId));
            }

            //6
            var emailOp = new EmailOperatorHelper().GetEmailOpTemplate("send_email", "rohit.naik@gep.com", "DAG execution complete: " + dagId,
                "<h3>UAP dataset pipeline completed</h3>");

            //7
            var taskSequenceList = new List<string>() { "ExternalDataLoad", "task_ExternalDataLoadReturnCheck", "DataCleaning_FeatureGeneration",
                "task_DataCleaning_FeatureGenerationReturnCheck", "AI_Training_And_Publish_Dataset", "task_AI_Training_And_Publish_DatasetReturnCheck", "email" };
            var taskSeq = string.Join(" >> ", taskSequenceList) + "\n \n";
            var finalString = imports + "azureDatabrickConnID = 'AzureDatabricksUAP' \n \n" + notebookReturnCheck + defaultArgs + dagDecl + bricksSB.ToString() +
                emailOp + taskSeq + "if __name__ == '__main__': \n" + " dag.cli()";

            var dagPath = @"D:\Handy Docs\Airflow related\";
            var fileName = dagId + ".py";
            File.WriteAllText(dagPath + fileName, finalString);

            var destinationSFTPPath = "/users/sdtftp4usr1/GeneratedDAGs/" + dagId + ".py";
            var connDetails = new SFTPConnectionDetails() { Host = "ftp4.gep.com", UserName = "sdtftp4usr1@spenddevtesting", Password = "Hr#mnHr624Ga", Port = 2380 };

            using (var stream = FileHelper.GenerateStreamFromString(finalString))
            {
                new SFTPHelper().MoveFileToSFTP(stream, destinationSFTPPath, connDetails);
            }

            var wfDAGInfo = new DagInfo() { DagFileName = fileName, DagFileLocation = destinationSFTPPath };

            return wfDAGInfo;

            /* move dag file to ftp location
             * trigger a dag which will only move the main dag to dags folder
             * trigger main dag
             */
        }

        public List<DataBricksOperator> FillBricksDetails()
        {
            var bricksOperators = new List<DataBricksOperator>() { new DataBricksOperator() {
                //DAGId = "myDAG",
                TaskId = "ExternalDataLoad",
                BricksJobId = "46454",
                NotebookParams = "{ 'csvFilePath': '/mnt/197250/users/sambit/Logistics_Data/Logistics_Import' }",
                EmailAlert = "True",
                DoXcomPush = "True",
                IsNotebookReturnCheck = true
            },
            new DataBricksOperator(){
                //DAGId = "myDAG",
                TaskId = "DataCleaning_FeatureGeneration",
                BricksJobId = "47574",
                NotebookParams = "{}",
                EmailAlert = "True",
                DoXcomPush = "True",
                IsNotebookReturnCheck = true
            },
            new DataBricksOperator()
            {
                //DAGId = "myDAG",
                TaskId = "AI_Training_And_Publish_Dataset",
                BricksJobId = "46538",
                NotebookParams = "{}",
                EmailAlert = "True",
                DoXcomPush = "True",
                IsNotebookReturnCheck = true
            } };

            return bricksOperators;
        }
    }

    public class DagInfo
    {
        public string DagFileName { get; set; }
        public string DagFileLocation { get; set; }
    }
}
