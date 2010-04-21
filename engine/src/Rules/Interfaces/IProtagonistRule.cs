
using System;
using System.Collections.Generic;
using Henge.Data.Entities;

namespace Henge.Engine.Ruleset
{
	public interface IProtagonistRule : IRule
	{
		Interaction BeginInteraction(Actor Protagonist, IList<HengeEntity> Antagonists);
	}
}
