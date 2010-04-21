/*
	Prototype Game Interface
	Author: Dan Parnham
	Date: 16/03/2008

	AJAX Request Class
*/

var giRequest = new Class(
{
	initialize: function()
	{
		this.queue 		= new Request.Queue({ concurrent: 4 });

		/*this.request = new Request({
			method: 'get',
			url:	'/ajax',
			onSuccess: this.handleData.bind(this),
			onFailure: this.handleFail.bind(this)
		});*/
	},


	getTileData: function(x, y, handler)
	{
		var request = new Request.JSON({
			url:	'/Map/Tile',
			onSuccess:	handler
		});
		
		//this.queue.addRequest('tile', request);
		
		request.send("x=" + x + "&y=" + y);
	}

	/*pushQueue: function(item)
	{
		this.queue.push(item);
		this.shiftQueue.delay(1, this);
	},


	shiftQueue: function()
	{
		if (!this.current && this.queue.length)
		{
			this.current = this.queue.shift();
			this.request.send(this.current.options);
		}
	},


	handleData: function(response)
	{
		this.current.onResponse(response);
		this.current = null;
		this.shiftQueue.delay(1, this);
	},


	handleFail: function()
	{
		// TODO
		alert('failed');

	},*/
});