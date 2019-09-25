using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using virtuallab.Models;

namespace virtuallab
{
    public partial class SiteMaster : MasterPage
    {
        public static LoginUser CurrentLoginUser = null;

        private const string AntiXsrfTokenKey = "__AntiXsrfToken";
        private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
        private string _antiXsrfTokenValue;

        protected void Page_Init(object sender, EventArgs e)
        {
            // 以下代码可帮助防御 XSRF 攻击
            var requestCookie = Request.Cookies[AntiXsrfTokenKey];
            Guid requestCookieGuidValue;
            if (requestCookie != null && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
            {
                // 使用 Cookie 中的 Anti-XSRF 令牌
                _antiXsrfTokenValue = requestCookie.Value;
                Page.ViewStateUserKey = _antiXsrfTokenValue;
            }
            else
            {
                // 生成新的 Anti-XSRF 令牌并保存到 Cookie
                _antiXsrfTokenValue = Guid.NewGuid().ToString("N");
                Page.ViewStateUserKey = _antiXsrfTokenValue;

                var responseCookie = new HttpCookie(AntiXsrfTokenKey)
                {
                    HttpOnly = true,
                    Value = _antiXsrfTokenValue
                };
                if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
                {
                    responseCookie.Secure = true;
                }
                Response.Cookies.Set(responseCookie);
            }

            Page.PreLoad += master_Page_PreLoad;
        }

        protected void master_Page_PreLoad(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // 设置 Anti-XSRF 令牌
                ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;
                ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name ?? String.Empty;
            }
            else
            {
                // 验证 Anti-XSRF 令牌
                if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
                    || (string)ViewState[AntiXsrfUserNameKey] != (Context.User.Identity.Name ?? String.Empty))
                {
                    throw new InvalidOperationException("Anti-XSRF 令牌验证失败。");
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadCookiedUser();
        }

        protected void Unnamed_LoggingOut(object sender, LoginCancelEventArgs e)
        {
            Context.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        }

        protected void LoggingOut(object sender, EventArgs e)
        {
            LogPart.ActiveViewIndex = 0;
            CurrentLoginUser = null;
            Request.Cookies["LoginStudent"].Value = "";
            Request.Cookies["LoginManager"].Value = "";

            Response.Redirect("~/");
        }

        protected void LoadCookiedUser()
        {
            LogPart.ActiveViewIndex = 0;
            FuncMenu.ActiveViewIndex = -1;

            // 设置好本页面的Cookie结构
            if (Request.Cookies["LoginStudent"] == null)
                Request.Cookies.Add(new HttpCookie("LoginStudent"));
            if (Request.Cookies["LoginManager"] == null)
                Request.Cookies.Add(new HttpCookie("LoginManager"));

            // 存在已登录对象的情况
            if (CurrentLoginUser != null)
            {
                LogPart.ActiveViewIndex = 1;
                if (CurrentLoginUser.type == 1)
                {
                    FuncMenu.ActiveViewIndex = 1;
                    Request.Cookies["LoginStudent"].Value = CurrentLoginUser.alias;
                }
                else
                {
                    FuncMenu.ActiveViewIndex = 0;
                    Request.Cookies["LoginManager"].Value = CurrentLoginUser.alias;
                }
                return;
            }

            // 即便已登录对象已经不存在，但反过来若Cookie还在，就尝试自动登录重建登录对象，除非Cookie也没有
            int userType = 0;
            string cmdString = null;
            string currentAlias = Request.Cookies["LoginStudent"].Value;
            if (currentAlias != null && !currentAlias.Equals(string.Empty))
            {
                cmdString = "SELECT DISTINCT [alias], [password], [id_student], [name], [gender], [grade], [belong], [phone], [email], [record_status] FROM[bhStudent] WHERE([alias] = @alias)";
                userType = 1;
            }
            else
            {
                currentAlias = Request.Cookies["LoginManager"].Value;
                if (currentAlias != null && !currentAlias.Equals(string.Empty))
                    cmdString = "SELECT DISTINCT [id_manager], [alias], [password], [name], [type], [phone], [email] FROM [bhManager] WHERE ([alias] = @alias)";
                else
                    return;
            }

            // 开始重建登录对象
            CurrentLoginUser = new LoginUser();

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
                    cmd.Parameters.Add("@alias", SqlDbType.VarChar, 50).Value = currentAlias;
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

            // 没有查到匹配Cookie的用户也不能自动登录
            if (outDS.Tables["BHUSER"].Rows.Count <= 0)
                return;

            // 创建登录用户对象，并使用ds来初始化所有数据
            CurrentLoginUser = new LoginUser();

            CurrentLoginUser.type = userType;
            CurrentLoginUser.alias = outDS.Tables["BHUSER"].Rows[0]["alias"].ToString();
            CurrentLoginUser.name = outDS.Tables["BHUSER"].Rows[0]["name"].ToString();

            if (outDS.Tables["BHUSER"].Columns.Contains("gender"))
                CurrentLoginUser.gender = (short)outDS.Tables["BHUSER"].Rows[0]["gender"];
            if (outDS.Tables["BHUSER"].Columns.Contains("grade"))
                CurrentLoginUser.grade = outDS.Tables["BHUSER"].Rows[0]["grade"].ToString();
            if (outDS.Tables["BHUSER"].Columns.Contains("belong"))
                CurrentLoginUser.belong = outDS.Tables["BHUSER"].Rows[0]["belong"].ToString();

            CurrentLoginUser.phone = outDS.Tables["BHUSER"].Rows[0]["phone"].ToString();
            CurrentLoginUser.email = outDS.Tables["BHUSER"].Rows[0]["email"].ToString();
            CurrentLoginUser.password = outDS.Tables["BHUSER"].Rows[0]["password"].ToString();

            LogPart.ActiveViewIndex = 1;
            FuncMenu.ActiveViewIndex = userType;
        }
    }
}