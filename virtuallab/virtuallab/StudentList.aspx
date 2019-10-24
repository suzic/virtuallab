<%@ Page Title="我的学生" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StudentList.aspx.cs" Inherits="virtuallab.StudentList" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>网络实验室学员列表</h2>
    <hr />
    <div class="row">
        <div class="col-md-12">
        <asp:GridView ID="gvStudents" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" CssClass="table col-md-12" DataSourceID="sdsStudentList" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black" GridLines="Horizontal" ShowFooter="True" PageSize="12">
            <Columns>
                <asp:TemplateField HeaderText="ID" InsertVisible="False" SortExpression="id_student" Visible="False">
                    <ItemTemplate>
                        <asp:Label ID="lbStudentID" runat="server" Text='<%# Bind("id_student") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="姓名" SortExpression="name">
                    <EditItemTemplate>
                        <asp:TextBox ID="tbName" runat="server" Text='<%# Bind("name", "{0}") %>' Width="100%"></asp:TextBox>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:LinkButton ID="lbInsertStudent" runat="server" OnCommand="InsertStudent" CommandName="Insert">新建学员</asp:LinkButton>
                    </FooterTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lbName" runat="server" Text='<%# Bind("name", "{0}") %>' CssClass="col-md-12"></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle CssClass="col-md-1" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="账号" SortExpression="alias">
                    <EditItemTemplate>
                        <asp:TextBox ID="tbAlias" runat="server" Text='<%# Bind("alias", "{0}") %>' Width="100%"></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lbAlias" runat="server" Text='<%# Bind("alias", "{0}") %>' CssClass="col-md-12"></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle CssClass="col-md-2" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="性别" SortExpression="gender">
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlGenderChange" runat="server" SelectedValue='<%# Bind("gender") %>' Width="100%">
                            <asp:ListItem Value="0">未知</asp:ListItem>
                            <asp:ListItem Value="1">男</asp:ListItem>
                            <asp:ListItem Value="2">女</asp:ListItem>
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lbGender" runat="server" CssClass="col-md-12" Text='<%# (short)Eval("gender") == 0 ? "未知": (short)Eval("gender") == 1 ? "男":"女" %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle CssClass="col-md-1" />
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="学级" SortExpression="grade">
                    <EditItemTemplate>
                        <asp:TextBox ID="tbGrade" runat="server" Text='<%# Bind("grade", "{0}") %>' Width="100%"></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lbGrade" runat="server" CssClass="col-md-12" Text='<%# Bind("grade", "{0}") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle CssClass="col-md-1" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="院系" SortExpression="belong">
                    <EditItemTemplate>
                        <asp:TextBox ID="tbBelong" runat="server" Text='<%# Bind("belong", "{0}") %>' Width="100%"></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblBelong" runat="server" CssClass="col-md-12" Text='<%# Bind("belong", "{0}") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle CssClass="col-md-2" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="电话" SortExpression="phone">
                    <EditItemTemplate>
                        <asp:TextBox ID="tbPhone" runat="server" Text='<%# Bind("phone", "{0}") %>' Width="100%"></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lbPhone" runat="server" CssClass="col-md-12" Text='<%# Bind("phone", "{0}") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle CssClass="col-md-1" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="邮件" SortExpression="email">
                    <EditItemTemplate>
                        <asp:TextBox ID="tbMail" runat="server" Text='<%# Bind("email", "{0}") %>' Width="100%"></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lbMail" runat="server" CssClass="col-md-12" Text='<%# Bind("email", "{0}") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle CssClass="col-md-2" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="启用状态及相关操作" SortExpression="record_status">
                    <EditItemTemplate>
                        <asp:LinkButton ID="lbUpdate" runat="server" CssClass="col-md-6" CausesValidation="True" CommandName="Cancel" OnCommand="UpdateStudent" ForeColor="Red" ValidationGroup="2" >更新</asp:LinkButton>
                        <asp:LinkButton ID="lbCancel" runat="server" CssClass="col-md-6" CausesValidation="False" CommandName="Cancel">取消</asp:LinkButton>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:LinkButton ID="lbEdit" runat="server" CssClass="col-md-6" CausesValidation="False" CommandName="Edit">编辑</asp:LinkButton>
                        <asp:LinkButton ID="lbDisable" runat="server" CssClass="col-md-6" Visible='<%# (short)Eval("record_status") == 1 %>' CausesValidation="True" OnCommand="EnableStudent" CommandName="Cancel">禁用</asp:LinkButton>
                        <asp:LinkButton ID="lbEnable" runat="server" CssClass="col-md-6" ForeColor="Red" Visible='<%# (short)Eval("record_status") == 0 %>' CausesValidation="True" OnCommand="EnableStudent" CommandName="Cancel">已禁用</asp:LinkButton>
                    </ItemTemplate>
                    <HeaderStyle CssClass="col-md-2" />
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>
                <h4>当前学生数据库为空</h4>
            </EmptyDataTemplate>
            <FooterStyle BackColor="#ECECEC" ForeColor="Black" />
            <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" />
            <PagerSettings FirstPageText="&amp;lt;&amp;lt;&amp;nbsp;首页&amp;nbsp;" LastPageText="&amp;nbsp;尾页&amp;nbsp;&amp;gt;&amp;gt;" Mode="NextPreviousFirstLast" NextPageText="下一页&amp;nbsp;&amp;gt;&amp;nbsp;" PreviousPageText="&amp;nbsp;&amp;lt;&amp;nbsp;上一页" />
            <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Right" />
            <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
            <SortedAscendingCellStyle BackColor="#F7F7F7" />
            <SortedAscendingHeaderStyle BackColor="#4B4B4B" />
            <SortedDescendingCellStyle BackColor="#E5E5E5" />
            <SortedDescendingHeaderStyle BackColor="#242121" />
        </asp:GridView>
        </div>
        <asp:SqlDataSource ID="sdsStudentList" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
            SelectCommand="SELECT [id_student], [alias], [password], [name], [gender], [grade], [belong], [phone], [email], [record_status] FROM [bhStudent]">
        </asp:SqlDataSource>
    </div>
</asp:Content>
