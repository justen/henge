<%@ Page Language="C#" Title="Henge" MasterPageFile="~/Views/Shared/Layout.master" Inherits="System.Web.Mvc.ViewPage<Henge.Web.Controllers.GameViewModel>" %>



<asp:Content ContentPlaceHolderID="MainContent" runat="server">
	<script type="text/javascript" src="<%= Url.Content("~/Content/interface/boot.js") %>"></script>


	<div id="map"></div>
		<div class="title">Map</div>	
		<br/>
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
	
			<form method="post" action="<%= Url.Action("Move") %>" >
				<dl>
					<dd class="button">
						<input type="submit" name="button" id="NorthWest" value="North West" tabindex="1" />
						<input type="submit" name="button" id="North" value="North" tabindex="2" />
						<input type="submit" name="button" id="NorthEast" value="North East" tabindex="3" />
					</dd>
					<dd class="button">
						<input type="submit" name="button" id="West" value="West" tabindex="4" />
						<input type="submit" name="button" id="Up" value="Up" tabindex="5" />
						<input type="submit" name="button" id="East" value="East" tabindex="6" />
					</dd>
					<dd class="button">
						<input type="submit" name="button" id="SouthWest" value="South West" tabindex="7" />
						<input type="submit" name="button" id="South" value="South" tabindex="8" />
						<input type="submit" name="button" id="SouthEast" value="South East" tabindex="9" />
					</dd>
					<dd class="button">
						<input type="submit" name="button" id="Down" value="Down" tabindex="10" />
					</dd>
				</dl>
			</form>	
	

</asp:Content>