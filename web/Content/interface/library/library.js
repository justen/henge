/*
	Henge Interface
	Library of styles and images
*/

var giLibrary = new Class(
{
	initialize: function()
	{
		/*var myImages = new Asset.images(['/images/myImage.png', '/images/myImage2.gif'], {
    	onComplete: function(){
        alert('All images loaded!');*/
        
        this.icons = new Asset.images([
        	root + 'Content/interface/images/icons/location.png',
			root + 'Content/interface/images/icons/people.png',
			root + 'Content/interface/images/icons/animals.png',
			root + 'Content/interface/images/icons/structures.png',
			root + 'Content/interface/images/icons/avatar.png',
		]);
    }
});

