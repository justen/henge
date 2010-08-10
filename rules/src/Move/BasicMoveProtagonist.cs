using System;
using System.Collections.Generic;

using Henge.Data.Entities;


namespace Henge.Rules.Protagonist.Move
{
	public class MoveRule : HengeRule, IProtagonist
	{
		public override bool Valid (Component subject)
		{
			return (subject is Actor);// && !((Actor)subject).Location.Traits.ContainsKey("Movement");
		}
		
		protected override double Visibility (HengeInteraction interaction, out Component subject)
		{
			//Moving; become more obvious:
			subject = interaction.Protagonist;
			return (Constants.StandardVisibility * interaction.ProtagonistCache.Conspicuousness);
		}
		
		protected override IInteraction Apply(HengeInteraction interaction)
		{
			return this.Move(interaction);
		}	
		
		protected new bool Validate(HengeInteraction interaction)
		{
			bool result = false;
			if (!interaction.Finished)
			{
				Location antagonist	= interaction.Antagonist as Location;
				Location source		= interaction.Protagonist.Location;
				if (base.Validate(interaction))
				{
					if (this.CalculateDistance(source, antagonist) <= 2)
					{
						result = true;	
					}
					else interaction.Failure("You cannot move that far", true);
				}
				
			}
			return result;
		}
		
		protected IInteraction Move(HengeInteraction interaction)
		{
			// structure of this rule is
			//
			//  IF (ConditionsMet(protagonist, antagonists, interaction))
			//  THEN ApplyChanges (protagonist, antagonists, interaction)
			//
			if (this.Validate(interaction))
			{
				Location antagonist	= interaction.Antagonist as Location;
				Location source		= interaction.Protagonist.Location;
				double impedance = 0.25 * (source.Traits.ContainsKey("Impede") ? source.Traits["Impede"].Value : Constants.Impedance);
				if (!interaction.ProtagonistCache.UseEnergy(impedance, EnergyType.Fitness))
				{
					interaction.Log = string.Empty;
					if (impedance > interaction.ProtagonistCache.Strength * interaction.Protagonist.Traits["Energy"].Maximum)
					{
						
						interaction.Failure("You seem to be trapped.", false);
					}
					else interaction.Failure("You are unable to summon sufficient energy to set out", false);
					
				}
				else
				{
					interaction.Log = string.Format("You set off through the {0}. {1}", source.Inspect(interaction.Protagonist).ShortDescription, interaction.Log);
					foreach (Action action in interaction.Actions)
					{
						if (interaction.Finished) break;
						else action.Invoke();
					}
				}
				if (!interaction.Finished)
				{
					this.ApplyInteraction(interaction, interaction.Protagonist, antagonist);	
				}

			}
	
			return interaction;
		}
		
		
		protected int CalculateDistance(Location source, Location destination)
		{
			if (source.Map == destination.Map)
			{
				int deltaX = source.X - destination.X;
				int deltaY = source.Y - destination.Y;
				// currently can't run in z, so don't bother calculating it.
				// int deltaZ = source.Coordinates.Z - destination.Coordinates.Z;
				return deltaX * deltaX + deltaY * deltaY;
			}
			else return int.MaxValue;
		}
		
		
		protected void ApplyInteraction (HengeInteraction interaction, Actor actor, Location target)
		{

			if (actor is Avatar)
			{
				Avatar avatar = actor as Avatar;
				
				using (interaction.Lock(avatar, avatar.Location.Inhabitants, target.Inhabitants))
				{
					avatar.Location.Inhabitants.Remove(avatar);
					target.Inhabitants.Add(avatar);
					avatar.Location = target;
				}
					
				interaction.Success(string.Format("You reach your destination, a {0}", target.Inspect(avatar).ShortDescription));	
			}
			else
			{
				Npc npc = actor as Npc;
				
				using (interaction.Lock(npc, npc.Location.Fauna, target.Fauna))
				{
					npc.Location.Fauna.Remove(npc);
					target.Fauna.Add(npc);
					npc.Location = target;
				}
					
				interaction.Success("Moved");	
			}
		}	
	}
}
