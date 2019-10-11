using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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
using virtuallab.Models;

namespace virtuallab
{
    /// <summary>
    /// 游戏进行状态枚举值
    /// </summary>
    public enum EnvironmentState
    {
        InEditing = 0,
        InCompiling,
        InUploading,
        InPlaying
    }

    public partial class Environment : System.Web.UI.Page
    {
        public UrlHelper Url = new UrlHelper(new HttpRequestMessage());

        public LoginUser CurrentLoginUser;
        public const string BaseURL = "http://192.168.200.119:8088/address/";
        public const string URIEnvironmentRequest = "environmentRequest";
        public const string URICodeSubmit = "codeSubmit";
        public const string URIProgramUpload = "programUpload";
        public const string URICompileResultTick = "compileResultTick";
        public const string URIRunResultTick = "runResultTick";

        public EnvironmentState currentState;
        public string currentCode;
        public bool compileSuccess;
        public bool uploadSuccess;

        // 声明一个委托类型，该委托类型无输入参数和输出参数
        public delegate void ProcessDelegate();

        protected void Page_Init(object sender, EventArgs e)
        {
            InitCodeMirrorStyles();

            CurrentLoginUser = SiteMaster.CurrentLoginUser;
            if (CurrentLoginUser == null)
                Response.Redirect("~/");
            else if (CurrentLoginUser.type == 0)
                Response.Redirect("~/ManagerPage");

            // 当前仅针对ExperimentID=3的实验进行，没有ID或ID不为3，重定向到未就绪页面
            if (string.IsNullOrEmpty(CurrentLoginUser.currentExperimentId)
                || !CurrentLoginUser.currentExperimentId.Equals("3"))
                Response.Redirect("~/NotReady");
        }

        // 页面加载，该方法先获取可用的目标开发环境
        protected void Page_Load(object sender, EventArgs e)
        {
            // 进入页面首先获取session_id（如果已经有，则可以跳过）
            if (string.IsNullOrEmpty(CurrentLoginUser.currentSessionId))
                EnvironmentRequest();
            // 在请求session_id之后如果仍然还是空，说明服务器暂时无法提供连接
            if (string.IsNullOrEmpty(CurrentLoginUser.currentSessionId))
                Response.Redirect("~/NotReady");
        }

        // 重新加载模板代码
        protected void ReloadCode(object sender, EventArgs e)
        {

        }

        // 编译代码
        protected void CodeComplie(object sender, EventArgs e)
        {
            currentState = EnvironmentState.InCompiling;
            btnCompile.Enabled = false;
            CodeSubmit();
        }

        // 上传代码
        protected void CodeUpload(object sender, EventArgs e)
        {
            //var client = new HttpClient();
            //client.BaseAddress = new Uri(BaseURL);
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //var retData = NetworkRun(client, URIProgramUpload, new EnvironmentRequest()
            //{
            //    exp_id = "",
            //    user_id = ""
            //});
        }

        // 初始化CodeMirror以呈现代码规范化效果
        protected void InitCodeMirrorStyles()
        {
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

            this.Page.Header.Controls.Add(CodeMirrorCSS);
            this.Page.Header.Controls.Add(CodeMirrorJS);
            this.Page.Header.Controls.Add(CodeMirrorTheme1);
            this.Page.Header.Controls.Add(CodeMirrorTheme2);
            this.Page.Header.Controls.Add(CodeFormatClike);
            this.Page.Header.Controls.Add(CodeFormatText);
            this.Page.Header.Controls.Add(CodeScrollbar);
        }

        // 接收线程
        //private void ThreadListener()
        //{
        //    while (bTaskExit == false)
        //    {
        //        Thread.Sleep(1000);

        //        switch (currentState)
        //        {
        //            case EnvironmentState.InCompiling:
        //                {
        //                    btnCompile.Enabled = true;
        //                }
        //                break;

        //            case EnvironmentState.InUploading:
        //                break;

        //            case EnvironmentState.InPlaying:
        //                break;

        //            case EnvironmentState.InEditing:
        //            default:
        //                {

        //                }
        //                break;
        //        }
        //    }
        //}

        #region 网络请求方法

        // 初始化开发环境，获取session_id的网络请求
        protected void EnvironmentRequest()
        {
            System.Diagnostics.Debug.Write("============= Environment Request ===============\n");
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(BaseURL);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var body = new FormUrlEncodedContent(new Dictionary<string, string> {
                        { "exp_id", CurrentLoginUser.currentExperimentId.ToString() }, // 目前仅支持数码管ZLG7290实验的ID=3
                        { "user_id", CurrentLoginUser.userId.ToString() }
                    });

                // response
                var response = httpClient.PostAsync(URIEnvironmentRequest, body).Result;
                var data = response.Content.ReadAsStringAsync().Result;
                var formatData = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);
                var result = (JObject)formatData["data"];
                bool success = (int)result["fail"] == 0;
                if (success)
                {
                    CurrentLoginUser.currentSessionId = result["session_id"].ToString();
                    System.Diagnostics.Debug.Write("-------- session_id = " + CurrentLoginUser.currentSessionId + "\n");
                }
            }
        }

        protected void CodeSubmit()
        {
            System.Diagnostics.Debug.Write("============= Code Submit ===============\n");
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(BaseURL);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var body = new FormUrlEncodedContent(new Dictionary<string, string> {
                        { "session_id", CurrentLoginUser.currentSessionId.ToString() }, 
                        { "code",  }
                    });

                // response
                var response = httpClient.PostAsync(URICodeSubmit, body).Result;
                var data = response.Content.ReadAsStringAsync().Result;
                var formatData = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);
                var result = (JObject)formatData["data"];
                bool success = (int)result["fail"] == 0;
                if (success)
                {
                    CurrentLoginUser.currentCompileId = result["compile_id"].ToString();
                    CurrentLoginUser.currentCodeUri = result["code_uri"].ToString();
                    System.Diagnostics.Debug.Write("-------- compile_id = " + CurrentLoginUser.currentCompileId + "\n");
                    System.Diagnostics.Debug.Write("-------- code_uri = " + CurrentLoginUser.currentCodeUri + "\n");
                }
            }
        }

        #endregion

        #region 界面切换按钮

        protected void SwitchViewToCode(object sender, EventArgs e)
        {
            btnCode.Enabled = false;
            btnBoard.Enabled = true;
            btnExp.Enabled = true;
            EnvironmentView.ActiveViewIndex = 0;
        }

        protected void SwitchViewToBoard(object sender, EventArgs e)
        {
            btnCode.Enabled = true;
            btnBoard.Enabled = false;
            btnExp.Enabled = true;
            EnvironmentView.ActiveViewIndex = 1;
        }

        protected void SwitchViewToIntro(object sender, EventArgs e)
        {
            btnCode.Enabled = true;
            btnBoard.Enabled = true;
            btnExp.Enabled = false;
            EnvironmentView.ActiveViewIndex = 2;
        }

        #endregion
    }
}