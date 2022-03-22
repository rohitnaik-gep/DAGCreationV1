using System;
using System.Collections.Generic;
using System.Text;

namespace DAGCreationV1
{
    public class DataBricksOperator
    {
        public int SequenceId { get; set; }
        public string TaskId { get; set; }
        public string DAGId { get; set; }
        public string NotebookParams { get; set; }
        public string BricksJobId { get; set; }
        public string EmailAlert { get; set; }
        public string DoXcomPush { get; set; }
        public bool IsNotebookReturnCheck { get; set; }
    }
}
