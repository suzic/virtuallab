<%@ Page Title="进行实验" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Environment.aspx.cs" Inherits="virtuallab.Environment" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:MultiView ID="EnvironmentView" runat="server" ActiveViewIndex="0">
        <asp:View ID="CodeView" runat="server">
            <div class="row" style="position: relative; margin-top: 20px; margin-bottom: 20px;">
                <div class="col-md-2">
                    <asp:Button runat="server" OnClick="ReloadCode" Text="重新加载模板代码" CssClass="btn btn-default form-control" />
                </div>
                <div class="col-md-6">
                    <asp:Label ID="lbGeneral" runat="server" Text=""></asp:Label>
                </div>
                <div class="col-md-2">
                    <asp:Button runat="server" OnClick="CodeComplie" Text="提交编译" CssClass="btn btn-default form-control" />
                </div>
                <div class="col-md-2">
                    <asp:Button runat="server" OnClick="CodeUpload" Text="上传程序" CssClass="btn btn-default form-control" />
                </div>
            </div>
            <div class="row">
                <div class="col-md-8" style="height: 800px; padding-right: 0px; border-style: solid; border-width: thin;">
                    <textarea id="code_text" class="form-control"></textarea>
                </div>
                <div class="col-md-4" style="height: 800px; padding-left: 5px;">
                    <textarea id="debug_text" class="form-control"></textarea>
                </div>
            </div>
        </asp:View>
        <asp:View ID="ModelView" runat="server">
            <div class="row">
                <div class="col-md-12" style="position: relative; margin-top: 20px; height: 855px; background-color: lightgray; border-style: solid; border-width: thin; border-color: darkgray">
                </div>
            </div>
        </asp:View>
        <asp:View ID="IntroView" runat="server">
            <h2>实验说明</h2>
            <hr />
            <div class="row">
                <div class="col-md-12" style="position: relative; height: 780px; background-color: whitesmoke; border-style: solid; border-width: thin; border-color: lightgray">
                </div>
            </div>
        </asp:View>
    </asp:MultiView>
    <div class="row" style="position: relative; margin-top: 20px; height: 50px;">
        <div class="col-md-2">
            <asp:Button runat="server" OnClick="SwitchViewToIntro" Text="实验说明" CssClass="btn btn-default form-control" />
        </div>
        <div class="col-md-2">
            <asp:Button runat="server" OnClick="SwitchViewToCode" Text="代码编辑" CssClass="btn btn-default form-control" />
        </div>
        <div class="col-md-6"></div>
        <div class="col-md-2">
            <asp:Button runat="server" OnClick="SwitchViewToBoard" Text="板卡效果" CssClass="btn btn-default form-control" />
        </div>
    </div>

</asp:Content>
