using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace virtuallab.API.Service.po
{
    public class CodeSubmitReq
    {
        public string session_id { get; set; }
        public string code_name { get; set; }
        public string code { get; set; }

    }
}