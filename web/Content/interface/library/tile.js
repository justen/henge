/*
	Henge Interface
	Map Tile
*/

var giTile = new Class(
{
	initialize: function(canvas, x, y, opacity)
	{
		this.x			= x;
		this.y			= y;
		this.visible 	= true;
		this.type 		= -1;
		
		/*this.bound = {
			handleData:		this.handleData.bind(this),
			handleUpdate:	this.handleUpdate.bind(this)
		};*/


		this.tile = new Element('div', {
			'class': 'tile',
			styles: {
				left:		this.x * TILE_SIZE,
				top:		this.y * TILE_SIZE,
				width:		TILE_SIZE,
				height: 	TILE_SIZE,
				opacity:	opacity
			}
		});

		//this.tile.set('text', x + ', ' + y);
		this.tile.inject(canvas);	
	},


	show: function(opacity)
	{
		if (!this.visible)
		{
			this.tile.setStyles({
				display: '',
				opacity: opacity
			});
			this.visible = true;
		}
	},


	hide: function()
	{
		if (this.visible)
		{
			this.tile.setStyle('display', 'none');
			this.visible = false;
		}
	},


	handleData: function(data)
	{
		this.type = data.Type;
		
		if (this.type != -1)
		{
			//this.locationIcon 	= new giIcon(this.tile, library.icons[0]);
			//this.peopleIcon 	= new giIcon(this.tile, library.icons[1]);
			//this.animalsIcon 	= new giIcon(this.tile, library.icons[2]);
			//this.structuresIcon = new giIcon(this.tile, library.icons[3]);

			this.tile.setStyles({
				'background-color': data.Colour
			});
			this.tile.set('title', data.Name);
		}
	},
	
	
	handleUpdate: function(data)
	{
		this.tile.set('text', data);
	},
});

