using System;
using System.Collections.Generic;
using System.Text;

namespace DAGCreationV1
{
    public class ImportStatementsHelper
    {
        private string _basicImports =  /*"import json \n"+
                                        "import requests  \n" +
                                        "import pandas as pd  \n" + */
                                        "from airflow.models import DAG  \n" +
                                        "from airflow.operators.python import PythonOperator  \n" +
                                        "from airflow.operators.email import EmailOperator \n" +
                                        "from airflow.providers.databricks.operators.databricks import DatabricksSubmitRunOperator \n" +
                                        "from airflow.contrib.operators.databricks_operator import DatabricksRunNowOperator \n" +
                                        "from airflow.decorators import task \n" +
                                        "from airflow.providers.databricks.hooks.databricks import DatabricksHook \n" +
                                        "from airflow.exceptions import AirflowFailException \n" +
                                        "from datetime import datetime,timedelta \n \n \n";
        public string GetBasicImportStatements()
        {
            return _basicImports;
        }

        public string GetSpecificImportStatements(string importType)
        {
            return string.Empty;
        }
    }
}
