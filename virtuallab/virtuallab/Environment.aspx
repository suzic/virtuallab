<%@ Page Title="进行实验" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Environment.aspx.cs" Inherits="virtuallab.Environment" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h4>&nbsp;</h4>
    <div class="row">
        <div class="col-md-2">
            <asp:Button runat="server" OnClick="Func" Text="重新加载实验模板" CssClass="btn btn-default form-control" />
        </div>
        <div class="col-md-6">
        </div>
        <div class="col-md-2">
            <asp:Button runat="server" OnClick="Func" Text="提交编译" CssClass="btn btn-default form-control" />
        </div>
        <%--<div class="col-md-6">
            <asp:Label ID="Label1" runat="server" Text="编译报告"></asp:Label>
        </div>--%>
        <div class="col-md-2">
            <asp:Button runat="server" OnClick="Func" Text="上传到板卡运行" CssClass="btn btn-default form-control" />
        </div>
    </div>
    <hr />
    <div class="row">
        <div class="col-md-8">
            <textarea id="TextArea1" class="col-md-12" rows="25" spellcheck="false"></textarea>
        </div>
        <div class="col-md-4">
            <textarea id="TextArea2" class="col-md-12" rows="25" contenteditable="false" spellcheck="false"></textarea>
        </div>
    </div>
    <h4>&nbsp;</h4>
    <div class="row">
        <div class="col-md-2">
            <asp:Button runat="server" OnClick="Func" Text="查看板卡效果" CssClass="btn btn-default form-control" />
        </div>
    </div>
</asp:Content>
