/*
	Henge Interface
	Bar widget
*/

var giBar = new Class(
{
	initialize: function(container, label)
	{
		this.outer	= new Element('div', { class: 'bar' });
		this.label	= new Element('label', { text: label });
		this.bar	= new Element('div');
		
		this.label.inject(this.outer);
		this.bar.inject(this.outer);
		this.outer.inject(container);
	},
	
	
	set: function(value)
	{
		this.bar.setStyles({
			width:				value + '%',
			'background-color': (value < 50) ? '#c00' : '#0c0',
		});
	},
});