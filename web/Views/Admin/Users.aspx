<%@ Page Language="C#" Title="User Administration" MasterPageFile="~/Views/Shared/Layout.master" Inherits="System.Web.Mvc.ViewPage<Henge.Web.Controllers.UsersViewModel>" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
			<div class="title">User Administration</div>
			<table class="main">
				<tr>
					<th>Name</th>
					<th>Clan</th>
					<th>Email</th>
					<th>Enabled</th>
					<th colspan="2" />
				</tr>
			<% foreach (var account in Model.accounts) { %>
				<tr>
					<form method="post" action="<%= Url.Action("UserDetail") %>" >
						<td><%= account.Name %></td>
						<td><%= account.Clan %></td>
						<td><%= account.Email %></td>
						<td><%= account.Enabled %></td>
						<td>
							<input type="hidden" name="name" id="name" value="<%= account.Name %>"/>
							<input type="submit" name="submit" id="submit" value="Edit"/>
						</td>
					</form>
					<form method="post" action="<%= Url.Action("DeleteAccount") %>" >
						<td>
							<input type="hidden" name="name" id="name" value="<%= account.Name %>"/>
							<input type="submit" name="submit" id="submit" value="Delete"/>
						</td>
					</form>
				</tr>
			<% } %>
			</table>
</asp:Content>
