/*
	Henge Interface
	AJAX Request Class
*/

var giRequest = new Class(
{
	initialize: function()
	{
		this.queue = new Request.Queue({ concurrent: 4 });
	},


	move: function(dx, dy)
	{
		new Request.JSON({
			url:		root + 'Interface/Move',
			data:		'dx=' + dx + '&dy=' + dy,
			onSuccess:	function(data) { interface.handleMove(data) }
		}).send();
	},
	
	getTileData: function(queue)
	{
		new Request.JSON({
			url:		root + 'Map/Tile',
			data:		queue.map(function(item) { return 'x=' + item.x + '&y=' + item.y }).join('&'),
			onSuccess:	function(data)
			{
				if (data.length == queue.length) data.each(function(item, index) { queue[index].handleData(item) });
			}
		}).send();
	}
});