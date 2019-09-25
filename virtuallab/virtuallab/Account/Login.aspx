<%@ Page Title="登录" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="virtuallab.Account.Login" Async="true" %>

<%@ Register Src="~/Account/OpenAuthProviders.ascx" TagPrefix="uc" TagName="OpenAuthProviders" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <h2><%: Title %></h2>

    <div class="row">
        <div class="col-md-12">
            <section id="loginForm">
                <div class="form-horizontal">
                    <hr />
                    <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
                        <p class="text-danger">
                            <asp:Literal runat="server" ID="FailureText" />
                        </p>
                    </asp:PlaceHolder>
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="Account" CssClass="col-md-2 control-label">账号</asp:Label>
                        <div class="col-md-10">
                            <asp:TextBox runat="server" ID="Account" CssClass="form-control" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="Account"
                                CssClass="text-danger" ErrorMessage="“账号”即学员的学号或身份证号码，必须填写。" />
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="Password" CssClass="col-md-2 control-label">密码</asp:Label>
                        <div class="col-md-10">
                            <asp:TextBox runat="server" ID="Password" TextMode="Password" CssClass="form-control" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="Password"
                                CssClass="text-danger" ErrorMessage="“密码”必须填写。如果你忘记了密码，请联系管理员。" />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-offset-2 col-md-10">
                            <asp:Button runat="server" OnClick="LogIn" Text="登录" CssClass="btn btn-default form-control" />
                        </div>
                    </div>
                    <hr />
                    <div class="form-group">
                        <div class="col-md-offset-2 col-md-12">
                            <asp:RadioButton ID="RadioStudent" runat="server" CssClass="col-md-2" Text="学员登录" ValidationGroup="0" Checked="True" GroupName="LoginWay" />
                            <asp:RadioButton ID="RadioManager" runat="server" CssClass="col-md-2" Text="管理登录" ValidationGroup="0" GroupName="LoginWay" />
                        </div>
                    </div>

                </div>
            </section>
        </div>
    </div>
</asp:Content>
