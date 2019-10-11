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
            // 默认是没有选择实验；
            InformationView.ActiveViewIndex = 0;

            // 提供了实验ID
            if (!string.IsNullOrEmpty(CurrentLoginUser.currentExperimentId))
            {
                // 目前除了实验ID=3之外的实验，都还未准备好
                if (!CurrentLoginUser.currentExperimentId.Equals("3"))
                    InformationView.ActiveViewIndex = 1;
                else // CurrentLoginUser.currentSessionId为空，未分配到实验环境
                    InformationView.ActiveViewIndex = 2;
            }
        }

        protected void GotoTaskView(object sender, EventArgs e)
        {
            Response.Redirect("~/StudentPage");
        }
    }
}