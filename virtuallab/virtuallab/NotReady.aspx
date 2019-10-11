<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="NotReady.aspx.cs" Inherits="virtuallab.NotReady" %>

<asp:Content ID="BodyConent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:MultiView ID="InformationView" runat="server" ActiveViewIndex="0">
        <asp:View runat="server">
            <h2>请先选择你的任务然后进入实验</h2>
            <hr />
            <div class="row">
                <div class="col-md-2">
                    <asp:Button runat="server" OnClick="GotoTaskView" Text="查看我的任务" CssClass="btn btn-default form-control" />
                </div>
            </div>
        </asp:View>
        <asp:View runat="server">
            <h2>该实验数据尚未准备就绪</h2>
            <hr />
            <div class="row">
                <div class="col-md-2">
                    <asp:Button runat="server" OnClick="GotoTaskView" Text="查看我的任务" CssClass="btn btn-default form-control" />
                </div>
            </div>
        </asp:View>
        <asp:View runat="server">
            <h2>服务忙，您暂时无法连接到实验服务，请稍后再试</h2>
            <hr />
            <div class="row">
                <div class="col-md-2">
                    <asp:Button runat="server" OnClick="GotoTaskView" Text="查看我的任务" CssClass="btn btn-default form-control" />
                </div>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
