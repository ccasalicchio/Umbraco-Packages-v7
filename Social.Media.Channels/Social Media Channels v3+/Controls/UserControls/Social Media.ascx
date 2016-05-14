<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Social Media.ascx.cs"
    Inherits="UserControls_Social_Media" ClientIDMode="Static" %>
<strong>Style:
    <asp:DropDownList ID="dpStyles" runat="server" AutoPostBack="true">
    </asp:DropDownList>
</strong><strong>
    <asp:CheckBox ID="chkShowLabel" runat="server" Text="Show Channel Label" OnCheckedChanged="chkShowLabel_CheckedChanged" /></strong>

<table style="width:100%;text-align:center;">
    <tr>
        <td>
            <asp:Table ID="tableLinks" runat="server">
            </asp:Table>
        </td>
        <td style="vertical-align:top; width:200px;text-align:left;">
            <table>
                <tr>
                    <th>Theme Name</th>
                    <td><%=Name %></td>
                </tr>
                <tr>
                    <th>Description</th>
                    <td><%=Description %></td>
                </tr>
                <tr>
                    <th>Creator</th>
                    <td><%=Creator %></td>
                </tr>
                <tr>
                    <th>Created Date</th>
                    <td><%=Created %></td>
                </tr>
                <tr>
                    <th>Url</th>
                    <td><a href="<%=Url %>" target="_blank"><%=Url %></a></td>
                </tr>
            </table>
        </td>
    </tr>
</table>
