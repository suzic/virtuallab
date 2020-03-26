using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Routing;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using virtuallab.API.Service;
using virtuallab.API.Service.po;
using virtuallab.Common;
using virtuallab.Common.po;
using virtuallab.Models;

namespace virtuallab
{
    /// <summary>
    /// 开发环境页面的后台代码
    /// </summary>
    public partial class StudentCode : System.Web.UI.Page
    {
        /// <summary>
        /// 内置结构类型SI，用于记录代码编辑界面的位置信息以便在页面刷新后可以还原
        /// </summary>
        private class SI
        {
            public int left { get; set; }
            public int top { get; set; }
            public int width { get; set; }
            public int height { get; set; }
            public int clientWidth { get; set; }
            public int clientHeight { get; set; }
        }

        /// <summary>
        /// 内置结构类型DT，用于与前端交换界面TAB状态，代码内容，以及编辑器的位置信息
        /// </summary>
        private class DT
        {
            public int tab { get; set; }
            public string code { get; set; }
            public SI pos { get; set; }
        }

        /// <summary>
        /// 运行等待队列
        /// </summary>
        public static List<LoginUser> runningQueue = new List<LoginUser>();

        public LoginUser CurrentLoginUser
        {
            get { return ((StudentCodeMaster)Master).CurrentLoginUser; }
        }
        public string currentCode;
        public string currentCodeOrigin;
        public int currentPosTop;
        public int defaultTab;

        public static string BaseURL = System.Configuration.ConfigurationManager.AppSettings["BaseURL"];
        public static string URIEnvironmentRequest = System.Configuration.ConfigurationManager.AppSettings["URIEnvironmentRequest"];
        public static string URICodeSubmit = System.Configuration.ConfigurationManager.AppSettings["URICodeSubmit"];
        public static string URICompileResultTick = System.Configuration.ConfigurationManager.AppSettings["URICompileResultTick"];
        public static string URIProgramUpload = System.Configuration.ConfigurationManager.AppSettings["URIProgramUpload"];
        public static string URIRun = System.Configuration.ConfigurationManager.AppSettings["URIRun"];
        public static string URIRunResultTick = System.Configuration.ConfigurationManager.AppSettings["URIRunResultTick"];
        public static bool EnableService = System.Configuration.ConfigurationManager.AppSettings["EnableService"].Equals("1");

        //新加
        public List<bhCode> bhCodes=new List<bhCode>();

        /// <summary>
        /// 初始化页面
        /// </summary>
        protected void Page_Init(object sender, EventArgs e)
        {
            InitCodeMirrorStyles();

            if (CurrentLoginUser == null)
                Response.Redirect("~/");

        }

        /// <summary>
        /// 页面加载，该方法先获取可用的目标开发环境；同时处理可能来自前端的接口方法回调，并启动计时器进行Tick操作
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) {
                // 加载代码
                ReloadCode(this, EventArgs.Empty);
                return;
            }
        }

        // 从参数中获得当前代码，同时保存代码编辑器滚动的垂直坐标
        private void ReloadCodeWithPosition(string info)
        {
            DT deserializedArgs = JsonConvert.DeserializeObject<DT>(info);
            defaultTab = deserializedArgs.tab;
            currentCodeOrigin = deserializedArgs.code;
            SI scrollPos = deserializedArgs.pos;
            currentCode = JsonConvert.SerializeObject(currentCodeOrigin);
            currentPosTop = scrollPos.top;
        }

        // 重新加载模板代码
        protected void ReloadCode(object sender, EventArgs e)
        {
            string taskid = Request.QueryString["taskid"];
            if (string.IsNullOrWhiteSpace(taskid))
                return;

            bhCodes = DB.GetStudentCodes(Convert.ToInt32(taskid));
            for (int i = 0; i < bhCodes.Count; i++) {
                if (i == 0)
                    bhCodes[i].active = "active";
                else
                    bhCodes[i].active = "deactive";
            }

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

       

    }
}
