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
		
		public void Tick()
		{
			/* This is the way we should do it once Coincidental is fixed...
			IList<Component> staleComponents = this.db.Query<Component>().Where(c=>c.NextTick!=null && c.NextTick.Scheduled < DateTime.Now).ToList();
			foreach (Component component in staleComponents) this.Tick(component);
			*/
			//But for now...
			foreach (Avatar avatar in this.db.Query<Avatar>().Where(c=>c.NextTick!=null && c.NextTick.Scheduled < DateTime.Now).ToList())
			{
				this.Tick(avatar as Component);
			}
			foreach (Npc npc in this.db.Query<Npc>().Where(c=>c.NextTick!=null && c.NextTick.Scheduled < DateTime.Now).ToList())
			{
				this.Tick(npc as Component);	
			}
			foreach (Edifice edifice in this.db.Query<Edifice>().Where(c=>c.NextTick!=null && c.NextTick.Scheduled < DateTime.Now).ToList())
			{
				this.Tick(edifice as Component);	
			}
			foreach (Item item in this.db.Query<Item>().Where(c=>c.NextTick!=null && c.NextTick.Scheduled < DateTime.Now).ToList())
			{
				this.Tick(item as Component);
			}
			foreach (Location location in this.db.Query<Location>().Where(c=>c.NextTick!=null && c.NextTick.Scheduled < DateTime.Now).ToList())
			{
				this.Tick(location as Component);	
			}
		
		}
		
		private void Tick(Component component)
		{
			IInteraction interaction = Interactor.Instance.Interact(null, component, component.NextTick.Name, null);
			if (interaction.Succeeded)
			{
				if (interaction.Chain!=string.Empty)
				{
					if (!Interactor.Instance.Interact(interaction.Protagonist, interaction.Antagonist, interaction.Chain, interaction.Arguments).Succeeded)
					{
						//TODO: log an error	
					}
				}
			}
			db.Delete(component.NextTick);
			using (db.Lock(component, component.NextTick, component.Ticks))
			{
				component.UpdateNextTick();
			}	
		}
		
		public IModifier Modifier(string target)
		{
			return this.rulebook.Modifiers.ContainsKey(target) ? this.rulebook.Modifiers[target] : null;
		}
	}
}
