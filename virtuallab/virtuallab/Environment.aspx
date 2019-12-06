<%@ Page Title="进行实验" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Environment.aspx.cs" Inherits="virtuallab.Environment" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">

        var session_id = "<%=CurrentLoginUser.currentSessionId %>";
        var compile_id = "<%=CurrentLoginUser.currentCompileId %>";
        var upload_id = "<%=CurrentLoginUser.currentUploadId %>";
        var run_id = "<%=CurrentLoginUser.currentRunId %>";

        var cm_editor;
        var cm_outer;
        var cm_console;
        var current_code = <%=currentCode %>;
        var scroll_pos = <%=currentPosTop %>;
        var default_tab = <%=defaultTab %>;
        var output_string = <%=outputFormatted %>;
        var animate_string = <%=runResultFormatted %>;

        var inError = <%=InError %>;
        var inCompiling = "<%=(CurrentLoginUser.currentState == virtuallab.Models.EnvironmentState.InCompiling) %>";
        var inUploading = "<%=(CurrentLoginUser.currentState == virtuallab.Models.EnvironmentState.InUploading) %>";
        var inRunning = "<%=(CurrentLoginUser.currentState == virtuallab.Models.EnvironmentState.InPlaying) %>";

        var layer_mask;
        var timerImageId;
        var timerId;
        var timerTextId;
        var tickImageCount;
        var tickTextCount;
        var tickStringArray;
        var tickImageArray;

        $(document).ready(function () {
            initCodeEditor();
            new Tab("#Tab", default_tab);
            showWaitingLayers();
        });

        function initCodeEditor() {
            cm_editor = CodeMirror.fromTextArea(document.getElementById('code_text'), {
                mode: 'text/x-c++src',
                lineNumbers: true,
                theme: 'mdn-like',
                matchBrackets: true,
                identUnit: 4,
                smartIdent: true,
                indentWithTabs: true
            });
            cm_editor.setSize('100%', '100%');
            cm_outer = CodeMirror.fromTextArea(document.getElementById('debug_text'), {
                mode: 'textile',
                theme: 'zenburn',
                identUnit: 4,
                readOnly: true
            });
            cm_outer.setSize('100%', '100%');
            cm_console = CodeMirror.fromTextArea(document.getElementById('run_console'), {
                mode: 'textile',
                theme: 'zenburn',
                identUnit: 4,
                readOnly: true
            });
            cm_console.setSize('100%', '100%');

            cm_editor.setValue(current_code);
            cm_editor.scrollTo(0, scroll_pos);
        }

        function startAnimation() {
            if (output_string != null && output_string.length > 0) {
                tickStringArray = output_string.split("\n");
                tickTextCount = 0;
                clearInterval(timerTextId);
                if (default_tab == 2) {
                    timerTextId = setInterval("consoleTick()", 100);
                }
                else
                    timerTextId = setInterval("outerTick()", 100);
            }
            if (animate_string != null && animate_string.length > 0) {
                if (default_tab == 2) {
                    tickImageArray = animate_string.split("\n");
                    tickImageCount = 0;
                    clearTimeout(timerImageId);
                    timerImageId = setTimeout("tickRunning()", 2000);
                }
            }
        }

        function showWaitingLayers() {
            layer_mask = document.getElementById('mask');
            tickCount = 0;
            clearTimeout(timerId);

            if (inError != 0) {
                layer_mask.innerHTML = '您提交的代码无法编译，或当前编译服务无法连接。' + '<br/>'
                timerId = setTimeout(function () {
                    layer_mask.style.display = "none";
                    startAnimation();
                }, 2000);
            }
            else if (inCompiling == "True") {
                layer_mask.innerHTML = '正在等待远程主机编译结果返回......' + '<br/>'
                    + '<div class="col-md-12" style="align-content:center;position:absolute;top:400px;"><input type="button" value="放弃等待" onclick="abortWaiting();" class="btn btn-default form-control" style="width:200px;"/></div>';
                timerId = setTimeout(function () {
                    compileTick();
                }, 5000);
                //layer_mask.onclick = function () {
                //    clearTimeout(timerId);
                //    layer_mask.style.display = "none";
                //    compileTick();
                //}
            }
            else if (inUploading == "True") {
                layer_mask.innerHTML = '正在等待远程主机上传结果返回......' + '<br/>'
                    + '<div class="col-md-12" style="align-content:center;position:absolute;top:400px;"><input type="button" value="放弃等待" onclick="abortWaiting();" class="btn btn-default form-control" style="width:200px;"/></div>';
                timerId = setTimeout(function () {
                    uploadTick();
                }, 5000);
                //layer_mask.onclick = function () {
                //    clearTimeout(timerId);
                //    layer_mask.style.display = "none";
                //    uploadTick();
                //}
            }
            else if (inRunning == "True") {
                layer_mask.innerHTML = '正在等待远程主机运行效果返回......' + '<br/>'
                    + '<div class="col-md-12" style="align-content:center;position:absolute;top:400px;"><input type="button" value="放弃等待" onclick="abortWaiting();" class="btn btn-default form-control" style="width:200px;"/></div>';
                timerId = setTimeout(function () {
                    runTick();
                }, 5000);
                //layer_mask.onclick = function () {
                //    clearTimeout(timerId);
                //    layer_mask.style.display = "none";
                //    runTick();
                //}
            }
            else {
                layer_mask.innerHTML = "已接收数据，加载中......";
                timerId = setTimeout(function () {
                    layer_mask.style.display = "none";
                    startAnimation();
                }, 2000);
                layer_mask.onclick = function () {
                    clearTimeout(timerId);
                    layer_mask.style.display = "none";
                    startAnimation();
                }
            }
        }

        function abortWaiting() {
            clearTimeout(timerId);
            layer_mask.style.display = "none";
        }

        // 上传代码编译接口调用
        function submitCode() {
            var codeText = cm_editor.getValue();
            var position = cm_editor.getScrollInfo();
            __doPostBack("SUBMIT_CODE", JSON.stringify({ "code": codeText, "pos": position, "tab": 0 }));
        }

        // 上传程序接口调用
        function uploadProgram() {
            var codeText = cm_editor.getValue();
            var position = cm_editor.getScrollInfo();
            __doPostBack("UPLOAD_PROGRAM", JSON.stringify({ "code": codeText, "pos": position, "tab": 0 }));
        }

        // 播放运行接口调用
        function runPlay() {
            var codeText = cm_editor.getValue();
            var position = cm_editor.getScrollInfo();
            __doPostBack("RUN_PLAY", JSON.stringify({ "code": codeText, "pos": position, "tab": 2 }));
        }

        // 获取编译信息的过程
        function compileTick() {
            var codeText = cm_editor.getValue();
            var position = cm_editor.getScrollInfo();
            __doPostBack("COMPILE_TICK", JSON.stringify({ "code": codeText, "pos": position, "tab": 0 }));
        }

        // 上传程序到板卡的过程
        function uploadTick() {
            var codeText = cm_editor.getValue();
            var position = cm_editor.getScrollInfo();
            __doPostBack("UPLOAD_TICK", JSON.stringify({ "code": codeText, "pos": position, "tab": 0 }));
        }

        // 执行动画的过程
        function runTick() {
            var codeText = cm_editor.getValue();
            var position = cm_editor.getScrollInfo();
            __doPostBack("RUN_TICK", JSON.stringify({ "code": codeText, "pos": position, "tab": 2 }));
        }

        ///=========================== Timer related =================================
        function outerTick() {
            if (tickTextCount >= tickStringArray.length) {
                clearInterval(timerTextId);
                tickTextCount = 0;
                return;
            }
            var line = tickStringArray[tickTextCount];
            setOutputAppend(cm_outer, line);
            tickTextCount++;
        }

        function consoleTick() {
            if (tickTextCount >= tickStringArray.length) {
                clearInterval(timerTextId);
                tickTextCount = 0;
                return;
            }
            var line = tickStringArray[tickTextCount];
            setOutputAppend(cm_console, line);
            tickTextCount++;
        }

        function tickRunning() {
            if (tickImageCount >= tickImageArray.length) {
                clearTimeout(timerImageId);
                tickImageCount = 0;
                return;
            }
            var line = tickImageArray[tickImageCount];
            var one = line.split("-");
            if (one == null || one.length < 6)
                return;
            showDigit(one[1], one[2], one[3], one[4], one[5]);
            var gap = parseInt(one[0]);
            if (gap == null || gap < 100)
                gap = 100;
            tickImageCount++;
            timerImageId = setTimeout("tickRunning()", gap);
        }

        function setOutputAppend(target, data) {
            var oldData = target.getValue();
            data = oldData + "\n" + data;
            target.setValue(data);
            var cur = target.getCursor();
            target.setCursor(target.lastLine(), cur.ch);
        }

        ///============================ show images ==================================
        function getImgNum(group, index, show) {
            var w = 25;
            var h = 34.5;
            var t = 515.5;
            var l = 567 + 22 * group;
            var images = "<img src='Content/digit_num/p" + index + ".png' style = 'position:absolute;";
            images += "display:" + (show == "1" ? "block;" : "none;");
            images += "left:" + l.toString() + "px;";
            images += "top:" + t.toString() + "px;";
            images += "width:" + w.toString() + "px;";
            images += "height:" + h.toString() + "px;";
            images += "' /> ";
            return images;
        }

        function getImgLight(index, show) {
            var images = "";
            switch (index) {
                case 0:
                    var images = "<img src='Content/digit_num/light.png' style = 'position:absolute;left:490px;top:721.5px;width:103px;height:50px;display:" + (show == "1" ? "block;" : "none;") + "' /> ";
                    break;
                case 1:
                    var images = "<img src='Content/digit_num/light.png' style = 'position:absolute;left:535px;top:721px;width:103px;height:50px;display:" + (show == "1" ? "block;" : "none;") + "' /> ";
                    break;
                case 2:
                    var images = "<img src='Content/digit_num/light.png' style = 'position:absolute;left:578.5px;top:722.5px;width:103px;height:50px;display:" + (show == "1" ? "block;" : "none;") + "' /> ";
                    break;
                case 3:
                    var images = "<img src='Content/digit_num/light.png' style = 'position:absolute;left:620.5px;top:720.5px;width:103px;height:50px;display:" + (show == "1" ? "block;" : "none;") + "' /> ";
                    break;
                case 4:
                    var images = "<img src='Content/digit_num/light.png' style = 'position:absolute;left:492.5px;top:767px;width:103px;height:50px;display:" + (show == "1" ? "block;" : "none;") + "' /> ";
                    break;
                case 5:
                    var images = "<img src='Content/digit_num/light.png' style = 'position:absolute;left:534px;top:767.5px;width:103px;height:50px;display:" + (show == "1" ? "block;" : "none;") + "' /> ";
                    break;
                case 6:
                    var images = "<img src='Content/digit_num/light.png' style = 'position:absolute;left:578.5px;top:768px;width:103px;height:50px;display:" + (show == "1" ? "block;" : "none;") + "' /> ";
                    break;
                case 7:
                    var images = "<img src='Content/digit_num/light.png' style = 'position:absolute;left:621.5px;top:769px;width:103px;height:50px;display:" + (show == "1" ? "block;" : "none;") + "' /> ";
                    break;
            }
            return images;
        }

        function showDigit(code1, code2, code3, code4, code5) {
            var contentHtml = "";
            for (var i = 0; i < 8; i++) {
                var num = code2.substr(i, 1);
                contentHtml += getImgNum(0, i, num);
            }
            for (var j = 0; j < 8; j++) {
                var num = code3.substr(j, 1);
                contentHtml += getImgNum(1, j, num);
            }
            for (var m = 0; m < 8; m++) {
                var num = code4.substr(m, 1);
                contentHtml += getImgNum(2, m, num);
            }
            for (var n = 0; n < 8; n++) {
                var num = code5.substr(n, 1);
                contentHtml += getImgNum(3, n, num);
            }
            for (var l = 0; l < 8; l++) {
                var num = code1.substr(l, 1);
                contentHtml += getImgLight(l, num);
            }
            $("#stage").html(contentHtml);
        }

        ///========================= JS Tab functions ================================
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
            if (active != 0) {
                this.tabPanel[0].classList.remove("active");
                this.tabPanel[0].classList.add("deactive");
                this.tabPanel[active].classList.remove("deactive");
                this.tabPanel[active].classList.add("active");
            }
            this.active(active);
            this.event();
        };

        Tab.prototype.active = function (index) {
            if (index == this.current || index < 0 || index >= this.tabTitle.length)
                return;

            this.tabTitle[index].classList.add("active");
            this.tabPanel[index].classList.remove("deactive");
            this.tabPanel[index].classList.add("active");
            document.getElementById("introTitle").style.display = (index == 1) ? "block" : "none";
            document.getElementById("playCtrl").style.display = (index == 2) ? "block" : "none";

            if (typeof this.current === "number") {
                this.tabTitle[this.current].classList.remove("active");
                this.tabPanel[this.current].classList.remove("active");
                this.tabPanel[this.current].classList.add("deactive");
                if (index == 2) {
                    document.getElementById("playCtrl").style.display = "block";
                    showDigit("00000000", "00000000", "00000000", "00000000", "00000000");
                }
                else if (index == 1)
                    document.getElementById("introTitle").style.display = "block";
            }
            this.current = index;
        };

        Tab.prototype.event = function () {
            var len = this.tabTitle.length;
            var that = this;

            this.tabTitle[0].addEventListener("click", function () {
                that.active.call(that, 0);
            });
            this.tabTitle[1].addEventListener("click", function () {
                that.active.call(that, 1);
            });
            this.tabTitle[2].addEventListener("click", function () {
                that.active.call(that, 2);
            });
        };

    </script>

    <div id="Tab" class="Tab row" style="position: relative; margin-top: 10px; margin-bottom: 10px;">
        <div class="tab_title">
            <div class="col-md-2">编辑调试</div>
            <div class="col-md-2">实验说明</div>
            <div class="col-md-2">板卡效果</div>
        </div>
        <div class="tab_panel">
            <div class="one_tab active">
                <div class="row" style="background-color:#ebebeb; position: relative; padding-top: 10px; padding-bottom: 10px;">
                    <div class="col-md-2">
                        <asp:Button ID="btnReload" runat="server" OnClick="ReloadCode" Text="重新加载模板代码" CssClass="btn btn-default form-control" />
                    </div>
                    <div class="col-md-6">
                        <asp:Label ID="lbGeneral" class="btn" runat="server" Text=""></asp:Label>
                    </div>
                    <div class="col-md-2">
                        <input id="btnSubmit" type="submit" value="提交编译" onclick="submitCode();" class="btn btn-default form-control" />
                    </div>
                    <%--<div class="col-md-1">
                        <input id="btnCompileTick" value="CT" onclick="compileTick();" class="btn btn-default form-control" />
                    </div>--%>
                    <div class="col-md-2">
                        <input id="btnUpload" type="submit" value="上传程序" onclick="uploadProgram();" class="btn btn-default form-control" />
                    </div>
                    <%--<div class="col-md-1">
                        <input id="btnUploadTick" value="UT" onclick="uploadTick();" class="btn btn-default form-control" />
                    </div>--%>
                </div>
                <div class="row">
                    <div class="col-md-8" style="height:700px; padding-left: 0px; padding-right: 0px; border-style: solid; border-width: thin;">
                        <textarea id="code_text" class="form-control"></textarea>
                    </div>
                    <div class="col-md-4" style="height:700px; padding-left: 5px; padding-right: 0px">
                        <textarea id="debug_text" class="form-control"></textarea>
                    </div>
                </div>
            </div>
            <div class="one_tab deactive">
                <div class="col-md-12" style="overflow-y: scroll; position: relative; height: 752px; background-color: #f8f8f8;">
                    <br />
                    <br />
                    <br />
                    <br />
                    <h4>实验步骤1 内容简介</h4>
                    阅读实验原理，了解zlg7290的读写流程和I2C总线的使用方法
                    <br />
                    根据实验步骤完成对zlg7290的读写程序设计和验证
                    <br />
                    <br />
                    <h4>实验步骤2 实验目的</h4>
                    了解zlg7290的控制流程
                    <br />
                    掌握使用I2C总线读写zlg7290的静态驱动程序设计方法
                    <br />
                    <br />
                    <h4>实验环境</h4>
                    硬件：装有Linux操作系统的开发板
                    <br />
                    软件：Ubuntu12.0，IDE，putty
                    <br />
                    * 经由北京航空航天大学计算机学院的自主开发所提供的WEB服务，学员现在可通过网上实验室提供的远程终端在网页浏览器中完成这些需要特殊软硬件环境的开发实践了。
                    <br />
                    <br />
                    <h4>实验步骤3 ZLG7290介绍</h4>
                    ZLG7290 是广州周立功单片机发展有限公司自行设计的数码管显示驱动及键盘扫描管理芯片。能够直接驱动8位共阴极数码管（或64只独立的LED），同时还可以扫面管理多大64只按键。其中有8只按键可以作为功能键使用，就像电脑键盘上的Ctrl，Shift、Alt键一样。另外ZLG7290 内部还设有连击计数器，能够使某键按下后不松手而连续有效。该芯片为工业级芯片，抗干扰能力强，在工业测控中已有大量应用。该器件通过I2C总线接口进行操作，ZLG7290引脚图如图1。
                    <br />
                    <img src="/Content/intro/board.png" style="width: 288px; height: 238px; margin: 10px;" />
                    <br />
                    <h4>下表说明了ZLG7290各引脚的功能</h4>
                    <br />
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
                        <tr>
                            <td>5</td>
                            <td>DIG1/KC1</td>
                            <td>数码管位选信号1／键盘列信号1</td>
                        </tr>
                        <tr>
                            <td>6</td>
                            <td>DIG0/KC0</td>
                            <td>数码管位选信号0／键盘列信号0</td>
                        </tr>
                        <tr>
                            <td>7</td>
                            <td>SE/KR4</td>
                            <td>数码管e 段／键盘行信号4</td>
                        </tr>
                        <tr>
                            <td>8</td>
                            <td>SF/KR5</td>
                            <td>数码管f 段／键盘行信号5</td>
                        </tr>
                        <tr>
                            <td>9</td>
                            <td>SG/KR6</td>
                            <td>数码管g 段／键盘行信号6</td>
                        </tr>
                        <tr>
                            <td>10</td>
                            <td>DP/KR7</td>
                            <td>数码管dp 段／键盘行信号7</td>
                        </tr>
                        <tr>
                            <td>11</td>
                            <td>GND</td>
                            <td>接地</td>
                        </tr>
                        <tr>
                            <td>12</td>
                            <td>DIG6/KC6</td>
                            <td>数码管位选信号6／键盘列信号6</td>
                        </tr>
                        <tr>
                            <td>13</td>
                            <td>DIG7/KC7</td>
                            <td>数码管位选信号7／键盘列信号7</td>
                        </tr>
                        <tr>
                            <td>14</td>
                            <td>INT</td>
                            <td>键盘中断请求信号，低电平（下降沿）有效</td>
                        </tr>
                        <tr>
                            <td>15</td>
                            <td>RST</td>
                            <td>复位信号，低电平有效</td>
                        </tr>
                        <tr>
                            <td>16</td>
                            <td>Vcc</td>
                            <td>电源，＋3.3～5.5V</td>
                        </tr>
                        <tr>
                            <td>17</td>
                            <td>OSC1</td>
                            <td>晶振输入信号</td>
                        </tr>
                        <tr>
                            <td>18</td>
                            <td>OSC2</td>
                            <td>晶振输出信号</td>
                        </tr>
                        <tr>
                            <td>19</td>
                            <td>SCL</td>
                            <td>I2C 总线时钟信号</td>
                        </tr>
                        <tr>
                            <td>20</td>
                            <td>SDA</td>
                            <td>I2C 总线数据信号</td>
                        </tr>
                        <tr>
                            <td>21</td>
                            <td>DIG5/KC5</td>
                            <td>数码管位选信号5／键盘列信号5</td>
                        </tr>
                        <tr>
                            <td>22</td>
                            <td>DIG4/KC4</td>
                            <td>数码管位选信号4／键盘列信号4</td>
                        </tr>
                        <tr>
                            <td>23</td>
                            <td>SA/KR0</td>
                            <td>数码管a 段／键盘行信号0</td>
                        </tr>
                        <tr>
                            <td>24</td>
                            <td>SB/KR1</td>
                            <td>数码管b 段／键盘行信号1</td>
                        </tr>
                    </table>
                    数码管驱动zlg7290.c实现了设备文件操作控制，用户态可以调用zlg7290_hw_write()，zlg7290_hw_read()和zlg_led_ioctl()函数来对数码管进行读写操作。
                    <br />
                    <br />
                    <h4>实验步骤4 数码管显示原理介绍</h4>
                    <img src="/Content/intro/digit.png" style="width: 646px; height: 230px; margin: 10px;" />
                    <br />
                    0xfc, 0x0c, 0xda, 0xf2, 0x66, 0xb6, 0xbe, 0xe0----对应显示0-7
                    <br />
                    0xfe, 0xf6, 0xee, 0x3e, 0x9c, 0x7a, 0x9e, 0x8e----对应显示8-F
                    <br />
                    <img src="/Content/intro/tb.png" style="width: 539px; height: 151px; margin: 10px;" />
                    <h4>实验步骤5 嵌入式开发Web在线仿真实验平台</h4>
                    <h5>1.本次实验依然在实验室上课</h5>
                    登录嵌入式开发Web在线仿真实验平台地址http://219.224.160.133:8090<br />
                    学员登录：用户名和密码为各位学号（字母大写）<br />
                    <h5>2.正确填写模板后，【提交编译】</h5>
                    <img src="/Content/intro/d1.png" style="width: 432px; height: 287px; margin: 10px;" />
                    <br />未出现failed、error等字样说明编译成功<br />
                    <h5>3.【上传程序】</h5>
                    <img src="/Content/intro/d2.png" style="width: 432px; height: 291px; margin: 10px;" />
                    <h5>4.切换到“板卡效果“页面，【运行程序】</h5>
                    <img src="/Content/intro/d3.png" style="width: 432px; height: 298px; margin: 10px;" />
                    <h4>说明：本次在线实验需要由助教查看效果</h4>
