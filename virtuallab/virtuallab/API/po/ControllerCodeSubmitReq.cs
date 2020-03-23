using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using virtuallab.API.Service.po;

namespace virtuallab.API.po
{
    public class ControllerCodeSubmitReq
    {
        public int fid_task { get; set; }
        public string session_id { get; set; }
        public int part { get; set; }
        public List<CodeFile> code { get; set; }

    }
}