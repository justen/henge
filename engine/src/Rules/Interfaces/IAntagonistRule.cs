
using System;
using System.Collections.Generic;
using Henge.Data.Entities;

namespace Henge.Engine.Ruleset
{
	public interface IAntagonistRule : IRule
	{
		Interaction ConcludeInteraction(Actor protagonist, IList<HengeEntity> antagonists, Interaction interaction);
	}
}
