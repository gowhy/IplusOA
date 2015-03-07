using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackWebAdmin
{
    public class UEditorReturnModel
    {
        public string state { get; set; }
        public string url { get; set; }
        public string title { get; set; }
        public string original { get; set; }
        public string error { get; set; }

    }

    public class UEditorReturnState
    {
        public static string Success = "SUCCESS";
        public static string Error = "Error";
    }

}