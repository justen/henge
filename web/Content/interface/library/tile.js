/*
	Henge Interface
	Map Tile
*/

var giTile = new Class(
{
	initialize: function(parent, x, y)
	{
		this.x			= x;
		this.y			= y;
		this.visible 	= false;
		this.type 		= null;
		this.name		= "";
		this.parent		= parent;

		this.tile = new Element('div', {
			'class': 'tile',
			styles: {
				left:		this.x * TILE_SIZE,
				top:		this.y * TILE_SIZE,
				width:		TILE_SIZE,
				height: 	TILE_SIZE,
			}
		});
		
		this.transitions	= new Element('div', { 'class': 'transition' });
		this.edge			= new Element('div', { 'class': 'transition' });
		
		this.transitions.hide();
		this.transitions.inject(this.tile);
		this.edge.inject(this.tile);
		this.tile.inject(parent.canvas);	
	},


	show: function(status)
	{
		if (!this.visible)
		{
			if (status)
			{
				this.edge.set('class', 'transition border' + status);
				this.edge.show();
			}
			else this.edge.hide();

			this.tile.show();
			this.visible = true;
		}
	},


	hide: function()
	{
		if (this.visible)
		{
			this.tile.hide();
			this.visible = false;
		}
	},


	handleData: function(data)
	{
		if (data.Type)
		{
			var type 		= library.types[data.Type];
			this.type		= data.Type;
			this.name		= type.Name;
			this.priority 	= type.Priority;
			
			this.tile.addClass('tile-' + type.Name);
			this.tile.setStyles({
				'background-color': type.Colour
			});
			this.tile.set('title', type.Name);
			
			this.parent.neighbours(this.x, this.y).each(function(item) {
				if (item.tile && item.tile.type && item.tile.type != this.type)
				{
					if (item.tile.priority > this.priority) this.addTransition(item.side, item.tile.name);
					else									item.tile.addTransition(item.opposite, this.name);
				}
			}, this);
		}
	},
	
	
	handleUpdate: function(data)
	{
		this.tile.set('text', data);
	},
	
	
	addTransition: function(side, name)
	{
		var images = this.transitions.getStyle('background-image');
		this.transitions.setStyle('background-image', (images ? images + ", " : "") + "url('" + root + "Content/interface/images/tiles/" + name + "-" + side + ".png')");
		this.transitions.show();
	}
});

