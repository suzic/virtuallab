<%@ Page Title="学员主页" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StudentPage.aspx.cs" Inherits="virtuallab.StudentPage" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>同学"<%: CurrentLoginUser.name %>"，欢迎你进来! </h2>
</asp:Content>
