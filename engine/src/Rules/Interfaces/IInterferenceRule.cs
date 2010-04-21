
using System;
using System.Collections.Generic;
using Henge.Data.Entities;

namespace Henge.Engine.Ruleset
{
	public interface IInterferenceRule : IRule
	{
		Interaction ContinueInteraction(Actor protagonist, IList<HengeEntity> antagonists, Interaction interaction);
	}
}
