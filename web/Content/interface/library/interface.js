/*
	Henge Interface
	Interface
*/


var giInterface = new Class(
{
	initialize: function()
	{
		this.status 		= $('status');
		this.ping			= new Element('div', { id: 'ping', tween: { duration: 150 }}).fade('hide').inject(this.status, 'top');
		this.health			= new giBar(this.status, 'Health');
		this.reserve		= new giBar(this.status, 'Energy');
		this.constitution	= new giBar(this.status, 'Constitution');
		
		this.statusPoll		= new Request.JSON({
			url:		root + 'Interface/GetStatus',
			initialDelay: 1000,
			delay: 5000,
			limit: 10000,
			onSuccess: 	this.onStatus.bind(this),
			onRequest: 	this.onPing.bind(this, true),
			onComplete: this.onPing.bind(this, false)
		}).startTimer();
	},
	
	
	onPing: function(on)
	{
		if (on) this.ping.fade('in');
		else	this.ping.fade('out');
	},
	
	
	move: function(dx, dy)
	{
		request.send('Interface/Move', 'dx=' + dx + '&dy=' + dy, this.onMove.bind(this)); 
	},
	
	defend: function(dx, dy, duration)
	{
		request.send('Interface/DefendLocation', 'dx=' + dx + '&dy=' + dy + '&duration=' + duration, function(data) { log.add(data) });
	},
	
	
	onMove: function(data)
	{
		log.add(data.Message);
		
		if (data.Valid) 
		{
			map.canvas.setLocation(data.X, data.Y);
			map.canvas.avatar.setEnergy(data.Energy);
		}
	},
	
	
	onStatus: function(data)
	{
		if (data.Health >= 0)
		{
			this.health.set(data.Health);
			this.reserve.set(data.Reserve);
			this.constitution.set(data.Constitution);
			if (data.Messages.length)
			{
				data.Messages.each(function(item) { log.add(item) });
			}
			map.canvas.avatar.setEnergy(data.Energy);
		}
		//else show respawn button??
	}
});