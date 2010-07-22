using System;
using System.Xml;
using System.Collections.Generic;

namespace Avebury
{
	public class Mapper
	{
		Dictionary<string, string> idToColour;
		Dictionary<string, string> colourToId;
		
		public Mapper ()
		{
			this.colourToId = new Dictionary<string, string>();
			this.idToColour = new Dictionary<string, string>();
		}
		
		public bool AddMapping (string id, string colour)
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
		
		public string Id(string colour)
		{
			if (this.colourToId.ContainsKey(colour))
			{
				return this.colourToId[colour];	
			}
			else
			{
				int i = 0; 
				string id = string.Format("terrain.{0}", i);
				while(idToColour.ContainsKey(id))
				{
					i++;
					id = string.Format("terrain.{0}", i);
				}
				this.idToColour.Add(id, colour);
				this.colourToId.Add(colour, id);
				return (id);
			}
		}

		public string Colour (string id)
		{
			string result = "#000000";
			if (this.idToColour.ContainsKey(id))
			{
				result =  this.idToColour[id];	
			}
			else throw new Exception("Avebury: Invalid terrain ID");
			return result;
		}
		
	}
}

