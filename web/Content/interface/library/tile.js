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
			class:	'tile',
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
			var colour = [Math.floor(Math.random() * 255), Math.floor(Math.random() * 255), Math.floor(Math.random() * 255)].rgbToHex();
			this.tile.setStyles({
				border: '1px solid #000',
				'background-color': colour
			});
			this.tile.set('text', data.Name);
		}
	},
	
	
	handleUpdate: function(data)
	{
		this.tile.set('text', data);
	},

	/*setData: function(data)
	{
		data = data.split(',');
		this.type = data[0];

		if (this.type == 0) this.tile.setStyle('background-color', '#888888');
		else
		{
			this.tile.addClass(this.type);

			if (this.type == 'mansion') this.tile.setStyle('background-image', 'url(/images/mountains2.png)');
		}
	},*/
});

