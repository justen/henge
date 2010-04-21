<%@ Page Language="C#" Title="CreateCharacter" MasterPageFile="~/Views/Shared/Layout.master" Inherits="System.Web.Mvc.ViewPage<Henge.Web.Controllers.AccountViewModel>" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
	<div class="title">Create Character</div>
	<form method="post" action="<%= Url.Action("Create") %>" >
		<dl>
			<dt><label for="name">Name:</label></dt>
			<dd><input type="text" name="name" id="name" tabindex="4" value=""/></dd>
			
			<dd class="button">
				<input type="submit" name="submit" id="submit" value="Create" tabindex="7" />
			</dd>
		</dl>
	</form>
</asp:Content>