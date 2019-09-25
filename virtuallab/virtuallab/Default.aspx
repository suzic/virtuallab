<%@ Page Title="首页" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="virtuallab._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1>单片机开发调试虚拟仿真网络实验室</h1>
        <h2>北京航空航天大学信息学院</h2>
        <br />
        <p class="lead">通过网站应用，编辑你的硬件控制程序代码，连接到实验室内的硬件设备进行远程仿真控制，并返回结果，达到远程调试开发的实验效果。</p>
        <p><asp:LinkButton runat="server" OnClick="FirstTask" CssClass="btn btn-primary btn-lg" Text="登录使用 &raquo;" ID="FuncMain" /></p>
    </div>

    <div class="row">
        <div class="col-md-4">
            <h2>专属学员账号</h2>
            <p>网络实验室仅供校内外相关学员使用。因此账号不能开放注册。要获取账号，可联系相关负责人。</p>
            <p><a class="btn btn-default" href="Contact">获取账号 &raquo;</a></p>
        </div>
        <div class="col-md-4">
            <h2>仿真实验项目</h2>
            <p>网络实验室支持丰富的实验项目。来此了解所有支持的仿真实验吧！</p>
            <p><asp:LinkButton runat="server" OnClick="ExperiTask" CssClass="btn btn-default" Text="所有实验 &raquo;" ID="FuncExperi" /></p>
        </div>
        <div class="col-md-4">
            <h2>实验任务成绩</h2>
            <p>教师会定期通过网站向学员公布新的实验任务。在期限内完成实验并查看自己的成绩。</p>
            <p><asp:LinkButton runat="server" OnClick="ScoreTask" CssClass="btn btn-default" Text="查看成绩 &raquo;" ID="FuncScore" /></p>
        </div>
    </div>

</asp:Content>
