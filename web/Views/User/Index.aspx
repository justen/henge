<%@ Page Language="C#" Title="Home" MasterPageFile="~/Views/Shared/Layout.master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
			<div class="title">Please login</div>
			<form method="post" action="<%= Url.Action("Login") %>" >
				<dl>
					<dt><label for="username">Username:</label></dt>
					<dd><input type="text" name="username" id="username" tabindex="1" value=""/></dd>
					
					<dt><label for="password">Password:</label></dt>
					<dd><input type="password" name="password" id="password" tabindex="2" /></dd>
					
					<dd class="button">
						<input type="submit" name="submit" id="submit" value="Login" tabindex="3" />
					</dd>
				</dl>
			</form>
			<div class="title">Create User</div>
			<form method="post" action="<%= Url.Action("Create") %>" >
				<dl>
					<dt><label for="name">Name:</label></dt>
					<dd><input type="text" name="name" id="name" tabindex="4" value=""/></dd>
					
					<dt><label for="password">Password:</label></dt>
					<dd><input type="password" name="password" id="password" tabindex="5" /></dd>
					<dt><label for="passwordRepeat">Repeat Password:</label></dt>
					<dd><input type="password" name="passwordRepeat" id="passwordRepeat" tabindex="6" /></dd>
					
					<dd class="button">
						<input type="submit" name="submit" id="submit" value="Create" tabindex="7" />
					</dd>
				</dl>
			</form>
</asp:Content>