using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using virtuallab.Models;

namespace virtuallab.Account
{
    public partial class Login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // 如果提供了注册功能，启用此项
            // RegisterHyperLink.NavigateUrl = "Register";

            // 在为密码重置功能启用帐户确认后，启用此项
            // ForgotPasswordHyperLink.NavigateUrl = "Forgot";

            // 如果实现了第三方登录功能，启用此项
            // OpenAuthLogin.        = Request.QueryString["ReturnUrl"];
            // var returnUrl = HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]);
            // if (!String.IsNullOrEmpty(returnUrl))
            // {
            //     RegisterHyperLink.NavigateUrl += "?ReturnUrl=" + returnUrl;
            // }
        }

        protected void LogIn(object sender, EventArgs e)
        {
            if (IsValid)
            {
                // 验证用户密码
                // var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                // var signinManager = Context.GetOwinContext().GetUserManager<ApplicationSignInManager>();
                // 这不会计入到为执行帐户锁定而统计的登录失败次数中
                // 若要在多次输入错误密码的情况下触发锁定，请更改为 shouldLockout: true
                // var result = signinManager.PasswordSignIn(Account.Text, Password.Text, false, shouldLockout: false);

                // 系统化的暂时用不到，自己重写本地登录验证逻辑
                var result = LocalSignIn(Account.Text, Password.Text);

                switch (result)
                {
                    case SignInStatus.Success:
                        Response.Redirect("~/StudentPage");
                        // IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
                        break;
                    case SignInStatus.LockedOut:
                        Response.Redirect("/Account/Lockout");
                        break;

                    case SignInStatus.RequiresVerification:
                        //Response.Redirect(String.Format("/Account/TwoFactorAuthenticationSignIn?ReturnUrl={0}&RememberMe={1}", 
                        //                                Request.QueryString["ReturnUrl"],
                        //                                false),
                        //                  true);
                        //break;
                    case SignInStatus.Failure:
                    default:
                        FailureText.Text = "登录失败";
                        ErrorMessage.Visible = true;
                        break;
                }
            }
        }

        protected SignInStatus LocalSignIn(string alias, string password)
        {
            SignInStatus status = SignInStatus.Failure;
            SiteMaster.CurrentLoginUser = null;

            int userType = 0;
            string cmdString = null;
            if (RadioStudent.Checked)
            {
                userType = 1;
                cmdString = "SELECT DISTINCT [alias], [password], [id_student], [name], [gender], [grade], [belong], [phone], [email], [record_status] FROM[bhStudent] WHERE([alias] = @alias)";
            }
            else if (RadioManager.Checked)
            {
                userType = 0;
                cmdString = "SELECT DISTINCT [id_manager], [alias], [password], [name], [type], [phone], [email] FROM [bhManager] WHERE ([alias] = @alias)";
            }

            // 获取数据库读取连接字符串并建立连接
            string sConnString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            DataSet outDS = new DataSet();

            // 准备查询命令和接受查询结果的数据工具集，进行查询，结果通过da格式化填充到ds中
            using (SqlConnection sSqlConn = new SqlConnection(sConnString))
            {
                SqlCommand cmd = sSqlConn.CreateCommand();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                try
                {
                    sSqlConn.Open();
                    cmd.CommandText = cmdString;
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add("@alias", SqlDbType.VarChar, 50).Value = alias;
                    da.FillSchema(outDS, SchemaType.Source, "BHUSER");
                    da.Fill(outDS, "BHUSER");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    sSqlConn.Close();
                }
            }

            // 没有查到用户的情况
            if (outDS.Tables["BHUSER"].Rows.Count <= 0)
                return SignInStatus.Failure;

            string correctPassword = outDS.Tables["BHUSER"].Rows[0]["password"].ToString();

            if (password.Equals(correctPassword))
            {
                // 创建登录用户对象，并使用ds来初始化所有数据
                SiteMaster.CurrentLoginUser = new LoginUser();

                SiteMaster.CurrentLoginUser.type = userType;
                SiteMaster.CurrentLoginUser.alias = outDS.Tables["BHUSER"].Rows[0]["alias"].ToString();
                SiteMaster.CurrentLoginUser.name = outDS.Tables["BHUSER"].Rows[0]["name"].ToString();

                if (outDS.Tables["BHUSER"].Columns.Contains("gender"))
                    SiteMaster.CurrentLoginUser.gender = (short)(outDS.Tables["BHUSER"].Rows[0]["gender"]);
                if (outDS.Tables["BHUSER"].Columns.Contains("grade"))
                    SiteMaster.CurrentLoginUser.grade = outDS.Tables["BHUSER"].Rows[0]["grade"].ToString();
                if (outDS.Tables["BHUSER"].Columns.Contains("belong"))
                    SiteMaster.CurrentLoginUser.belong = outDS.Tables["BHUSER"].Rows[0]["belong"].ToString();

                SiteMaster.CurrentLoginUser.phone = outDS.Tables["BHUSER"].Rows[0]["phone"].ToString();
                SiteMaster.CurrentLoginUser.email = outDS.Tables["BHUSER"].Rows[0]["email"].ToString();
                SiteMaster.CurrentLoginUser.password = outDS.Tables["BHUSER"].Rows[0]["password"].ToString();

                int ValidStudent = 1;
                if (outDS.Tables["BHUSER"].Columns.Contains("record_status"))
                    ValidStudent = (short)outDS.Tables["BHUSER"].Rows[0]["record_status"];

                status = (ValidStudent == 1) ? SignInStatus.Success : SignInStatus.Failure;
            }
            else
                status = SignInStatus.Failure;

            return status;
        }
    }
}