using System;
using System.Collections.Generic;
using Henge.Data.Entities;


namespace Henge.Rules.Protaganist.Move.Run
{
	public class BasicRun : Rule, IProtagonistRule
	{
		public BasicRun()
		{
		}
		
		
		public Interaction BeginInteraction(Actor protagonist, IList<HengeEntity> interferers, IList<HengeEntity> antagonists)
		{
			return new Interaction(){Protagonist = protagonist, Interferers = interferers, Antagonists = antagonists};
		}
		
		
		public Interaction BeginInteraction(Actor protagonist, IList<HengeEntity> interferers, HengeEntity antagonist)
		{
			return new Interaction() { Protagonist = protagonist, Interferers = interferers, Antagonists = new List<HengeEntity> { antagonist } };
		}
		
		
		public Interaction BeginInteraction(Actor protagonist, IList<HengeEntity> antagonists)
		{
			return new Interaction(){Protagonist = protagonist, Antagonists = antagonists};
		}
		
		
		public Interaction BeginInteraction(Actor protagonist, HengeEntity antagonist)
		{
			return new Interaction(){Protagonist = protagonist, Antagonists = new List<HengeEntity> { antagonist } };
		}
	}
}