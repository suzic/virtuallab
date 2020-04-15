using Pocoor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using virtuallab.API.Service.po;

namespace virtuallab.API.Service
{
    public class BhServiceMock : IBhApi
    {
        public EnvironmentRequestRes EnvironmentRequest(EnvironmentRequestReq req)
        {
            EnvironmentRequestRes e = new EnvironmentRequestRes();
            e.fail = 0;
            e.session_id = Guid.NewGuid().ToString("D");
            return e;
        }
        public CodeSubmitRes CodeSubmit(CodeSubmitReq req)
        {
            CodeSubmitRes e = new CodeSubmitRes();
            e.fail = 0;
            return e;
        }
        public CompileRes Compile(CompileReq req)
        {
            CompileRes e = new CompileRes();
            e.fail = 0;
            e.res = "编译成功";
            System.Threading.Thread.Sleep(1000);
            return e;
        }
        public DeviceRequestRes DeviceRequest(DeviceRequestReq req)
        {
            DeviceRequestRes e = new DeviceRequestRes();
            e.fail = 0;
            e.device_id = Guid.NewGuid().ToString("D");
            e.ssh_uuid = Guid.NewGuid().ToString("D");
            return e;
        }
        public ProgramUploadRes ProgramUpload(ProgramUploadReq req)
        {
            ProgramUploadRes e = new ProgramUploadRes();
            e.fail = 0;
            e.app_name = "zlg9290.obj";
            return e;
        }
        public ConsoleSendRes ConsoleSend(ConsoleSendReq req)
        {
            ConsoleSendRes e = new ConsoleSendRes();
            e.fail = 0;
            e.res = "您放大3D模型图，能看到数码管的动画显示效果";
            e.Continue = 1;
            return e;
        }
        public ConsoleReceiveRes ConsoleReceive(ConsoleReceiveReq req)
        {
            ConsoleReceiveRes e = RecieveMockData.GetRecieve();
            return e;
        }
        public RunResultTickRes runResultTick(ConsoleReceiveReq req)
        {
            RunResultTickRes e = RecieveMockData.GetRecieve2();
            return e;
        }
    }
}