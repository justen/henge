using System;
using System.Collections.Generic;

namespace Henge.Data.Entities
{
	public class Modifier : Entity
	{
		public virtual string Name {get; set;}
		public virtual IList<Parameter> Expiration {get; set;}
	}
}
