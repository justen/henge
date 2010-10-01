<%@ Page Language="C#" Title="User Administration" MasterPageFile="~/Views/Shared/Layout.master" Inherits="System.Web.Mvc.ViewPage<Henge.Web.Controllers.UserDetailViewModel>" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
			<div class="title">User Administration for <%= Model.user.Name %></div>
			<dl>
				<dt><label for="current">Clan:</label></dt>
				<dd>
					<input type="text" name="clan" value="<%= Model.user.Clan %>" />
				</dd>
				<dt><label for="current">E-Mail:</label></dt>
				<dd>
					<input type="text" name="email" value="<%= Model.user.Email %>" />
				</dd>
				<dt><label for="current">Enabled:</label></dt>
				<dd>
					<input type="checkbox" name="clan" value="<%= Model.user.Enabled %>" />
				</dd>
			</dl>	
			<div class="title">Avatars</div>	
			<table class="main">
			<% int i=0; %>
			<% foreach (var avatar in Model.user.Avatars) { %>
				<tr>
					<form method="post" action="<%= Url.Action("EditAvatar") %>" >
						<td><%= avatar.Name %></td>
						<td>
							<input type="hidden" name="userName" id="userName" value="%<= Model.user.Name %>" />
							<input type="hidden" name="index" id="index" value="<%= i %>"/>
							<input type="submit" name="submit" id="submit" value="Connect"/>
						</td>
					</form>
					<form method="post" action="<%= Url.Action("DeleteAvatar") %>" >
						<td>
							<input type="hidden" name="userName" id="userName" value="%<= Model.user.Name %>" />
							<input type="hidden" name="index" id="index" value="<%= i++ %>"/>
							<input type="submit" name="submit" id="submit" value="Delete"/>
						</td>
					</form>
				</tr>
			<% } %>
			</table>
</asp:Content>
