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
		this.queue = new Request.Queue({ concurrent: 4 });
	},


	/*getTileData: function(x, y, handler)
	{
		var request = new Request.JSON({
			url:	'/Map/Tile',
			onSuccess:	handler
		});
		
		//this.queue.addRequest('tile', request);
		
		request.send("x=" + x + "&y=" + y);
	}*/
	
	getTileData: function(queue)
	{
		new Request.JSON({
			url:		'/Map/Tile',
			data:		queue.map(function(item) { return 'x=' + item.x + '&y=' + item.y }).join('&'),
			onSuccess:	function(data)
			{
				if (data.length == queue.length) data.each(function(item, index) { queue[index].handleData(item) });
			}
		}).send();
	}
});