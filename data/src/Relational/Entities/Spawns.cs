using System;
using System.Collections.Generic;

namespace Henge.Data.Entities
{
	public class Spawns : RelationalEntity
	{
		public virtual string LocationType 	{ get; set; }
		public virtual string Type 			{ get; set; }
		//one of "Item" or "NPC"
		public virtual string Class			{ get; set; }
		public virtual IList<string> Conditions	{ get; set; }
		public virtual int 	  MinZ			{ get; set; }
		public virtual int 	  MaxZ			{ get; set; }
		public virtual double SpawnRate		{ get; set; }
		public virtual string SpawnType		{ get; set; }
	}
}

