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
            return e;
        }
        public CodeSubmitRes CodeSubmit(CodeSubmitReq req)
        {
            CodeSubmitRes e = new CodeSubmitRes();
            e.fail = 0;
            e.info_buffer = "编译成功";
            System.Threading.Thread.Sleep(3000);
            return e;
        }
        public DeviceRequestRes DeviceRequest(DeviceRequestReq req)
        {
            DeviceRequestRes e = new DeviceRequestRes();
            e.fail = 0;
            e.device_id = Guid.NewGuid().ToString("D");
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
            e.finish = 0;
            e.output = "您放大3D模型图，能看到数码管的动画显示效果";
            return e;
        }
        public ConsoleReceiveRes ConsoleReceive(ConsoleReceiveReq req)
        {
            ConsoleReceiveRes e = RecieveMockData.GetRecieve();
            return e;
        }
    }
}