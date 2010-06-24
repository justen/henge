/*
	Henge Interface
	Interface
*/


var giInterface = new Class(
{
	initialize: function()
	{
		var status = $('status');
		
		this.health			= new giBar(status, 'Health');
		this.energy			= new giBar(status, 'Energy');
		this.constitution	= new giBar(status, 'Constitution');
		
		// Debug
		this.health.set(50);
		this.energy.set(100);
		this.constitution.set(25);
	},
	
	
	move: function(dx, dy)
	{
		request.move(dx, dy);
	},
	
	defend: function(dx, dy, duration)
	{
		request.defend(dx, dy, duration);
	},
	
	handleMove: function(data)
	{
		if (data.Valid)
		{
			map.canvas.setLocation(data.X, data.Y);
			//$('description').set('text', 'You are standing at ' + data.X + ', ' + data.Y);
		}
		else alert(data.Message);
	},
	
	showMessage: function(data)
	{
		alert(data.Message);
	}


});