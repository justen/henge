/*
	Henge Interface
	Interface
*/


var giInterface = new Class(
{
	initialize: function()
	{
	},
	
	
	move: function(dx, dy)
	{
		request.move(dx, dy);
	},
	
	
	handleMove: function(data)
	{
		if (data.Valid)
		{
			map.canvas.setLocation(data.X, data.Y);
			$('description').set('text', 'You are standing at ' + data.X + ', ' + data.Y);
		}
		else alert(data.Message);
	},


});