⦁	扩展实验：<br />
⦁	编一个时钟程序，获取当前的年（2019），月日（1218），时分（0855），并在数码管中依次显示出来，例如：2019 1218 0855表示2019年12月18日08时55分<br />
⦁	数码管依次显示 "2.0.1.9."、"1.2.1.8."、"0.8.5.5."<br />
                </div>
                <div id="introTitle" class="col-md-12" style="display:none;position:absolute; top:34px; padding-top: 10px; padding-bottom: 10px; background-color: #ebebeb; filter: alpha(opacity=80); -moz-opacity: 0.5; opacity: 0.8;">
                    <h4>数码管驱动zlg7290.c实现了设备文件操作控制，用户态可以调用zlg7290_hw_write()，zlg7290_hw_read()和zlg_led_ioctl()函数来对数码管进行读写操作，阅读ZLG7290数码管驱动的代码程序清单，了解其实现的具体方法。</h4>
                </div>
            </div>
            <div class="one_tab deactive">
                <img style="position:absolute;left:0px; width:1200px;" src="Content/zlg7290.png" />
                <div id="stage" class="col-md-12" style="position: relative; min-width: 780px; height: 752px; background-color:rgba(0, 0, 0, 0.00);"></div>
                <div id="playCtrl" class="col-md-12" style="display:none;position:absolute; top:34px; padding-top: 10px; padding-bottom: 10px; background-color: #ebebeb; filter: alpha(opacity=80); -moz-opacity: 0.5; opacity: 0.8;">
                    <div class="col-md-2">
                        <input id="btnRun" type="submit" value="运行程序" onclick="runPlay();" class="btn btn-default form-control" />
                    </div>
                    <%--<div class="col-md-1">
                        <input id="btnRunTick" value="RT" onclick="runTick();" class="btn btn-default form-control" />
                    </div>--%>
                    <div class="col-md-10" style="height: 320px; padding-left: 0px; padding-right: 0px; border-style: solid; border-width: thin;">
                        <textarea id="run_console" class="form-control"></textarea>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="mask" id="mask" style="display:block;text-align:center;line-height:650px;min-height:650px;font-size:2em;">
        Waiting...<div style="align-content:center"></div>
        <input type="submit" value="放弃等待" onclick="abortWaiting();" class="btn btn-default form-control" style="position:absolute;align-content:center;" />
    </div>
</asp:Content>
