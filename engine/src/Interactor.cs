using System;
using System.Collections.Generic;

using Henge.Rules;
using Henge.Data;
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
		

		public IInteraction Interact(DataProvider db, Actor protagonist, Component antagonist, string interactionType, Dictionary<string, object> arguments)
		{
			IInteraction interaction = this.rulebook.CreateInteraction(protagonist, antagonist, arguments);
			
			if (interaction != null)
			{
				this.rulebook.Section(interactionType).ApplyRules(interaction);
				interaction.Conclude();
				if (interaction.Finished && !interaction.Illegal)
				{
					// while not failing to commit
					// {
						foreach (var delta in interaction.Deltas)
						{
							if (!delta(interaction.Succeeded)) break;
						}
						foreach (Entity entity in interaction.PendingDeletions)
						{
							db.Delete(entity);
						}
						//try
						//{
							db.Flush();	
						//}
						//catch // ----- Db4o commit exception?
						//{
							
						//}
					//}		
				}
				else
				{
					// Log something?
				}
			}

			return interaction;
		}
	}
}
