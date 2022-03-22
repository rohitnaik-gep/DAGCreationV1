using System;
using System.Collections.Generic;
using System.Text;

namespace DAGCreationV1
{
    public class EmailOperatorHelper
    {
        private string _emailTemplate = "      email = EmailOperator( \n" +
                                        "          task_id='{0}', \n " +
                                        "          to='{1}', \n " +
                                        "          subject='{2}', \n " +
                                        "          html_content='{3}' \n" +
                                        "          ) \n \n \n";

        public string GetEmailOpTemplate(string taskId, string emailTo, string subject, string htmlTemplate)
        {
            return string.Format(_emailTemplate, taskId, emailTo, subject, htmlTemplate);
        }
    }
}
