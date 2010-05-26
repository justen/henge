/*
	Prototype Game Interface
	Author: Dan Parnham
	Date: 16/03/2008

	Bootstrap
*/

var map 	= null;
var request = null;

new Asset.css(root + 'Content/interface/styles/map.css');
new Asset.javascript(root + 'Content/interface/library/library.js');
new Asset.javascript(root + 'Content/interface/library/request.js');
new Asset.javascript(root + 'Content/interface/library/tile.js');
new Asset.javascript(root + 'Content/interface/library/canvas.js');
new Asset.javascript(root + 'Content/interface/library/map.js');

function boot()
{
	if (typeof(giMap) != 'undefined')
	{
		request = new giRequest();
		map 	= new giMap();
		resize();
	}
	else boot.delay(10);
}


function resize()
{
	var height = window.getSize().y - $('content').getPosition().y - 12;

	//$('panel').setStyle('height', height);
	if (map) map.resize(height);
}


window.addEvent('domready', boot);
window.addEvent('resize', resize);


/*-------------------------------------- Constants ------------------------------------------*/
//var TILE_SIZES		= [400, 200, 100, 50];
var TILE_SIZE = 96;
var MAP_RANGE = 10;
