using Pocoor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using virtuallab.API.Service.po;

namespace virtuallab.API.Service
{
    public interface IBhApi
    {
        EnvironmentRequestRes EnvironmentRequest(EnvironmentRequestReq req);
        CodeSubmitRes CodeSubmit(CodeSubmitReq req);
        CompileRes Compile(CompileReq req);
        DeviceRequestRes DeviceRequest(DeviceRequestReq req);
        ProgramUploadRes ProgramUpload(ProgramUploadReq req);
        ConsoleSendRes ConsoleSend(ConsoleSendReq req);
        ConsoleReceiveRes ConsoleReceive(ConsoleReceiveReq req);
    }
}