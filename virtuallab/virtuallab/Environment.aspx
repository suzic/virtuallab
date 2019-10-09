<%@ Page Title="进行实验" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Environment.aspx.cs" Inherits="virtuallab.Environment" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row" style="position:relative; top:20px; bottom:20px; min-height:70px">
        <div class="col-md-2">
            <asp:Button runat="server" OnClick="Func" Text="重新加载模板代码" CssClass="btn btn-default form-control" />
        </div>
        <div class="col-md-2">
            <asp:Button runat="server" OnClick="Func" Text="查看实验说明" CssClass="btn btn-default form-control" />
        </div>
        <div class="col-md-6">
            <asp:Label ID="lbGeneral" runat="server" Text=""></asp:Label>
        </div>
        <div class="col-md-2">
            <asp:Button runat="server" OnClick="Func" Text="提交编译" CssClass="btn btn-default form-control" />
        </div>
    </div>
    <div class="row">
        <div class="col-md-8" style="height:800px; padding-right:0px; border-style:solid; border-width:thin;">
            <textarea id="code_text" class="form-control"></textarea>
        </div>
        <div class="col-md-4" style="height:800px; padding-left:5px;">
            <textarea id="debug_text" class="form-control"></textarea>
        </div>
    </div>
    <div class="row" style="position:relative; top:20px; height:50px;">
        <div class="col-md-2">
            <asp:Button runat="server" OnClick="Func" Text="查看板卡" CssClass="btn btn-default form-control" />
        </div>
        <div class="col-md-8">
        </div>
        <div class="col-md-2">
            <asp:Button runat="server" OnClick="Func" Text="上传程序" CssClass="btn btn-default form-control" />
        </div>
    </div>

</asp:Content>
