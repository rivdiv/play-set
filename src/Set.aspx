<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Set.aspx.cs" Inherits="Set" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="Scripts/jquery-1.7.1.js"></script>
    <link href="Content/Set.css" rel="stylesheet" />
    <style>
 
    </style>
</head>
<body>
    <div class="mainContainer">
        <div class="header">SET<a href="https://github.com/you"><img class="forkme" src="Images/fork-me-right-white.png" alt="Fork me on GitHub" /></a></div>
        <div class="mainContent">
            <form id="form1" runat="server">

                <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                <asp:UpdatePanel ID="upNotify" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="notifyContainer">
                            <asp:Panel ID="panelNotify" runat="server">
                                <asp:Label ID="lblNotify" runat="server" />&nbsp;<asp:ImageButton ID="btnTweet" runat="server" ToolTip="Tweet this" ImageUrl="~/Images/bird_blue_16.png" Visible="false" />
                            </asp:Panel>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <table style="width: 100%">
                    <tr>
                        <td class="col1">
                            <asp:UpdatePanel ID="upControls" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div>
                                        <asp:Button ID="btnReset" runat="server" OnClick="btnReset_Click" Text="Reset" BackColor="#339933" />
                                        &nbsp;<asp:Button ID="btnPick3More" runat="server" Text="Pick 3 More" OnClick="btnPick3More_Click" BackColor="Purple" />
                                        &nbsp;<asp:Button ID="btnStop" runat="server" OnClick="btnStop_Click" Text="Stop" BackColor="Red" />
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td style="text-align: right">
                            <asp:UpdatePanel ID="upCountDown" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div>
                                        Time:
                            <asp:Label ID="lblCountDown" runat="server" />
                                        <asp:Timer ID="Timer1" runat="server" Interval="1000" OnTick="Timer1_Tick">
                                        </asp:Timer>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>

                        </td>
                    </tr>
                    <tr>
                        <td class="col1" colspan="2" style="height: 30px"></td>
                    </tr>
                    <tr>
                        <td class="col1">
                            <asp:UpdatePanel ID="upCards" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="title">Current Deck</div>
                                    <asp:DataList ID="dlCards" runat="server" RepeatColumns="4" OnItemDataBound="dlCards_ItemDataBound"
                                        DataKeyField="CardID" OnItemCommand="dlCards_ItemCommand">
                                        <ItemTemplate>
                                            <div class="bigCard">
                                                <asp:HiddenField ID="hfSelected" runat="server" />
                                                <asp:ImageButton ID="ImageButton1" runat="server" CssClass="roundedCornerBig" Height="130" Width="130" CommandName="Click" CommandArgument='<%#Eval("CardID") %>' />
                                            </div>
                                        </ItemTemplate>
                                    </asp:DataList>
                                </ContentTemplate>
                            </asp:UpdatePanel>

                        </td>
                        <td>
                            <asp:UpdatePanel ID="upYourSets" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>

                                    <div class="title">
                                        Your Sets (<asp:Label ID="lblYourSetsCount" runat="server">0</asp:Label>)
                                    </div>

                                    <asp:DataList ID="dlYourSets" runat="server" RepeatColumns="3" OnItemDataBound="dlYourSets_ItemDataBound" RepeatDirection="Horizontal">
                                        <ItemTemplate>
                                            <div class="smallCard">
                                                <asp:Image ID="Image1" runat="server" Height="65" Width="65" CssClass="roundedCornerSmall" />
                                            </div>
                                        </ItemTemplate>
                                    </asp:DataList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>

                </table>

            </form>
        </div>
        <div class="footer">Powered by ASP.NET - @rivdiv 2013</div>
    </div>
</body>
</html>
