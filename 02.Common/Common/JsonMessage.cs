using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class JsonMessage
    {
        public JsonMessage(bool status)
            : this(status, string.Empty, new object())
        {

        }
        public JsonMessage(bool status, object data)
            : this(status, string.Empty, data)
        {

        }

        public JsonMessage(bool status, string msg)
            : this(status, msg, new object { })
        {

        }

        public JsonMessage(bool status, string msg, object data)
        {
            Status = status;
            Info = msg;
            Data = data;
        }

        public bool Status { get; set; }
        public string Info { get; set; }
        public object Data { get; set; }
    }
}
