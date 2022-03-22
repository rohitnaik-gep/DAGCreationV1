using Microsoft.Rest;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DAGCreationV1
{
    public class DataLakeHelper
    {
        private string applicationIdValue;
        private string authenticationKeyValue;
        private string storageName;
        private string tenantIDValue;

        public bool MoveFileToDatalake(string fileSystem, string folderPath, string fileName, Stream fileStream)
        {
            bool isFileCopied = false;
            try
            {
                CreateClient(applicationIdValue, authenticationKeyValue, tenantIDValue, storageName);
                //DLStorageManagementClient dcl = new DLStorageManagementClient();
                var isFileCreated = CreateFileAsync(fileSystem, folderPath, fileName, fileStream);
                if (isFileCreated.Result.IsSuccessStatusCode)
                    isFileCopied = true;
                else
                {
                    isFileCopied = false;
                    throw new Exception("MoveFileToDataLake Method Create file Failed as -" + isFileCreated.Result.StatusMessage);
                }
            }
            catch (Exception ex)
            {
                isFileCopied = false;
                throw ex;
            }
            return isFileCopied;
        }

        public static HttpClient CreateClient(string applicationId, string secretKey, string tenantId, string accountName)
        {
            Client = new HttpClient();
            StorageAccountName = accountName;
            TokenProvider tp = new TokenProvider(applicationId, secretKey, tenantId);
            Client.DefaultRequestHeaders.Authorization = tp.GetAuthenticationHeaderAsync().Result;
            string dt = DateTime.UtcNow.ToString("R", CultureInfo.InvariantCulture);
            var clientRequestId = Guid.NewGuid().ToString();
            Client.DefaultRequestHeaders.Add("x-ms-date", dt);
            Client.DefaultRequestHeaders.Add("x-ms-version", "2018-11-09");
            Client.DefaultRequestHeaders.Add("x-ms-client-request-id", clientRequestId);
            return Client;
        }


        /// <summary>
        /// Credentials needed for the client to connect to Azure.
        /// </summary>
        public ServiceClientCredentials Credentials { get; private set; }

        /// <summary>
        /// StorageAccountName needed for the client to connect to Azure Storage Account.
        /// </summary>
        public static string StorageAccountName { get; set; }

        public int Timeout { get; set; }

        public static HttpClient Client { get; set; }
        public DataLakeHelper()
        {
            //if (credentials == null)
            //{
            //    throw new System.ArgumentNullException("credentials");
            //}
            //this.Credentials = credentials;
            //if (this.Credentials != null)
            //{
            //    this.Credentials.InitializeServiceClient(this);
            //}
            this.Timeout = 10000;
        }

        public async Task<OperationResult> CreateFilesystemAsync(string filesystem)
        {
            var resourceUrl = $"https://{StorageAccountName}.dfs.core.windows.net/{filesystem}?resource=filesystem&timeout={this.Timeout}";
            var response = await Client.PutAsync(resourceUrl, null);
            return new OperationResult { IsSuccessStatusCode = response.IsSuccessStatusCode, StatusMessage = response.ReasonPhrase };
        }

        public async Task<OperationResult> DeleteFilesystemAsync(string filesystem)
        {
            var resourceUrl = $"https://{StorageAccountName}.dfs.core.windows.net/{filesystem}?resource=filesystem&timeout={this.Timeout}";
            var response = await Client.DeleteAsync(resourceUrl);
            return new OperationResult { IsSuccessStatusCode = response.IsSuccessStatusCode, StatusMessage = response.ReasonPhrase };
        }

        public async Task<OperationResult> CreateFileSystemAsync(string filesystem)
        {
            var resourceUrl = $"https://{StorageAccountName}.dfs.core.windows.net/{filesystem}?resource=directory&timeout={this.Timeout}";
            Uri uri = new Uri(resourceUrl);
            var response = Client.PutAsync(uri, null).Result;
            return new OperationResult { IsSuccessStatusCode = response.IsSuccessStatusCode, StatusMessage = response.ReasonPhrase };
        }
        public async Task<OperationResult> CreateDirectoryAsync(string filesystem, string path)
        {
            var resourceUrl = $"https://{StorageAccountName}.dfs.core.windows.net/{filesystem}/{path}?resource=directory&timeout={this.Timeout}";
            Uri uri = new Uri(resourceUrl);
            var response = Client.PutAsync(uri, null).Result;
            return new OperationResult { IsSuccessStatusCode = response.IsSuccessStatusCode, StatusMessage = response.ReasonPhrase };
        }

        public async Task<OperationResult> CreateEmptyFileAsync(string filesystem, string path, string fileName)
        {
            var resourceUrl = $"https://{StorageAccountName}.dfs.core.windows.net/{filesystem}/{path}/{fileName}?resource=file&timeout={this.Timeout}";
            using (var tmpContent = new StreamContent(new MemoryStream()))
            {
                HttpRequestMessage newFileMsg = new HttpRequestMessage(HttpMethod.Put, resourceUrl);
                newFileMsg.Content = tmpContent;
                var response = Client.SendAsync(newFileMsg).Result;
                return new OperationResult { IsSuccessStatusCode = response.IsSuccessStatusCode, StatusMessage = response.ReasonPhrase };
            }
        }

        public async Task<OperationResult> RenameFileOrFolderAsync(string filesystem, string path, string fileName, string newFileName)
        {
            var newResourceUrl = $"https://{StorageAccountName}.dfs.core.windows.net/{filesystem}/{path}/{newFileName}";
            var resourceUrl = $"/{filesystem}/{path}/{fileName}";
            Client.DefaultRequestHeaders.Add("x-ms-rename-source", resourceUrl);
            Uri uri = new Uri(newResourceUrl);
            var response = Client.PutAsync(uri, null).Result;
            Client.DefaultRequestHeaders.Remove("x-ms-rename-source");
            return new OperationResult { IsSuccessStatusCode = response.IsSuccessStatusCode, StatusMessage = response.ReasonPhrase };
        }

        public async Task<OperationResult> CreateFileAsync(string filesystem, string path, string fileName, Stream stream)
        {
            var operationResult = await this.CreateEmptyFileAsync(filesystem, path, fileName);
            if (operationResult.IsSuccessStatusCode)
            {
                using (var streamContent = new StreamContent(stream))
                {
                    //upload to the file buffer
                    var resourceUrl = $"https://{StorageAccountName}.dfs.core.windows.net/{filesystem}/{path}/{fileName}?action=append&timeout={this.Timeout}&position=0";
                    HttpRequestMessage msg = new HttpRequestMessage(new HttpMethod("PATCH"), resourceUrl);
                    msg.Content = streamContent;
                    var response = Client.SendAsync(msg).Result;

                    //flush the buffer to commit the file
                    var flushUrl = $"https://{StorageAccountName}.dfs.core.windows.net/{filesystem}/{path}/{fileName}?action=flush&timeout={this.Timeout}&position={msg.Content.Headers.ContentLength}";
                    HttpRequestMessage flushMsg = new HttpRequestMessage(new HttpMethod("PATCH"), flushUrl);
                    response = Client.SendAsync(flushMsg).Result;

                    return new OperationResult { IsSuccessStatusCode = response.IsSuccessStatusCode, StatusMessage = response.ReasonPhrase };
                }
            }
            else
            {
                return operationResult;
            }
        }

        public async Task<OperationResult> RenameAndMoveFileAsync(string filesystem, string path, string fileName, Stream stream, bool isRename)
        {
            if (isRename)
            {
                string timeTicker = DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss");
                string newFilename = string.Format("{0}_{1}.{2}", fileName.Split('.')[0], timeTicker, fileName.Split('.')[1]);
                var renameResult = await this.RenameFileOrFolderAsync(filesystem, path, fileName, newFilename);
            }

            return await this.CreateFileAsync(filesystem, path, fileName, stream);
        }

        public OperationResult DeleteFileOrDirectory(string filesystem, string path, bool recursive = false)
        {
            //delete the file
            var resourceUrl = $"https://{StorageAccountName}.dfs.core.windows.net/{filesystem}/{path}?recursive={recursive}&timeout={this.Timeout}";
            var response = Client.DeleteAsync(resourceUrl).Result;
            return new OperationResult { IsSuccessStatusCode = response.IsSuccessStatusCode, StatusMessage = response.ReasonPhrase };
        }

        public async Task<OperationResult> DownloadFileAsync(string filesystem, string path, Stream streamToSave)
        {
            //delete the file
            var resourceUrl = $"https://{StorageAccountName}.dfs.core.windows.net/{filesystem}/{path}?timeout={this.Timeout}";
            var response = Client.GetAsync(resourceUrl).Result;

            streamToSave.Seek(0, SeekOrigin.Begin);
            await response.Content.CopyToAsync(streamToSave);

            return new OperationResult { IsSuccessStatusCode = response.IsSuccessStatusCode, StatusMessage = response.ReasonPhrase };
        }

        public async Task<OperationResult> DownloadFileAsyncUrl(string url, Stream streamToSave)
        {
            var resourceUrl = $"{url}?timeout={this.Timeout}";
            var response = Client.GetAsync(resourceUrl).Result;

            streamToSave.Seek(0, SeekOrigin.Begin);
            await response.Content.CopyToAsync(streamToSave);

            return new OperationResult { IsSuccessStatusCode = response.IsSuccessStatusCode, StatusMessage = response.ReasonPhrase };
        }

        public async Task<Stream> DownloadFileStream(string url)
        {
            var resourceUrl = $"{url}?timeout={this.Timeout}";
            var response = Client.GetAsync(resourceUrl).Result;
            return await response.Content.ReadAsStreamAsync();
        }
    }

    public class OperationResult
    {

        public bool IsSuccessStatusCode { get; set; }

        public string StatusMessage { get; set; }

    }
}
