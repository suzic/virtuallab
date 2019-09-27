﻿<%@ Page Title="报告列表" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReportList.aspx.cs" Inherits="virtuallab.ReportList" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>学员实验报告</h2>
    <div class="row">
        <asp:GridView ID="gvRepots" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="4" CssClass="table col-md-12" DataKeyNames="id_task" DataSourceID="sdsReports" ForeColor="Black" GridLines="Horizontal" AllowPaging="True" AllowSorting="True">
            <Columns>
                <asp:BoundField DataField="id_experiment" HeaderText="id_experiment" ReadOnly="True" SortExpression="id_experiment" Visible="False" />

                <asp:TemplateField ConvertEmptyStringToNull="False" HeaderText="实验名称" SortExpression="title">
                    <ItemTemplate>
                        <asp:Label ID="lbTitle" runat="server" CssClass="col-md-12" Text='<%# Eval("title", "{0}") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle CssClass="col-md-5" />
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
                <asp:TemplateField ConvertEmptyStringToNull="False" HeaderText="成绩" SortExpression="score">
                    <ItemTemplate>
                        <asp:Label ID="lbScore" runat="server" CssClass="col-md-12" Text='<%# Eval("score", "{0}") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle CssClass="col-md-1" />
                </asp:TemplateField>
                <asp:BoundField DataField="id_record" HeaderText="id_record" ReadOnly="True" SortExpression="id_record" Visible="False" />
                <asp:TemplateField ConvertEmptyStringToNull="False" HeaderText="完成时间" SortExpression="finish_date">
                    <ItemTemplate>
                        <asp:Label ID="lbFinishDate" runat="server" CssClass="col-md-12" Text='<%# Eval("finish_date", "{0:g}") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle CssClass="col-md-2" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="实验数据">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbCode" runat="server" CommandArgument='<%# Eval("final_code_uri") %>' CssClass="col-md-6">实验代码</asp:LinkButton>
                        <asp:LinkButton ID="lbReport" runat="server" CommandArgument='<%# Eval("result_json_uri") %>' CssClass="col-md-6">报告</asp:LinkButton>
                    </ItemTemplate>
                    <HeaderStyle CssClass="col-md-3" />
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
        <asp:SqlDataSource ID="sdsReports" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="SELECT [id_experiment], [title], [id_task], [score], [id_student], [name], [complete], [id_record], [finish_date], [result_json_uri], [final_code_uri] FROM [bh_view_manager_reports]"></asp:SqlDataSource>
    </div>
</asp:Content>
