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
		
		var i		= 0;
		this.arrows = new Array();
		this.centre = TILE_SIZE / 2;
		
		for (y=-1; y<2; y++)
		{
			for(x=-1; x<2; x++)
			{
				if (x != 0 || y != 0)
				{
					hy = (y > 0) ? "S" : ((y < 0) ? "N" : "");
					hx = (x > 0) ? "E" : ((x < 0) ? "W" : "");
					var arrow = new Element('div', {
						'class':	'arrow', //, heading' + hy + hx,
						opacity:	0.5,
					});
					
					arrow.x		= x;
					arrow.y		= y;
					arrow.ox 	= x * TILE_SIZE / 2 + (x - 1) * 12;
					arrow.oy 	= y * TILE_SIZE / 2 + (y - 1) * 12;		
					arrow.inject(container);
					
					arrow.set('tween', { duration: 150 });
					arrow.addEvents({
						mouseover:	this.mouseOver.bind(this),
						mouseout:	this.mouseOut.bind(this),
						mouseup:	this.arrowClick.bind(this)
					});
					
					this.arrows[i++] = arrow;
				}
			}
		}
		
		
		
		image.clone().inject(this.icon);
		this.icon.inject(container);
		
		this.icon.set('tween', { duration: 150 });
		this.icon.addEvents({
			mouseover:	this.mouseOver.bind(this),
			mouseout:	this.mouseOut.bind(this),
			mouseup:	this.avatarClick.bind(this)
		});
		
		this.setLocation(0, 0);
	},

	
	mouseOver: function(event)
	{
		$(event.target).tween('opacity', 1.0);
	},
	
	mouseOut: function(event)
	{
		$(event.target).tween('opacity', 0.5);
	},
	
	avatarClick: function(event)
	{
		if (!event.rightClick && !map.dragged)
		{
			log.add("Avatar click");
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
		
		this.icon.setStyles({
			left: 	cx - 24,
			top:	cy - 24,
		});
		
		this.arrows.each(function(arrow) {
			arrow.setStyles({
				left:	cx + arrow.ox,
				top:	cy + arrow.oy
			});
		});
	}
});
