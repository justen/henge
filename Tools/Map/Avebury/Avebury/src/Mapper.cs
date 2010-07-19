using System;
using System.Xml;
using System.Collections.Generic;
using System.Drawing;
namespace Avebury
{
	public class Mapper
	{
		Dictionary<string, Color> idToColour;
		Dictionary<Color, string> colourToId;
		
		public Mapper ()
		{
			this.colourToId = new Dictionary<Color, string>();
			this.idToColour = new Dictionary<string, Color>();
		}
		
		public bool AddMapping (string id, Color colour)
		{
			bool result = false;
			if (!this.colourToId.ContainsKey(colour) && !this.idToColour.ContainsKey(id))
			{
				this.colourToId.Add(colour, id);
				this.idToColour.Add(id, colour);
				result = true;
			}
			return result;
		}
		
		public string Id(Color colour)
		{
			if (this.colourToId.ContainsKey(colour))
			{
				return this.colourToId[colour];	
			}
			else
			{
				int i = 0; 
				while(idToColour.ContainsKey(i.ToString()))
				{
					i++;
				}
				this.idToColour.Add(i.ToString(), colour);
				this.colourToId.Add(colour, i.ToString());
				return i.ToString();
			}
		}

		public Color Colour (string id)
		{
			Color result = Color.Black;
			if (this.idToColour.ContainsKey(id))
			{
				result =  this.idToColour[id];	
			}
			else throw new Exception("Avebury: Invalid terrain ID");
			return result;
		}
		
	}
}

