<%@ Page Title="报告列表" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReportList.aspx.cs" Inherits="virtuallab.ReportList" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>学员实验报告</h2>
    <asp:GridView ID="gvRepots" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="4" CssClass="table col-md-12" DataKeyNames="id_task" DataSourceID="sdsReports" ForeColor="Black" GridLines="Horizontal">
        <Columns>
            <asp:BoundField DataField="id_task" HeaderText="id_task" InsertVisible="False" ReadOnly="True" SortExpression="id_task" />
            <asp:BoundField DataField="fid_experiment" HeaderText="fid_experiment" SortExpression="fid_experiment" />
            <asp:BoundField DataField="fid_student" HeaderText="fid_student" SortExpression="fid_student" />
            <asp:BoundField DataField="fid_manager" HeaderText="fid_manager" SortExpression="fid_manager" />
            <asp:BoundField DataField="score" HeaderText="score" SortExpression="score" />
            <asp:BoundField DataField="complete" HeaderText="complete" SortExpression="complete" />
        </Columns>
        <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
        <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Right" />
        <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
        <SortedAscendingCellStyle BackColor="#F7F7F7" />
        <SortedAscendingHeaderStyle BackColor="#4B4B4B" />
        <SortedDescendingCellStyle BackColor="#E5E5E5" />
        <SortedDescendingHeaderStyle BackColor="#242121" />
    </asp:GridView>
    <asp:SqlDataSource ID="sdsExperiment" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="SELECT [id_experiment], [title], [template_uri], [rjson_uri], [memo], [create_date], [update_date], [delete_date], [record_status] FROM [bhExperiment]"></asp:SqlDataSource>
    <asp:SqlDataSource ID="sdsStudent" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="SELECT [name], [alias], [password], [gender], [grade], [belong], [phone], [email], [record_status], [id_student] FROM [bhStudent]"></asp:SqlDataSource>
    <asp:SqlDataSource ID="sdsReports" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="SELECT [id_task], [fid_experiment], [fid_student], [fid_manager], [score], [complete] FROM [bhTask]"></asp:SqlDataSource>
</asp:Content>
