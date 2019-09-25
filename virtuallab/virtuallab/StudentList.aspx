<%@ Page Title="我的学生" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StudentList.aspx.cs" Inherits="virtuallab.StudentList" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
        <h2>老师"<%: CurrentLoginUser.name %>"，请查看学生列表！ </h2>
</asp:Content>
