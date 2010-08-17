/*
	Henge Interface
	AJAX Request Class
*/

var giRequest = new Class(
{
	initialize: function()
	{
		//this.queue = new Request.Queue({ concurrent: 4 });
	},
	
	
	send: function(url, data, success)
	{
		new Request.JSON({
			url:		root + url,
			data:		data,
			onSuccess:	success
		}).send();
	},

});