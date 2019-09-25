<%@ Page Title="我的任务" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StudentPage.aspx.cs" Inherits="virtuallab.StudentPage" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>同学"<%: CurrentLoginUser.name %>"，来看看你有什么实验任务吧！ </h2>
</asp:Content>
