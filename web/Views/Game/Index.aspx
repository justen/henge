<%@ Page Language="C#" Title="Henge" MasterPageFile="~/Views/Shared/Layout.master" Inherits="System.Web.Mvc.ViewPage<Henge.Web.Controllers.GameViewModel>" %>



<asp:Content ContentPlaceHolderID="MainContent" runat="server">
	<script type="text/javascript" src="<%= Url.Content("~/Content/interface/boot.js") %>"></script>


	<div id="map"></div>
	<div class="title">Map</div>	
		
	<br/>
	You are <%= Model.Avatar.BaseAppearance.Name %> of clan <%= Model.Clan %>. 
	
	<div id="description">
		You are standing at <%= Model.Avatar.Location.Coordinates.X %>, <%= Model.Avatar.Location.Coordinates.Y %>, A <%= Model.Avatar.Location.BaseAppearance.Name %>.
		It appears <%= Model.Avatar.Location.BaseAppearance.ShortDescription %>. After looking for a while you determine that it <%= Model.Avatar.Location.BaseAppearance.Description %>.
		<%	if (Model.Others.Count > 0) { %>
			<%= string.Format("Also here {0} {1}", (Model.Others.Count > 1) ? "are" : "is", Model.Others[0].BaseAppearance.Name) %>
			<% for (int i=1; i<Model.Others.Count; i++) { %>
				<%= string.Format(" and {0}", Model.Others[i].BaseAppearance.Name) %>
			<% } %>
		<% } %>
	</div>

	<table>
		<tr>
			<td><button onclick="interface.move(-1, -1);" tabindex="1">NW</button></td>
			<td><button onclick="interface.move(0, -1);" tabindex="2">N</button></td>
			<td><button onclick="interface.move(1, -1);" tabindex="3">NE</button></td>
		</tr>
		<tr>
			<td><button onclick="interface.move(-1, 0);" tabindex="4">W</button></td>
			<td></td>
			<td><button onclick="interface.move(1, 0);" tabindex="6">E</button></td>
		</tr>
		<tr>
			<td><button onclick="interface.move(-1, 1);" tabindex="7">SW</button></td>
			<td><button onclick="interface.move(0, 1);" tabindex="8">S</button></td>
			<td><button onclick="interface.move(1, 1);" tabindex="9">SE</button></td>
		</tr>
	</table>

</asp:Content>