
using System;
using System.Collections.Generic;
using Henge.Data.Entities;

namespace Henge.Engine.Ruleset
{
	public interface IProtagonistRule : IRule
	{
		Interaction BeginInteraction(Actor protagonist, IList<HengeEntity> interferers, IList<HengeEntity> antagonists);
		Interaction BeginInteraction(Actor protagonist, IList<HengeEntity> interferers, HengeEntity antagonists);
		Interaction BeginInteraction(Actor protagonist, IList<HengeEntity> antagonists);
		Interaction BeginInteraction(Actor protagonist, HengeEntity antagonists);
	}
}
