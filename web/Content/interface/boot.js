/*
	Henge Interface
	Bootstrap
*/


var map 		= null;
var log			= null;
//var menu		= null;
var request 	= null;
var interface	= null;

new Asset.css(root + 'Content/interface/styles/map.css');
new Asset.javascript(root + 'Content/interface/library/log.js');
new Asset.javascript(root + 'Content/interface/library/interface.js');
new Asset.javascript(root + 'Content/interface/library/library.js');
new Asset.javascript(root + 'Content/interface/library/request.js');
new Asset.javascript(root + 'Content/interface/library/tile.js');
new Asset.javascript(root + 'Content/interface/library/canvas.js');
new Asset.javascript(root + 'Content/interface/library/map.js');


function boot()
{
	if (typeof(giMap) != 'undefined')
	{
		request 	= new giRequest();
		log			= new giLog();
		map 		= new giMap();
		interface	= new giInterface();
		
		resize();
	}
	else boot.delay(10);
}


function resize()
{
	if (map) 
	{
		var height	= window.getSize().y - $('content').getPosition().y;
		var menu	= $('menu');
		var inner	= height - log.height - 25;
		
		$('content').setStyle('height', height);
		$('menu').setStyle('height', inner);
		map.resize(inner);	
	}
}


window.addEvent('domready', boot);
window.addEvent('resize', resize);


/*-------------------------------------- Constants ------------------------------------------*/
var TILE_SIZE = 96;
var MAP_RANGE = 10;
