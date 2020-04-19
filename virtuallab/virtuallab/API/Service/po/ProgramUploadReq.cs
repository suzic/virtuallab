using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace virtuallab.API.Service.po
{
    public class ProgramUploadReq
    {
        public string session_id { get; set; }
        public string device_id { get; set; }

        public int exp_type { get; set; }

    }
}