/*
	Henge Interface
	Log System
*/

var giLog = new Class(
{
	initialize: function()
	{
		this.container	= $('log');
		this.log		= new HtmlTable($('log-table'));
		this.scroller	= new Fx.Scroll(this.container);
		this.height		= this.container.getSize().y;
		
		this.container.set('tween', { duration: 100 });
		this.container.addEvents({
			mouseover:	this.mouseOver.bind(this),
			mouseout:	this.mouseOut.bind(this)
		});
	},
	
	
	add: function(time, message)
	{
		this.log.push([time, message]);
		this.scroller.toBottom();
	},

	
	mouseOver: function(event)
	{
		this.container.tween('height', 172);
	},
	
	mouseOut: function(event)
	{
		this.container.tween('height', this.height);
	},
	
});