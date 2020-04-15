using Newtonsoft.Json;
using Pocoor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using virtuallab.API.Service.po;

namespace virtuallab.API.Service
{
    public class BhService:IBhApi
    {
        static string BaseURL = System.Configuration.ConfigurationManager.AppSettings["BaseURL"].TrimEnd('/')+"/";
        public EnvironmentRequestRes EnvironmentRequest(EnvironmentRequestReq req)
        {
            return PostJson<EnvironmentRequestReq,EnvironmentRequestRes>("EnvironmentRequest",req);
        }
        public CodeSubmitRes CodeSubmit(CodeSubmitReq req)
        {
            return PostJson<CodeSubmitReq, CodeSubmitRes>("CodeSubmit", req);
        }
        public CompileRes Compile(CompileReq req)
        {
            return PostJson<CompileReq, CompileRes>("compile", req);
        }
        public DeviceRequestRes DeviceRequest(DeviceRequestReq req)
        {
            return PostJson<DeviceRequestReq, DeviceRequestRes>("DeviceRequest", req);
        }
        public ProgramUploadRes ProgramUpload(ProgramUploadReq req)
        {
            return PostJson<ProgramUploadReq, ProgramUploadRes>("ProgramUpload", req);
        }
        public ConsoleSendRes ConsoleSend(ConsoleSendReq req)
        {
            return PostJson<ConsoleSendReq, ConsoleSendRes>("ConsoleSend", req);
        }
        public ConsoleReceiveRes ConsoleReceive(ConsoleReceiveReq req)
        {
            return PostJson<ConsoleReceiveReq, ConsoleReceiveRes>("ConsoleReceive", req);
        }
        public RunResultTickRes runResultTick(ConsoleReceiveReq req)
        {
            return PostJson<ConsoleReceiveReq, RunResultTickRes>("runResultTick", req);
        }



        R PostJson<Q, R>(string path, Q req)
        {
            string resultStriing = Post(path, toDict(req));
            R res = JsonConvert.DeserializeObject<R>(resultStriing);
            return res;
        }
        static string Post(string path, Dictionary<string, string> dict) {
            var httpClient = new System.Net.Http.HttpClient();
            httpClient.BaseAddress = new Uri(BaseURL);
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            var body = new System.Net.Http.FormUrlEncodedContent(dict);

            // response
            var response = httpClient.PostAsync(path, body).Result;
            var data = response.Content.ReadAsStringAsync().Result;
            return data;

        }
        static Dictionary<string, string> toDict(object obj) {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            if (obj != null)
            {
                Type type = obj.GetType();
                object value;
                foreach (var p in type.GetProperties())
                {
                    value = p.GetValue(obj);
                    dict.Add(p.Name, value==null?"":value.ToString());
                }
            }
            return dict;
        }
    }
}