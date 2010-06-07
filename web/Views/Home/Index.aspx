<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Layout.master" Inherits="System.Web.Mvc.ViewPage" %>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
	<script type="text/javascript" src="<%= Url.Content("~/Content/interface/boot.js") %>"></script>
	<div id="main">
		<div id="menu">Menu here</div>
		<div id="map"></div>
	</div>
	<div id="log">
		<table id="log-table">
			<tr><td>Time</td><td>Log entry</td></tr>
			<tr><td>Time</td><td>Log entry</td></tr>
			<tr><td>Time</td><td>Log entry</td></tr>
			<tr><td>Time</td><td>Log entry</td></tr>
			<tr><td>Time</td><td>Log entry</td></tr>
		</table>
	</div>
</asp:Content>