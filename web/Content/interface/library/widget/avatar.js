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
		
		this.offset = TILE_SIZE / 2 - 16;
		
		image.clone().inject(this.icon);
		this.icon.inject(container);
		
		this.icon.set('tween', { duration: 150 });
		this.icon.addEvents({
			mouseover:	this.mouseOver.bind(this),
			mouseout:	this.mouseOut.bind(this)
		});
		
		this.setLocation(0, 0);
	},

	
	mouseOver: function(event)
	{
		this.icon.tween('opacity', 1.0);
	},
	
	mouseOut: function(event)
	{
		this.icon.tween('opacity', 0.5);
	},
	
	setLocation: function(x, y)
	{
		this.x = x;
		this.y = y;
		
		this.icon.setStyles({
			left: 	x * TILE_SIZE + this.offset,
			top:	y * TILE_SIZE + this.offset
		});
	}
});
