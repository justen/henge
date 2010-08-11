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
    	this.list = data;
    	
    	dialog.show('Loading images...', data.length);
    	
    	this.sources = new Asset.images(data, {
    		onProgress: function(counter) { dialog.update(counter); },
    		onComplete: this.handleAssets.bind(this)
    	});
    },
    
    
    handleAssets: function()
    {
    	this.images = {};
    	var re		= /[/]([^/]+)\.png$/;
    	
    	for (i=0; i<this.list.length; i++)
    	{
    		var name = re.exec(this.list[i]);
    		if (name) this.images[name[1]] = this.sources[i];
    	}
    	
    	for (key in this.types)
    	{
    		type 			= this.types[key];
    		type.Image		= this.images[type.Name];
    		type.Transition = this.images[type.Name + '-transition'];
    	}
    
    	boot();
    },
});

