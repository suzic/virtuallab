<%@ Page Title="报告列表" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReportList.aspx.cs" Inherits="virtuallab.ReportList" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="Content/layui/css/layui.css">
    <style type="text/css">
        a{color:#337ab7;}
    </style>
    <script src="Content/layui/layui.all.js"></script>
    <script type="text/javascript">
        var current_code = <%=currentCode %>;
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
            layer_mask = document.getElementById('mask');
            if (current_code != "") {
                layer_mask.style.display = "block";
                cm_editor.setValue(current_code);
            }
            else
                layer_mask.style.display = "none";
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
        }

        function showWaitingLayers() {
            layer_mask.style.display = "block";
        }

        function closeCode() {
            layer_mask.style.display = "none";
        }

        function showCodeWindown(taskid, name, experiment) {
            var url = 'StudentCode.aspx?taskid=' + taskid;
            var wndtitle = name + ' - ' + experiment +' - 实验代码';
            layer.open({
                type: 2,
                title: wndtitle,
                area: ['1024px', '960px'],
                resize: false,
                content: url
            });
        }
    </script>

    <h2>学员实验报告</h2>
    <hr />
    <div class="row">
        <div class="col-md-12">
        <asp:GridView ID="gvRepots" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="4" CssClass="table col-md-12" DataKeyNames="id_task, id_experiment, id_student" DataSourceID="sdsReports" ForeColor="Black" GridLines="Horizontal" AllowPaging="True" AllowSorting="True" PageSize="12">
            <Columns>
                <asp:BoundField DataField="id_experiment" HeaderText="id_experiment" ReadOnly="True" SortExpression="id_experiment" Visible="False" />
                <asp:TemplateField ConvertEmptyStringToNull="False" HeaderText="实验名称" SortExpression="title">
                    <ItemTemplate>
                        <asp:Label ID="lbTitle" runat="server" CssClass="col-md-12" Text='<%# Eval("title", "{0}") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle CssClass="col-md-4" />
                </asp:TemplateField>
                <asp:BoundField DataField="id_student" HeaderText="id_student" ReadOnly="True" SortExpression="id_student" Visible="False" />
                <asp:TemplateField ConvertEmptyStringToNull="False" HeaderText="参与人" SortExpression="name">
                    <ItemTemplate>
                        <asp:Label ID="lbName" runat="server" CssClass="col-md-12" Text='<%# Eval("name", "{0}") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle CssClass="col-md-1" />
                </asp:TemplateField>
                <asp:BoundField DataField="id_task" HeaderText="id_task" ReadOnly="True" SortExpression="id_task" Visible="False" />
                <asp:BoundField DataField="complete" HeaderText="complete" SortExpression="complete" Visible="False" />
                <asp:BoundField DataField="id_record" HeaderText="id_record" ReadOnly="True" SortExpression="id_record" Visible="False" />
                <asp:TemplateField ConvertEmptyStringToNull="False" HeaderText="完成时间" SortExpression="finish_date">
                    <ItemTemplate>
                        <asp:Label ID="lbFinishDate" runat="server" CssClass="col-md-12" Text='<%# Eval("finish_date", "{0:F}") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle CssClass="col-md-3" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="实验数据">
                    <ItemTemplate>
                        <a class="col-md-6" href="javascript:showCodeWindown(<%# Eval("id_task") %>,'<%# Eval("name") %>','<%# Eval("title") %>')">实验代码</a>
                        <%--<asp:LinkButton ID="lbCode" runat="server" CommandArgument='<%# Eval("final_code_uri") %>' OnCommand="lbCode_Command" CssClass="col-md-6">实验代码</asp:LinkButton>--%>
                        <asp:LinkButton ID="lbReport" runat="server" CommandArgument='<%# Eval("result_json_uri") %>' CssClass="col-md-6" CommandName="Edit">打分</asp:LinkButton>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:LinkButton ID="lbUpdate" runat="server" CssClass="col-md-6" CausesValidation="True" CommandName="Cancel" OnCommand="lbScoreConfirm" ForeColor="Red" ValidationGroup="2" >确定</asp:LinkButton>
                        <asp:LinkButton ID="lbCancel" runat="server" CssClass="col-md-6" CausesValidation="False" CommandName="Cancel">取消</asp:LinkButton>
                    </EditItemTemplate>
                    <HeaderStyle CssClass="col-md-3" />
                </asp:TemplateField>
                <asp:TemplateField ConvertEmptyStringToNull="False" HeaderText="成绩" SortExpression="score">
                    <EditItemTemplate>
                        <asp:TextBox ID="tbScore" runat="server" Text='<%# Bind("score", "{0}") %>' Width="100%"></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lbScore" runat="server" CssClass="col-md-12" Text='<%# Eval("score", "{0}") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle CssClass="col-md-1" />
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>
                <h3>当前暂未有学生完成实验</h3>
            </EmptyDataTemplate>
            <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
            <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" />
            <PagerSettings FirstPageText="&amp;lt;&amp;lt;&amp;nbsp;首页&amp;nbsp;" LastPageText="&amp;nbsp;尾页&amp;nbsp;&amp;gt;&amp;gt;" Mode="NextPreviousFirstLast" NextPageText="下一页&amp;nbsp;&amp;gt;&amp;nbsp;" PreviousPageText="&amp;nbsp;&amp;lt;&amp;nbsp;上一页" />
            <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Right" />
            <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
            <SortedAscendingCellStyle BackColor="#F7F7F7" />
            <SortedAscendingHeaderStyle BackColor="#4B4B4B" />
            <SortedDescendingCellStyle BackColor="#E5E5E5" />
            <SortedDescendingHeaderStyle BackColor="#242121" />
        </asp:GridView>
        </div>
        <asp:SqlDataSource ID="sdsReports" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="SELECT [id_experiment], [title], [id_task], [score], [id_student], [name], [complete], [id_record], [finish_date], [result_json_uri], [final_code_uri] FROM [bh_view_manager_reports]"></asp:SqlDataSource>
    </div>
    <div class="row" id="mask" style="position:absolute; display:block; left:80px; top:120px; width:900px;">
        <div class="row">
            <div class="col-md-12"  style="height:750px; padding:15px; border-style:solid; border-width:thin; background-color:#ebebeb" >
                <textarea id="code_text" class="form-control"></textarea>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12" style="padding-left:0px;">
                <input type="button" value="关闭" onclick="closeCode();" class="btn btn-default form-control" style="position:absolute;align-content:center;" />
            </div>
        </div>
    </div>

</asp:Content>
