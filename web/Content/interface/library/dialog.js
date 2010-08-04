/*
	Henge Interface
	Dialog utility class
*/


var giDialog = new Class(
{
	initialize: function()
	{
		this.max	= 1;
		this.value	= 0;
		
		this.background = new Element('div', {
			styles: {
				'background-color': '#000',
				'z-index': 99,
				position: 'absolute',
				width: '100%',
				height: '100%',
				left: 0
			}
		});
		
		this.container = new Element('div', {
			styles: {
				'z-index': 99,
				position: 'absolute',
				width: '100%',
				height: '100%',
				left: 0
			}
		});
		
		this.dialog = new Element('div', {
			styles: {
				'background-color': '#ccc',
				'border': '2px solid #888',
				'z-index': 99,
				position: 'relative',
				left: '33%',
				top: '40%',
				width: '33%',
				height: '20%',
			},
			tween: { duration: 'short' }
		});
		
		this.message = new Element('div', {
			styles: {
				position: 'relative',
				'text-align': 'center',
				display: 'table',
				margin: '20px auto',
				width: '75%',
				'font-size': '1.4em'
			}
		});
				
		this.progress = new Element('div', {
			styles: {
				'background-color': '#fff',
				'border': '1px solid #000',
				position: 'absolute',
				width: '75%',
				height: '20px',
				bottom: '20px',
				left: '12.5%'
			}
		});
		
		this.bar = new Element('div', {
			styles: {
				'background-color': '#00c',
				position: 'absolute',
				top: 0,
				bottom: 0,
				left: 0
			}
		});
		
		this.message.inject(this.dialog);
		this.progress.inject(this.dialog);
		this.bar.inject(this.progress);
		this.dialog.inject(this.container);
		
		this.background.fade('hide');
		this.container.fade('hide');
		
		this.container.inject($('header').getParent(), 'top');
		this.background.inject($('header').getParent(), 'top');
	},
	
	
	show: function(message, max)
	{
		this.background.set('opacity', 0.5);
		this.message.set('text', message);
		
		if (max) 	
		{
			this.max	= max;
			this.value 	= 0;
			this.progress.show();
			this.bar.setStyle('width', '0%');
		}
		else this.progress.hide();
		
		this.container.fade('in');
	},
	
	
	hide: function()
	{
		this.background.fade('hide');
		this.container.fade('out');
	},
	
	
	update: function(progress)
	{
		this.bar.setStyle('width', Math.round(100 * progress / this.max) + '%');
	},
	
	
	increment: function()
	{
		if (this.value < this.max)
		{
			this.value++;
			this.update(this.value);
		}
	}
});


