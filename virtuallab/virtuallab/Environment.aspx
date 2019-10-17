<%@ Page Title="进行实验" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Environment.aspx.cs" Inherits="virtuallab.Environment" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <%--<script type="text/javascript" src="Scripts/environment.js"></script>--%>
    <script type="text/javascript">

        var inCompiling = "<%=(CurrentLoginUser.currentState == virtuallab.Models.EnvironmentState.InCompiling) %>";
        var inRunning = "<%=(CurrentLoginUser.currentState == virtuallab.Models.EnvironmentState.InPlaying) %>";
        var session_id = "<%=CurrentLoginUser.currentSessionId %>";
        var compile_id = "<%=CurrentLoginUser.currentCompileId %>";
        var upload_id = "<%=CurrentLoginUser.currentUploadId %>";
        var playOK = 0;
        var current_code = <%=currentCode %>;
        var scroll_pos = <%=currentPosTop %>;
        var timerId;
        var tickCount;

        $(document).ready(function () {
            editor = CodeMirror.fromTextArea(document.getElementById('code_text'), {
                mode: 'text/x-c++src',
                lineNumbers: true,
                theme: 'mdn-like',
                matchBrackets: true,
                identUnit: 4,
                smartIdent: true,
                indentWithTabs: true
            });
            editor.setSize('100%', '100%');
            outer = CodeMirror.fromTextArea(document.getElementById('debug_text'), {
                mode: 'textile',
                theme: 'zenburn',
                identUnit: 4,
                readOnly: true
            });
            outer.setSize('100%', '100%');

            editor.setValue(current_code);
            editor.scrollTo(0, scroll_pos);

            new Tab("#Tab");
        });

        // 上传代码编译接口调用，后端完成
        function submitCode() {
            var codeText = editor.getValue();
            var position = editor.getScrollInfo();
            __doPostBack("SUBMIT_CODE", JSON.stringify({ "code": codeText, "pos": position }));
        }

        // 上传程序接口调用，后端完成
        function uploadProgram() {
            var codeText = editor.getValue();
            var position = editor.getScrollInfo();
            __doPostBack("UPLOAD_PROGRAM", JSON.stringify({ "code": codeText, "pos": position }));
        }

        // 获取编译信息，前端完成
        function compileTick() {
            playOK = 0;
            tickCount = 0;
            clearInterval(timerId);
            timerId = setInterval("tickCompile()", 500);
            //var codeText = editor.getValue();
            //__doPostBack("COMPILE_TICK", codeText);
            //$.ajax({
            //    type: "POST",
            //    dataType: "json",
            //    url: "http://192.168.200.119:8088/address/" + "CompileResultTick",
            //    data: {
            //        "session_id": session_id,
            //        "compile_id": compile_id
            //    },
            //    success: function (data) {
            //        getDebugOutput(data);
            //    },
            //    error: function (er) {
            //    }
            //});
        }

        // 上传并获取运行效果，前端完成
        function runTick() {
            playOK = 0;
            tickCount = 0;
            clearInterval(timerId);
            timerId = setInterval("tickRunning()", 500);
            //var codeText = editor.getValue();
            //__doPostBack("RUN_TICK", codeText);
        }

        /// === Timer related ====
        function tickCompile() {
            if (tickCount == 0)
                setDebugOutput("\nIn compiling...\n");
            else if (tickCount > 10) {
                setDebugOutput("Completed.\n");
                clearInterval(timerId);
            }
            else {
                setDebugOutput("Progress " + tickCount.toString() + "0%......\n");
            }
            tickCount++;
        }

        function tickRunning() {
            if (tickCount == 0)
                setDebugOutput("\n开始将程序上传到板卡...\n");
            else if (tickCount > 10) {
                playOK = 1;
                setDebugOutput("已完成，请切换到板卡效果查看.\n");
                clearInterval(timerId);
            }
            else {
                setDebugOutput("上传完成了 " + tickCount.toString() + "0%......\n");
            }
            tickCount++;
        }

        function tickPlaying() {
            if (tickCount >= 30) {
                clearInterval(timerId);
                return;
            }
            var singleNum = tickCount % 10;
            var num = singleNum.toString();
            showDigit(num + num + num + num);
            tickCount++;
        }

        function setDebugOutput(data) {
            var oldData = outer.getValue();
            data = oldData + data;
            outer.setValue(data);
            var cur = outer.getCursor();
            outer.setCursor(outer.lastLine(), cur.ch);
        }

        /// === show images ======
        function getImg(num, stageW, stageH, index, total) {
            var w = 90;
            var h = 150;
            var wT = w * total;
            var hT = h;
            var top = (stageH - hT) / 2;
            // var left = (stageW - wT) / 2 + w * index;
            var left = 405 + w * index;
            var images = "<img src='Content/digit_num/" + num + ".png' style = 'position:absolute;";
            images += "left:" + left.toString() + "px;";
            images += "top:" + top.toString() + "px;";
            images += "width:" + w.toString() + "px;";
            images += "height:" + h.toString() + "px;";
            images += "' /> ";
            return images;
        }

        function showDigit(varNumber) {
            var stageW = $("#stage").width();
            var stageH = $("#stage").height();
            stageW = stageW < 780 ? 780 : stageW;
            var len = varNumber.length;
            var contentHtml = "";
            for (var i = 0; i < len; i++) {
                var num = varNumber.substr(i, 1);
                contentHtml += getImg(num, stageW, stageH, i, len);
            }
            $("#stage").html(contentHtml);
        }

        /// === JS Tab functions ====
        function Tab(tabId, active, tab) {
            this.init(tabId, active, tab);
        }

        Tab.prototype.init = function (tabId, active, tab) {
            tab = tab || {};
            this.titles = tab.titles || ".tab_title div";
            this.tabs = tab.tabs || ".tab_panel div";
            active = active || 0;
            var elem = document.querySelector(tabId);
            this.tabTitle = elem.querySelectorAll(this.titles);
            this.tabPanel = $(".one_tab");
            this.active(active);
            this.event();
        };

        Tab.prototype.active = function (index) {
            if (index === this.current)
                return;

            this.tabTitle[index].classList.add("active");
            this.tabPanel[index].classList.remove("deactive");
            this.tabPanel[index].classList.add("active");
            if (typeof this.current === "number") {
                this.tabTitle[this.current].classList.remove("active");
                this.tabPanel[this.current].classList.remove("active");
                this.tabPanel[this.current].classList.add("deactive");
                if (playOK == 1 && index == 2) {
                    tickCount = 0;
                    clearInterval(timerId);
                    timerId = setInterval("tickPlaying()", 1000);
                } else
                    showDigit("8888");
            }
            this.current = index;
        };

        Tab.prototype.event = function () {
            var len = this.tabTitle.length;
            var that = this;
            for (let i = 0; i < len; i++) {
                this.tabTitle[i].addEventListener("click", function () {
                    that.active.call(that, i);
                })
            }
        };

    </script>

    <div id="Tab" class="Tab row" style="position: relative; margin-top: 10px; margin-bottom: 10px;">
        <div class="tab_title">
            <div class="col-md-2">编辑调试</div>
            <div class="col-md-2">实验说明</div>
            <div class="col-md-2">板卡效果</div>
        </div>
        <div class="tab_panel">
            <div class="one_tab col-md-12 active">
                <div class="row" style="background-color:#ebebeb; position: relative; padding-top: 10px; padding-bottom: 10px;">
                    <div class="col-md-2">
                        <asp:Button ID="btnReload" runat="server" OnClick="ReloadCode" Text="重新加载模板代码" CssClass="btn btn-default form-control" />
                    </div>
                    <div class="col-md-4">
                        <asp:Label ID="lbGeneral" class="btn" runat="server" Text=""></asp:Label>
                    </div>
                    <div class="col-md-2">
                        <input id="btnSubmit" type="submit" value="提交编译" onclick="submitCode();" class="btn btn-default form-control" />
                    </div>
                    <div class="col-md-1">
                        <input id="btnCompileTick" value="CT" onclick="compileTick();" class="btn btn-default form-control" />
                    </div>
                    <div class="col-md-2">
                        <input id="btnUpload" type="submit" value="上传程序" onclick="uploadProgram();" class="btn btn-default form-control" />
                    </div>
                    <div class="col-md-1">
                        <input id="btnRunTick" value="RT" onclick="runTick();" class="btn btn-default form-control" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-8" style="height: 800px; padding-left: 0px; padding-right: 0px; border-style: solid; border-width: thin;">
                        <textarea id="code_text" class="form-control">
                    </textarea>
                    </div>
                    <div class="col-md-4" style="height: 800px; padding-left: 5px; padding-right: 0px">
                        <textarea id="debug_text" class="form-control"></textarea>
                    </div>
                </div>
            </div>
            <div class="one_tab deactive">
                <div class="col-md-12"; style="overflow-y: scroll; position: relative; height: 852px; background-color: #ebebeb;">
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
            <div class="one_tab deactive">
                <div id="stage" class="col-md-12" style="position: relative; min-width:780px; height: 852px; background-color: #ebebeb;">
                </div>
            </div>
        </div>
    </div>

</asp:Content>
