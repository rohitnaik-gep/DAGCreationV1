using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DAGCreationV1
{
    public class AirflowRESTApiHelper
    {
        string _dagsBaseUrl = "http://44.198.85.30:8080/api/v1/dags/";
        //public void CallAPI()
        //{
        //    HttpClient client = new HttpClient();
        //    client.BaseAddress = new Uri(dagRunsUrl);

        //    // Add an Accept header for JSON format.
        //    client.DefaultRequestHeaders.Accept.Add(
        //    new MediaTypeWithQualityHeaderValue("application/json"));

        //    // List data response.
        //    //client.po
        //    HttpResponseMessage response = client.GetAsync(dagsParams).Result;  // Blocking call! Program will wait here until a response is received or a timeout occurs.
        //    if (response.IsSuccessStatusCode)
        //    {
        //        var res = response.Content.ReadAsStringAsync().Result;
        //        // Parse the response body.
        //        //var dataObjects = response.Content.ReadAsAsync<IEnumerable<DataObject>>().Result;  //Make sure to add a reference to System.Net.Http.Formatting.dll
        //        //foreach (var d in dataObjects)
        //        //{
        //        Console.WriteLine(res);
        //        //}
        //    }
        //    else
        //    {
        //        Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
        //    }

        //    // Make any other calls using HttpClient here.

        //    // Dispose once all HttpClient calls are complete. This is not necessary if the containing object will be disposed of; for example in this case the HttpClient instance will be disposed automatically when the application terminates so the following call is superfluous.
        //    client.Dispose();
        //}

        //public async Task CallAPI2()
        //{
        //    HttpClient client = new HttpClient();
        //    client.DefaultRequestHeaders.Accept.Clear();
        //    client.DefaultRequestHeaders.Accept.Add(
        //        new MediaTypeWithQualityHeaderValue("application/json"));

        //    client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

        //    var stringTask = client.GetStringAsync("http://3.93.147.139:8080/api/v1/dags/sftp_hooks_workflow/dagRuns");

        //    var msg = await stringTask;
        //    Console.Write(msg);
        //}

        //public void CurlReq()
        //{
        //    var url = "http://3.93.147.139:8080/api/v1/dags/sftp_operators_workflow/dagRuns";
        //    var postReqBody = new Root() { conf = new Conf() { fileLocation = "/users/sdtftp4usr1/GeneratedDAGs/UAP_110322.py", dagFileName = "UAP_110322.py" } };
        //    byte[] byte1 = new ASCIIEncoding().GetBytes(JsonConvert.SerializeObject(postReqBody));


        //    var httpReq = (HttpWebRequest)WebRequest.Create(url);
        //    httpReq.ContentType = "application/json";
        //    httpReq.Accept = "application/json";
        //    httpReq.Headers["Authorization"] = "Basic YWRtaW46YWRtaW4=";
        //    httpReq.Method = "POST";
        //    //httpReq.AddParameter("application/json", postReqBody, ParameterType.RequestBody);

        //    httpReq.ContentLength = byte1.Length;
        //    var newStream = httpReq.GetRequestStream(); // get a ref to the request body so it can be modified
        //    newStream.Write(byte1, 0, byte1.Length);
        //    newStream.Close();

        //    //YWlyZmxvdzphaXJmbG93 = base64 string

        //    var httpResp = (HttpWebResponse)httpReq.GetResponse();

        //    using (var streamReader = new StreamReader(httpResp.GetResponseStream()))
        //    {
        //        var res = streamReader.ReadToEnd();
        //    }
        //}

        public async Task MoveWorkflowDAG(string fileLocation, string fileName, string moverDagName)
        {
            var client = new RestClient(_dagsBaseUrl + moverDagName + "/dagRuns");
            RestRequest request = new RestRequest();
            request.Method = Method.Post;
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", "Basic YWRtaW46YWRtaW4=");
            request.AddHeader("Cookie", "session=60037990-6dfa-4272-832a-d284b20761b6.gDG8h-i7VSOK-5riS-vGm3Aacbc");            
            var postReqBody = new Root() { conf = new Conf() { fileLocation = fileLocation, dagFileName = fileName }, dag_run_id = "" };
            request.AddJsonBody<Root>(postReqBody);
            var response = await client.ExecuteAsync(request);

            //Console.WriteLine(response.Content);
        }

        public async Task<DAGExecutionResponse> UnPauseWorkflowDAG(string workflowDagName)
        {
            var client = new RestClient(_dagsBaseUrl + workflowDagName);

            RestRequest request = new RestRequest();
            request.Method = Method.Patch;
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", "Basic YWRtaW46YWRtaW4=");
            request.AddHeader("Cookie", "session=60037990-6dfa-4272-832a-d284b20761b6.gDG8h-i7VSOK-5riS-vGm3Aacbc");
            //var postReqBody = new Root() { conf = new Conf() { fileLocation = "/users/sdtftp4usr1/GeneratedDAGs/UAP_110322.py", dagFileName = "UAP_110322.py" }, dag_run_id = "" };
            request.AddJsonBody<Pause>(new Pause() { is_paused = false });
            var response = await client.ExecuteAsync(request);

            var dagResp = JsonConvert.DeserializeObject<DAGExecutionResponse>(response.Content);
            return dagResp;
            //Console.WriteLine(response.Content);
        }

        public async Task<DAGExecutionResponse> ExecuteWorkflowDAG(string workflowDagName)
        {
            var client = new RestClient(_dagsBaseUrl + workflowDagName + "/dagRuns");

            RestRequest request = new RestRequest();
            request.Method = Method.Post;
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", "Basic YWRtaW46YWRtaW4=");
            request.AddHeader("Cookie", "session=60037990-6dfa-4272-832a-d284b20761b6.gDG8h-i7VSOK-5riS-vGm3Aacbc");
            //var postReqBody = new Root() { conf = new Conf() { fileLocation = "/users/sdtftp4usr1/GeneratedDAGs/UAP_110322.py", dagFileName = "UAP_110322.py" }, dag_run_id = "" };
            request.AddJsonBody<Root>(new Root());
            var response = await client.ExecuteAsync(request);

            var dagResp = JsonConvert.DeserializeObject<DAGExecutionResponse>(response.Content);
            return dagResp;
            //Console.WriteLine(response.Content);
        }

        public async Task<DAGExecutionResponse> CheckWorkflowDAGStatus(string workflowDagId, string workflowDagRunId)
        {
            var client = new RestClient(_dagsBaseUrl + workflowDagId + "/dagRuns/" + workflowDagRunId);

            RestRequest request = new RestRequest();
            request.Method = Method.Get;
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", "Basic YWRtaW46YWRtaW4=");
            request.AddHeader("Cookie", "session=60037990-6dfa-4272-832a-d284b20761b6.gDG8h-i7VSOK-5riS-vGm3Aacbc");
            var response = await client.ExecuteAsync(request);

            var dagResp = JsonConvert.DeserializeObject<DAGExecutionResponse>(response.Content);
            return dagResp;
        }

        public async Task GetAllDags()
        {
            var client = new RestClient(_dagsBaseUrl.TrimEnd('/'));

            RestRequest request = new RestRequest();
            request.Method = Method.Get;
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", "Basic YWRtaW46YWRtaW4=");
            request.AddHeader("Cookie", "session=60037990-6dfa-4272-832a-d284b20761b6.gDG8h-i7VSOK-5riS-vGm3Aacbc");
            var response = await client.ExecuteAsync(request);

            var dagResp = JsonConvert.DeserializeObject<AllDagsDetails>(response.Content);
        }

        public async Task<Dag> GetDagInfo(string dagId)
        {
            var client = new RestClient(_dagsBaseUrl + dagId);
            Dag dagInfo = new Dag();
            RestRequest request = new RestRequest();
            request.Method = Method.Get;
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", "Basic YWRtaW46YWRtaW4=");
            request.AddHeader("Cookie", "session=60037990-6dfa-4272-832a-d284b20761b6.gDG8h-i7VSOK-5riS-vGm3Aacbc");
            var response = await client.ExecuteAsync(request);

            if (response.Content.Contains("DAG not found"))
            {
                Thread.Sleep(5000);
                await GetDagInfo(dagId);//waiting for DAG deployment
            }
            else
                dagInfo = JsonConvert.DeserializeObject<Dag>(response.Content);                
            return dagInfo;
        }

        public async Task<DagRuns> GetDagRuns(string dagId)
        {
            //http://54.197.22.184:8080/api/v1/dags/UAP_1/dagRuns
            var client = new RestClient(_dagsBaseUrl + dagId + "/dagRuns");
            var dagInfo = new DagRuns();
            RestRequest request = new RestRequest();
            request.Method = Method.Get;
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", "Basic YWRtaW46YWRtaW4=");
            request.AddHeader("Cookie", "session=60037990-6dfa-4272-832a-d284b20761b6.gDG8h-i7VSOK-5riS-vGm3Aacbc");

            var response = await client.ExecuteAsync(request);

            if (response.Content.Contains("DAG not found"))
            {
                Thread.Sleep(5000);
                await GetDagInfo(dagId);//waiting for DAG deployment
            }
            else
                dagInfo = JsonConvert.DeserializeObject<DagRuns>(response.Content);
            return dagInfo;
        }


    }
}