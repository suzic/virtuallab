<%@ Page Title="管理员主页" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManagerPage.aspx.cs" Inherits="virtuallab.ManagerPage" %>

<asp:Content ID="BodyConent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>老师"<%: CurrentLoginUser.name %>"，欢迎你进来! </h2>
</asp:Content>
