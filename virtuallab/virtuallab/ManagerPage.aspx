<%@ Page Title="实验管理" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManagerPage.aspx.cs" Inherits="virtuallab.ManagerPage" %>

<asp:Content ID="BodyConent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>仿真实验管理中心</h2>
    <hr />
    <div class="row">
        <div class="col-md-8">
            <asp:GridView ID="gvExperiment" runat="server" AllowPaging="True" AllowSorting="True" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="4" CssClass="table col-md-12" DataSourceID="sdsExperiment" ForeColor="Black" GridLines="Horizontal" AutoGenerateColumns="False" PageSize="12" ShowFooter="True" DataKeyNames="id_experiment" OnSelectedIndexChanged="gvExperiment_SelectedIndexChanged" SelectedIndex="0" OnRowDataBound="gvExperiment_RowDataBound" OnDataBinding="gvExperiment_DataBinding" OnDataBound="gvExperiment_DataBound">
                <Columns>
                    <asp:TemplateField HeaderText="ID" SortExpression="id_experiment" Visible="False">
                        <ItemTemplate>
                            <asp:Label ID="lbID" runat="server" Text='<%# Eval("id_experiment", "{0}") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Template" SortExpression="template_uri" Visible="False">
                        <ItemTemplate>
                            <asp:Label ID="lbTemplateUri" runat="server" Text='<%# Eval("template_uri", "{0}") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="实验名称" SortExpression="title">
                        <EditItemTemplate>
                            <asp:TextBox ID="tbTitle" runat="server" Text='<%# Bind("title", "{0}") %>' Width="100%"></asp:TextBox>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:LinkButton ID="lbNewExp" runat="server" OnCommand="InsertExperiment" CommandName="Insert">新建实验</asp:LinkButton>
                        </FooterTemplate>
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
                            <asp:LinkButton ID="lbDistribute" runat="server" CausesValidation="True" OnCommand="DistributeTasks" CssClass="col-md-9">全员分配</asp:LinkButton>
                        </ItemTemplate>
                        <HeaderStyle CssClass="col-md-3" />
                    </asp:TemplateField>
                    <asp:TemplateField ShowHeader="False">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbSelect" runat="server" CausesValidation="True" CommandName="Select" Text="详情&gt;"></asp:LinkButton>
                        </ItemTemplate>
                        <HeaderStyle CssClass="col-md-1" />
                    </asp:TemplateField>
                </Columns>
                <FooterStyle BackColor="#ECECEC" ForeColor="Black" />
                <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" />
                <PagerSettings FirstPageText="&amp;lt;&amp;lt;&amp;nbsp;首页&amp;nbsp;" LastPageText="&amp;nbsp;尾页&amp;nbsp;&amp;gt;&amp;gt;" Mode="NextPreviousFirstLast" NextPageText="下一页&amp;nbsp;&amp;gt;&amp;nbsp;" PreviousPageText="&amp;nbsp;&amp;lt;&amp;nbsp;上一页" />
                <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Right" />
                <SelectedRowStyle BackColor="Silver" Font-Bold="True" ForeColor="White" />
                <SortedAscendingCellStyle BackColor="#F7F7F7" />
                <SortedAscendingHeaderStyle BackColor="#4B4B4B" />
                <SortedDescendingCellStyle BackColor="#E5E5E5" />
                <SortedDescendingHeaderStyle BackColor="#242121" />
            </asp:GridView>
        </div>
        <div class="col-md-4">
            <asp:FormView ID="fvExperiment" runat="server" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black" GridLines="Horizontal" CssClass="col-md-12" DataSourceID="sdsExperiment" DefaultMode="Edit">
                <EditItemTemplate>
                    <table class="table">
                        <tr>
                            <td class="col-md-12" colspan="2">
                                <asp:Label ID="lbExpID" runat="server" Text='<%# Eval("id_experiment", "{0}") %>' Visible="False"></asp:Label>
                                <asp:TextBox ID="tbTitle" runat="server" Width="100%" Text='<%# Bind("title", "{0}") %>' BackColor="Silver" BorderStyle="None"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="col-md-3">详细描述</td>
                            <td class="col-md-9">
                                <asp:TextBox ID="tbDetail" runat="server" Width="100%" Height="100%" Text='<%# Bind("memo", "{0}") %>' BackColor="Silver" BorderStyle="None" Rows="14" TextMode="MultiLine" Wrap="False"></asp:TextBox>
                        </tr>
                        <tr>
                            <td>代码模板</td>
                            <td>
                                <asp:TextBox ID="tbCodeUri" runat="server" Width="100%" Text='<%# Bind("template_uri", "{0}") %>' BackColor="Silver" BorderStyle="None"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>报告链接</td>
                            <td>
                                <asp:TextBox ID="tbRjsonUri" runat="server" Width="100%" Text='<%# Bind("rjson_uri", "{0}") %>' BackColor="Silver" BorderStyle="None"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>创建时间</td>
                            <td>
                                <asp:Label ID="lbCreateDate" runat="server" Text='<%# Eval("create_date", "{0}") %>'></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>更新时间</td>
                            <td>
                                <asp:Label ID="lbUpdateDate" runat="server" Text='<%# Eval("update_date", "{0}") %>'></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td style="text-align:right">
                                <asp:LinkButton ID="lbEdit" runat="server" OnCommand="UpdateExperiment" CommandName="Cancel" ForeColor="Yellow">更新</asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </EditItemTemplate>
                <EditRowStyle BackColor="Gray" Font-Bold="True" ForeColor="White" />
                <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Right" />
            </asp:FormView>
        </div>
        <asp:SqlDataSource ID="sdsExperiment" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
            SelectCommand="SELECT [id_experiment], [title], [rjson_uri], [template_uri], [memo], [create_date], [update_date], [delete_date], [record_status] FROM [bhExperiment]">
        </asp:SqlDataSource>
    </div>
</asp:Content>
