using System;
using System.Collections.Generic;

using Henge.Data.Entities;


namespace Henge.Rules
{
	public interface IAntagonistRule : IRule
	{
		Interaction ConcludeInteraction(Interaction interaction);
	}
}
