/*
	Henge Interface
	Interface
*/


var giInterface = new Class(
{
	initialize: function()
	{
		this.status 		= $('status');
		this.health			= new giBar(this.status, 'Health');
		this.reserve		= new giBar(this.status, 'Energy');
		this.constitution	= new giBar(this.status, 'Constitution');
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
		
		if (data.Valid) 
		{
			map.canvas.setLocation(data.X, data.Y);
			map.canvas.avatar.setEnergy(data.Energy);
		}
	},
	
	handleStatus: function(data)
	{
		if (data.Health >= 0)
		{
			this.health.set(data.Health);
			this.reserve.set(data.Reserve);
			this.constitution.set(data.Constitution);
			
			map.canvas.avatar.setEnergy(data.Energy);
		}
		//else show respawn button??
	}
});