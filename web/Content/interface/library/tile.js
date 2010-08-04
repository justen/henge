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
		this.type 		= -1;
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
		
		this.transitions = new Element('div', {
			'class': 'transition'
		});
		
		this.edge = new Element('div', {
			'class': 'transition'
		});
		
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
		this.type = data.Type;
		
		if (this.type != -1)
		{
			//this.locationIcon 	= new giIcon(this.tile, library.icons[0]);
			//this.peopleIcon 	= new giIcon(this.tile, library.icons[1]);
			//this.animalsIcon 	= new giIcon(this.tile, library.icons[2]);
			//this.structuresIcon = new giIcon(this.tile, library.icons[3]);
			this.name		= data.Name;
			this.priority 	= data.Priority;
			
			this.tile.addClass('tile-' + data.Name);
			this.tile.setStyles({
				'background-color': data.Colour
			});
			this.tile.set('title', data.Name);
			
			this.parent.neighbours(this.x, this.y).each(function(item) {
				if (item.tile && item.tile.type != -1 && item.tile.name != this.name)
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

