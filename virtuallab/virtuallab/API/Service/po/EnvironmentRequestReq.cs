﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace virtuallab.API.Service.po
{
    public class EnvironmentRequestReq
    {
        public int exp_id { get; set; }
        public int user_id { get; set; }

        public int exp_type { get; set; }

    }
}