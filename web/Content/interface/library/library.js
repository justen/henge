/*
	Henge Interface
	Library of styles and images
*/

var giLibrary = new Class(
{
	initialize: function()
	{
		this.images = null;
		this.types	= null;
		
		dialog.show('Loading images...', 1);

		new Request.JSON({
			url:	root + 'Map/TypeList',
			onSuccess:	this.loadTypes.bind(this)
		}).send();
		
		
    },
    
    
    loadTypes: function(data)
    {
    	this.types = data;
    	
    	new Request.JSON({
			url:		root + 'Map/AssetList',
			onSuccess:	this.loadAssets.bind(this)
		}).send();
    },
    
    
    loadAssets: function(data)
    {
    	this.images = data;
    	dialog.show('Loading images...', this.images.length);
    	
    	new Asset.images(this.images, {
    		onProgress: function(counter) { dialog.update(counter); },
    		onComplete: boot
    	});
    }
});

