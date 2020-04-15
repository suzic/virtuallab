using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using virtuallab.API.Service.po;
using virtuallab.API.Service;
using virtuallab.API.po;
using virtuallab.Common;
using virtuallab.Models;

namespace virtuallab.API
{
    public class BhController : ApiController
    {
        IBhApi service;
        public BhController()
        {
            service = GetService();
        }
        public static IBhApi GetService() {
            if (System.Configuration.ConfigurationManager.AppSettings["EnableService"] == "1")
                return new BhService();
            else
                return new BhServiceMock();
        }

        [HttpGet, HttpPost]
        public string Test()
        {
            return DateTime.Now.ToString();
        }

        [HttpPost]
        public EnvironmentRequestRes EnvironmentRequest(EnvironmentRequestReq req)
        {
            return service.EnvironmentRequest(req);
        }
        [HttpPost]
        public CodeSubmitRes CodeSubmit(CodeSubmitReq req)
        {
            return service.CodeSubmit(req);
        }
        [HttpPost]
        public CompileRes Compile(ControllerCodeSubmitReq req)
        {
            CompileReq r = new CompileReq();
            r.session_id = req.session_id;
            CompileRes res = service.Compile(r);

            //编译成功保存bhRecord
            if (res.fail == 0)
            {
                if (System.Configuration.ConfigurationManager.AppSettings["EnableService"] == "1")
                    DB.SaveRecordInfo(req);

                LoginUser u = (LoginUser)System.Web.HttpContext.Current.Session["user"];
                u.currentState = (EnvironmentState)2;
            }
            else {
                LoginUser u = (LoginUser)System.Web.HttpContext.Current.Session["user"];
                u.currentState = (EnvironmentState)1;
            }
            return res;
        }
        [HttpPost]
        public DeviceRequestRes DeviceRequest(DeviceRequestReq req)
        {
            DeviceRequestRes res= service.DeviceRequest(req);

            //请求设备成功保存device_id
            if (res.fail == 0&& !string.IsNullOrWhiteSpace(res.device_id))
            {
                LoginUser u = (LoginUser)System.Web.HttpContext.Current.Session["user"];
                u.device_id = res.device_id;
                u.ssh_uuid = res.ssh_uuid;
                u.currentState = (EnvironmentState)3;
            }
            return res;
        }
        [HttpPost]
        public ProgramUploadRes ProgramUpload(ProgramUploadReq req)
        {
            ProgramUploadRes res= service.ProgramUpload(req); 

            //请求设备成功保存device_id
            if (res.fail == 0 && !string.IsNullOrWhiteSpace(res.app_name))
            {
                LoginUser u = (LoginUser)System.Web.HttpContext.Current.Session["user"];
                u.app_name = res.app_name;
                u.currentState = (EnvironmentState)4;
            }
            return res;
        }
        [HttpPost]
        public ConsoleSendRes ConsoleSend(ConsoleSendReq req)
        {
            return service.ConsoleSend(req); 
        }
        [HttpPost]
        public ConsoleReceiveRes ConsoleReceive(ConsoleReceiveReq req)
        {
            return service.ConsoleReceive(req); 
        }
        [HttpPost]
        public RunResultTickRes runResultTick(ConsoleReceiveReq req)
        {
            return service.runResultTick(req);
        }
    }
}