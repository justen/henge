/*
	Henge Interface
	Map System
*/

var giMap = new Class(
{
	initialize: function()
	{
		this.map		= $('map');
		this.document	= this.map.getDocument();
		this.canvas		= new giCanvas(this.map);

		this.mouse = {
			current: {}
		};

		this.bound = {
			mouseDown:	this.mouseDown.bind(this),
			mouseUp:	this.mouseUp.bind(this),
			mouseDrag:	this.mouseDrag.bind(this),
			mouseWheel:	this.mouseWheel.bind(this),
			mouseClick:	this.mouseClick.bind(this)
		};

		this.map.addEvents({
			//mousewheel:	this.bound.mouseWheel,
			mousedown:	this.bound.mouseDown,
			dblclick:	this.bound.mouseClick
		});
	},


	resize: function(height)
	{
		this.map.setStyle('height', height);
		this.canvas.resize();
	},


	mouseWheel: function(event)
	{
		//this.canvas.zoom(event.wheel);
	},


	mouseClick: function(event)
	{
		// Debug - trigger an update
		//this.canvas.update();
	},


	mouseDown: function(event)
	{
		if (!event.rightClick)
		{
			this.mouse.current = event.page;
	
			this.document.addEvents({
				mouseup:	this.bound.mouseUp,
				mousemove:	this.bound.mouseDrag
			});
			
			log.add('Mouse down');
		}

		return false;	// Stops the event and prevents FF3 from dragging images
	},


	mouseUp: function(event)
	{
		this.document.removeEvent('mouseup', this.bound.mouseUp);
		this.document.removeEvent('mousemove', this.bound.mouseDrag);
	},


	mouseDrag: function(event)
	{
		var position = event.page;

		this.canvas.move(position.x - this.mouse.current.x, position.y - this.mouse.current.y);
		this.mouse.current = position;
	},
});
