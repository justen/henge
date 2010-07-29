/*
	Henge Interface
	Avatar widget
*/

var giAvatar = new Class(
{
	initialize: function(container, image)
	{
		this.icon = new Element('div', {
			'class': 	'avatar',
			opacity:	0.5,
		});
		
		this.bound = {
			mouseOver:		this.mouseOver.bind(this),
			mouseOut:		this.mouseOut.bind(this),
			arrowClick:		this.arrowClick.bind(this),
			avatarClick:	this.avatarClick.bind(this)
		};
		
		var i			= 0;
		this.arrows 	= new Array();
		this.navigation = false;
		this.centre 	= TILE_SIZE / 2;
		
		for (y=-1; y<2; y++)
		{
			for(x=-1; x<2; x++)
			{
				if (x != 0 || y != 0)
				{
					hy = (y > 0) ? "s" : ((y < 0) ? "n" : "");
					hx = (x > 0) ? "e" : ((x < 0) ? "w" : "");
					var arrow = new Element('div', {
						id: 		'arrow-' + hy + hx,
						'class': 	'arrow',
						opacity: 	0,
					});
					
					arrow.x		= x;
					arrow.y		= y;
					arrow.ox 	= x * TILE_SIZE / 2 + (x - 1) * 12;
					arrow.oy 	= y * TILE_SIZE / 2 + (y - 1) * 12;		

					arrow.set('tween', { duration: 150 });
					arrow.set('morph', { duration: 150 });
					arrow.addEvents({
						mouseover:	this.bound.mouseOver,
						mouseout:	this.bound.mouseOut,
						mouseup:	this.bound.arrowClick
					});
					
					arrow.inject(container);
					
					this.arrows[i++] = arrow;
				}
			}
		}
		
		
		
		image.clone().inject(this.icon);
		this.icon.inject(container);
		
		this.icon.set('tween', { duration: 150 });
		this.icon.set('morph', { duration: 150 });
		this.icon.addEvents({
			mouseover:	function(event) { $(event.target).tween('opacity', 1.0) },
			mouseout:	function(event) { $(event.target).tween('opacity', 0.5) },
			mouseup:	this.bound.avatarClick
		});
		
		this.setLocation(0, 0);
	},

	
	mouseOver: function(event)
	{
		if (this.navigation) $(event.target).tween('opacity', 1.0);
	},
	
	mouseOut: function(event)
	{
		if (this.navigation) $(event.target).tween('opacity', 0.5);
	},
	
	avatarClick: function(event)
	{
		if (!event.rightClick && !map.dragged)
		{
			var cx	= this.x * TILE_SIZE + this.centre;
			var cy	= this.y * TILE_SIZE + this.centre;
			
			if (this.navigation)
			{
				this.arrows.each(function(arrow) { arrow.morph({ opacity: 0, left: cx - 12, top: cy - 12 }); });
				this.navigation = false;
			}
			else
			{
				this.arrows.each(function(arrow) { arrow.morph({ opacity: 0.5, left: cx + arrow.ox, top: cy + arrow.oy }); });
				this.navigation = true;
			}
		}
	},
	
	
	arrowClick: function(event)
	{
		if (!event.rightClick && !map.dragged)
		{
			var target = $(event.target);
			interface.move(target.x, target.y);
		}
	},
	
	setLocation: function(x, y)
	{
		this.x	= x;
		this.y	= y;
		var cx	= x * TILE_SIZE + this.centre;
		var cy	= y * TILE_SIZE + this.centre;
		
		this.icon.morph({ left: cx - 24, top: cy - 24 });
		if (this.navigation) 	this.arrows.each(function(arrow) { arrow.morph({ left: cx + arrow.ox, top: cy + arrow.oy}); });
		else					this.arrows.each(function(arrow) { arrow.set({ left: cx - 12, top: cy - 12 }); }); 
	}
});
