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
        private bool bTaskExit;
        public UrlHelper Url = new UrlHelper(new HttpRequestMessage());

        private class EnvironmentRequest
        {
            public string exp_id;
            public string user_id;
        }
        public LoginUser CurrentLoginUser;
        public const string BaseURL = "http://path/";
        public const string URIEnvironmentRequest = "path1";
        public const string URICodeSubmit = "path2";
        public const string URIProgramUpload = "path3";
        public const string URIComplieResultTick = "path4";
        public const string URIRunResultTick = "path5";

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

            if (string.IsNullOrEmpty(CurrentLoginUser.currentExperiment)
                || !CurrentLoginUser.currentExperiment.Equals("3"))
                Response.Redirect("~/NotReady");
        }

        // 页面加载，该方法先获取可用的目标开发环境
        protected void Page_Load(object sender, EventArgs e)
        {
            bTaskExit = false;
            currentState = EnvironmentState.InEditing;
            Thread callbackThread = new Thread(delegate () { ThreadListener(); });
            callbackThread.Start();


            //using (var httpClient = new HttpClient())
            //{
            //    httpClient.BaseAddress = new Uri(BaseURL);
            //    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //    var body = new FormUrlEncodedContent(new Dictionary<string, string>
            //    {
            //        { "exp_id", "3" }, // 3是数码管 ZLG7290实验的ID
            //        { "user_id", CurrentLoginUser.alias}
            //    });

            //    // response
            //    var response = httpClient.PostAsync(URIEnvironmentRequest, body).Result;
            //    var data = response.Content.ReadAsStringAsync().Result;
            //    // TODO: parse data
            //}
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
            //var client = new HttpClient();
            //client.BaseAddress = new Uri(BaseURL);
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //var retData = NetworkRun(client, URIComplieResultTick, new EnvironmentRequest()
            //{
            //    exp_id = "",
            //    user_id = ""
            //});
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
        private void ThreadListener()
        {
            while (bTaskExit == false)
            {
                Thread.Sleep(1000);

                switch (currentState)
                {
                    case EnvironmentState.InCompiling:
                        {
                            btnCompile.Enabled = true;
                        }
                        break;

                    case EnvironmentState.InUploading:
                        break;

                    case EnvironmentState.InPlaying:
                        break;

                    case EnvironmentState.InEditing:
                    default:
                        {

                        }
                        break;
                }
            }
        }


        #region 网络请求方法

        private static async Task<EnvironmentRequest> NetworkRun(HttpClient client, string uri, EnvironmentRequest param)
        {
            return await await client.PostAsJsonAsync(uri, param)
                .ContinueWith(x => x.Result.Content.ReadAsAsync<EnvironmentRequest>(
                    new List<MediaTypeFormatter>() { new JsonMediaTypeFormatter(), new XmlMediaTypeFormatter() })
                );
        }

        private string HTTPGet(string uri)
        {
            string serviceUrl = string.Format("{0}/{1}", BaseURL, uri);
            HttpWebRequest myRequest = WebRequest.Create(serviceUrl) as HttpWebRequest;
            HttpWebResponse myResponse = myRequest.GetResponse() as HttpWebResponse;

            StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
            //string returnXml = HttpUtility.UrlDecode(reader.ReadToEnd());//如果有编码问题就用这个方法
            string returnXml = reader.ReadToEnd();
            reader.Close();
            myResponse.Close();
            return returnXml;
        }

        private string HTTPPost(string data, string uri)
        {
            string serviceUrl = string.Format("{0}/{1}", BaseURL, uri);
            HttpWebRequest myRequest = WebRequest.Create(serviceUrl) as HttpWebRequest;
            byte[] buf = System.Text.Encoding.GetEncoding("UTF-8").GetBytes(data);

            myRequest.Method = "POST";
            myRequest.ContentLength = buf.Length;
            myRequest.ContentType = "application/json";
            myRequest.MaximumAutomaticRedirections = 1;
            myRequest.AllowAutoRedirect = true;

            //发送请求
            Stream stream = myRequest.GetRequestStream();
            stream.Write(buf, 0, buf.Length);
            stream.Close();

            //获取接口返回值
            HttpWebResponse myResponse = myRequest.GetResponse() as HttpWebResponse;
            StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
            //string returnXml = HttpUtility.UrlDecode(reader.ReadToEnd());//如果有编码问题就用这个方法
            string returnXml = reader.ReadToEnd();
            reader.Close();
            myResponse.Close();
            return returnXml;
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