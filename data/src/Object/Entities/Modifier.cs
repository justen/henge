using System;
using System.Collections.Generic;


namespace Henge.Data.Entities
{
	public class Modifier : ObjectEntity
	{
		//textual value of the modifier (if required)
		public string Text {get; set;}
		//numeric value of the modifier (if required)
		public long Value {get; set;}
		//Expiration conditions - <Thing To Check, Condition We're Checking For>
		public Dictionary<string, string> Expiration {get; set;}
		
		public Modifier()
		{
			this.Expiration = new Dictionary<string, string>();
		}
	}
}
