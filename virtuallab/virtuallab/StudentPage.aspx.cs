using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using virtuallab.Models;

namespace virtuallab
{
    /// <summary>
    /// 学生首页的后台代码
    /// </summary>
    public partial class StudentPage : System.Web.UI.Page
    {
        public LoginUser CurrentLoginUser
        {
            get { return ((SiteMaster)Master).CurrentLoginUser; }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (CurrentLoginUser == null)
                Response.Redirect("~/");
            else if (CurrentLoginUser.type == 0)
                Response.Redirect("~/ManagerPage");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 学生选择任务进入操作
        /// </summary>
        protected void lbEnterTask_Command(object sender, CommandEventArgs e)
        {
            // string expId = e.CommandArgument.ToString();
            int nIndex = ((GridViewRow)((LinkButton)sender).NamingContainer).RowIndex;
            string expId = gvMyTasks.DataKeys[nIndex][0].ToString();
            string taskId = gvMyTasks.DataKeys[nIndex][1].ToString();
            if (string.IsNullOrEmpty(CurrentLoginUser.currentExperimentId)
                || !CurrentLoginUser.currentExperimentId.Equals(expId))
            {
                CurrentLoginUser.currentExperimentId = expId;
                CurrentLoginUser.currentTaskId = taskId;
                CurrentLoginUser.currentSessionId = "";
                CurrentLoginUser.currentCompileId = "";
                CurrentLoginUser.currentUploadId = "";
                CurrentLoginUser.currentRunId = "";

                CurrentLoginUser.device_id = "";
                CurrentLoginUser.app_name = "";
                CurrentLoginUser.currentState = EnvironmentState.NotReady;

                CurrentLoginUser.compileSuccess = false;
                CurrentLoginUser.uploadSuccess = false;
                CurrentLoginUser.playSuccess = false;
            }
            Response.Redirect("~/Environment");
        }
    }
}