<%@ Page Title="实验管理" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManagerPage.aspx.cs" Inherits="virtuallab.ManagerPage" %>

<asp:Content ID="BodyConent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>老师"<%: CurrentLoginUser.name %>"，在这里查看实验并向学生发布任务! </h2>
</asp:Content>
