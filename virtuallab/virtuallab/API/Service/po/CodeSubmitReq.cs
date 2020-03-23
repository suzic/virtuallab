using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace virtuallab.API.Service.po
{
    public class CodeSubmitReq
    {
        public string session_id { get; set; }
        public int part { get; set; }
        public List<CodeFile> code { get; set; }

    }
}