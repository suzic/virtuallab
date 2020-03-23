using Common;
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


        R PostJson<Q, R>(string path, Q req) {
            string reqString = JsonConvert.SerializeObject(req);
            string resultStriing = Http.PostJson(BaseURL + path, reqString);
            R res = JsonConvert.DeserializeObject<R>(resultStriing);
            return res;
        }
    }
}