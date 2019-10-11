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
        public LoginUser CurrentLoginUser;
        public string currentCode;

        public const string BaseURL = "http://192.168.200.119:8088/address/";
        public const string URIEnvironmentRequest = "environmentRequest";
        public const string URICodeSubmit = "codeSubmit";
        public const string URIProgramUpload = "ProgramUpload";
        public const string URICompileResultTick = "CompileResultTick";
        public const string URIRunResultTick = "RunResultTick";

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

        // 页面加载，该方法先获取可用的目标开发环境；同时处理可能来自前端的接口方法回调，并启动计时器进行Tick操作
        protected void Page_Load(object sender, EventArgs e)
        {
            // 进入页面首先获取session_id，并且确保当前用户状态不是NotReady
            if (string.IsNullOrEmpty(CurrentLoginUser.currentSessionId)
                || CurrentLoginUser.currentState == EnvironmentState.NotReady)
            {
                EnvironmentRequest();

                // 在请求session_id之后如果仍然还是空，说明服务器暂时无法提供连接
                if (string.IsNullOrEmpty(CurrentLoginUser.currentSessionId))
                    Response.Redirect("~/NotReady");
            }
            // 如果不需要获取session_id的情况下，检查是否是前端操作的回调，并执行对应的动作命令
            else
            {
                String key = Request.Form["__EVENTTARGET"];
                if (!string.IsNullOrEmpty(key) && key.Equals("SUBMIT_CODE"))
                {
                    currentCode = Request.Form["__EVENTARGUMENT"];
                    CodeSubmit(); 
                }
                else if (!string.IsNullOrEmpty(key) && key.Equals("UPLOAD_PROGRAM"))
                {
                    ProgramUpload();
                }
                else if (!string.IsNullOrEmpty(key) && key.Equals("COMPILE_TICK"))
                {
                    CompileTick();
                }
                else if (!string.IsNullOrEmpty(key) && key.Equals("RUN_TICK"))
                {
                    UploadTick();
                }
            }
        }

        // 重新加载模板代码
        protected void ReloadCode(object sender, EventArgs e)
        {
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

        #region 网络请求方法

        // 初始化开发环境，获取session_id的网络请求
        protected void EnvironmentRequest()
        {
            CurrentLoginUser.currentState = EnvironmentState.NotReady;

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

                System.Diagnostics.Debug.WriteLine("============= Code Submit ===============");
                System.Diagnostics.Debug.WriteLine(currentCode);
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(BaseURL);
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var body = new FormUrlEncodedContent(new Dictionary<string, string> {
                        { "session_id", CurrentLoginUser.currentSessionId.ToString() },
                        { "code",  currentCode }
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
        }

        // 编译结果心跳检查
        protected void CompileTick()
        {
            if (CurrentLoginUser.currentState == EnvironmentState.InCompiling)
            {
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
        }

        // 代码上传到板卡
        protected void ProgramUpload()
        {
            if (CurrentLoginUser.currentState == EnvironmentState.InEditing && CurrentLoginUser.compileSuccess)
            {
                CurrentLoginUser.currentState = EnvironmentState.InUploading;
                CurrentLoginUser.uploadSuccess = false;

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
        }

        // 上传结果心跳检查
        protected void UploadTick()
        {
            if (CurrentLoginUser.currentState == EnvironmentState.InUploading)
            {
                System.Diagnostics.Debug.WriteLine("============= Run Tick ===============");
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
                        if (finishInt != 0)
                        {
                            CurrentLoginUser.currentState = EnvironmentState.InEditing;
                            CurrentLoginUser.uploadSuccess = (finishInt == 1);
                            if (CurrentLoginUser.uploadSuccess)
                            {
                                string resultInfo = result["result_json"].ToString();
                                System.Diagnostics.Debug.WriteLine("-------- Result info ----------------");
                                System.Diagnostics.Debug.WriteLine(resultInfo);
                            }
                        }
                    }
                    else
                    {
                        CurrentLoginUser.currentState = EnvironmentState.InEditing;
                        CurrentLoginUser.uploadSuccess = false;
                    }
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