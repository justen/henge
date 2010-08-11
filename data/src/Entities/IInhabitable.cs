using System;
using System.Collections.Generic;

namespace Henge.Data.Entities
{
	public interface IInhabitable
	{
		IList<Avatar> Inhabitants	{ get; set; }
		IList<Npc> Fauna			{ get; set; }
	}
}

