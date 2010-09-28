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
		this.z			= 0;
		this.visible 	= false;
		this.type 		= null;
		this.name		= "";
		this.parent		= parent;
		this.contents	= { people: new Array(), animals: new Array(), structures: new Array(), items: new Array() };

		this.tile = new Element('div', {
			'class':	'tile',
			styles: {
				left:	this.x * TILE_SIZE,
				top:	this.y * TILE_SIZE,
				width:		TILE_SIZE,
				height: 	TILE_SIZE
			}
		});
		
		this.canvas = new Element('canvas', {
			'class':	'layer',
			width:		TILE_SIZE,
			height: 	TILE_SIZE,
		}).inject(this.tile);
		
		this.output = new Element('div', {
			'class': 'layer'
		}).inject(this.tile);
		
		this.rendered	= false;
		this.edge		= new Element('div', { 'class': 'layer' }).hide().inject(this.tile);

		this.tile.inject(parent.canvas);	
	},


	show: function(status)
	{
		if (!this.visible)
		{
			if (status)
			{
				this.edge.set('class', 'layer border' + status);
				this.edge.show();
			}
			else this.edge.hide();

			this.tile.show();
			this.visible = true;
		}
		
		
			var text = "<small>";
			for (type in this.contents) if (this.contents[type].length) text += type.capitalize() + ': ' + this.contents[type].length + '<br />';
			text += "</small>";
			this.output.set('html', text);
		
	},
	
	
	addContent: function(type, id)
	{
		if (!this.contents[type].contains(id)) this.contents[type].push(id);
	},
	
	removeContent: function(type, id)
	{
		var index = this.contents[type].indexOf(id);
		if (index > -1) this.contents[type].splice(index, 1);
	},


	hide: function()
	{
		if (this.visible)
		{
			this.tile.hide();
			this.visible = false;
		}
	},


	initialise: function(data)
	{
		if (data.Type)
		{
			var type 		= library.types[data.Type];
			this.type		= type;
			this.z			= data.Z;
			this.name		= type.Name;
			this.priority 	= type.Priority;

			this.tile.setStyle('background-color', type.Colour);
			this.tile.set('title', type.Name + ' (' + this.z + ')');
			
			this.canvas.getContext("2d").drawImage(this.type.Image, 0, 0);
			this.checkNeighbours(true);
		}
	},
	
	
	/*handleUpdate: function(data)
	{
		this.tile.set('text', data);
	},*/
	
	
	checkNeighbours: function(recurse)
	{
		var n = null;
		
		if (recurse)
		{
			n = this.parent.neighbours(this.x, this.y);
			n.each(function(item) { if (item) item.checkNeighbours(false); });
		}
		
		if (this.type && !this.rendered)
		{
			var all 	= true;
			if (!n) n	= this.parent.neighbours(this.x, this.y);
		
			for (var i=0; i<8; i++)
			{
				if (!n[i] || !n[i].type)
				{
					all = false;
					break;
				}
			}
			
			if (all)
			{
				this.rendered = true;
				
				var transitions = [
					(n[2].type != this.type && n[2].priority > this.priority) ? n[2].type.Transition : null,
					(n[4].type != this.type && n[4].priority > this.priority) ? n[4].type.Transition : null,
					(n[6].type != this.type && n[6].priority > this.priority) ? n[6].type.Transition : null,
					(n[0].type != this.type && n[0].priority > this.priority) ? n[0].type.Transition : null
				];
				
				var gradients = [ 
					((this.z - n[2].z) + (this.z - n[0].z) + (n[2].z - n[1].z) + (n[0].z - n[1].z)) / 4,
					this.z - n[2].z,
					((this.z - n[2].z) + (n[4].z - this.z) + (n[3].z - n[2].z) + (n[4].z - n[3].z)) / 4,
					this.z - n[0].z,
					0,
					n[4].z - this.z,
					((this.z - n[0].z) + (n[6].z - this.z) + (n[7].z - n[0].z) + (n[6].z - n[7].z)) / 4,
					n[6].z - this.z,
					((n[4].z - this.z) + (n[6].z - this.z) + (n[5].z - n[4].z) + (n[5].z - n[6].z)) / 4
				];
				
				for (i=0; i<9; i++) 
				{
					if (gradients[i] > GRADIENT_LIMIT)	gradients[i] = GRADIENT_LIMIT;
					if (gradients[i] < -GRADIENT_LIMIT)	gradients[i] = -GRADIENT_LIMIT;
				}

				this.renderTransitions(transitions);
				this.renderGradients(gradients);
			}
		}
	},

	
	renderTransitions: function(transitions)
	{
		var ctx = this.canvas.getContext("2d");
		
		ctx.translate(HALF_TILE, HALF_TILE);
		
		for (i=0; i<4; i++)
		{
			if (transitions[i]) ctx.drawImage(transitions[i], -HALF_TILE, -HALF_TILE);
			ctx.rotate(HALF_PI);
		}
	},
	
	
	renderGradients: function(g)
	{
		var flat = true;
		
		for (i=0; i<9; i++) 
		{
			if (Math.abs(g[i]) > GRADIENT_THRESHOLD)
			{
				flat = false;
				break;
			}
		}
		
		if (!flat)
		{
			var ctx			= this.canvas.getContext("2d");
			var image		= ctx.getImageData(0, 0, TILE_SIZE, TILE_SIZE);
			var data		= image.data;
			var index		= 0;
			var dt			= 1.0 / HALF_TILE;
			
			for (dy=0; dy<1.0; dy+=dt)
			{
				iy 	= 1 - dy;
				v	= dy * g[3] + iy * g[0];
				for (dx=0; dx<1.0; dx+=dt)
				{
					ix		= 1 - dx;	
					h 		= dx * g[1] + ix * g[0];
					value 	= Math.floor(Math.pow(iy, Math.abs(h * EDGE_SCALE)) * h + Math.pow(ix, Math.abs(v * EDGE_SCALE)) * v);

					data[index++] += value;	data[index++] += value;	data[index++] += value;	index++;
				}
				v = dy * g[5] + iy * g[2];
				for (dx=0; dx<1.0; dx+=dt)
				{
					ix		= 1 - dx;	
					h 		= ix * g[1] + dx * g[2];		
					value 	= Math.floor(Math.pow(iy, Math.abs(h * EDGE_SCALE)) * h + Math.pow(dx, Math.abs(v * EDGE_SCALE)) * v);

					data[index++] += value;	data[index++] += value;	data[index++] += value;	index++;
				}
			}
			for (dy=0; dy<1.0; dy+=dt)
			{
				iy	= 1 - dy;
				v	= iy * g[3] + dy * g[6];
				for (dx=0; dx<1.0; dx+=dt)
				{
					ix		= 1 - dx;	
					h 		= dx * g[7] + ix * g[6];
					value 	= Math.floor(Math.pow(dy, Math.abs(h * EDGE_SCALE)) * h + Math.pow(ix, Math.abs(v * EDGE_SCALE)) * v);

					data[index++] += value;	data[index++] += value;	data[index++] += value;	index++;
				}
				v = iy * g[5] + dy * g[8];
				for (dx=0; dx<1.0; dx+=dt)
				{
					ix		= 1 - dx;	
					h 		= ix * g[7] + dx * g[8];
					value 	= Math.floor(Math.pow(dy, Math.abs(h * EDGE_SCALE)) * h + Math.pow(dx, Math.abs(v * EDGE_SCALE)) * v);
					
					data[index++] += value;	data[index++] += value;	data[index++] += value;	index++;
				}
			}
	
			ctx.putImageData(image, 0, 0);
			this.canvas.show();
		}
	},	
});



