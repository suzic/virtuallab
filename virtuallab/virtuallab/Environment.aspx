<%@ Page Title="进行实验" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Environment.aspx.cs" Inherits="virtuallab.Environment" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript" src="Scripts/environment.js"></script>
    <script type="text/javascript">
        function submitCode() {
            var codeText = editor.getValue();
            __doPostBack("SUBMIT_CODE", codeText);
        }
        function uploadProgram() {
            __doPostBack("UPLOAD_PROGRAM", "");
        }
        function compileTick() {
            __doPostBack("COMPILE_TICK", "");
        }
        function runTick() {
            __doPostBack("RUN_TICK", "");
        }
    </script>
    <asp:MultiView ID="EnvironmentView" runat="server" ActiveViewIndex="0">
        <asp:View ID="CodeView" runat="server">
            <div class="row" style="position: relative; margin-top: 20px; margin-bottom: 20px;">
                <%--<div class="col-md-2">
                    <asp:Button runat="server" OnClick="ReloadCode" Text="重新加载模板代码" CssClass="btn btn-default form-control" />
                </div>--%>
                <div class="col-md-2">
                    <input type="file" name="codeFile" id="codeFileID" onchange="loadCodeFile(this)" class="form-control" />
                </div>
                <div class="col-md-2">
                    <asp:Label ID="lbGeneral" runat="server" Text=""></asp:Label>
                </div>
                <div class="col-md-2">
                    <input id="btnSubmit" type="submit" value="提交编译" onclick="submitCode();" class="btn btn-default form-control" />
                </div>
                <div class="col-md-2">
                    <input id="btnCompileTick" type="submit" value="编译Tick" onclick="compileTick();" class="btn btn-default form-control" />
                </div>
                <div class="col-md-2">
                    <input id="btnUpload" type="submit" value="上传程序" onclick="uploadProgram();" class="btn btn-default form-control" />
                </div>
                <div class="col-md-2">
                    <input id="btnRunTick" type="submit" value="上传Tick" onclick="runTick();" class="btn btn-default form-control" />
                </div>
            </div>
            <div class="row">
                <div class="col-md-8" style="height: 800px; padding-right: 0px; border-style: solid; border-width: thin;">
                    <textarea id="code_text" class="form-control">
                    </textarea>
                </div>
                <div class="col-md-4" style="height: 800px; padding-left: 5px;">
                    <textarea id="debug_text" class="form-control"></textarea>
                </div>
            </div>
        </asp:View>
        <asp:View ID="ModelView" runat="server">
            <div class="row">
                <div class="col-md-12" style="position: relative; margin-top: 20px; height: 852px; background-color: lightgray; border-style: solid; border-width: thin; border-color: darkgray">
                </div>
            </div>
        </asp:View>
        <asp:View ID="IntroView" runat="server">
            <h2>实验说明</h2>
            <hr />
            <div class="row">
                <div class="col-md-12" style="overflow-y: scroll; position: relative; height: 780px; background-color: whitesmoke; border-style: solid; border-width: thin; border-color: lightgray">
                    <h4>实验步骤1 内容简介</h4>
                    阅读实验原理，了解zlg7290的读写流程和I2C总线的使用方法<br />
                    根据实验步骤完成对zlg7290的读写程序设计和验证<br />
                    <br />
                    <h4>实验步骤2 实验目的</h4>
                    了解zlg7290的控制流程</h4>掌握使用I2C总线读写zlg7290的静态驱动程序设计方法<br />
                    <br />
                    <h4>实验环境</h4>
                    硬件：装有Linux操作系统的开发板<br />
                    软件：Ubuntu12.0，IDE，putty<br />
                    <br />
                    <h4>实验原理</h4>
                    【ZLG7290介绍】 ZLG7290 是广州周立功单片机发展有限公司自行设计的数码管显示驱动及键盘扫描管理芯片。能够直接驱动8位共阴极数码管（或64只独立的LED），同时还可以扫面管理多大64只按键。其中有8只按键可以作为功能键使用，就像电脑键盘上的Ctrl，Shift、Alt键一样。另外ZLG7290 内部还设有连击计数器，能够使某键按下后不松手而连续有效。该芯片为工业级芯片，抗干扰能力强，在工业测控中已有大量应用。该器件通过I2C总线接口进行操作，ZLG7290引脚图如图1。 
                    <br />
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Content/board.png" />
                    <br />
                    <h4>表 11.4说明了ZLG7290各引脚的功能。</h4>
                    <table style="width: 100%;" class="table">
                        <tr>
                            <td>引脚序号</td>
                            <td>引脚名</td>
                            <td>功能</td>
                        </tr>
                        <tr>
                            <td>1</td>
                            <td>SC/KR2</td>
                            <td>数码管c 段／键盘行信号2</td>
                        </tr>
                        <tr>
                            <td>2</td>
                            <td>SD/KR3</td>
                            <td>数码管d 段／键盘行信号3</td>
                        </tr>
                        <tr>
                            <td>3</td>
                            <td>DIG3/KC3</td>
                            <td>数码管位选信号3／键盘列信号3</td>
                        </tr>
                        <tr>
                            <td>4</td>
                            <td>DIG2/KC2</td>
                            <td>数码管位选信号2／键盘列信号2</td>
                        </tr>
                    </table>
                    <br />
                    数码管驱动zlg7290.c实现了设备文件操作控制，用户态可以调用zlg7290_hw_write()，zlg7290_hw_read()和zlg_led_ioctl()函数来对数码管进行读写操作，阅读ZLG7290数码管驱动的代码程序清单1.1，了解其实现的具体方法。
                </div>
            </div>
        </asp:View>
    </asp:MultiView>
    <div class="row" style="position: relative; margin-top: 20px; height: 50px;">
        <div class="col-md-2">
            <asp:Button ID="btnExp" runat="server" OnClick="SwitchViewToIntro" Text="实验说明" CssClass="btn btn-default form-control" />
        </div>
        <div class="col-md-2">
            <asp:Button ID="btnCode" type="button" runat="server" OnClick="SwitchViewToCode" Text="代码编辑" CssClass="btn btn-default form-control" Enabled="False" />
        </div>
        <div class="col-md-6"></div>
        <div class="col-md-2">
            <asp:Button ID="btnBoard" runat="server" OnClick="SwitchViewToBoard" Text="板卡效果" CssClass="btn btn-default form-control" />
        </div>
    </div>

</asp:Content>
