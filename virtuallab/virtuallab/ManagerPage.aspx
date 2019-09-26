<%@ Page Title="实验管理" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManagerPage.aspx.cs" Inherits="virtuallab.ManagerPage" %>

<asp:Content ID="BodyConent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>仿真实验管理中心</h2>
    <div class="row">
        <div class="col-md-8">
            <asp:GridView ID="gvExperiment" runat="server" AllowPaging="True" AllowSorting="True" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="4" CssClass="table col-md-12" DataSourceID="sdsExperiment" ForeColor="Black" GridLines="Horizontal" AutoGenerateColumns="False">
                <Columns>
                    <asp:TemplateField HeaderText="实验名称" SortExpression="title">
                        <EditItemTemplate>
                            <asp:TextBox ID="tbTitle" runat="server" Text='<%# Bind("title", "{0}") %>' Width="100%"></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lbTitle" runat="server" CssClass="col-md-12" Text='<%# Eval("title", "{0}") %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle CssClass="col-md-5" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="描述">
                        <EditItemTemplate>
                            <asp:TextBox ID="tbMemo" runat="server" Text='<%# Bind("memo", "{0}") %>' Width="100%"></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lbMemo" runat="server" CssClass="col-md-12" Text='<%# Eval("memo", "{0}") %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle CssClass="col-md-3" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="实验分配">
                        <ItemTemplate>
                            <asp:Label ID="lbTaskRatio" runat="server" CssClass="col-md-3" Text="0/100"></asp:Label>
                            <asp:LinkButton ID="lbDistribute" runat="server" CssClass="col-md-9">全员分配</asp:LinkButton>
                        </ItemTemplate>
                        <HeaderStyle CssClass="col-md-3" />
                    </asp:TemplateField>
                    <asp:TemplateField ShowHeader="False">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbSelect" runat="server" CausesValidation="False" CommandName="Select" Text="详情&gt;"></asp:LinkButton>
                        </ItemTemplate>
                        <HeaderStyle CssClass="col-md-1" />
                    </asp:TemplateField>
                </Columns>
                <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
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
        <div class="col-md-4">
            <asp:DetailsView ID="dvExperiment" runat="server" Height="50px" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="4" CssClass="table col-md-12" DataSourceID="sdsExperiment" ForeColor="Black" GridLines="Horizontal" AutoGenerateRows="False" DataKeyNames="id_experiment">
                <EditRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
                <EmptyDataTemplate>
                    <h4>请在左侧列表中选择一个实验的详情后查看</h4>
                </EmptyDataTemplate>
                <Fields>
                    <asp:BoundField DataField="memo" HeaderText="描述" SortExpression="memo">
                        <HeaderStyle CssClass="col-md-3" />
                    </asp:BoundField>
                    <asp:BoundField DataField="template_uri" HeaderText="代码模板" SortExpression="template_uri" />
                    <asp:BoundField DataField="rjson_uri" HeaderText="报告格式" SortExpression="rjson_uri" />
                    <asp:BoundField DataField="create_date" HeaderText="创建时间" SortExpression="create_date" />
                    <asp:BoundField DataField="update_date" HeaderText="更新时间" SortExpression="update_date" />
                </Fields>
                <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" />
                <HeaderTemplate>
                    <asp:Label ID="lbDetialTitle" runat="server" CssClass="col-md-12" Text='<%# Eval("title", "{0}") %>'></asp:Label>
                </HeaderTemplate>
                <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Right" />
            </asp:DetailsView>
        </div>
        <asp:SqlDataSource ID="sdsExperiment" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="SELECT [id_experiment], [title], [rjson_uri], [template_uri], [memo], [create_date], [update_date], [delete_date], [record_status] FROM [bhExperiment]"></asp:SqlDataSource>
    </div>
</asp:Content>
