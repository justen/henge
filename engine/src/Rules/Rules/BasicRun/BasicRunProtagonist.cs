
using System;
using System.Collections.Generic;
using Henge.Data.Entities;

namespace Henge.Engine.Ruleset.Core
{


	public class BasicRunProtagonist : BasicRun, IProtagonistRule
	{

		public BasicRunProtagonist ()
		{
		}
		
		public Interaction BeginInteraction(Actor protagonist, IList<HengeEntity> interferers, IList<HengeEntity> antagonists)
		{
			return new Interaction(){Protagonist = protagonist, Interferers = interferers, Antagonists = antagonists};
		}
		
		public Interaction BeginInteraction(Actor protagonist, IList<HengeEntity> interferers, HengeEntity antagonist)
		{
			return new Interaction(){Protagonist = protagonist, Interferers = interferers, Antagonist = antagonist};;
		}
		
		public Interaction BeginInteraction(Actor protagonist, IList<HengeEntity> antagonists)
		{
			return new Interaction(){Protagonist = protagonist, Antagonists = antagonists};
		}
		
		public Interaction BeginInteraction(Actor protagonist, HengeEntity antagonist)
		{
			return new Interaction(){Protagonist = protagonist, Antagonist = antagonist};;
		}
	}
}
