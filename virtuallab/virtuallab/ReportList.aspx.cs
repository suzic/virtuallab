using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using virtuallab.Models;

namespace virtuallab
{
    public partial class ReportList : System.Web.UI.Page
    {
        public LoginUser CurrentLoginUser;

        protected void Page_Init(object sender, EventArgs e)
        {
            CurrentLoginUser = SiteMaster.CurrentLoginUser;
            if (CurrentLoginUser == null)
                Response.Redirect("~/");
            else if (CurrentLoginUser.type == 1)
                Response.Redirect("~/StudentPage");
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}