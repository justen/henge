/*
	Henge Interface
	Library of styles and images
*/

var giLibrary = new Class(
{
	initialize: function()
	{
		this.images = null;
		dialog.show('Loading images...', 1);
		
		
		new Request.JSON({
			url:		root + 'Map/AssetList',
			onSuccess:	this.load.bind(this)
		}).send();
    },
    
    
    load: function(data)
    {
    	this.images = data;
    	dialog.show('Loading images...', this.images.length);
    	
    	new Asset.images(this.images, {
    		onProgress: function(counter) { dialog.update(counter); },
    		onComplete: boot
    	});
    }
});

