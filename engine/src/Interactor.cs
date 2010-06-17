using System;
using System.Collections.Generic;

using Henge.Rules;
using Henge.Data.Entities;


namespace Henge.Engine
{
	public sealed class Interactor
	{
		private Rulebook rulebook;
		private static readonly Interactor instance = new Interactor();

		public static Interactor Instance { get { return instance; }}
		
		
		static Interactor () {}
		private Interactor() {}
		
		
		public void Initialise(string applicationPath)
		{
			this.rulebook = Loader.LoadRules(applicationPath);
		}
		

		public Interaction Interact(Actor protagonist, Component antagonist, string interactionType)
		{
			Interaction interaction = this.rulebook.Section(interactionType).ApplyRules(new Interaction(protagonist, antagonist));
			
			//try
			//{
			
			if (interaction.Antagonist.ApplyDeltas(interaction.Succeeded))
			{
				bool result = true;
				foreach (Delta interferer in interaction.Interferers)
				{
					result = interferer.ApplyDeltas(interaction.Succeeded);
					if (!result) break;
				}
				
				if (result) interaction.Protagonist.ApplyDeltas(interaction.Succeeded);
			}
				
			//}
			//catch // ----- Db4o commit exception?
			//{
				
			//}
			
			//return this.rulebook.Section(interaction).ApplyRules(new Interaction { Antagonist = antagonist, Protagonist = protagonist });
			return interaction;
		}
	}
}
