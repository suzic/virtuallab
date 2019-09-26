<%@ Page Title="我的任务" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StudentPage.aspx.cs" Inherits="virtuallab.StudentPage" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>我的实验中心</h2>
    <div class="row">
        <asp:GridView ID="gvMyTasks" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" CellPadding="4" CssClass="table col-md-12" DataKeyNames="id_task" DataSourceID="sdsTasks" ForeColor="#333333" GridLines="None">
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            <Columns>
                <asp:BoundField DataField="fid_student" HeaderText="我" SortExpression="fid_student" Visible="False" />
                <asp:BoundField DataField="id_task" HeaderText="任务ID" InsertVisible="False" ReadOnly="True" SortExpression="id_task" Visible="False" />
                <asp:BoundField DataField="fid_experiment" HeaderText="实验ID" SortExpression="fid_experiment" Visible="False" />
                <asp:TemplateField HeaderText="实验名称">
                    <ItemTemplate>
                        <asp:Label ID="lbExperiName" runat="server" CssClass="col-md-12" Text="Label"></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle CssClass="col-md-4" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="实验描述">
                    <ItemTemplate>
                        <asp:Label ID="lbExperiMemo" runat="server" CssClass="col-md-12" Text="Label"></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle CssClass="col-md-4" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="分配人" SortExpression="fid_manager">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("fid_manager") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lbManagerName" runat="server" CssClass="col-md-12" Text='<%# Bind("fid_manager") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle CssClass="col-md-1" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="得分" SortExpression="score">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("score") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lbTaskScore" runat="server" CssClass="col-md-12" Text='<%# Bind("score") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle CssClass="col-md-1" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="完成情况" SortExpression="complete">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("complete") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lbComplete" runat="server" Text='<%# (short)Eval("complete")==1?"已完成":"未完成" %>' CssClass="col-md-6"></asp:Label>
                        <asp:LinkButton ID="lbEnterTask" runat="server" CssClass="col-md-6">进入实验&gt;</asp:LinkButton>
                    </ItemTemplate>
                    <HeaderStyle CssClass="col-md-2" />
                </asp:TemplateField>
            </Columns>
            <EditRowStyle BackColor="#999999" />
            <EmptyDataTemplate>
                <h2>你还尚未获得任何实验任务</h2>
            </EmptyDataTemplate>
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <PagerSettings FirstPageText="&amp;lt;&amp;lt;&amp;nbsp;首页&amp;nbsp;" LastPageText="&amp;nbsp;尾页&amp;nbsp;&amp;gt;&amp;gt;" Mode="NextPreviousFirstLast" NextPageText="下一页&amp;nbsp;&amp;gt;&amp;nbsp;" PreviousPageText="&amp;nbsp;&amp;lt;&amp;nbsp;上一页" />
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <SortedAscendingCellStyle BackColor="#E9E7E2" />
            <SortedAscendingHeaderStyle BackColor="#506C8C" />
            <SortedDescendingCellStyle BackColor="#FFFDF8" />
            <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
        </asp:GridView>
        <asp:SqlDataSource ID="sdsManager" runat="server"></asp:SqlDataSource>
        <asp:SqlDataSource ID="sdsTasks" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="SELECT [id_task], [fid_student], [fid_experiment], [fid_manager], [score], [complete] FROM [bhTask]"></asp:SqlDataSource>
        <asp:SqlDataSource ID="sdsExperiment" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="SELECT DISTINCT [id_experiment], [title], [template_uri], [rjson_uri], [memo], [create_date], [update_date], [delete_date], [record_status] FROM [bhExperiment] WHERE ([id_experiment] = @id_experiment)">
            <SelectParameters>
                <asp:ControlParameter ControlID="gvMyTasks" Name="id_experiment" PropertyName="SelectedValue" Type="Int32" />
            </SelectParameters>
        </asp:SqlDataSource>
    </div>
</asp:Content>
