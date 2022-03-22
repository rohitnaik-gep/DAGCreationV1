using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using WinSCP;

namespace DAGCreationV1
{
    public class WinSCPHelper
    {
        //scp -i "../Airflow_POC_Sambit.pem" first_dag_new.py ubuntu@ec2-3-93-147-139.compute-1.amazonaws.com:/home/airflow/airflow/dags

        public int MoveFile()
        {
            try
            {
                // Setup session options
                SessionOptions sessionOptions = new SessionOptions
                {
                    Protocol = Protocol.Scp,
                    HostName = "ubuntu@ec2-3-93-147-139.compute-1.amazonaws.com",
                    //HostName = "ip-172-31-224-95.ec2.internal",
                    //UserName = "ubuntu",
                    //Password = "mypassword",
                    PortNumber = 22,
                    //SshHostKeyPolicy = SshHostKeyPolicy.GiveUpSecurityAndAcceptAny,
                    //SshHostKeyFingerprint = "ssh-rsa 2048 xxxxxxxxxxx...=",
                    //be:6b:ee:e1:51:33:ce:e3:21:7d:f4:2e:d6:8a:73:4a:12:41:b4:3e
                    //nSmqDkzG5xb8CqHOuLW2EDoOFynCqYVZZVatz1omkDk=
                    SshHostKeyFingerprint = "ssh-rsa 2048 MIIEvQIBADANBgkqhkiG9w0BAQEFAASCBKcwggSjAgEAAoIBAQCyVZ89P2fxWRNWBY1fO73/wr0ayuPYU1qsHrrbwykOYQUPAFQcrFf0IzOGNLdcdoE9sU8dU9QzR72R8fFuQ/RLxU6BM61cILbJCPI23R0vIxDnWorE+/l6JtAXhVJNN1BkJvOZSaVO0kmTuvkOObeU3rKQlLn6w8qvRfy0S4PtCiFtH/mIM+0IJ/Gx1ZzVhJu/x5M+epJTFeKoSFddPfcRfYsagazwaJvaj0sgbVKqXNZ7JLZfNuhW3/WeGPlMu+Cs5Sezi+GTh4KWaYX1DPFHWlMvk2YIXI+73/P4nu7JV2f5oRSxw7ytMEZ9OJFOyfsL1DAT415fu1wm0Epr9ivBAgMBAAECggEAdMTbn58E0GMi5hB8oP7dSbqZWBHBXsQ1er4kXAFNwLyGM2xjpuKyNIlYE2gxJ7nwphSTWQCq0WkIF6f0Dd8w9uwamz7bJJfHF5tj/ciKc3LifpoqgxD6KaGAX+ZMHYM6gFmA573xH8tRogmrSkVAj5nCEilQHYJWFK4e6W8I0wvXReoPQdnM2RcW8d4zIWgDVeeLcmMTymh8AS25ZLozRKOrc1j88TLkMMa8UZytDSDsViZOoLjtv3JphW56+7Ny0fyBAtl3Zj1hfzfdGbE4j3AP0wwqcrjBNidtbFi1vwne3lNe2wZVWzvEzZjyVQdLhY7z9IgSfnLwIZ2FX+luAQKBgQDXBfXjacA1M+I+nY36EyNA9aaJJt48JpdIz0sSC8G4hzEZlzd9e4T+tRkaARmFMN+Uty1JerW7LQ59wWGZRkK7SSvxc6u0hGq1PyoefYR2ZIfa2z4DgXn36OVgNwHdbpG8xEhYi9qYxLO3/CsRbukbWwz/1VQcxYQtMcUaUGFgTQKBgQDUUcZ//6mn3yFVGhJYHmyW76wCHEvuDTVyds65Ff9xIuI0qylxo5DTVzjViPAo27h5wM0EPHcpxcjS+KLBUyE+jmTpIPuCZOUDMg+zV2BIGiHjyfQKJVCf8amc/hfONMjtNjE8KFyJ1zQDeKqczu0Wl/5l3fyZ84aDBcL8MpeTRQKBgCavhNjAevtXdVoMoLGgZdRXHfpJCuzJiKhp2zjI8raPZC8VfL9PzNLCpFyAXCYRb/hiqHwy3qVpatUHSNb5xCHL/WL4i6jztfsb/Sj1LjLazXJ7xYF99wK6XSJdYzGCpifPLLD5oH+hzC/K57jAOqJYVFNm/zYZ9zjmXm7ojUx1AoGAINCWvle0T/bDfxSteyMQo29dhWH8NkmiVhOYtbB5r3G1xnAh0qd2DcPwS8iDXb0/0MmEpj/2JuWjuT+mX/zKKZH2dLWdSbTwVEMbfBtoDRzE2iDvb9X+lQnTrijzYvJzj34Ns/+E9eTl7BdtPxkYOaK6NeJOVTj3NC7iyPnExYUCgYEAqPru0WJw8mrhC/CV2fKvb/F1jPjW3j/Gv/XOoseI/sLSAt6pW3yThWI0zxQp9GCRwz1DAmovdRpD0psM9gXVwp8lQPNoueiWgVeEYiu4pMOR5/Uinvpfubk2RTm31vXI7VfDzoLJIRdy+m+hiJ5mdIgIdKJNbUSP8+KuKQZ9xVU=",
                    SshPrivateKeyPath = @"D:\Airflow\Airflow_POC_Sambit.pem"
                    //SshPrivateKeyPath = @"D:\Airflow\Airflow_POC_Sambit.ppk"

                };

                using (Session session = new Session())
                {
                    // Connect
                    session.Open(sessionOptions);

                    // Upload files
                    TransferOptions transferOptions = new TransferOptions();
                    transferOptions.TransferMode = TransferMode.Binary;

                    var bytes = File.ReadAllBytes(@"D:\Airflow\SB_bricks.py");
                    Stream fs = new MemoryStream(bytes);

                    //TransferOperationResult transferResult;
                    //transferResult =

                    session.PutFile(fs, "/home/user/", transferOptions);

                    // Throw on any error
                    //transferResult.Check();

                    // Print results
                    //foreach (TransferEventArgs transfer in transferResult.Transfers)
                    //{
                    //    Console.WriteLine("Upload of {0} succeeded", transfer.FileName);
                    //}
                }

                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex);
                return 1;
            }
        }
    }
}
