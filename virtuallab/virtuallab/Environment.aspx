<%@ Page Title="进行实验" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Environment.aspx.cs" Inherits="virtuallab.Environment" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h4>&nbsp;</h4>
    <div class="row">
        <div class="col-md-2">
            <asp:Button runat="server" OnClick="Func" Text="重新加载实验" CssClass="btn btn-default form-control" />
        </div>
        <div class="col-md-2">
            <asp:Button runat="server" OnClick="Func" Text="查看代码" CssClass="btn btn-default form-control" />
        </div>
        <div class="col-md-2">
            <asp:Button runat="server" OnClick="Func" Text="提交编译" CssClass="btn btn-default form-control" />
        </div>
        <div class="col-md-4">
            <asp:Label ID="Label1" runat="server" Text="编译返回结果"></asp:Label>
        </div>
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

</asp:Content>
