<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Interaction.ascx.cs" Inherits="virtuallab.Interaction" %>

<div class="row" style="background-color:#ebebeb; position: relative; padding-top: 10px; padding-bottom: 10px;">
    <div class="col-md-2">
        <asp:Button ID="btnReload" runat="server" OnClick="ReloadCode" Text="重新加载模板代码" CssClass="btn btn-default form-control" />
    </div>
    <div class="col-md-6">
        <asp:Label ID="lbGeneral" class="btn" runat="server" Text=""></asp:Label>
    </div>
    <div class="col-md-2">
        <input id="btnSubmit" type="submit" value="提交编译" onclick="submitCode();" class="btn btn-default form-control" />
    </div>
    <%--<div class="col-md-1">
        <input id="btnCompileTick" value="CT" onclick="compileTick();" class="btn btn-default form-control" />
    </div>--%>
    <div class="col-md-2">
        <input id="btnUpload" type="submit" value="上传程序" onclick="uploadProgram();" class="btn btn-default form-control" />
    </div>
    <%--<div class="col-md-1">
        <input id="btnUploadTick" value="UT" onclick="uploadTick();" class="btn btn-default form-control" />
    </div>--%>
</div>
<div class="row">
    <div class="col-md-8" style="height:700px; padding-left: 0px; padding-right: 0px; border-style: solid; border-width: thin;">
        <textarea id="code_text" class="form-control"></textarea>
    </div>
    <div class="col-md-4" style="height:700px; padding-left: 5px; padding-right: 0px">
        <textarea id="debug_text" class="form-control"></textarea>
    </div>
</div>
