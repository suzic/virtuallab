using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace virtuallab.API.Service.po
{
    public class RunResultTickRes
    {
        public int fail { get; set; }

        public List<one_effect> output { get; set; }

    }
}