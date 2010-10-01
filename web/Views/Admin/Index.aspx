<%@ Page Language="C#" Title="Administration" MasterPageFile="~/Views/Shared/Layout.master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
			<div class="title">User Administration</div>
			<a href="<%= Url.Action("Users") %>">Administer Users</a>
</asp:Content>