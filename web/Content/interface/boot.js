/*
	Henge Interface
	Bootstrap
*/


var map 		= null;
var log			= null;
var dialog		= null;
//var menu		= null;
var request 	= null;
var interface	= null;
var library		= null;
var avatar		= null;
var assets		= [
	{ type: 'js',	path: 'library/map.js' },
	{ type: 'js',	path: 'library/canvas.js' },
	{ type: 'js',	path: 'library/tile.js' },
	{ type: 'js',	path: 'library/request.js' },
	{ type: 'js',	path: 'library/library.js' },
	{ type: 'js',	path: 'library/interface.js' },
	{ type: 'js',	path: 'library/log.js' },
	{ type: 'js',	path: 'library/widget/bar.js' },
	{ type: 'js',	path: 'library/widget/icon.js' },
	{ type: 'js',	path: 'library/widget/avatar.js' },
	{ type: 'css',	path: 'styles/map.css' },
	{ type: 'css',	path: 'styles/interface.css' },
];


function boot()
{
	if (!dialog)
	{
		dialog = new giDialog();
		dialog.show('Loading scripts...', assets.length);
	}
	
	if (asset = assets.pop())
	{
		dialog.increment();
		switch (asset.type)
		{
			case 'css':	new Asset.css(root + 'Content/interface/' + asset.path);								boot();			break;				
			case 'js':	new Asset.javascript(root + 'Content/interface/' + asset.path, { onload: function() { 	boot(); }});	break;
		}
	}
	else if (!library)
	{
		library	= new giLibrary();
	}
	else if (!map)
	{
		dialog.hide();
		request 	= new giRequest();
		log			= new giLog();
		map 		= new giMap();
		interface	= new giInterface();
		resize();
	}
}


function resize()
{
	if (map) 
	{
		var height	= window.getSize().y - $('content').getPosition().y;
		var inner	= height - log.height - 25;
		
		$('content').setStyle('height', height);
		$('menu').setStyle('height', inner);
		map.resize(inner);	
	}
}


//window.addEvent('domready', boot);
window.addEvent('domready', function () { new Asset.javascript(root + 'Content/interface/library/dialog.js', { onload: boot }); });
window.addEvent('resize', resize);


/*-------------------------------------- Constants ------------------------------------------*/
var TILE_SIZE	= 128;
var MAP_RANGE	= 10;
var ENERGY_GAIN = 1.0;
