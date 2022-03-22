using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace DAGCreationV1
{
    public class DockerHelper
    {
        public void ExecuteDockerCommand()
        {
            //docker cp "C:\Dev\poc\Airflow\click.py" 50f946893fdb:/usr/local/lib/python3.8/dist-packages/airflow/example_dags/Click.py
            var processInfo = new ProcessStartInfo("docker", $"run -it --rm blahblahblah");
        }
    }
}
