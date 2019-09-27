<%@ Page Title="ModifyPass" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ModifyPass.aspx.cs" Inherits="virtuallab.ModifyPass" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="form-horizontal">
        <h2>重设登录密码</h2>
        <hr />
        <asp:ValidationSummary runat="server" ShowModelStateErrors="true" CssClass="text-danger" />
        <div class="form-group">
            <asp:Label runat="server" ID="CurrentPasswordLabel" AssociatedControlID="CurrentPassword" CssClass="col-md-2 control-label">当前密码</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="CurrentPassword" TextMode="Password" CssClass="form-control" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="CurrentPassword"
                    CssClass="text-danger" ErrorMessage="“当前密码”字段是必填字段。"
                    ValidationGroup="ChangePassword" />
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" ID="NewPasswordLabel" AssociatedControlID="NewPassword" CssClass="col-md-2 control-label">新密码</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="NewPassword" TextMode="Password" CssClass="form-control" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="NewPassword"
                    CssClass="text-danger" ErrorMessage="“新密码”字段是必填字段。"
                    ValidationGroup="ChangePassword" />
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" ID="ConfirmNewPasswordLabel" AssociatedControlID="ConfirmNewPassword" CssClass="col-md-2 control-label">确认新密码</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="ConfirmNewPassword" TextMode="Password" CssClass="form-control" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="ConfirmNewPassword"
                    CssClass="text-danger" Display="Dynamic" ErrorMessage="“确认新密码”字段是必填字段。"
                    ValidationGroup="ChangePassword" />
                <asp:CompareValidator runat="server" ControlToCompare="NewPassword" ControlToValidate="ConfirmNewPassword"
                    CssClass="text-danger" Display="Dynamic" ErrorMessage="新密码和确认密码不匹配。"
                    ValidationGroup="ChangePassword" />
                <asp:Label runat="server">&nbsp;</asp:Label>
            </div>
        </div>
        <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
                <asp:Button runat="server" Text="更改密码" ValidationGroup="ChangePassword" OnClick="ChangePassword_Click" CssClass="btn btn-default form-control" />
            </div>
        </div>
    </div>
</asp:Content>
