using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using virtuallab.Models;

namespace virtuallab
{
    public partial class ReportList : System.Web.UI.Page
    {
        public LoginUser CurrentLoginUser
        {
            get { return ((SiteMaster)Master).CurrentLoginUser; }
        }
        public string currentCode;

        protected void Page_Init(object sender, EventArgs e)
        {
            currentCode = JsonConvert.SerializeObject("");
            if (CurrentLoginUser == null)
                Response.Redirect("~/");
            else if (CurrentLoginUser.type == 1)
                Response.Redirect("~/StudentPage");
            else
                InitCodeMirrorStyles();
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        // 初始化CodeMirror以呈现代码规范化效果
        protected void InitCodeMirrorStyles()
        {
            HtmlGenericControl TabCSS = new HtmlGenericControl("link");
            TabCSS.Attributes.Add("href", ResolveUrl(Page.ResolveClientUrl("~/Content/tab.css")));
            TabCSS.Attributes.Add("rel", "stylesheet");

            HtmlGenericControl CodeMirrorJS = new HtmlGenericControl("script");
            CodeMirrorJS.Attributes.Add("src", ResolveUrl(Page.ResolveClientUrl("~/CM/lib/codemirror.js")));

            HtmlGenericControl CodeMirrorCSS = new HtmlGenericControl("link");
            CodeMirrorCSS.Attributes.Add("href", ResolveUrl(Page.ResolveClientUrl("~/CM/lib/codemirror.css")));
            CodeMirrorCSS.Attributes.Add("rel", "stylesheet");

            HtmlGenericControl CodeMirrorTheme1 = new HtmlGenericControl("link");
            CodeMirrorTheme1.Attributes.Add("href", ResolveUrl(Page.ResolveClientUrl("~/CM/theme/zenburn.css")));
            CodeMirrorTheme1.Attributes.Add("rel", "stylesheet");
            HtmlGenericControl CodeMirrorTheme2 = new HtmlGenericControl("link");
            CodeMirrorTheme2.Attributes.Add("href", ResolveUrl(Page.ResolveClientUrl("~/CM/theme/mdn-like.css")));
            CodeMirrorTheme2.Attributes.Add("rel", "stylesheet");

            HtmlGenericControl CodeScrollbar = new HtmlGenericControl("script");
            CodeScrollbar.Attributes.Add("src", ResolveUrl(Page.ResolveClientUrl("~/CM/addon/scroll/simplescrollbars.js")));

            HtmlGenericControl CodeFormatClike = new HtmlGenericControl("script");
            CodeFormatClike.Attributes.Add("src", ResolveUrl(Page.ResolveClientUrl("~/CM/mode/clike/clike.js")));
            HtmlGenericControl CodeFormatText = new HtmlGenericControl("script");
            CodeFormatText.Attributes.Add("src", ResolveUrl(Page.ResolveClientUrl("~/CM/mode/textile/textile.js")));

            this.Page.Header.Controls.Add(TabCSS);
            this.Page.Header.Controls.Add(CodeMirrorCSS);
            this.Page.Header.Controls.Add(CodeMirrorJS);
            this.Page.Header.Controls.Add(CodeMirrorTheme1);
            this.Page.Header.Controls.Add(CodeMirrorTheme2);
            this.Page.Header.Controls.Add(CodeFormatClike);
            this.Page.Header.Controls.Add(CodeFormatText);
            this.Page.Header.Controls.Add(CodeScrollbar);
        }

        /// <summary>
        /// 查看学生的代码
        /// </summary>
        protected void lbCode_Command(object sender, CommandEventArgs e)
        {
            HttpWebRequest myHttpWebRequest;

            // 当前约定了用户的代码存储位置，然并未通过接口参数返回；因此这里需要根据约定来主动拼写请求地址
            // string resultCodeUrl = e.CommandArgument.ToString();
            int nIndex = ((GridViewRow)((LinkButton)sender).NamingContainer).RowIndex;
            string idExp = gvRepots.DataKeys[nIndex][1].ToString();
            string idStu = gvRepots.DataKeys[nIndex][2].ToString();
            string resultCodeUrl = "192.168.18.80/workdir/" + idStu + "/" + idExp + "/app/src/" + idStu + "_" + idExp + ".c;";
            resultCodeUrl += "192.168.18.80/workdir/" + idStu + "/" + idExp + "/kernel/src/" + idStu + "_" + idExp + ".c";

            string[] results = resultCodeUrl.Split(';');
            string readCode = "";
            for (int i = 0; i < results.Length; i++)
            {
                myHttpWebRequest = System.Net.WebRequest.Create("http://" + results[i]) as HttpWebRequest;
                myHttpWebRequest.KeepAlive = false;
                myHttpWebRequest.AllowAutoRedirect = false;
                myHttpWebRequest.UserAgent = "Mozilla/5.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727)";
                myHttpWebRequest.Timeout = 10000;
                myHttpWebRequest.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
                using (HttpWebResponse res = (HttpWebResponse)myHttpWebRequest.GetResponse())
                {
                    if (res.StatusCode == HttpStatusCode.OK || res.StatusCode == HttpStatusCode.PartialContent) // 返回为200或206
                    {
                        string dd = res.ContentEncoding;
                        System.IO.Stream strem = res.GetResponseStream();
                        System.IO.StreamReader r = new System.IO.StreamReader(strem);

                        if (i > 0)
                            readCode += "\n//====================//";
                        readCode += r.ReadToEnd();
                    }
                }
            }

            // 需要使用JSON封装的方法将该字符串传至前端
            currentCode = JsonConvert.SerializeObject(readCode);
        }

        protected void lbScoreConfirm(object sender, CommandEventArgs e)
        {
            string sConnString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            SqlConnection sSqlConn = new SqlConnection(sConnString);
            int nIndex = ((GridViewRow)((LinkButton)sender).NamingContainer).RowIndex;
            int id_task = int.Parse(gvRepots.DataKeys[nIndex][0].ToString());
            TextBox tbScore = (TextBox)gvRepots.Rows[nIndex].FindControl("tbScore");
            try
            {
                sSqlConn.Open();
                SqlCommand cmd = sSqlConn.CreateCommand();
                cmd.CommandText = "UPDATE bhTask SET score = @score WHERE (id_task = @id_task)";
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@id_task", SqlDbType.Int).Value = id_task;
                cmd.Parameters.Add("@score", SqlDbType.Int).Value = int.Parse(tbScore.Text);
                cmd.ExecuteNonQuery();
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
    }
}