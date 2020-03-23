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
using virtuallab.Models;

namespace virtuallab
{
    /// <summary>
    /// 开发环境页面的后台代码
    /// </summary>
    public partial class Environment_old : System.Web.UI.Page
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
            get { return ((SiteMaster)Master).CurrentLoginUser; }
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

        public string tipInfo = "已接收数据，加载中......";
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
            InitCodeMirrorStyles();

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

        /// <summary>
        /// 学生每次实验将更新自己的任务数据
        /// </summary>
        protected void SaveRecordInfo()
        {
            string sConnString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            SqlConnection sSqlConn = new SqlConnection(sConnString);

            try
            {
                sSqlConn.Open();
                SqlCommand cmd = sSqlConn.CreateCommand();
                cmd.CommandText = "SELECT id_record FROM [bhRecord] WHERE (fid_task = @id_task and is_result = 1)";
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@id_task", SqlDbType.Int).Value = CurrentLoginUser.currentTaskId;
                var retId = cmd.ExecuteScalar();

                // 如果没有数据，那么新建一条记录
                if (retId == null)
                {
                    InsertRecord(cmd);
                }
                // 如果有数据，且数据的id和当前session id相同，表明是同一次实验，仅针对此次实验进行更新即可
                else if (retId.ToString().Equals(CurrentLoginUser.currentSessionId))
                {
                    UpdateRecord(cmd);
                }
                // 如果有数据，但是数据的id和当前session id不同，表明是再次实验，首先要将上次的is_result清除为0，然后新建数据
                else
                {
                    ResetOldRecord(cmd, retId.ToString());
                    InsertRecord(cmd);
                }
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

        protected void InsertRecord(SqlCommand cmd)
        {
            cmd.CommandText = "INSERT INTO bhRecord(id_record, fid_task, submit_times, final_code_uri, result_json_uri, finish_date, score, is_result) VALUES (@id, @task, @times, @code_uri, 'result_json_uri', @date, '0', 1)";
            cmd.Parameters.Clear();
            cmd.Parameters.Add("@id", SqlDbType.VarChar, 50).Value = CurrentLoginUser.currentSessionId;
            cmd.Parameters.Add("@task", SqlDbType.Int).Value = int.Parse(CurrentLoginUser.currentTaskId);
            cmd.Parameters.Add("@times", SqlDbType.Int).Value = 1;
            cmd.Parameters.Add("@code_uri", SqlDbType.VarChar, 256).Value = CurrentLoginUser.currentCodeUri;
            cmd.Parameters.Add("@date", SqlDbType.DateTime).Value = DateTime.Now;
            cmd.ExecuteNonQuery();

            // 只要插入了一个任务的完成记录，任务就设置为完成状态了。
            cmd.CommandText = "UPDATE bhTask SET complete = 1 WHERE (id_task = @task)";
            cmd.Parameters.Clear();
            cmd.Parameters.Add("@task", SqlDbType.Int).Value = int.Parse(CurrentLoginUser.currentTaskId);
            cmd.ExecuteNonQuery();
        }

        protected void UpdateRecord(SqlCommand cmd)
        {
            cmd.CommandText = "UPDATE bhRecord SET final_code_uri = @code_uri, submit_times = submit_times + 1, finish_date = @date WHERE (id_record = @id_record)";
            cmd.Parameters.Clear();
            cmd.Parameters.Add("@code_uri", SqlDbType.VarChar, 256).Value = CurrentLoginUser.currentCodeUri;
            cmd.Parameters.Add("@id_record", SqlDbType.VarChar, 50).Value = CurrentLoginUser.currentSessionId;
            cmd.Parameters.Add("@date", SqlDbType.DateTime).Value = DateTime.Now;
            cmd.ExecuteNonQuery();
        }

        protected void ResetOldRecord(SqlCommand cmd, string oldId)
        {
            cmd.CommandText = "UPDATE bhRecord SET is_result = 0 WHERE (id_record = @id_record)";
            cmd.Parameters.Clear();
            cmd.Parameters.Add("@id_record", SqlDbType.VarChar, 50).Value = oldId;
            cmd.ExecuteNonQuery();
        }

        #region 运行队列方法

        private LoginUser CurrentRunning()
        {
            if (runningQueue.Count == 0)
                return null;
            return runningQueue[0];
        }

        private int QueueLength()
        {
            return runningQueue.Count;
        }

        private void PushToQueue(LoginUser user)
        {
            runningQueue.Add(user);
        }

        private LoginUser PopFromQueue()
        {
            LoginUser user = runningQueue[0];
            runningQueue.RemoveAt(0);
            return user;
        }

        #endregion

        #region 网络请求方法

        // 初始化开发环境，获取session_id的网络请求
        protected void EnvironmentRequest()
        {
            CurrentLoginUser.currentState = EnvironmentState.NotReady;
            if (!EnableService)
            {
                CurrentLoginUser.currentState = EnvironmentState.InEditing;
                CurrentLoginUser.currentSessionId = ("Task_" + CurrentLoginUser.currentTaskId + "_" + Guid.NewGuid());
                return;
            }

            CurrentLoginUser.InError = 0;
            System.Diagnostics.Debug.WriteLine("============= Environment Request ===============");
            try
            {
                var httpClient = new HttpClient();
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
            catch
            {
                CurrentLoginUser.InError = 1;
                CurrentLoginUser.currentState = EnvironmentState.NotReady;
            }
        }

        // 代码提交，后台将会执行编译动作
        protected void CodeSubmit()
        {
            if (CurrentLoginUser.currentState == EnvironmentState.InEditing)
            {
                tipInfo = "正在等待远程主机编译结果返回......";
                CurrentLoginUser.currentState = EnvironmentState.InCompiling;
                CurrentLoginUser.compileSuccess = false;
                if (!EnableService)
                {
                    CurrentLoginUser.currentCompileId = "test_compile_sz19";
                    CurrentLoginUser.currentCodeUri = "test_code_uri_sz19";
                    SaveRecordInfo();
                    return;
                }

                CurrentLoginUser.InError = 0;
                System.Diagnostics.Debug.WriteLine("============= Code Submit ===============");
                System.Diagnostics.Debug.WriteLine(currentCode);
                try
                {
                    var httpClient = new HttpClient();
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
                        SaveRecordInfo();
                    }
                    else
                        CurrentLoginUser.currentState = EnvironmentState.InEditing;
                }
                catch
                {
                    CurrentLoginUser.InError = 1;
                    CurrentLoginUser.currentState = EnvironmentState.InEditing;
                    tipInfo = "您提交的代码无法编译，或当前编译服务无法连接。";
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
                    outputString.Append("\n");
                    outputString.Append("In compiling...\n");
                    for (int i = 1; i <= 10; i++)
                        outputString.Append("Progress " + i.ToString() + "0 % ......\n");
                    outputString.Append("Completed.\n");
                    outputString.Append("\n");
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
                        outputString.AppendFormat(infoBuffer);
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
                tipInfo = "正在等待远程主机上传结果返回......";
                CurrentLoginUser.currentState = EnvironmentState.InUploading;
                CurrentLoginUser.uploadSuccess = false;
                if (!EnableService)
                {
                    CurrentLoginUser.currentUploadId = "test_upload_id_sz19";
                    return;
                }

                CurrentLoginUser.InError = 0;
                System.Diagnostics.Debug.WriteLine("============= Program Uploading ===============");
                try
                {
                    var httpClient = new HttpClient();
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
                        //CurrentLoginUser.currentUploadId = result["upload_id"].ToString();
                        //System.Diagnostics.Debug.Write("-------- upload_id = " + CurrentLoginUser.currentUploadId + "\n");
                    }
                    else
                        CurrentLoginUser.currentState = EnvironmentState.InEditing;
                }
                catch
                {
                    CurrentLoginUser.InError = 1;
                    CurrentLoginUser.currentState = EnvironmentState.InEditing;
                    tipInfo = "您上传程序到板卡的请求服务未能完成，当前上传服务无法连接。";
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
                // NOTE: 现在暂时没有upload tick过程，因此无论是否EnableSevice均走模拟数据
                //if (!EnableService)
                {
                    CurrentLoginUser.currentState = EnvironmentState.InEditing;
                    CurrentLoginUser.uploadSuccess = true;
                    outputString.Append("\n");
                    outputString.Append("开始将程序上传到板卡...\n");
                    for (int i = 1; i <= 10; i++)
                        outputString.Append("上传完成了 " + i.ToString() + "0%......\n");
                    outputString.Append("已完成，请切换到板卡页面可以运行了.\n");
                    outputString.Append("\n");
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

                PushToQueue(CurrentLoginUser);
                if (CurrentRunning() == CurrentLoginUser)
                {
                    CurrentLoginUser.InWaiting = 0;
                    tipInfo = "你的程序已在执行，正在等待远程主机运行效果返回......";
                    if (!EnableService)
                    {
                        CurrentLoginUser.currentRunId = "test_run_id_sz19";
                        return;
                    }

                    CurrentLoginUser.InError = 0;
                    System.Diagnostics.Debug.WriteLine("============= Run Program ===============");
                    try
                    {
                        var httpClient = new HttpClient();
                        httpClient.BaseAddress = new Uri(BaseURL);
                        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        var body = new FormUrlEncodedContent(new Dictionary<string, string> {
                            { "session_id", CurrentLoginUser.currentSessionId.ToString() }
                        });

                        // response
                        var response = httpClient.PostAsync(URIRun, body).Result;
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
                    catch
                    {
                        CurrentLoginUser.InError = 1;
                        CurrentLoginUser.currentState = EnvironmentState.InEditing;
                        tipInfo = "您运行程序的请求未能完成，或当前运行服务无法连接。";
                    }
                }
                else
                {
                    CurrentLoginUser.InWaiting = (QueueLength() - 1);
                    tipInfo = "当前你还需要等待前面" + CurrentLoginUser.InWaiting + "个程序的运行完成.";
                }
            }
            else
            {
                outputString.AppendLine("ERROR：你还没有上传程序到板卡，当前无法运行。");
            }
        }

        // 运行结果心跳检查
        protected void RunTick()
        {
            if (CurrentRunning() != CurrentLoginUser)
            {
                CurrentLoginUser.playSuccess = false;
                CurrentLoginUser.InWaiting = (QueueLength() - 1);
                tipInfo = "当前你还需要等待前面" + CurrentLoginUser.InWaiting + "个程序的运行完成.";
            }
            else if (CurrentLoginUser.currentState == EnvironmentState.InPlaying)
            {
                // 如果当前用户还是等待状态，那么现在可以执行了。
                if (CurrentLoginUser.InWaiting > 0)
                {
                    CurrentLoginUser.currentState = EnvironmentState.InEditing;
                    RunPlay();
                    return;
                }

                if (!EnableService)
                {
                    CurrentLoginUser.currentState = EnvironmentState.InEditing;
                    CurrentLoginUser.playSuccess = true;
                    PopFromQueue(); // 运行完成时，移出队列

                    outputString.Append("\n");
                    outputString.Append("程序执行输出结果...\n");
                    for (int i = 1; i <= 10; i++)
                        outputString.Append("输出了 " + i.ToString() + "0%......\n");
                    outputString.Append("全部输出已完成.\n");
                    outputString.Append("\n");
                    SetupDemoData();
                    return;
                }

                CurrentLoginUser.InError = 0;
                System.Diagnostics.Debug.WriteLine("============= Run Tick ===============");
                try
                {
                    var httpClient = new HttpClient();
                    httpClient.BaseAddress = new Uri(BaseURL);
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var body = new FormUrlEncodedContent(new Dictionary<string, string> {
                        { "session", CurrentLoginUser.currentSessionId.ToString() },
                        { "upload_id", CurrentLoginUser.currentRunId.ToString() }
                    });

                    // response
                    var response = httpClient.PostAsync(URIRunResultTick, body).Result;
                    var data = response.Content.ReadAsStringAsync().Result;
                    var formatData = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);
                    bool hasContent = formatData.ContainsKey("data");
                    if (!hasContent)
                    {
                        System.Diagnostics.Debug.WriteLine(">>>> Finished : NO Content DATA");
                        return;
                    }
                    var result = (JObject)formatData["data"];
                    bool success = (int)result["fail"] == 0;
                    if (success)
                    {
                        int finishInt = (int)result["finish"];
                        string finishFlag = finishInt == 0 ? "未完成" : finishInt == 1 ? "成功" : "失败";
                        System.Diagnostics.Debug.WriteLine(">>>> Finished :" + finishFlag);
                        var effectInfo = (JArray)result["effect"];
                        var consoleInfo = (JArray)result["console"];
                        System.Diagnostics.Debug.WriteLine("-------- Result info ----------------");
                        // 创建动画描述信息animateString，前端页面通过调用属性runResultFormatted获得animateString的可运行效果
                        SetupAnimationData(effectInfo);
                        foreach (var info in effectInfo)
                            System.Diagnostics.Debug.WriteLine("EFFECT: " + info.ToString());
                        SetupOutputData(consoleInfo);
                        foreach (var text in consoleInfo)
                            System.Diagnostics.Debug.WriteLine("CONSOLE: " + text.ToString());

                        if (finishInt != 0)
                        {
                            CurrentLoginUser.currentState = EnvironmentState.InEditing;
                            CurrentLoginUser.playSuccess = (finishInt == 1);
                            PopFromQueue(); // 运行完成时，移出队列，无论成功还是失败
                        }
                        // 如果没有运行完成，则不要动队列。
                        tipInfo = "你的程序已在执行，正在等待远程主机运行效果返回......";
                    }
                    else
                    {
                        CurrentLoginUser.currentState = EnvironmentState.InEditing;
                        CurrentLoginUser.playSuccess = false;
                        PopFromQueue(); // 操作失败，移出队列
                    }
                }
                catch
                {
                    CurrentLoginUser.InError = 1;
                    CurrentLoginUser.currentState = EnvironmentState.InEditing;
                    CurrentLoginUser.playSuccess = false;
                    outputString.AppendLine("ERROR:运行的程序发生异常，没能查询到结果。");
                    PopFromQueue(); // 网络请求失败的情况下，移出队列
                }
            }
            else
            {
                outputString.AppendLine("ERROR：没有正在运行的程序.");
            }
        }

        private void SetupAnimationData(JArray data)
        {
            animateString.Clear();
            string line;
            foreach (JObject info in data)
            {
                line = info["wait"].ToString();
                var digits = info["value"].ToString().Split(',');
                foreach (string part in digits)
                    line += "-" + part;
                animateString.AppendLine(line);
            }
        }

        private void SetupOutputData(JArray data)
        {

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
