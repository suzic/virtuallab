using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using virtuallab.Models;

namespace virtuallab
{
    public partial class StudentPage : System.Web.UI.Page
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
            CurrentLoginUser = SiteMaster.CurrentLoginUser;
        }

        protected void lbEnterTask_Command(object sender, CommandEventArgs e)
        {
            string expId = e.CommandArgument.ToString();
            if (string.IsNullOrEmpty(CurrentLoginUser.currentExperimentId)
                || !CurrentLoginUser.currentExperimentId.Equals(expId))
            {
                CurrentLoginUser.currentExperimentId = expId;
                CurrentLoginUser.currentTaskId = "";
                CurrentLoginUser.currentSessionId = "";
                CurrentLoginUser.currentCompileId = "";
                CurrentLoginUser.currentUploadId = "";
                CurrentLoginUser.currentRunId = "";
                CurrentLoginUser.compileSuccess = false;
                CurrentLoginUser.uploadSuccess = false;
                CurrentLoginUser.playSuccess = false;
            }
            Response.Redirect("~/Environment");
        }
    }
}