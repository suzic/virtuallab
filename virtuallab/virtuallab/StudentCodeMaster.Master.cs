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
    /// <summary>
    /// 主页面框架后台代码
    /// </summary>
    public partial class StudentCodeMaster : MasterPage
    {
        /// <summary>
        /// 用于缓存当前登录用户对象
        /// </summary>
        private LoginUser currentLoginUser;
        public LoginUser CurrentLoginUser
        {
            get
            {
                if (currentLoginUser == null && Session["user"] != null && (Session["user"] is LoginUser))
                    currentLoginUser = (LoginUser)Session["user"];
                return currentLoginUser; 
            }
        }
        public void ResetLoginUser()
        {
            currentLoginUser = null;
            Session.Remove("user");
        }
        public void CreateCurrentLoginUser(LoginUser user)
        {
            currentLoginUser = user;
            if (Session["user"] != null)
                Session["user"] = currentLoginUser;
            else
                Session.Add("user", currentLoginUser);
        }

        private const string AntiXsrfTokenKey = "__AntiXsrfToken";
        private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
        private string _antiXsrfTokenValue;

        /// <summary>
        /// 页面的数据初始化
        /// </summary>
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

        /// <summary>
        /// 框架页面加载
        /// </summary>
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

        /// <summary>
        /// 页面加载
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadCookiedUser();
        }

        /// <summary>
        /// 登出操作（未登录状态下，避免Cookie存储登录状态而做的清理）
        /// </summary>
        protected void Unnamed_LoggingOut(object sender, LoginCancelEventArgs e)
        {
            Context.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        }

        /// <summary>
        /// 登出操作
        /// </summary>
        protected void LoggingOut(object sender, EventArgs e)
        {
            //LogPart.ActiveViewIndex = 0;
            ResetLoginUser();

            Request.Cookies["LoginStudent"].Value = "";
            Request.Cookies["LoginManager"].Value = "";
            Response.Redirect("~/");
        }

        protected void InitCookieStructure()
        {
            if (Request.Cookies["UserID"] == null)
                Request.Cookies.Add(new HttpCookie("UserID"));
            if (Request.Cookies["LoginStudent"] == null)
                Request.Cookies.Add(new HttpCookie("LoginStudent"));
            if (Request.Cookies["LoginManager"] == null)
                Request.Cookies.Add(new HttpCookie("LoginManager"));

            if (Request.Cookies["ExperimentID"] == null)
                Request.Cookies.Add(new HttpCookie("ExperimentID"));
            if (Request.Cookies["TaskID"] == null)
                Request.Cookies.Add(new HttpCookie("TaskID"));
            if (Request.Cookies["SessionID"] == null)
                Request.Cookies.Add(new HttpCookie("SessionID"));
            if (Request.Cookies["CompileID"] == null)
                Request.Cookies.Add(new HttpCookie("CompileID"));
            if (Request.Cookies["UploadID"] == null)
                Request.Cookies.Add(new HttpCookie("UploadID"));
            if (Request.Cookies["RunID"] == null)
                Request.Cookies.Add(new HttpCookie("RunID"));
            if (Request.Cookies["CodeURI"] == null)
                Request.Cookies.Add(new HttpCookie("CodeURI"));
        }

        /// <summary>
        /// 将当前登录用户信息和Cookie信息进行同步
        /// </summary>
        protected void LoadCookiedUser()
        {
            // 无论何时，均先设置好本页面的Cookie结构
            InitCookieStructure();

            //LogPart.ActiveViewIndex = 0;
            //FuncMenu.ActiveViewIndex = -1;

            // 存在已登录对象的情况，通过对象重设Cookies
            if (CurrentLoginUser != null)
            {
                //LogPart.ActiveViewIndex = 1;
                Request.Cookies["UserID"].Value = CurrentLoginUser.userId.ToString();
                if (CurrentLoginUser.type == 1)
                {
                    //FuncMenu.ActiveViewIndex = 1;
                    Request.Cookies["LoginStudent"].Value = CurrentLoginUser.alias;
                }
                else
                {
                    //FuncMenu.ActiveViewIndex = 0;
                    Request.Cookies["LoginManager"].Value = CurrentLoginUser.alias;
                }
                Request.Cookies["ExperimentID"].Value = CurrentLoginUser.currentExperimentId;
                Request.Cookies["TaskID"].Value = CurrentLoginUser.currentTaskId;
                Request.Cookies["SessionID"].Value = CurrentLoginUser.currentSessionId;
                Request.Cookies["CompileID"].Value = CurrentLoginUser.currentCompileId;
                Request.Cookies["UploadID"].Value = CurrentLoginUser.currentUploadId;
                Request.Cookies["RunID"].Value = CurrentLoginUser.currentRunId;
                Request.Cookies["CodeURI"].Value = CurrentLoginUser.currentCodeUri;
                return;
            }

            // 已登录对象已经不存在，但反过来若Cookie还在，就尝试自动登录重建登录对象，除非Cookie也没有
            string currentUserID = Request.Cookies["UserID"].Value;
            if (currentUserID == null || currentUserID.Equals(String.Empty))
                return;

            int userType = 0;
            string cmdString;
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
            LoginUser loginUser = new LoginUser();

            loginUser.type = userType;
            // loginUser.alias = outDS.Tables["BHUSER"].Rows[0]["alias"].ToString(); 可以不必反写一次
            loginUser.name = outDS.Tables["BHUSER"].Rows[0]["name"].ToString();

            if (outDS.Tables["BHUSER"].Columns.Contains("gender"))
                loginUser.gender = (short)outDS.Tables["BHUSER"].Rows[0]["gender"];
            if (outDS.Tables["BHUSER"].Columns.Contains("grade"))
                loginUser.grade = outDS.Tables["BHUSER"].Rows[0]["grade"].ToString();
            if (outDS.Tables["BHUSER"].Columns.Contains("belong"))
                loginUser.belong = outDS.Tables["BHUSER"].Rows[0]["belong"].ToString();

            loginUser.phone = outDS.Tables["BHUSER"].Rows[0]["phone"].ToString();
            loginUser.email = outDS.Tables["BHUSER"].Rows[0]["email"].ToString();
            loginUser.password = outDS.Tables["BHUSER"].Rows[0]["password"].ToString();

            // 尝试从Cookie中恢复当前登录对象的实验信息
            //loginUser.currentExperimentId = Request.Cookies["ExperimentID"].Value;
            //loginUser.currentTaskId = Request.Cookies["TaskID"].Value;
            //loginUser.currentSessionId = Request.Cookies["SessionID"].Value;
            //loginUser.currentCompileId = Request.Cookies["CompileID"].Value;
            //loginUser.currentUploadId = Request.Cookies["UploadID"].Value;
            //loginUser.currentCodeUri = Request.Cookies["CodeURI"].Value;
            loginUser.currentExperimentId = "";
            loginUser.currentTaskId = "";
            loginUser.currentSessionId = "";
            loginUser.currentCompileId = "";
            loginUser.currentUploadId = "";
            loginUser.currentRunId = "";
            loginUser.currentCodeUri = "";

            loginUser.currentState = EnvironmentState.NotReady;
            loginUser.compileSuccess = false;
            loginUser.uploadSuccess = false;
            loginUser.playSuccess = false;

            //LogPart.ActiveViewIndex = 1;
            //FuncMenu.ActiveViewIndex = userType;

            // 将创建好的User对象交付Master框架完成最后创建。该过程将会把此用户对象写入Session
            ((SiteMaster)Master).CreateCurrentLoginUser(loginUser);
        }
    }
}