/*
	Henge Interface
	Map Canvas
*/

var giCanvas = new Class(
{
	initialize: function(map)
	{
		this.map		= map;
		this.mapSize	= { x: 0, y: 0 }
		this.centre		= { x: 0, y: 0 };
		this.position	= { x: 0, y: 0 };
		this.mapWidth	= 0;
		this.mapHeight	= 0;
		this.canvas		= new Element('div', {
			id:	'canvas'
		});
		
		this.canvas.set('morph', { duration: 150, onComplete: this.refresh.bind(this) });
		this.avatar = new giAvatar(this.canvas);
		
		this.canvas.inject(this.map);

		this.tiles		= new Array();
		this.visible 	= new Array();
	},


	move: function(dx, dy)
	{
		this.position.x += dx;
		this.position.y += dy;
		this.refresh();
	},
	
	setLocation: function(x, y)
	{
		this.avatar.setLocation(x, y);
		this.jump(x, y);
	},
	

	jump: function(x, y)
	{
		this.position = { 
			x: this.centre.x - x * TILE_SIZE - TILE_SIZE / 2, 
			y: this.centre.y - y * TILE_SIZE - TILE_SIZE / 2 
		};
		
		this.canvas.morph({ left: this.position.x, top: this.position.y });
	},


	resize: function()
	{
		this.mapSize 	= this.map.getSize();
		this.mapWidth 	= Math.ceil(this.mapSize.x / TILE_SIZE);
		this.mapHeight 	= Math.ceil(this.mapSize.y / TILE_SIZE);
		this.centre		= { x: this.mapSize.x / 2, y: this.mapSize.y / 2 };
		this.jump(this.avatar.x, this.avatar.y);
	},
	
	
	update: function()
	{
	},


	refresh: function()
	{
		this.canvas.setStyles({
			left: this.position.x,
			top: this.position.y
		});

		var startX = Math.floor(-this.position.x / TILE_SIZE);
		var startY = Math.floor(-this.position.y / TILE_SIZE);
		var endX	= startX + this.mapWidth;
		var endY	= startY + this.mapHeight;

		for (var i=0; i<this.visible.length; i++)
		{
			var tile = this.visible[i];

			if (tile.y < startY || tile.y > endY || tile.x < startX || tile.x > endX) tile.hide();
		}

		this.visible.length = 0;
		
		
		var x, y, dx, dy, opacity, tile, status;
		var queue = new Array();
	
		for (y=startY; y<=endY; y++)
		{
			dy = Math.abs(y - this.avatar.y);
	
			if (dy <= MAP_RANGE)
			{
				if (!this.tiles[y]) this.tiles[y] = new Array();
	
				for (x=startX; x<=endX; x++)
				{
					dx = Math.abs(x - this.avatar.x);
	
					if (dx <= MAP_RANGE)
					{
						status = '';
						if (dy == MAP_RANGE) status += (y < this.avatar.y) ? '-top' : '-bottom';
						if (dx == MAP_RANGE) status += (x < this.avatar.x) ? '-left' : '-right';
	
						tile = this.tiles[y][x];
						if (!tile) queue.push(this.tiles[y][x] = tile = new giTile(this, x, y));
						
						tile.show(status);
						this.visible.push(tile);
					}
				}
			}	
		}
		
		if (queue.length > 0) request.getTileData(queue);
	},
	
	neighbours: function(x, y)
	{
		return [
			{ side: 'left',		opposite: 'right',  tile: this.tiles[y][x-1] },
			{ side: 'right',	opposite: 'left',	tile: this.tiles[y][x+1] },
			{ side: 'top',		opposite: 'bottom', tile: this.tiles[y-1] ? this.tiles[y-1][x] : null },
			{ side: 'bottom',	opposite: 'top',	tile: this.tiles[y+1] ? this.tiles[y+1][x] : null }
		];
	}
});

