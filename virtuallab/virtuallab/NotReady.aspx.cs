using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using virtuallab.Models;

namespace virtuallab
{
    public partial class NotReady : System.Web.UI.Page
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
            // 默认是没有选择实验；但如果仍然提供了实验ID，说明实验还未准备好
            if (!string.IsNullOrEmpty(CurrentLoginUser.currentExperiment))
                InformationView.ActiveViewIndex = 1;
        }

        protected void GotoTaskView(object sender, EventArgs e)
        {
            Response.Redirect("~/StudentPage");
        }
    }
}