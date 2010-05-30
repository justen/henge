<%@ Page Language="C#" Title="Account" MasterPageFile="~/Views/Shared/Layout.master" Inherits="System.Web.Mvc.ViewPage<Henge.Web.Controllers.AccountViewModel>" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
			<div class="title">Change Password</div>
			<form method="post" action="<%= Url.Action("Account") %>" >
				<dl>
					<dt><label for="current">Current Password:</label></dt>
					<dd><input type="password" name="current" id="current" tabindex="1" value=""/></dd>
					
					<dt><label for="password">New Password:</label></dt>
					<dd><input type="password" name="password" id="password" tabindex="2" /></dd>
					<dt><label for="passwordRepeat">Repeat Password:</label></dt>
					<dd><input type="password" name="passwordRepeat" id="passwordRepeat" tabindex="3" /></dd>
					
					<dd class="button">
						<input type="submit" name="submit" id="submit" value="Change" tabindex="3" />
					</dd>
				</dl>
			</form>

			<div class="title">Characters</div>
				<form method="post" action="<%= Url.Action("Clan") %>" >
					<dl>
						<dt><label for="current">Clan:</label></dt>
						<dd><input type="text" name="clan" id="clan" tabindex="4" value="<%= Model.Clan %>"/></dd>
						<dd class="button">
							<input type="submit" name="submit" id="submit" value="Change" tabindex="5" />
						</dd>
					</dl>
				</form>
			<table class="main">
			<% int i=0; %>
			<% foreach (var avatar in Model.Avatars) { %>
				<tr>
					<form method="post" action="<%= Url.Action("ConnectAvatar") %>" >
						<td><%= avatar.BaseAppearance.Name %></td>
						<td>
							<input type="hidden" name="index" id="index" value="<%= i %>"/>
							<input type="submit" name="submit" id="submit" value="Connect"/>
						</td>
					</form>
					<form method="post" action="<%= Url.Action("DeleteAvatar") %>" >
						<td>
							<input type="hidden" name="index" id="index" value="<%= i++ %>"/>
							<input type="submit" name="submit" id="submit" value="Delete"/>
						</td>
					</form>
				</tr>
			<% } %>
			</table>
			<form method="post" action="<%= Url.Action("CreateAvatar") %>" >	
				<input type="submit" name="new" id="new" value="New" tabindex="7" />
			</form>
</asp:Content>