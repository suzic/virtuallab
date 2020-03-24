<%@ Page Title="进行实验" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Environment.aspx.cs" Inherits="virtuallab.Environment" %>
<%@ Import Namespace="virtuallab.Common.po" %>
<%@ Import Namespace="Newtonsoft.Json" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="Content/layui/css/layui.css">
    <style type="text/css">
        .cusbtn{width:100px; margin-left:20px;margin-right:20px;}
    </style>
    <script src="Content/layui/layui.all.js"></script>

    <script type="text/javascript">
        function showDebugWindown() {
            layer.open({
                type: 1,
                title: '编译结果输出窗口',
                area: ['1024px', '960px'],
                resize:false,
                content: $('#debugWnd') 
            });
        }
        function showRunWindown() {
            if (fid_experiment == 3)
                $('#frmWebGl').attr('src','DanPianJi/bh.html');
            layer.open({
                type: 1,
                title: '设备仿真控制台',
                area: ['1024px', '960px'],
                resize: false,
                content: $('#runWnd'),
                cancel: function (index, layero) {
                    if (fid_experiment == 3)
                        $('#frmWebGl').attr('src', '');
                    releaseDevice();
                    return true;
                }    
            });
        }
    </script>
    <script type="text/javascript">

        var userId = <%=CurrentLoginUser.userId %>;
        var fid_task = <%=CurrentLoginUser.currentTaskId %>;
        var fid_experiment = <%=CurrentLoginUser.currentExperimentId %>;
        var session_id = "<%=CurrentLoginUser.currentSessionId %>";
        var compile_id = "<%=CurrentLoginUser.currentCompileId %>";
        var device_id = "<%=CurrentLoginUser.device_id %>";
        var app_name = "<%=CurrentLoginUser.app_name %>";
        var ssh_uuid = "<%=CurrentLoginUser.ssh_uuid %>";
        var upload_id = "<%=CurrentLoginUser.currentUploadId %>";
        var run_id = "<%=CurrentLoginUser.currentRunId %>";
        var tip_info = "<%=tipInfo %>";

        var cm_editor = {};
        var cm_outer;
        var cm_console;
        var bhCodes =<%=JsonConvert.SerializeObject(bhCodes) %>;
        var scroll_pos = <%=currentPosTop %>;
        var default_tab = <%=defaultTab %>;
        var output_string = <%=outputFormatted %>;
        var animate_string = <%=runResultFormatted %>;

        var timer_outer;
        var timer_console;
        var timer_recieve;
        var wndGL;
        var compileIndex=0;

        var inError = <%=CurrentLoginUser.InError %>;
        var inCompiling = "<%=(CurrentLoginUser.currentState == virtuallab.Models.EnvironmentState.InCompiling) %>";
        var inUploading = "<%=(CurrentLoginUser.currentState == virtuallab.Models.EnvironmentState.InUploading) %>";
        var inRunning = "<%=(CurrentLoginUser.currentState == virtuallab.Models.EnvironmentState.InPlaying) %>";
        var currentState =<%=(int)CurrentLoginUser.currentState %>;

        var layer_mask;
        var timerImageId;
        var timerId;
        var timerTextId;
        var tickImageCount;
        var tickTextCount;
        var tickStringArray;
        var tickImageArray;

        $(document).ready(function () {
            initButtons();
            init_frmWebGl();
            initCodeEditors();
            new Tab("#Tab", default_tab);
            //showWaitingLayers();
        });
        function initCodeEditors() {
            for (var i = 0; i < 1; i++) {
                initCodeEditor(bhCodes[i].id_code, bhCodes[i].filecontent);
            }

            cm_outer = CodeMirror.fromTextArea(document.getElementById('debug_text'), {
                mode: 'textile',
                theme: 'zenburn',
                identUnit: 4,
                readOnly: true
            });
            cm_outer.setSize('100%', '100%');


            cm_console = CodeMirror.fromTextArea(document.getElementById('cm_console'), {
                mode: 'textile',
                theme: 'zenburn',
                identUnit: 4,
                readOnly: true
            });
            cm_console.setSize('100%', '100%');
        }
        function initCodeEditor(id, code) {
            var edid = 'code_text' + id;
            cm_editor[edid] = CodeMirror.fromTextArea(document.getElementById(edid), {
                mode: 'text/x-c++src',
                lineNumbers: true,
                theme: 'mdn-like',
                matchBrackets: true,
                identUnit: 4,
                smartIdent: true,
                indentWithTabs: true
            });
            cm_editor[edid].setSize('100%', '100%');
            cm_editor[edid].setValue(code);
            //cm_editor[edid].scrollTo(0, scroll_pos);
        }
        function initButtons() {
            if (Object.getOwnPropertyNames(cm_editor).length == bhCodes.length) {
                $('#btnSubmit').removeAttr("disabled");
            }

            if (device_id) {
                $('#btnDevice').val('已申请设备');
                $('#btnDevice').prop("disabled", true);

                $('#btnUpload').removeAttr("disabled");
            } else {
                if (currentState == 2)
                    $('#btnDevice').removeAttr("disabled");
            }
        }
        function init_frmWebGl() {
            if (fid_experiment == 3)
                return;


            $("#frmWebGl").css('display', 'none');//隐藏

            $("#runWndText").height(881);
            
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
                    timerImageId = setTimeout("tickRunning()", 1000);
                }
            }
        }

        function showWaitingLayers() {
            layer_mask = document.getElementById('mask');
            tickCount = 0;
            clearTimeout(timerId);

            if (inError != 0) {
                layer_mask.innerHTML = tip_info + '<br/>'
                timerId = setTimeout(function () {
                    layer_mask.style.display = "none";
                    startAnimation();
                }, 2000);
            }
            else if (inCompiling == "True") {
                layer_mask.innerHTML = tip_info + '<br/>';
                //    + '<div class="col-md-12" style="align-content:center;position:absolute;top:400px;"><input type="button" value="放弃等待" onclick="abortWaiting();" class="btn btn-default form-control" style="width:200px;"/></div>';
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
                layer_mask.innerHTML = tip_info + '<br/>';
                //    + '<div class="col-md-12" style="align-content:center;position:absolute;top:400px;"><input type="button" value="放弃等待" onclick="abortWaiting();" class="btn btn-default form-control" style="width:200px;"/></div>';
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
                layer_mask.innerHTML = tip_info + '<br/>';
                //    + '<div class="col-md-12" style="align-content:center;position:absolute;top:400px;"><input type="button" value="放弃等待" onclick="abortWaiting();" class="btn btn-default form-control" style="width:200px;"/></div>';
                timerId = setTimeout(function () {
                    runTick();
                }, 10000);
                //layer_mask.onclick = function () {
                //    clearTimeout(timerId);
                //    layer_mask.style.display = "none";
                //    runTick();
                //}
            }
            else {
                layer_mask.innerHTML = tip_info;
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

        function isNotReady() {
            if (currentState == 0) {
                layer.alert('当前未获服务器授权，请刷新页面重新获取服务器授权', { title: '警告' });
                return true;
            }
            return false;
        }
        // 上传代码编译接口调用
        function submitCode() {
            //是否已获取session_id
            if (isNotReady())
                return;

            //判断编辑器是否编辑完成
            if (Object.getOwnPropertyNames(cm_editor).length < bhCodes.length) {
                layer.alert('请先完成所有代码文件编辑再尝试提交编译', {title:'警告'});
                return;
            }
            if (currentState >= 2) {
                layer.confirm('你已经有成功的编译，确定要重新提交编译代码吗?', { title: '询问' }, function (index) {
                    layer.close(index);
                    submitCode_Core();
                });
            }
            else {
                submitCode_Core();
            }
        }
        function submitCode_Core_Back() {
            showDebugWindown();
            cm_outer.setValue("正在等待远程主机编译结果返回，请不要关闭此窗口！！！.");
            waitingTimerOut();

            var data = {};
            data.fid_task = fid_task;
            data.session_id = session_id;
            data.part = 1;
            data.code = [];
            for (var i = 0; i < bhCodes.length; i++) {
                data.code[i] = {};
                data.code[i].filename = bhCodes[i].filename;
                data.code[i].content = cm_editor['code_text' + bhCodes[i].id_code].getValue();
            }

            $.post("api/bh/CodeSubmit", data, function (result) {
                //清除等待定时器
                clearTimerOut();
                cm_outer.setValue(cm_outer.getValue() + "\n" + (result.fail == 0 ? "SUCCESS：" : "ERROR：") + result.info_buffer+"\n\n您的代码已编译成功，现在你可以关闭此窗口，然后进行以下操作：\r1.申请设备：只有先申请到设备才能在设备中运行你的程序\n2.上传到设备：申请设备成功后，你就可以将程序上传到设备并运行了");

                //成功后续动作
                if (result.fail == 0) {
                    currentState = 2;//已编译成功

                    //设置申请设备按钮
                    if (!device_id) {
                        $('#btnDevice').removeAttr("disabled");
                    }
                }

            }, "json").fail(function (xhr, errorText, errorType) {
                clearTimerOut();
                cm_outer.setValue(cm_outer.getValue() + "\nERROR：" + xhr.responseJSON.ExceptionMessage);
            });
        }
        function submitCode_Core() {
            showDebugWindown();
            cm_outer.setValue("正在等待远程主机编译结果返回，请不要关闭此窗口！！！.");
            waitingTimerOut();

            compileIndex = 0;
            if (compileIndex < bhCodes.length)
            submitCode_files(function (result) {
                //清除等待定时器
                clearTimerOut();
                cm_outer.setValue(cm_outer.getValue() + "\n" + (result.fail == 0 ? "SUCCESS：" : "ERROR：") + result.res + "\n\n您的代码已编译成功，现在你可以关闭此窗口，然后进行以下操作：\r1.申请设备：只有先申请到设备才能在设备中运行你的程序\n2.上传到设备：申请设备成功后，你就可以将程序上传到设备并运行了");

                //成功后续动作
                if (result.fail == 0) {
                    currentState = 2;//已编译成功

                    //设置申请设备按钮
                    if (!device_id) {
                        $('#btnDevice').removeAttr("disabled");
                    }
                }

            });
        }
        //提交单个文件
        function submitCode_files(end) {
            if (compileIndex < bhCodes.length) {
                submitCode_file(bhCodes[compileIndex].filename, cm_editor['code_text' + bhCodes[compileIndex].id_code].getValue(), function (result) {
                    compileIndex++;
                    if (result.fail == 0) {
                        submitCode_files(end);
                    }

                });
            }
            else {
                compile(end);
            }
        }
        function submitCode_file(code_name,code,success) {
            var data = {};
            data.session_id = session_id;
            data.code_name = code_name;
            data.code = code;

            $.post("api/bh/CodeSubmit", data, success, "json").fail(function (xhr, errorText, errorType) {
                clearTimerOut();
                cm_outer.setValue(cm_outer.getValue() + "\nERROR：" + xhr.responseJSON.ExceptionMessage);
            });
        }
        function compile(success) {
            var data = {};
            data.fid_task = fid_task;
            data.session_id = session_id;
            data.part = 1;
            data.code = [];
            for (var i = 0; i < bhCodes.length; i++) {
                data.code[i] = {};
                data.code[i].filename = bhCodes[i].filename;
                data.code[i].content = cm_editor['code_text' + bhCodes[i].id_code].getValue();
            }

            $.post("api/bh/Compile", data, success, "json").fail(function (xhr, errorText, errorType) {
                clearTimerOut();
                cm_outer.setValue(cm_outer.getValue() + "\nERROR：" + xhr.responseJSON.ExceptionMessage);
            });
        }
        function waitingTimerOut() {
            timer_outer = setInterval(function () {
                cm_outer.setValue(cm_outer.getValue() + "." );
            }, 1000);
        }
        function clearTimerOut() {
            if (timer_outer)
                clearInterval(timer_outer);
        }

        //申请设备
        function applyDevice() {
            //是否已获取session_id
            if (isNotReady())
                return;

            var data = {};
            data.session_id = session_id;
            data.device_type = "1";
            
            $.post("api/bh/DeviceRequest", data, function (result) {
                //成功后续动作
                if (result.fail == 0) {
                    currentState = 3;//已申请设备
                    device_id = result.device_id;
                    ssh_uuid = result.ssh_uuid;

                    //设置申请设备按钮
                    if (device_id) {
                        $('#btnDevice').val('已申请设备');
                        $('#btnDevice').prop("disabled", true);

                        $('#btnUpload').removeAttr("disabled");
                    }

                    layer.alert('申请设备成功，你可以上传程序到设备了', { title: '成功' });
                } else {
                    layer.alert('申请设备失败', { title: '失败' });
                }

            }, "json").fail(function (xhr, errorText, errorType) {
                layer.alert(xhr.responseJSON.ExceptionMessage, { title: '异常' });
            });
        }

        //释放设备
        function releaseDevice() {
            var data = {};
            data.session_id = session_id;
            data.device_type = "0";

            $.post("api/bh/DeviceRequest", data, function (result) {
                //成功后续动作
                if (result.fail == 0) {
                    currentState = 2;
                    device_id = '';
                    app_name = '';

                    $('#btnDevice').val('申请设备');
                    $('#btnDevice').removeAttr("disabled");
                    $('#btnUpload').prop("disabled", true);


                } else {
                    layer.alert('释放设备失败', { title: '失败' });
                }

            }, "json").fail(function (xhr, errorText, errorType) {
                layer.alert(xhr.responseJSON.ExceptionMessage, { title: '异常' });
            });
        }

        // 上传程序接口调用
        function uploadProgram() {
            //是否已获取session_id
            if (isNotReady())
                return;

            if (currentState == 4 && app_name) {
                showRunWnd();
                return;
            }

            var data = {};
            data.session_id = session_id;
            data.device_id = device_id;

            $.post("api/bh/ProgramUpload", data, function (result) {
                //成功后续动作
                if (result.fail == 0) {
                    currentState = 4;//已申请设备
                    app_name = result.app_name;
                    
                    showRunWnd();
                    
                } else {
                    layer.alert('上传程序到设备失败，错误码：' + result.fail, { title: '失败' });
                }

            }, "json").fail(function (xhr, errorText, errorType) {
                layer.alert(xhr.responseJSON.ExceptionMessage, { title: '异常' });
            });
        }
        function showRunWnd() {
            showRunWindown();
            cm_console.setValue("你的程序已上传到设备，文件名为：" + app_name+"\n你可以在下面编辑框输入命令，点击“运行”，会将命令发送到设备上去执行，并将执行结果显示在这里\n> ");
        }

        // 运行命令
        function txtCommand_onKeyPress(e) {
            var keyCode = null;
            if (e.which)
                keyCode = e.which;
            else if (e.keyCode)
                keyCode = e.keyCode;

            if (keyCode == 13) {
                $('#btnRun').click();
                return false;
            }
            return true;
        }
        function runCommand() {
            if (!session_id || !device_id || !app_name || !ssh_uuid)
                return;

            var cmd = $('#txtCommand').val().trim();
            if (!cmd) {
                cm_console.setValue(cm_console.getValue() + '\n>');
                return;
            }

            $('#txtCommand').val('');
            runCommandBegin();

            cm_console.setValue(cm_console.getValue() + cmd + ' ');
            waitingTimerConsole();

            var data = {};
            data.session_id = session_id;
            data.device_id = device_id;
            data.app_name = app_name;
            data.ssh_uuid = ssh_uuid;
            data.cmd = cmd;

            $.post("api/bh/ConsoleSend", data, function (result) {
                clearTimerConsole();
                showCommandResult(result.res);

                //如果未返回全部信息
                if (false) {
                    waitingTimerRecieve();
                } else {
                    runCommandComplete();
                }

            }, "json").fail(function (xhr, errorText, errorType) {
                clearTimerConsole();
                showCommandResult(xhr.responseJSON.ExceptionMessage);
                runCommandComplete();
            });
        }
        function waitingTimerConsole() {
            timer_console = setInterval(function () {
                cm_console.setValue(cm_console.getValue() + ".");
            }, 1000);
        }
        function clearTimerConsole() {
            if (timer_console)
                clearInterval(timer_console);
        }

        // 获取命令结果
        function getCommandResult() {
            if (!session_id || !device_id || !app_name)
                return;

            var data = {};
            data.session_id = session_id;
            data.device_id = device_id;

            $.post("api/bh/ConsoleReceive", data, function (result) {
                showCommandResult(result.output);

                //如果完成停止
                if (result.finish) {
                    clearTimerRecieve();
                    runCommandComplete();
                }

            }, "json").fail(function (xhr, errorText, errorType) {
                showCommandResult(xhr.responseJSON.ExceptionMessage);
            });
        }
        function waitingTimerRecieve() {
            timer_recieve = setInterval(function () {
                getCommandResult();
            }, 1000);
        }
        function clearTimerRecieve() {
            if (timer_recieve)
                clearInterval(timer_recieve);
        }
        function showCommandResult(res) {
            if (!res)
                return;
            var index = res.indexOf('<Console>');
            var jsonstr;
            var jsonobj;
            if (index == 0) {
                // 控制台数据
                jsonstr = res.substr(9);
                jsonobj = $.parseJSON(jsonstr);
                for (let i = 0; i < jsonobj.length; i++) {
                    cm_console.setValue(cm_console.getValue() + "\n" + jsonobj[i].value);
                }

            } else {
                index = res.indexOf('<Effect>');
                if (index == 0) {
                    // 效果数据
                    jsonstr = res.substr(8);
                    jsonobj = $.parseJSON(jsonstr);
                    for (let i = 0; i < jsonobj.length; i++) {
                        if (i < 1) {
                            showEffect(jsonobj[i].value);
                        } else {
                            setTimeout(function () {
                                showEffect(jsonobj[i].value);
                            }, 1000*i)
                        }
                    }

                } else {
                    // 纯字符
                    cm_console.setValue(cm_console.getValue() + "\n" + res);
                }
            }
        }
        function runCommandBegin() {
            $('#txtCommand').prop("disabled", true);
            $('#btnRun').prop("disabled", true);
        }
        function runCommandComplete() {
            cm_console.setValue(cm_console.getValue() + '\n>');
            $('#txtCommand').removeAttr("disabled");
            $('#btnRun').removeAttr("disabled");
        }

        // 显示动画
        function showEffect(data) {
            if (!wndGL)
                wndGL = document.getElementById('frmWebGl').contentWindow;

            wndGL.SendDataToUnity(data);
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
            //document.getElementById("introTitle").style.display = (index == 1) ? "block" : "none";
            //document.getElementById("playCtrl").style.display = (index == 2) ? "block" : "none";

            if (typeof this.current === "number") {
                this.tabTitle[this.current].classList.remove("active");
                this.tabPanel[this.current].classList.remove("active");
                this.tabPanel[this.current].classList.add("deactive");
                //if (index == 2) {
                //    document.getElementById("playCtrl").style.display = "block";
                //    showDigit("00000000", "00000000", "00000000", "00000000", "00000000");
                //}
                //else if (index == 1)
                //    document.getElementById("introTitle").style.display = "block";
            }
            this.current = index;

            //初始化编辑器
            if (index > 0) {
                if (!bhCodes[index].isInit) {
                    initCodeEditor(bhCodes[index].id_code, bhCodes[index].filecontent);
                    bhCodes[index].isInit = true;

                    if (Object.getOwnPropertyNames(cm_editor).length == bhCodes.length) {
                        $('#btnSubmit').removeAttr("disabled");
                    }
                }
            }
        };

        Tab.prototype.event = function () {
            var len = this.tabTitle.length;
            var that = this;

            for (let i = 0; i < len; i++) {
                this.tabTitle[i].addEventListener("click", function () {
                    that.active.call(that, i);
                });
            }
        };

    </script>

    <!-- 按钮栏 -->
    <div class="row" style="position:absolute; padding-top: 9px; padding-bottom: 10px; padding-right:20px; width:600px; right:0px; z-index:20; ">
        <table border="0" style="width:100%;">
            <tr>
                <td style="width:100%;">
                    <asp:Button ID="btnReload" runat="server" OnClick="ReloadCode" Text="重新加载模板代码" CssClass="btn btn-default form-control cusbtn" Visible="false" />
                    <asp:Label ID="lbGeneral" class="btn" runat="server" Text="" Visible="false"></asp:Label></td>

                <td>
                    <asp:HyperLink ID="HyperLinkHelp" runat="server" class="btn btn-default form-control cusbtn" Target="_blank">实验说明</asp:HyperLink>
                </td>
                <td><input id="btnSubmit" type="button" value="提交编译" onclick="submitCode();" class="btn btn-default form-control cusbtn"  disabled="disabled"/></td>
                <td><input id="btnDevice" type="button" value="申请设备" onclick="applyDevice();" class="btn btn-default form-control cusbtn"  disabled="disabled"/></td>
                <td><input id="btnUpload" type="button" value="上传到设备" onclick="uploadProgram();" class="btn btn-default form-control cusbtn" disabled="disabled" /></td>
            </tr>
        </table>
    </div>

    <!-- 代码编辑区 -->
    <div id="Tab" class="Tab row" style="position:relative; margin-top: 10px; margin-bottom: 10px;">
        <div class="tab_title">
            <%foreach (bhCode code in bhCodes){%>
                <div class="col-md-1"><%=code.filename %></div>
            <% }%>
        </div>
        <div class="tab_panel">
            <%foreach (bhCode code in bhCodes){%>
                <div class="one_tab <%=code.active %>">
                    <div class="row">
                        <div class="col-md-12" style="height:870px; padding-left: 0px; padding-right: 0px; border-style: solid; border-width: thin;">
                            <textarea id="code_text<%=code.id_code %>" class="form-control"></textarea>
                        </div>
                    </div>
                </div>
            <% }%>
            
        </div>
    </div>

    <!-- 编译结果区 -->
    <div id="debugWnd" style="display:none;width:1024px; height:918px;">
        <textarea id="debug_text" class="form-control"></textarea>
    </div>

    <!-- 运行结果区 -->
    <div id="runWnd" style="display:none;width:1024px; height:918px;">
        <div id="runWndText" style="width:100%; height:431px;">
            <textarea id="cm_console" class="form-control"></textarea>
        </div>

        <div style="width:100%; height:34px;">
            <table style="width:100%; height:100%; border:none;margin:0px;padding:0px;">
                <tr style="width:100%; height:100%; border:none;margin:0px;padding:0px;">
                    <td style="width:100%; height:34px; border:none;margin:0px;padding:0px;">
                        <input id="txtCommand" type="text" class="form-control" style="width:100%; height:34px; max-width:973px;" onkeypress="return txtCommand_onKeyPress(event)"/>
                    </td>
                    <td style="width:50px; height:34px; border:none;margin:0px;padding:0px;">
                        <input id="btnRun" type="button" value="运行" onclick="runCommand();" class="btn btn-danger form-control" style="width:50px; height:34px;margin:0px;padding:0px;"/>
                    </td>
                </tr>
            </table>
        </div>

        <iframe id="frmWebGl" style="width: 1024px; height: 450px; border:none; overflow:hidden;" src="">
        </iframe>
    </div>

    <!-- 遮罩 -->
    <div class="mask" id="mask" style="display:none;text-align:center;line-height:650px;min-height:650px;font-size:2em;">
    </div>
</asp:Content>
