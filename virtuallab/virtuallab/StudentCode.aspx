<%@ Page Title="进行实验" Language="C#" MasterPageFile="~/StudentCodeMaster.Master" AutoEventWireup="true" CodeBehind="StudentCode.aspx.cs" Inherits="virtuallab.StudentCode" %>
<%@ Import Namespace="virtuallab.Common.po" %>
<%@ Import Namespace="Newtonsoft.Json" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="Content/layui/css/layui.css">
    <script src="Content/layui/layui.all.js"></script>

    <script type="text/javascript">

        var userId = <%=CurrentLoginUser.userId %>;
        var default_tab = <%=defaultTab %>;
        var cm_editor = {};
        var bhCodes =<%=JsonConvert.SerializeObject(bhCodes) %>;


        $(document).ready(function () {
            initCodeEditors();
            new Tab("#Tab", default_tab);
            //showWaitingLayers();
        });
        function initCodeEditors() {
            for (var i = 0; i < 1; i++) {
                initCodeEditor(bhCodes[i].id_code, bhCodes[i].filecontent);
            }
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
                readOnly: true,
                indentWithTabs: true
            });
            cm_editor[edid].setSize('100%', '100%');
            cm_editor[edid].setValue(code);
            //cm_editor[edid].scrollTo(0, scroll_pos);
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

    <!-- 代码编辑区 -->
    <div id="Tab" class="Tab row" style="position:relative;">
        <div class="tab_title">
            <%foreach (bhCode code in bhCodes){%>
                <div class="col-md-2"><%=code.filename %></div>
            <% }%>
        </div>
        <div class="tab_panel">
            <%foreach (bhCode code in bhCodes){%>
                <div class="one_tab <%=code.active %>">
                    <div class="row">
                        <div class="col-md-12" style="height:890px; padding-left: 0px; padding-right: 0px; border-style: solid; border-width: thin;">
                            <textarea id="code_text<%=code.id_code %>" class="form-control"></textarea>
                        </div>
                    </div>
                </div>
            <% }%>
            
        </div>
    </div>

</asp:Content>
