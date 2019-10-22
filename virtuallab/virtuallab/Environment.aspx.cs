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
    public partial class Environment : System.Web.UI.Page
    {
        private class SI
        {
            public int left { get; set; }
            public int top { get; set; }
            public int width { get; set; }
            public int height { get; set; }
            public int clientWidth { get; set; }
            public int clientHeight { get; set; }
        }

        private class DT
        {
            public int tab { get; set; }
            public string code { get; set; }
            public SI pos { get; set; }
        }

        public LoginUser CurrentLoginUser;
        public string currentCode;
        public string currentCodeOrigin;
        public int currentPosTop;
        public int defaultTab;

        public string BaseURL = "http://192.168.200.119:8088/address/";
        public string URIEnvironmentRequest = "environmentRequest";
        public string URICodeSubmit = "codeSubmit";
        public string URIProgramUpload = "ProgramUpload";
        public string URICompileResultTick = "CompileResultTick";
        public string URIRunResultTick = "RunResultTick";

        public bool EnableService = true;
        public StringBuilder outputString = new StringBuilder();
        public StringBuilder animateString = new StringBuilder();
        public string runResultFormatted
        {
            get { return JsonConvert.SerializeObject(animateString.ToString()); }
        }
        public string outputFormatted
        {
            get { return JsonConvert.SerializeObject(outputString.ToString()); }
        }

        /// <summary>
        /// 初始化页面
        /// </summary>
        protected void Page_Init(object sender, EventArgs e)
        {
            InitNetworkParams();
            InitCodeMirrorStyles();

            CurrentLoginUser = SiteMaster.CurrentLoginUser;
            if (CurrentLoginUser != null)
                CurrentLoginUser.StateChangedEvent += new StateChanged(EnvironmentStateChanged);

            if (CurrentLoginUser == null)
                Response.Redirect("~/");
            else if (CurrentLoginUser.type == 0)
                Response.Redirect("~/ManagerPage");

            // 当前仅针对ExperimentID=3的实验进行，没有ID或ID不为3，重定向到未就绪页面
            if (string.IsNullOrEmpty(CurrentLoginUser.currentExperimentId)
                || !CurrentLoginUser.currentExperimentId.Equals("3"))
                Response.Redirect("~/NotReady");
        }

        /// <summary>
        /// 页面加载，该方法先获取可用的目标开发环境；同时处理可能来自前端的接口方法回调，并启动计时器进行Tick操作
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            // 进入页面首先获取session_id，并且确保当前用户状态不是NotReady
            if (string.IsNullOrEmpty(CurrentLoginUser.currentSessionId)
                || CurrentLoginUser.currentState == EnvironmentState.NotReady)
            {
                EnvironmentRequest();
                ReloadCode(this, EventArgs.Empty);

                // 在请求session_id之后如果仍然还是空，说明服务器暂时无法提供连接
                if (string.IsNullOrEmpty(CurrentLoginUser.currentSessionId))
                    Response.Redirect("~/NotReady");
            }
            // 如果不需要获取session_id的情况下，检查是否是前端操作的回调，并执行对应的动作命令
            else
            {
                //// 暂时不显示标记
                //lbGeneral.Visible = false;

                String key = Request.Form["__EVENTTARGET"];
                if (!string.IsNullOrEmpty(key) && key.Equals("SUBMIT_CODE"))
                {
                    ReloadCodeWithPosition(Request["__EVENTARGUMENT"]);
                    CodeSubmit(); 
                }
                else if (!string.IsNullOrEmpty(key) && key.Equals("UPLOAD_PROGRAM"))
                {
                    ReloadCodeWithPosition(Request["__EVENTARGUMENT"]);
                    ProgramUpload();
                }
                else if (!string.IsNullOrEmpty(key) && key.Equals("RUN_PLAY"))
                {
                    ReloadCodeWithPosition(Request["__EVENTARGUMENT"]);
                    RunPlay();
                }
                else if (!string.IsNullOrEmpty(key) && key.Equals("COMPILE_TICK"))
                {
                    ReloadCodeWithPosition(Request["__EVENTARGUMENT"]);
                    CompileTick();
                }
                else if (!string.IsNullOrEmpty(key) && key.Equals("UPLOAD_TICK"))
                {
                    ReloadCodeWithPosition(Request["__EVENTARGUMENT"]);
                    UploadTick();
                }
                else if (!string.IsNullOrEmpty(key) && key.Equals("RUN_TICK"))
                {
                    ReloadCodeWithPosition(Request["__EVENTARGUMENT"]);
                    RunTick();
                }
                // 可能不是前端操作的回调，只是刷新了，这种情况单纯的重新加载代码
                else
                {
                    CurrentLoginUser.currentState = EnvironmentState.InEditing;
                    ReloadCode(this, EventArgs.Empty);
                }
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
            HttpWebRequest myHttpWebRequest = System.Net.WebRequest.Create(Request.Url.GetLeftPart(UriPartial.Authority) + "/Content/codeSample.txt") as HttpWebRequest;
            myHttpWebRequest.KeepAlive = false;
            myHttpWebRequest.AllowAutoRedirect = false;
            myHttpWebRequest.UserAgent = "Mozilla/5.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727)";
            myHttpWebRequest.Timeout = 10000;
            myHttpWebRequest.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
            using (HttpWebResponse res = (HttpWebResponse)myHttpWebRequest.GetResponse())
            {
                if (res.StatusCode == HttpStatusCode.OK || res.StatusCode == HttpStatusCode.PartialContent)//返回为200或206
                {
                    string dd = res.ContentEncoding;
                    System.IO.Stream strem = res.GetResponseStream();
                    System.IO.StreamReader r = new System.IO.StreamReader(strem);
                    currentCode = r.ReadToEnd();
                    // 需要使用JSON封装的方法将该字符串传至前端
                    currentCode = JsonConvert.SerializeObject(currentCode);
                    currentPosTop = 0;
                }
            }
        }

        // 从配置文件中读取接口设置
        protected void InitNetworkParams()
        {
            BaseURL = System.Configuration.ConfigurationManager.AppSettings["BaseURL"];
            URIEnvironmentRequest = System.Configuration.ConfigurationManager.AppSettings["URIEnvironmentRequest"];
            URICodeSubmit = System.Configuration.ConfigurationManager.AppSettings["URICodeSubmit"];
            URIProgramUpload = System.Configuration.ConfigurationManager.AppSettings["URIProgramUpload"];
            URICompileResultTick = System.Configuration.ConfigurationManager.AppSettings["URICompileResultTick"];
            URIRunResultTick = System.Configuration.ConfigurationManager.AppSettings["URIRunResultTick"];
            EnableService = System.Configuration.ConfigurationManager.AppSettings["EnableService"].Equals("1");
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

        // 状态提示器
        protected void EnvironmentStateChanged(object sender, EventArgs e)
        {
            LoginUser user = sender as LoginUser;
            switch (user.currentState)
            {
                case EnvironmentState.InEditing:
                    lbGeneral.Text = "实验服务正常。编辑你的代码。";
                    btnReload.Enabled = true;
                    break;
                case EnvironmentState.InCompiling:
                    lbGeneral.Text = "正在编译……";
                    btnReload.Enabled = false;
                    break;
                case EnvironmentState.InUploading:
                    lbGeneral.Text = "正在上传程序到板卡……";
                    btnReload.Enabled = false;
                    break;
                case EnvironmentState.InPlaying:
                    lbGeneral.Text = "板卡正在运行程序，请查看板卡效果。";
                    btnReload.Enabled = false;
                    break;
                case EnvironmentState.NotReady:
                default:
                    lbGeneral.Text = "未能连接到实验室服务，请稍后刷新页面重试。";
                    btnReload.Enabled = false;
                    break;
            }
        }

        #region 网络请求方法

        // 初始化开发环境，获取session_id的网络请求
        protected void EnvironmentRequest()
        {
            CurrentLoginUser.currentState = EnvironmentState.NotReady;
            if (!EnableService)
            {
                CurrentLoginUser.currentState = EnvironmentState.InEditing;
                CurrentLoginUser.currentSessionId = "test_session_sz19";
                return;
            }

            System.Diagnostics.Debug.WriteLine("============= Environment Request ===============");
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
                    CurrentLoginUser.currentState = EnvironmentState.InEditing;

                    CurrentLoginUser.currentSessionId = result["session_id"].ToString();
                    System.Diagnostics.Debug.Write("-------- session_id = " + CurrentLoginUser.currentSessionId + "\n");
                }
            }
        }

        // 代码提交，后台将会执行编译动作
        protected void CodeSubmit()
        {
            if (CurrentLoginUser.currentState == EnvironmentState.InEditing)
            {
                CurrentLoginUser.currentState = EnvironmentState.InCompiling;
                CurrentLoginUser.compileSuccess = false;
                if (!EnableService)
                {
                    CurrentLoginUser.currentCompileId = "test_compile_sz19";
                    CurrentLoginUser.currentCodeUri = "test_code_uri_sz19";
                    return;
                }

                System.Diagnostics.Debug.WriteLine("============= Code Submit ===============");
                System.Diagnostics.Debug.WriteLine(currentCode);
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(BaseURL);
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var body = new FormUrlEncodedContent(new Dictionary<string, string> {
                        { "session_id", CurrentLoginUser.currentSessionId.ToString() },
                        { "code",  currentCodeOrigin }
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
                        CurrentLoginUser.currentCodeUri = result["code_url"].ToString();
                        System.Diagnostics.Debug.Write("-------- compile_id = " + CurrentLoginUser.currentCompileId + "\n");
                        System.Diagnostics.Debug.Write("-------- code_uri = " + CurrentLoginUser.currentCodeUri + "\n");
                    }
                    else
                        CurrentLoginUser.currentState = EnvironmentState.InEditing;
                }
            }
            else
            {
                outputString.AppendLine("ERROR：当前状态不允许进行编译。");
            }
        }

        // 编译结果心跳检查
        protected void CompileTick()
        {
            if (CurrentLoginUser.currentState == EnvironmentState.InCompiling)
            {
                if (!EnableService)
                {
                    CurrentLoginUser.currentState = EnvironmentState.InEditing;
                    CurrentLoginUser.compileSuccess = true;
                    outputString.AppendLine("");
                    outputString.AppendLine("In compiling...");
                    for (int i = 1; i <= 10; i++)
                        outputString.AppendLine("Progress " + i.ToString() + "0 % ......");
                    outputString.AppendLine("Completed.");
                    outputString.AppendLine("");
                    return;
                }

                System.Diagnostics.Debug.WriteLine("============= Compile Tick ===============");
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(BaseURL);
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var body = new FormUrlEncodedContent(new Dictionary<string, string> {
                        { "session_id", CurrentLoginUser.currentSessionId.ToString() },
                        { "compile_id", CurrentLoginUser.currentCompileId.ToString() }
                    });

                    // response
                    var response = httpClient.PostAsync(URICompileResultTick, body).Result;
                    var data = response.Content.ReadAsStringAsync().Result;
                    var formatData = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);
                    var result = (JObject)formatData["data"];
                    bool success = (int)result["fail"] == 0;
                    if (success)
                    {
                        string infoBuffer = result["info_buffer"].ToString();
                        int finishInt = (int)result["finish"];
                        string finishFlag = finishInt == 0 ? "未完成" : finishInt == 1 ? "成功" : "失败";
                        System.Diagnostics.Debug.WriteLine("-------- infoBuffer ----------------");
                        System.Diagnostics.Debug.WriteLine(infoBuffer);
                        System.Diagnostics.Debug.WriteLine(">>>> Finished :" + finishFlag);
                        if (finishInt != 0)
                        {
                            CurrentLoginUser.currentState = EnvironmentState.InEditing;
                            CurrentLoginUser.compileSuccess = (finishInt == 1);
                        }
                    }
                    else
                    {
                        CurrentLoginUser.currentState = EnvironmentState.InEditing;
                        CurrentLoginUser.compileSuccess = false;
                    }
                }
            }
            else
            {
                outputString.AppendLine("ERROR：没有正在进行的编译。");
            }
        }

        // 代码上传到板卡
        protected void ProgramUpload()
        {
            if (CurrentLoginUser.currentState == EnvironmentState.InEditing && CurrentLoginUser.compileSuccess)
            {
                CurrentLoginUser.currentState = EnvironmentState.InUploading;
                CurrentLoginUser.uploadSuccess = false;
                if (!EnableService)
                {
                    CurrentLoginUser.currentUploadId = "test_upload_id_sz19";
                    return;
                }

                System.Diagnostics.Debug.WriteLine("============= Program Uploading ===============");
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(BaseURL);
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var body = new FormUrlEncodedContent(new Dictionary<string, string> {
                        { "session_id", CurrentLoginUser.currentSessionId.ToString() }
                    });

                    // response
                    var response = httpClient.PostAsync(URIProgramUpload, body).Result;
                    var data = response.Content.ReadAsStringAsync().Result;
                    var formatData = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);
                    var result = (JObject)formatData["data"];
                    bool success = (int)result["fail"] == 0;
                    if (success)
                    {
                        CurrentLoginUser.currentUploadId = result["upload_id"].ToString();
                        System.Diagnostics.Debug.Write("-------- upload_id = " + CurrentLoginUser.currentUploadId + "\n");
                    }
                    else
                        CurrentLoginUser.currentState = EnvironmentState.InEditing;
                }
            }
            else
            {
                outputString.AppendLine("ERROR：你还没有编译完成，当前无法上传程序。");
            }
        }

        // 上传结果心跳检查
        protected void UploadTick()
        {
            if (CurrentLoginUser.currentState == EnvironmentState.InUploading)
            {
                if (!EnableService)
                {
                    CurrentLoginUser.currentState = EnvironmentState.InEditing;
                    CurrentLoginUser.uploadSuccess = true;
                    outputString.AppendLine("");
                    outputString.AppendLine("开始将程序上传到板卡...");
                    for (int i = 1; i <= 10; i++)
                        outputString.AppendLine("上传完成了 " + i.ToString() + "0%......");
                    outputString.AppendLine("已完成，请切换到板卡页面可以运行了.");
                    outputString.AppendLine("");
                    return;
                }

                System.Diagnostics.Debug.WriteLine("============= Upload Tick ===============");
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(BaseURL);
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var body = new FormUrlEncodedContent(new Dictionary<string, string> {
                        { "session", CurrentLoginUser.currentSessionId.ToString() },
                        { "upload_id", CurrentLoginUser.currentUploadId.ToString() }
                    });

                    // response
                    var response = httpClient.PostAsync(URIRunResultTick, body).Result;
                    var data = response.Content.ReadAsStringAsync().Result;
                    var formatData = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);
                    var result = (JObject)formatData["data"];
                    bool success = (int)result["fail"] == 0;
                    if (success)
                    {
                        int finishInt = (int)result["finish"];
                        string finishFlag = finishInt == 0 ? "未完成" : finishInt == 1 ? "成功" : "失败";
                        System.Diagnostics.Debug.WriteLine(">>>> Finished :" + finishFlag);
                        string resultInfo = result["result_json"].ToString();
                        System.Diagnostics.Debug.WriteLine("-------- Result info ----------------");
                        System.Diagnostics.Debug.WriteLine(resultInfo);

                        if (finishInt != 0)
                        {
                            CurrentLoginUser.currentState = EnvironmentState.InEditing;
                            CurrentLoginUser.uploadSuccess = (finishInt == 1);
                        }
                    }
                    else
                    {
                        CurrentLoginUser.currentState = EnvironmentState.InEditing;
                        CurrentLoginUser.uploadSuccess = false;
                    }
                }
            }
            else
            {
                outputString.AppendLine("ERROR：没有正在进行的上传。");
            }
        }

        // 调用开始执行程序操作
        protected void RunPlay()
        {
            if (CurrentLoginUser.currentState == EnvironmentState.InEditing && CurrentLoginUser.uploadSuccess)
            {
                CurrentLoginUser.currentState = EnvironmentState.InPlaying;
                CurrentLoginUser.playSuccess = false;
                if (!EnableService)
                {
                    CurrentLoginUser.currentRunId = "test_run_id_sz19";
                    return;
                }

                System.Diagnostics.Debug.WriteLine("============= Run Program ===============");
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(BaseURL);
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var body = new FormUrlEncodedContent(new Dictionary<string, string> {
                        { "session_id", CurrentLoginUser.currentSessionId.ToString() }
                    });

                    // response
                    var response = httpClient.PostAsync(URIProgramUpload, body).Result;
                    var data = response.Content.ReadAsStringAsync().Result;
                    var formatData = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);
                    var result = (JObject)formatData["data"];
                    bool success = (int)result["fail"] == 0;
                    if (success)
                    {
                        CurrentLoginUser.currentRunId = result["run_id"].ToString();
                        System.Diagnostics.Debug.Write("-------- run_id = " + CurrentLoginUser.currentRunId + "\n");
                    }
                    else
                        CurrentLoginUser.currentState = EnvironmentState.InEditing;
                }
            }
            else
            {
                outputString.AppendLine("ERROR：你还没有上传程序到板卡，当前无法运行。");
            }
        }

        protected void RunTick()
        {
            if (CurrentLoginUser.currentState == EnvironmentState.InPlaying)
            {
                if (!EnableService)
                {
                    CurrentLoginUser.currentState = EnvironmentState.InEditing;
                    CurrentLoginUser.playSuccess = true;

                    outputString.AppendLine("");
                    outputString.AppendLine("程序执行输出结果...");
                    for (int i = 1; i <= 10; i++)
                        outputString.AppendLine("输出了 " + i.ToString() + "0%......");
                    outputString.AppendLine("全部输出已完成.");
                    outputString.AppendLine("");
                    SetupDemoData();
                    return;
                }

                System.Diagnostics.Debug.WriteLine("============= Upload Tick ===============");
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(BaseURL);
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var body = new FormUrlEncodedContent(new Dictionary<string, string> {
                        { "session", CurrentLoginUser.currentSessionId.ToString() },
                        { "upload_id", CurrentLoginUser.currentUploadId.ToString() }
                    });

                    // response
                    var response = httpClient.PostAsync(URIRunResultTick, body).Result;
                    var data = response.Content.ReadAsStringAsync().Result;
                    var formatData = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);
                    var result = (JObject)formatData["data"];
                    bool success = (int)result["fail"] == 0;
                    if (success)
                    {
                        int finishInt = (int)result["finish"];
                        string finishFlag = finishInt == 0 ? "未完成" : finishInt == 1 ? "成功" : "失败";
                        System.Diagnostics.Debug.WriteLine(">>>> Finished :" + finishFlag);
                        string resultInfo = result["result_json"].ToString();
                        System.Diagnostics.Debug.WriteLine("-------- Result info ----------------");
                        System.Diagnostics.Debug.WriteLine(resultInfo);

                        if (finishInt != 0)
                        {
                            CurrentLoginUser.currentState = EnvironmentState.InEditing;
                            CurrentLoginUser.playSuccess = (finishInt == 1);
                        }
                    }
                    else
                    {
                        CurrentLoginUser.currentState = EnvironmentState.InEditing;
                        CurrentLoginUser.playSuccess = false;
                    }
                }
            }
            else
            {
                outputString.AppendLine("ERROR：没有正在运行的程序.");
            }
        }

        private void SetupDemoData()
        {
            string oneStr;
            string line;
            for (int index = 0; index < 40; index++)
            {
                line = "500-";
                switch (index % 8)
                {
                    case 0:
                        oneStr = "10000000";
                        break;
                    case 1:
                        oneStr = "11000000";
                        break;
                    case 2:
                        oneStr = "11100000";
                        break;
                    case 3:
                        oneStr = "11110000";
                        break;
                    case 4:
                        oneStr = "11111000";
                        break;
                    case 5:
                        oneStr = "11111100";
                        break;
                    case 6:
                        oneStr = "11111110";
                        break;
                    case 7:
                        oneStr = "11111111";
                        break;
                    default:
                        oneStr = "00000000";
                        break;
                }
                switch (index / 8)
                {
                    case 0:
                        line += oneStr + "-00000000" + "-00000000" + "-00000000" + "-00000000";
                        break;
                    case 1:
                        line += "00000000-" + oneStr + "-00000000" + "-00000000" + "-00000000";
                        break;
                    case 2:
                        line += "00000000-" + "00000000-" + oneStr + "-00000000" + "-00000000";
                        break;
                    case 3:
                        line += "00000000-" + "00000000-" + "00000000-" + oneStr + "-00000000";
                        break;
                    case 4:
                        line += "00000000-" + "00000000-" + "00000000-" + "00000000-" + oneStr;
                        break;
                    default:
                        line += "00000000-" + "00000000-" + "00000000-" + "00000000-" + "00000000";
                        break;
                }
                animateString.AppendLine(line);
            }
        }

        #endregion
    }
}