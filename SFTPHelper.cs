using Renci.SshNet;
using Renci.SshNet.Sftp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DAGCreationV1
{
    public class SFTPHelper
    {

        public void CreateProcessedErrorDirectories(string directorySuccess, string directoryFailed, SftpClient sftp)
        {
            try
            {
                if (!sftp.Exists(directorySuccess))
                    sftp.CreateDirectory(directorySuccess);

                if (!sftp.Exists(directoryFailed))
                    sftp.CreateDirectory(directoryFailed);
            }
            catch (Exception ex)
            {
                //SpendLoggerHelper.LogError(ex, "CreateProcessedErrorDirectories", partnerCode, partnerCode, "SFTPHelper");
                throw;
            }
        }

        public bool MoveFileToSFTP(Stream stream, string destinationPath, SFTPConnectionDetails connDetails)
        {
            try
            {
                using (SftpClient client = new SftpClient(connDetails.Host, connDetails.Port, connDetails.UserName, connDetails.Password))
                {
                    client.Connect();
                    client.UploadFile(stream, destinationPath, true);
                }
            }
            catch (Exception ex)
            {
                return false;
            }            
            return true;
        }
    }

    public class SFTPConnectionDetails
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
