using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using virtuallab.Models;

namespace virtuallab
{
    public partial class _Default : Page
    {
        public LoginUser CurrentLoginUser;

        protected void Page_Init(object sender, EventArgs e)
        {
            CurrentLoginUser = SiteMaster.CurrentLoginUser;
            if (CurrentLoginUser == null)
            {
                FuncMain.Text = "登录使用 &raquo;";
                FuncExperi.Text = "登录查看 &raquo;";
                FuncScore.Text = "登录查看 &raquo;";
            }
            else
            {
                if (CurrentLoginUser.type == 0)
                {
                    FuncMain.Text = "进入管理 &raquo;";
                    FuncExperi.Text = "实验管理 &raquo;";
                    FuncScore.Text = "成绩报告 &raquo;";
                }
                else if (CurrentLoginUser.type == 1)
                {
                    FuncMain.Text = "进入实验 &raquo;";
                    FuncExperi.Text = "我的实验 &raquo;";
                    FuncScore.Text = "查看成绩 &raquo;";
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void FirstTask(object sender, EventArgs e)
        {
            if (CurrentLoginUser == null)
                Response.Redirect("~/Account/Login");
            else
            {
                if (CurrentLoginUser.type == 0)
                    Response.Redirect("~/ManagerPage");
                else if (CurrentLoginUser.type == 1)
                    Response.Redirect("~/StudentPage");
            }
        }

        protected void ExperiTask(object sender, EventArgs e)
        {
            if (CurrentLoginUser == null)
                Response.Redirect("~/Account/Login");
            else
            {
                if (CurrentLoginUser.type == 0)
                    Response.Redirect("~/ManagerPage");
                else if (CurrentLoginUser.type == 1)
                    Response.Redirect("~/StudentPage");
            }
        }

        protected void ScoreTask(object sender, EventArgs e)
        {
            if (CurrentLoginUser == null)
                Response.Redirect("~/Account/Login");
            else
            {
                if (CurrentLoginUser.type == 0)
                    Response.Redirect("~/ReportList");
                else if (CurrentLoginUser.type == 1)
                    Response.Redirect("~/StudentPage");
            }
        }
    }
}