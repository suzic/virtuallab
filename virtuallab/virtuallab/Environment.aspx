<%@ Page Title="进行实验" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Environment.aspx.cs" Inherits="virtuallab.Environment" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row" style="position:relative; top:20px; height:50px;">
        <div class="col-md-2">
            <asp:Button runat="server" OnClick="Func" Text="重新加载模板代码" CssClass="btn btn-default form-control" />
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
        <div class="col-md-8" style="height:800px; padding-right:0px; border-style:solid; border-width:thin;">
            <textarea id="code_text" class="form-control">
       protected void Page_Init(object sender, EventArgs e)
        {
            HtmlGenericControl CodeMirrorJS = new HtmlGenericControl("script");
            CodeMirrorJS.Attributes.Add("type", "text/javascript");
            CodeMirrorJS.Attributes.Add("src", ResolveUrl(Page.ResolveClientUrl("~/CM/lib/codemirror.js")));
            HtmlGenericControl CodeFormatClike = new HtmlGenericControl("script");
            CodeMirrorJS.Attributes.Add("type", "text/javascript");
            CodeMirrorJS.Attributes.Add("src", ResolveUrl(Page.ResolveClientUrl("~/CM/mode/clike/clike.js")));
            HtmlGenericControl CodeMirrorCSS = new HtmlGenericControl("link");
            CodeMirrorCSS.Attributes.Add("rel", "stylesheet");
            CodeMirrorCSS.Attributes.Add("href", ResolveUrl(Page.ResolveClientUrl("~/CM/lib/codemirror.css")));

            this.Page.Header.Controls.AddAt(1, CodeMirrorJS);
            this.Page.Header.Controls.AddAt(1, CodeFormatClike);
            this.Page.Header.Controls.AddAt(1, CodeMirrorCSS);

            //myJs = new HtmlGenericControl();
            //myJs.TagName = "script";
            //myJs.Attributes.Add("type", "text/javascript");
            //myJs.Attributes.Add("src", ResolveUrl(Page.ResolveClientUrl("~/Content/javascript/javascript.js")));
            //this.Page.Header.Controls.AddAt(1, myJs);

            //string script = "<script>var editor = CodeMirror.fromTextArea(document.getElementById(\"code\"), {mode: \"text/groovy\",  lineNumbers: true,theme: \"dracula\",lineWrapping: true,matchBrackets: true}); editor.setSize('800px', '950px');</script>";
            //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "text", script);

            CurrentLoginUser = SiteMaster.CurrentLoginUser;
            if (CurrentLoginUser == null)
                Response.Redirect("~/");
            else if (CurrentLoginUser.type == 0)
                Response.Redirect("~/ManagerPage");
        }
            </textarea>
        </div>
        <div class="col-md-4" style="height:800px; padding-left:5px;">
            <textarea id="debug_text" class="form-control">
Compile results >>
________________________________________________
            </textarea>
        </div>
    </div>
    <div class="row" style="position:relative; top:20px; height:50px;">
        <div class="col-md-2">
            <asp:Button runat="server" OnClick="Func" Text="查看板卡效果" CssClass="btn btn-default form-control" />
        </div>
    </div>

</asp:Content>
