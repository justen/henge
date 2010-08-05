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
		this.statusPoll		= new Request.JSON({
			url:		root + 'Interface/GetStatus',
			initialDelay: 1000,
			delay: 5000,
			limit: 10000,
			onSuccess: this.handleStatus.bind(this)
		}).startTimer();
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
		log.add(data.Message);
		
		if (data.Valid) map.canvas.setLocation(data.X, data.Y);
	},
	
	handleStatus: function(data)
	{
		this.health.set(data.Health);
		this.energy.set(data.Energy);
		this.constitution.set(data.Constitution);
	}
});