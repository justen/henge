using System;
using System.Collections.Generic;


namespace Henge.Data.Entities
{
	public class Tick : ObjectEntity
	{
		public virtual string Name 			{ get; set; }
		public virtual DateTime Scheduled  	{ get; set; }
		public virtual int Period			{ get; set; }
	}
}