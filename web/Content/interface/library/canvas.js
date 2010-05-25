/*
	Prototype Game Interface
	Author: Dan Parnham
	Date: 16/03/2008

	Map Canvas
*/

var giCanvas = new Class(
{
	initialize: function(map)
	{
		this.map		= map;
		this.mapSize	= 0;
		this.mapWidth	= 0;
		this.mapHeight	= 0;
		this.position	= { x: 0, y: 0 };
		this.location	= { x: 0, y: 0 };
		//this.queryData	= new Array();

		this.canvas = new Element('div', {
			'id':	'canvas'
		});

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


	/*zoom: function(direction)
	{
		var cx = (this.position.x - 0.5 * this.mapSize.x) / this.tileSize;
		var cy = (this.position.y - 0.5 * this.mapSize.y) / this.tileSize;

		if (direction > 0) 	this.mapZoom--;
		else				this.mapZoom++;

		if (this.mapZoom < 0) this.mapZoom = 0;
		if (this.mapZoom >= TILE_SIZES.length) this.mapZoom = TILE_SIZES.length - 1;

		this.tileSize	= TILE_SIZES[this.mapZoom];
		this.mapWidth 	= Math.ceil(this.mapSize.x / this.tileSize);
		this.mapHeight 	= Math.ceil(this.mapSize.y / this.tileSize);
		this.position.x = cx * this.tileSize + 0.5 * this.mapSize.x;
		this.position.y = cy * this.tileSize + 0.5 * this.mapSize.y;

		this.refresh();
	},*/


	jump: function(x, y)
	{
		this.position.x = -x * TILE_SIZES[this.mapZoom];
		this.position.y = -y * TILE_SIZES[this.mapZoom];
		this.refresh();
	},


	resize: function()
	{
		this.mapSize 	= this.map.getSize();
		this.mapWidth 	= Math.ceil(this.mapSize.x / TILE_SIZE);
		this.mapHeight 	= Math.ceil(this.mapSize.y / TILE_SIZE);

		this.refresh();
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
							this.tiles[y][x] = tile = new giTile(this.canvas, x, y, opacity);
	
							queue[i++] = tile;
							//request.getTileData(x, y, tile.bound.handleData);
						}
	
						tile.show(opacity);
						this.visible.push(tile);
					}
				}
			}	
		}
		
		if (queue.length > 0) request.getTileData(queue);	

		/* Old PHP ajax code
			var requiredData = '';
	
			for (var y=startY; y<=endY; y++)
			{
				if (!this.tiles[y]) this.tiles[y] = new Array();
	
				for (var x=startX; x<=endX; x++)
				{
					if (!this.tiles[y][x])
					{
						this.tiles[y][x] = new giTile(this.canvas, x, y, this.mapZoom);
	
						this.queryData.push([x,y]);
						requiredData += x + ',' + y + ';';
					}
	
					this.tiles[y][x].show(this.mapZoom);
					this.visible.push(this.tiles[y][x]);
				}
			}
	
			if (requiredData)
			{
				request.pushQueue({
					options: 'm=' + requiredData,
					onResponse: this.handleTileData.bind(this)
				});
			}
		*/
		
		/* Previous jquery code
			var x, y, dx, dy, opacity, tile;
		
			request.beginBatch();
		
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
								this.tiles[y][x] = tile = new giTile(this.canvas, x, y, opacity);
		
								request.map.getTileData(x, y, tile.bound.handleData);
							}
		
							tile.show(opacity);
							this.visible.push(tile);
						}
					}
				}
			}
		
			request.endBatch();
		*/
	},


/*	handleTileData: function(response)
	{
		var data = response.split(';');

		for (var i=0; i<data.length; i++)
		{
			var loc = this.queryData.shift();
			this.tiles[loc[1]][loc[0]].setData(data[i]);
		}
	},*/
});

