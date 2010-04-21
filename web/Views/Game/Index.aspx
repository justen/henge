<%@ Page Language="C#" Title="Henge" MasterPageFile="~/Views/Shared/Layout.master" Inherits="System.Web.Mvc.ViewPage<Henge.Web.Controllers.GameViewModel>" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
	<div class="title">Map</div>
	You are <%= Model.Avatar.BaseAppearance.Name %> of clan <%= Model.Clan %>. You are standing at <%= Model.Avatar.Location.X %>, <%= Model.Avatar.Location.Y %>, A <%= Model.Avatar.Location.Type.BaseAppearance.Name %>.
	It appears <%= Model.Avatar.Location.Type.BaseAppearance.ShortDescription %>. After looking for a while you determine that it <%= Model.Avatar.Location.Type.BaseAppearance.Description %>.
	<% if (Model.Others.Count > 0) { %>
		Also here
		<% if (Model.Others.Count > 1) { %>
		 are <%= Model.Others[0].BaseAppearance.Name %><%
		  for (int i = 1; i < Model.Others.Count -1; i ++ ) { %>, <%= Model.Others[i].BaseAppearance.Name %> 
			<% } %> and 
		<% } else { %> is <% } %> <%= Model.Others[Model.Others.Count -1].BaseAppearance.Name %>
	<% } %>	
</asp:Content>