using System;
using System.Collections.Generic;

namespace Henge.Data.Entities
{


	public class Modifier : Ruleset
	{
		public virtual IList<Parameter> Expiration {get; set;}
	}
}
