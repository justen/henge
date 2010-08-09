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

		this.tile = new Element('div', {
			'class': 'tile',
			styles: {
				left:		this.x * TILE_SIZE,
				top:		this.y * TILE_SIZE,
				width:		TILE_SIZE,
				height: 	TILE_SIZE,
			}
		});
		
		//this.vertexCount	= 0;
		//this.vertices		= [ 0, 0, 0, 0 ];
		//this.hasNormal		= false;
		//this.normalCount	= 0;
		//this.normals		= [ null, null, null, null ];
		this.rendered		= false;
		this.hasVertices	= false;
		this.vertices		= null;
		
		
		this.transitions	= new Element('div', { 'class': 'transition' }).hide().inject(this.tile);	
		/*this.quads			= [
			new Element('div', { 'class': 'quad0'}).hide().inject(this.tile),
			new Element('div', { 'class': 'quad1'}).hide().inject(this.tile),
			new Element('div', { 'class': 'quad2'}).hide().inject(this.tile),
			new Element('div', { 'class': 'quad3'}).hide().inject(this.tile)
		];*/
		this.canvas			= new Element('canvas', { 'class': 'transition', width: TILE_SIZE, height: TILE_SIZE }).hide().inject(this.tile);
			
		//this.gradientBuffer = '';
		this.gradients		= new Element('div', { 'class': 'transition' }).hide().inject(this.tile);

		this.edge = new Element('div', { 'class': 'transition' }).inject(this.tile);

		this.tile.inject(parent.canvas);	
	},


	show: function(status)
	{
		if (!this.rendered) this.checkNeighbours();
		
		if (!this.visible)
		{
			if (status)
			{
				this.edge.set('class', 'transition border' + status);
				//if (Browser.Engine.gecko) 		this.edge.setStyle('background', '-moz-linear-gradient(' + status + ', rgba(238,238,238,255), rgba(238,238,238,0))');
				//else if (Browser.Engine.webkit)	this.edge.setStyle('background', '-webkit-gradient(linear, top left, bottom right, from(rgba(238,238,238,0)), to(rgba(238,238,238,255)))');
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
			this.z			= data.Z;
			this.name		= type.Name;
			this.priority 	= type.Priority;
			
			this.tile.addClass('tile-' + type.Name);
			this.tile.setStyles({
				'background-color': type.Colour
			});
			this.tile.set('title', type.Name);
			
			//this.overlay.set('opacity', this.z * 0.005); //15);
			//this.overlay.show();
			
			var neighbours = this.parent.neighbours(this.x, this.y);
			
			/*neighbours.each(function(item) {
				if (item.side && item.tile && item.tile.type)
				{
					if (item.tile.type != this.type)
					{
						if (item.tile.priority > this.priority) this.addTransition(item.side, item.tile.name);
						else									item.tile.addTransition(item.opposite, this.name);
					}
				}
			}, this);*/
			
			for (var i=0; i<8; i++)
			{
				var t = neighbours[i];
				
				if (t && t.type)
				{
					if (i % 2 == 0 && t.type != this.type)
					{
						if (t.priority > this.priority) this.addTransition(SIDES[i], t.name);
						else							t.addTransition(OPPOSITES[i], this.name);
					}
				}
				else neighbours[i] = null;
			}
			
			/*if (neighbours[0] &&  neighbours[1] && neighbours[2])
			{
				var mean = (neighbours[0].z + neighbours[1].z + neighbours[2].z + this.z) / 4;
				
				this.addVertex(0, mean);
				neighbours[0].addVertex(1, mean);
				neighbours[1].addVertex(2, mean);
				neighbours[2].addVertex(3, mean);
			}
			if (neighbours[2] && neighbours[3] && neighbours[4])
			{
				var mean = (neighbours[2].z + neighbours[3].z + neighbours[4].z + this.z) / 4;
				
				this.addVertex(1, mean);
				neighbours[2].addVertex(2, mean);
				neighbours[3].addVertex(3, mean);
				neighbours[4].addVertex(0, mean);
			}
			if (neighbours[4] && neighbours[5] && neighbours[6])
			{
				var mean = (neighbours[4].z + neighbours[5].z + neighbours[6].z + this.z) / 4;
				
				this.addVertex(2, mean);
				neighbours[4].addVertex(3, mean);
				neighbours[5].addVertex(0, mean);
				neighbours[6].addVertex(1, mean);
			}
			if (neighbours[6] && neighbours[7] && neighbours[0])
			{
				var mean = (neighbours[6].z + neighbours[7].z + neighbours[0].z + this.z) / 4;
				
				this.addVertex(3, mean);
				neighbours[6].addVertex(0, mean);
				neighbours[7].addVertex(1, mean);
				neighbours[0].addVertex(2, mean);
			}*/
			
			
			//this.checkNeighbours();
			
			//neighbours.each(function(neighbour) { if (neighbour) neighbour.checkNeighbours(); });
			
			
			/*if (neighbours[0] && neighbours[1] && neighbours[2])
			{
				var vertices = this.getVertices(neighbours[1].z, neighbours[2].z, this.z, neighbours[0].z);
				
				neighbours[1].renderQuad(2, vertices[0], vertices[1], vertices[3], vertices[4]);
				neighbours[2].renderQuad(3, vertices[1], vertices[2], vertices[4], vertices[5]);
				this.renderQuad(0, vertices[4], vertices[5], vertices[7], vertices[8]);
				neighbours[0].renderQuad(1, vertices[3], vertices[4], vertices[6], vertices[7]);
			}
			if (neighbours[2] && neighbours[3] && neighbours[4])
			{
				var vertices = this.getVertices(neighbours[2].z, neighbours[3].z, neighbours[4].z, this.z);
				
				neighbours[2].renderQuad(2, vertices[0], vertices[1], vertices[3], vertices[4]);
				neighbours[3].renderQuad(3, vertices[0], vertices[1], vertices[3], vertices[4]);
				neighbours[4].renderQuad(0, vertices[4], vertices[5], vertices[7], vertices[8]);
				this.renderQuad(1, vertices[0], vertices[1], vertices[3], vertices[4]);
			}
			if (neighbours[4] && neighbours[5] && neighbours[6])
			{
				var vertices = this.getVertices(this.z, neighbours[4].z, neighbours[5].z, neighbours[6].z);
				
				this.renderQuad(2, vertices[0], vertices[1], vertices[3], vertices[4]);
				neighbours[4].renderQuad(3, vertices[0], vertices[1], vertices[3], vertices[4]);
				neighbours[5].renderQuad(0, vertices[4], vertices[5], vertices[7], vertices[8]);
				neighbours[6].renderQuad(1, vertices[0], vertices[1], vertices[3], vertices[4]);
			}
			if (neighbours[6] && neighbours[7] && neighbours[0])
			{
				var vertices = this.getVertices(neighbours[0].z, this.z, neighbours[6].z, neighbours[7].z);
				
				neighbours[0].renderQuad(2, vertices[0], vertices[1], vertices[3], vertices[4]);
				this.renderQuad(3, vertices[0], vertices[1], vertices[3], vertices[4]);
				neighbours[6].renderQuad(0, vertices[4], vertices[5], vertices[7], vertices[8]);
				neighbours[7].renderQuad(1, vertices[0], vertices[1], vertices[3], vertices[4]);
			}
					
			/*		var dz 			= item.tile.z - this.z;
					var strength	= Math.abs(dz) / 255;	
					
					if (dz > 0) 		
					{
						var shade = (item.side == 'top' || item.side == 'right') ? '0,0,0' : '238,238,238';
						this.addGradient(shade, strength, item.side);
						item.tile.addGradient(shade, strength, item.opposite);
					}
					else if (dz < 0)
					{
						var shade = (item.side == 'top' || item.side == 'right') ? '238,238,238' : '0,0,0';
						this.addGradient(shade, strength, item.side);
						item.tile.addGradient(shade, strength, item.opposite);
					}
				}
			}, this);*/
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
	},
	
	
	checkNeighbours: function()
	{
		//if (!this.rendered)
		//{
			var n 	= this.parent.neighbours(this.x, this.y);
			var all	= true;
			
			if (!this.hasVertices)
			{
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
					this.vertices		= this.getVertices(n[0].z, n[1].z, n[2].z, n[3].z, n[4].z, n[5].z, n[6].z, n[7].z);
					this.hasVertices	= true;
				}
			}
			
			if (this.hasVertices)
			{
				for (var i=0; i<8; i++)
				{
					all = n[i].hasVertices;
					if (!all) break;
				}
				
				if (all)
				{
					this.rendered = true;
					
					var shades = [
						this.getIllumination(this.getNormal(n[1].vertices[7], n[1].vertices[5], this.vertices[1], this.vertices[3], this.vertices[0])),
						this.getIllumination(this.getNormal(this.vertices[0], n[2].vertices[4], this.vertices[2], this.vertices[2], this.vertices[1])),
						this.getIllumination(this.getNormal(this.vertices[1], n[3].vertices[3], n[3].vertices[7], this.vertices[5], this.vertices[2])),
						this.getIllumination(this.getNormal(n[0].vertices[4], this.vertices[0], this.vertices[4], this.vertices[6], this.vertices[3])),
						this.getIllumination(this.getNormal(this.vertices[3], this.vertices[1], this.vertices[5], this.vertices[7], this.vertices[4])),
						this.getIllumination(this.getNormal(this.vertices[4], this.vertices[2], n[4].vertices[4], this.vertices[8], this.vertices[5])),
						this.getIllumination(this.getNormal(n[7].vertices[1], this.vertices[3], this.vertices[7], n[7].vertices[5], this.vertices[6])),
						this.getIllumination(this.getNormal(this.vertices[6], this.vertices[4], this.vertices[8], n[6].vertices[4], this.vertices[7])),
						this.getIllumination(this.getNormal(this.vertices[7], this.vertices[5], n[5].vertices[1], n[5].vertices[3], this.vertices[8]))
					];
					
					this.renderShades(shades);
					/*this.renderQuad(this.quads[0], [ shades[0], shades[1], shades[4], shades[3] ]);
					this.renderQuad(this.quads[1], [ shades[1], shades[2], shades[5], shades[4] ]);
					this.renderQuad(this.quads[2], [ shades[4], shades[5], shades[8], shades[7] ]);
					this.renderQuad(this.quads[3], [ shades[3], shades[4], shades[7], shades[6] ]);*/
				}
			}
		//}
	},
	
	
	getNormal: function(az, bz, cz, dz, ez)
	{
		var u	= { x: -HALF_TILE, 	y: 0,			z: az - ez };
		var v	= { x: 0, 			y: -HALF_TILE,	z: bz - ez };
		var n0	= { x: u.y * v.z - u.z * v.y, y: u.z * v.x - u.x * v.z, z: u.x * v.y - u.y * v.x };
		
		u		= v;
		v		= { x: HALF_TILE, 	y: 0,			z: cz - ez };
		var n1	= { x: u.y * v.z - u.z * v.y, y: u.z * v.x - u.x * v.z, z: u.x * v.y - u.y * v.x };

		v		= u;
		u		= { x: 0, 			y: HALF_TILE,	z: dz - ez };
		var n2	= { x: u.y * v.z - u.z * v.y, y: u.z * v.x - u.x * v.z, z: u.x * v.y - u.y * v.x };
		
		u		= v;
		v		= { x: -HALF_TILE, 	y: 0,			z: az - ez };
		var n3	= { x: u.y * v.z - u.z * v.y, y: u.z * v.x - u.x * v.z, z: u.x * v.y - u.y * v.x };

		return { x: n0.x + n1.x + n2.x + n3.x, y: n0.y + n1.y + n2.y + n3.y, z: n0.z + n1.z + n2.z + n3.z };
	},
	
	
	getVertices: function(z0, z1, z2, z3, z4, z5, z6, z7)
	{
		var result = [
			(z0 + z1 + z2 + this.z) / 4, 0, (z2 + z3 + z4 + this.z) / 4, 
			0, this.z, 0,
			(z0 + z6 + z7 + this.z) / 4, 0, (z4 + z5 + z6 + this.z) / 4
		];
		
		result[1] = (result[0] + result[2] + z2 + this.z) / 4;
		result[3] = (result[0] + result[6] + z0 + this.z) / 4;
		result[5] = (result[2] + result[8] + z4 + this.z) / 4;
		result[7] = (result[8] + result[6] + z6 + this.z) / 4;
		
		return result;
	},
	
	
	renderShades: function(shades) //quad, shades)
	{
		/*var mean0		= (shades[0] + shades[1] + shades[3]) / 3;
		var mean1		= (shades[1] + shades[2] + shades[3]) / 3;
		var shade0		= (mean0 > 0) ? '255,255,255,' : '0,0,0,';
		var shade1		= (mean1 > 0) ? '255,255,255,' : '0,0,0,';
		var strength0	= 0.5 * Math.abs(mean0);
		var strength1	= 0.5 * Math.abs(mean1);
		
		quad.setStyle('background', 
			'-moz-linear-gradient(top left, rgba(' + shade0 + strength0 + '), rgba(' + shade0 + strength0 + ') 50%, rgba(' + shade1 + strength1 + ') 50%)'
		).show();*/
		
		/*var top		= (shades[0] + shades[1]) / 2;			var topShade 	= (top > 0)		? '255,255,255,' : '0,0,0,';	var topStrength		= 0.5 * Math.abs(top);
		var right	= (shades[1] + shades[2]) / 2;			var rightShade	= (right > 0)	? '255,255,255,' : '0,0,0,';	var rightStrength	= 0.5 * Math.abs(right);
		var bottom	= (shades[2] + shades[3]) / 2;			var bottomShade = (bottom > 0)	? '255,255,255,' : '0,0,0,';	var bottomStrength	= 0.5 * Math.abs(bottom);
		var left	= (shades[3] + shades[0]) / 2;			var leftShade	= (left > 0)	? '255,255,255,' : '0,0,0,';	var leftStrength	= 0.5 * Math.abs(left);
		var centre	= (top + bottom + left + right) / 4;	var centreShade = (centre > 0)	? '255,255,255,' : '0,0,0,';	var centreStrength	= 0.5 * Math.abs(centre);
		
		var topWidth 	= 45 - topStrength * 90;	var bottomWidth	= 45 - bottomStrength * 90;		var verWidth = 100 - bottomWidth;
		var rightWidth	= 45 - rightStrength * 90;	var leftWidth	= 45 - leftStrength * 90;		var horWidth = 100 - leftWidth;
		
		quad.setStyle('background',
			'-moz-linear-gradient(top, rgba(' + topShade + topStrength + ') ' + topWidth + '%, rgba(' + centreShade + centreStrength + ') ' + verWidth + '%, rgba(' + bottomShade + bottomStrength + ')), ' +
			'-moz-linear-gradient(left, rgba(' + leftShade + leftStrength + ') ' + leftWidth + '%, rgba(' + centreShade + centreStrength + ') ' + horWidth + '%, rgba(' + rightShade + rightStrength + '))'
		).show();*/
		
		
		/*var c = [ { value: shades[0] }, { value: shades[1] }, { value: shades[2] }, { value: shades[3] }, { value: (shades[0] + shades[1] + shades[2] + shades[3]) / 4 } ];
		c[1].value -= c[4].value;
		c[3].value -= c[4].value;
		c.each(function (item) { item.shade = (item.value > 0) ? '255,255,255,' : '0,0,0,'; item.strength = 0.5 * Math.abs(item.value); item.out = 'rgba(' + item.shade + item.strength + ')'; });
		
		quad.setStyle('background',
			'-moz-linear-gradient(top left, ' + c[0].out + ', ' + c[0].out + ' 25%, ' + c[4].out + ' 25%, ' + c[4].out + ' 75%, ' + c[2].out + ' 75%, ' + c[2].out + '), ' + 
			'-moz-linear-gradient(top right, ' + c[1].out + ', ' + c[1].out + ' 25%, transparent 25%, transparent 75%, ' + c[3].out + ' 75%, ' + c[3].out + ')' 
		).show();*/

		var canvas	= this.canvas.getContext("2d");
		var image	= canvas.createImageData(TILE_SIZE, TILE_SIZE);
		var data	= image.data;
		var index	= 0;
		
		for (y=0; y<HALF_TILE; y++)
		{
			dy = y / HALF_TILE;
			iy = 1 - dy;
			for (x=0; x<HALF_TILE; x++)
			{
				dx		= x / HALF_TILE;
				ix		= 1 - dx;
				value 	= shades[0] * ix * iy + shades[1] * dx * iy + shades[4] * dx * dy + shades[3] * ix * dy;
				shade	= (value > 0) ? 255 : 0;
				data[index++] = shade;	data[index++] = shade;	data[index++] = shade;	data[index++] = Math.floor(128 * Math.abs(value));
				//data[index++] = 0;	data[index++] = 0;	data[index++] = 0;	data[index++] = Math.max(128 - 64 * (value + 1), 0);
			}
			for (x=0; x<HALF_TILE; x++)
			{
				dx		= x / HALF_TILE;
				ix		= 1 - dx;
				value 	= shades[1] * ix * iy + shades[2] * dx * iy + shades[5] * dx * dy + shades[4] * ix * dy;
				shade	= (value > 0) ? 255 : 0;
				data[index++] = shade;	data[index++] = shade;	data[index++] = shade;	data[index++] = Math.floor(128 * Math.abs(value));
				//data[index++] = 0;	data[index++] = 0;	data[index++] = 0;	data[index++] = 128 - 64 * (value + 1.0);
			}
		}
		for (y=0; y<HALF_TILE; y++)
		{
			dy = y / HALF_TILE;
			iy = 1 - dy;
			for (x=0; x<HALF_TILE; x++)
			{
				dx		= x / HALF_TILE;
				ix		= 1 - dx;
				value 	= shades[3] * ix * iy + shades[4] * dx * iy + shades[7] * dx * dy + shades[6] * ix * dy;
				shade	= (value > 0) ? 255 : 0;
				data[index++] = shade;	data[index++] = shade;	data[index++] = shade;	data[index++] = Math.floor(128 * Math.abs(value));
				//data[index++] = 0;	data[index++] = 0;	data[index++] = 0;	data[index++] = 128 - 64 * (value + 1.0);
			}
			for (x=0; x<HALF_TILE; x++)
			{
				dx		= x / HALF_TILE;
				ix		= 1 - dx;
				value 	= shades[4] * ix * iy + shades[5] * dx * iy + shades[8] * dx * dy + shades[7] * ix * dy;
				shade	= (value > 0) ? 255 : 0;
				data[index++] = shade;	data[index++] = shade;	data[index++] = shade;	data[index++] = Math.floor(128 * Math.abs(value));
				//data[index++] = 0;	data[index++] = 0;	data[index++] = 0;	data[index++] = 128 - 64 * (value + 1.0);
			}
		}

		canvas.putImageData(image, 0, 0);
		this.canvas.show();
	},
			
				
	/*checkNeighbours: function()
	{
		if (!this.hasNormal)
		{
			if (neighbours[0] && neighbours[1] && neighbours[2] && neighbours[4] && neighbours[6])
			{
				if (neighbours[0].type && neighbours[1].type && neighbours[2].type && neighbours[4].type && neighbours[6].type)
				{
					var normal	= this.getNormal(neighbours[0].z, neighbours[2].z, neighbours[4].z, neighbours[6].z);
					var illum	= this.getIllumination(normal);
					
					this.hasNormal = true;
					
					this.addNormal(0, illum);
					neighbours[0].addNormal(1, illum);
					neighbours[1].addNormal(2, illum);
					neighbours[2].addNormal(3, illum);
				}
			}
		}
	},*/
	
	
	addNormal: function(index, value)
	{
		this.normals[index] = value;
		this.normalCount++;
		
		if (this.normalCount == 4)
		{
			var mean0		= (this.normals[0] + this.normals[1] + this.normals[3]) / 3;
			var mean1		= (this.normals[1] + this.normals[2] + this.normals[3]) / 3;
			var shade0		= (mean0 > 0) ? '255,255,255,' : '0,0,0,';
			var shade1		= (mean1 > 0) ? '255,255,255,' : '0,0,0,';
			var strength0	= 0.5 * Math.abs(mean0);
			var strength1	= 0.5 * Math.abs(mean1);
			this.gradients.setStyle('background', 
				'-moz-linear-gradient(top left, rgba(' + shade0 + strength0 + '), rgba(' + shade0 + strength0 + ') 50%, rgba(' + shade1 + strength1 + ') 50%)' //, rgba(' + shade1 + strength1 + '))' //, ' +
				//'-moz-linear-gradient(top right, rgba(0,0,0,' + Math.abs(dz[1]) + '), rgba(0,0,0,' + Math.abs(mean) + '), rgba(0,0,0,' + Math.abs(dz[3]) + '))'
			).show();
		}
		
		this.checkNeighbours();
	},
	
	
	/*getNormal: function(az, bz, cz, dz)
	{
		var u	= { x: -TILE_SIZE, 	y: 0,			z: az - this.z };
		var v	= { x: 0, 			y: -TILE_SIZE,	z: bz - this.z };
		var n0	= { x: u.y * v.z - u.z * v.y, y: u.z * v.x - u.x * v.z, z: u.x * v.y - u.y * v.x };
		
		u		= v;
		v		= { x: TILE_SIZE, 	y: 0,			z: cz - this.z };
		var n1	= { x: u.y * v.z - u.z * v.y, y: u.z * v.x - u.x * v.z, z: u.x * v.y - u.y * v.x };

		v		= u;
		u		= { x: 0, 			y: TILE_SIZE,	z: dz - this.z };
		var n2	= { x: u.y * v.z - u.z * v.y, y: u.z * v.x - u.x * v.z, z: u.x * v.y - u.y * v.x };
		
		u		= v;
		v		= { x: -TILE_SIZE, 	y: 0,			z: az - this.z };
		var n3	= { x: u.y * v.z - u.z * v.y, y: u.z * v.x - u.x * v.z, z: u.x * v.y - u.y * v.x };

		return { x: n0.x + n1.x + n2.x + n3.x, y: n0.y + n1.y + n2.y + n3.y, z: n0.z + n1.z + n2.z + n3.z };
	},*/
	
	
	getIllumination: function(normal)
	{
		var mag	= Math.sqrt(normal.x*normal.x + normal.y*normal.y + normal.z*normal.z);
		
		return (mag != 0) ? (normal.x / mag) * LIGHT.x + (normal.y / mag) * LIGHT.y + (normal.z / mag) * LIGHT.z : 0;
	},
	
	
	addVertex: function(index, value)
	{
		this.vertices[index] = value;
		this.vertexCount++;
		
		if (this.vertexCount == 4)
		{
			var dz = [ (this.vertices[0] - this.z) / 128, (this.vertices[1] - this.z) / 128, (this.vertices[2] - this.z) / 128, (this.vertices[3] - this.z) / 128 ];
			var mean = (dz[0] + dz[1] + dz[2] + dz[3]) / 4;
			this.gradients.setStyle('background', 
				'-moz-linear-gradient(top left, rgba(0,0,0,' + Math.abs(dz[0]) + '), rgba(0,0,0,' + Math.abs(mean) + '), rgba(0,0,0,' + Math.abs(dz[2]) + ')), ' +
				'-moz-linear-gradient(top right, rgba(0,0,0,' + Math.abs(dz[1]) + '), rgba(0,0,0,' + Math.abs(mean) + '), rgba(0,0,0,' + Math.abs(dz[3]) + '))'
			).show();
			
			/*this.gradients.setStyle('background', 
				'-moz-linear-gradient(top, rgba(0,0,0,' + Math.abs(0.5 * (dz[0] + dz[1])) + '), rgba(0,0,0,' + Math.abs(mean) + '), rgba(0,0,0,' + Math.abs(0.5 * (dz[2] + dz[3])) + ')), ' +
				'-moz-linear-gradient(left, rgba(0,0,0,' + Math.abs(0.5 * (dz[0] + dz[3])) + '), rgba(0,0,0,' + Math.abs(mean) + '), rgba(0,0,0,' + Math.abs(0.5 * (dz[1] + dz[2])) + '))'
			).show();*/
		}
	},
	
	
	/*getVertices: function(az, bz, cz, dz)
	{
		return [ az, (az+bz) / 2, bz, (az+dz) / 2, (az + bz + cz + dz) / 4, (bz + cz) / 2, dz, (cz+dz) / 2, cz ];
	}, 
	
	renderQuad: function(index, av, bv, cv, dv)
	{
		var quad	= this.quads[index];
		var u		= { x: TILE_SIZE - 0, 	y: 0 - 0,			z: bv - av };
		var v		= { x: TILE_SIZE - 0, 	y: TILE_SIZE - 0,	z: cv - av };
		var w		= { x: 0 - 0,			y: TILE_SIZE - 0,	z: dv - av };
		var nuv		= { x: u.y * v.z - u.z * v.y, y: u.z * v.x - u.x * v.z, z: u.x * v.y - u.y * v.x };	
		var nvw		= { x: v.y * w.z - v.z * w.y, y: v.z * w.x - v.x * w.z, z: v.x * w.y - v.y * w.x };
		var muv		= Math.sqrt(nuv.x*nuv.x + nuv.y*nuv.y + nuv.z*nuv.z);
		var mvw		= Math.sqrt(nvw.x*nvw.x + nvw.y*nvw.y + nvw.z*nvw.z);
		var dotuv	= (nuv.x / muv) * LIGHT.x + (nuv.y / muv) * LIGHT.y + (nuv.z / muv) * LIGHT.z;
		var dotvw	= (nvw.x / mvw) * LIGHT.x + (nvw.y / mvw) * LIGHT.y + (nvw.z / mvw) * LIGHT.z;
		
		var cuv		= '0,0,0,'; //(dotuv > 0) ? '255,255,255,' : '0,0,0,';
		var cvw		= '0,0,0,'; //(dotvw > 0) ? '255,255,255,' : '0,0,0,';
		var suv		= 0.5 - 0.5 * dotuv * dotuv * dotuv * dotuv;
		var svw		= 0.5 - 0.5 * dotvw * dotvw * dotvw * dotvw;
	
		
		quad.setStyle('background', '-moz-linear-gradient(top right, rgba(' + cuv + suv + '), rgba(' + cvw + svw + '))');
		
		//quad.setStyles({
		//	'background-color'	: '#000', //(dotuv > 0) ? '#fff' : '#000',
		//	'opacity'			: 0.5 - 0.5 * dotuv * dotuv * dotuv,
		//});
		quad.show();
	},*/
	
	
	/*addGradient: function(shade, strength, side)
	{
		var width = Math.round(50 - 100 * strength);
		this.gradientBuffer += (this.gradientBuffer ? ', ' : '') + '-moz-linear-gradient(' + side + ', rgba(' + shade + ',' + strength + '), rgba(' + shade + ',0) ' + width + '%)';
		
		this.gradients.setStyle('background', this.gradientBuffer);
		this.gradients.show();
	},*/
});
/*nx = ( ( y1 - y2 ) * ( z1 - z3 ) ) - ( ( z1 - z2 ) * ( y1 - y3 ) );
ny = ( ( z1 - z2 ) * ( x1 - x3 ) ) - ( ( x1 - x2 ) * ( z1 - z3 ) );
nz = ( ( x1 - x2 ) * ( y1 - y3 ) ) - ( ( y1 - y2 ) * ( x1 - x3 ) );*/


