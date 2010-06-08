/*
	Henge Interface
	Bar widget
*/

var giIcon = new Class(
{
	initialize: function(container, image)
	{
		this.icon = new Element('div', {
			class: 		'icon',
			opacity:	0.5,
		});
		
		image.clone().inject(this.icon);
		this.icon.inject(container);
		
		this.icon.set('tween', { duration: 150 });
		this.icon.addEvents({
			mouseover:	this.mouseOver.bind(this),
			mouseout:	this.mouseOut.bind(this)
		});
	},

	
	mouseOver: function(event)
	{
		this.icon.tween('opacity', 1.0);
	},
	
	mouseOut: function(event)
	{
		this.icon.tween('opacity', 0.5);
	},
});
