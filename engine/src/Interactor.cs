using System;
using System.Linq;
using System.Collections.Generic;

using Henge.Rules;
using Henge.Data;
using Henge.Data.Entities;


namespace Henge.Engine
{
	public sealed class Interactor
	{
		private Rulebook rulebook;
		private DataProvider db;
		private static readonly Interactor instance = new Interactor();

		public static Interactor Instance { get { return instance; }}
		
		
		static Interactor () {}
		private Interactor() {}
		
		
		public void Initialise(string applicationPath, DataProvider db)
		{
			this.rulebook 	= Loader.LoadRules(db, applicationPath);
			this.db			= db;
		}
		

		public IInteraction Interact(Actor protagonist, Component antagonist, string interactionType, Dictionary<string, object> arguments)
		{
			IInteraction interaction = this.rulebook.CreateInteraction(this.db, protagonist, antagonist, arguments);
			
			if (interaction != null)
			{
				this.rulebook.Section(interactionType).ApplyRules(interaction);
				
				interaction.Conclude();
				
				if (interaction.Finished && !interaction.Illegal)
				{
					// Hurray?
				}
				else
				{
					// Log something?
				}
			}

			return interaction;
		}
		
		
		public IModifier Modifier(string target)
		{
			return this.rulebook.Modifiers.ContainsKey(target) ? this.rulebook.Modifiers[target] : null;
		}
	}
}
