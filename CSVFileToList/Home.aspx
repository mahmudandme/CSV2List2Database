<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="CSVFileToList.Home" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table>
        <tr>
            <td>
                <asp:Label ID="Label1" runat="server" Text="Enter the file name"></asp:Label> 
            </td>
            <td>
                <asp:TextBox ID="fileNameTextBox" runat="server"></asp:TextBox>
            </td>
            <td>
                <asp:Button ID="importButton" runat="server" Text="Import" OnClick="importButton_Click" />
            </td>
            <td>
                <asp:Label ID="importStatusLabel" runat="server" Text="Label"></asp:Label>
            </td>
        </tr>
    </table>
    </div>
        <div>
            <asp:GridView ID="GridView1" runat="server"></asp:GridView>
        </div>
    </form>
</body>
</html>
