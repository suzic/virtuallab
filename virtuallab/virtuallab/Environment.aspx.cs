﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using virtuallab.Models;

namespace virtuallab
{
    public partial class Environment : System.Web.UI.Page
    {
        public LoginUser CurrentLoginUser;

        protected void Page_Init(object sender, EventArgs e)
        {
            CurrentLoginUser = SiteMaster.CurrentLoginUser;
            if (CurrentLoginUser == null)
                Response.Redirect("~/");
            else if (CurrentLoginUser.type == 0)
                Response.Redirect("~/ManagerPage");
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void Func(object sender, EventArgs e)
        {

        }
    }
}