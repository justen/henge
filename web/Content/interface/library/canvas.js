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
		this.location	= { x: 0, y: 0 };
		this.position	= { x: 0, y: 0 };
		this.mapWidth	= 0;
		this.mapHeight	= 0;
		this.canvas		= new Element('div', {
			id:	'canvas'
		});
		this.marker		= new Element('div', {
			id: 	'youarehere',
			left: 	0,
			top:	0,
		});

		this.marker.inject(this.canvas);
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
		this.location.x = x;
		this.location.y = y;
		
		this.marker.setStyles({
			left: 	x * TILE_SIZE,
			top:	y * TILE_SIZE
		});
		
		this.jump(x, y);
	},
	

	jump: function(x, y)
	{
		this.position = { 
			x: this.centre.x - x * TILE_SIZE - TILE_SIZE / 2, 
			y: this.centre.y - y * TILE_SIZE - TILE_SIZE / 2 
		};
		this.refresh();
	},


	resize: function()
	{
		this.mapSize 	= this.map.getSize();
		this.mapWidth 	= Math.ceil(this.mapSize.x / TILE_SIZE);
		this.mapHeight 	= Math.ceil(this.mapSize.y / TILE_SIZE);
		this.centre		= { x: this.mapSize.x / 2, y: this.mapSize.y / 2 };
		this.jump(this.location.x, this.location.y);
	},
	
	
	// Update is called to update elements within the map on a regular tick
	update: function()
	{
		/* Previous jquery code:
			request.beginBatch();

			for (var i=0; i<this.visible.length; i++)
			{
				var tile = this.visible[i];
		
				request.map.getTileUpdate(tile.x, tile.y, tile.bound.handleUpdate);
			}
		
			request.endBatch();
		*/
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
		
		
		var x, y, dx, dy, opacity, tile, i=0;
		var queue = new Array();
	
		for (y=startY; y<=endY; y++)
		{
			dy = Math.abs(y - this.location.y);
	
			if (dy <= MAP_RANGE)
			{
				if (!this.tiles[y]) this.tiles[y] = new Array();
	
				for (x=startX; x<=endX; x++)
				{
					dx = Math.abs(x - this.location.x);
	
					if (dx <= MAP_RANGE)
					{
						opacity = 1.0;
						if (dx >= MAP_RANGE - 1 || dy >= MAP_RANGE - 1) opacity -= 0.5;
						if (dx == MAP_RANGE || dy == MAP_RANGE)			opacity -= 0.3;
	
						tile = this.tiles[y][x];
	
						if (!tile)
						{ 
							queue[i++] = this.tiles[y][x] = tile = new giTile(this.canvas, x, y, opacity);
						}
						
						tile.show(opacity);
						this.visible.push(tile);
					}
				}
			}	
		}
		
		if (queue.length > 0) request.getTileData(queue);
	},	
});

