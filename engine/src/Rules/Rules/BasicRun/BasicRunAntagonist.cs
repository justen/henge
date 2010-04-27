
using System;
using System.Collections.Generic;
using Henge.Data.Entities;
using Henge.Engine.Ruleset;

namespace Henge.Engine.Ruleset.Core
{


	public class BasicRunAntagonist : BasicRun, IAntagonistRule
	{
		public BasicRunAntagonist()
		{
			
		}
		
		public  Interaction ConcludeInteraction(Actor protagonist, IList<HengeEntity> antagonists, Interaction interaction)
		{
			//structure of this rule is
			/*
			 *  IF (ConditionsMet(protagonist, antagonists, interaction))
			 *  THEN ApplyChanges (protagonist, antagonists, interaction)
			 */
			if (interaction.Concluded) return interaction;
			if (antagonists.Count == 1)
			{
				if ((antagonists[0] is Location) && (protagonist.Location.Map == ((Location)antagonists[0]).Map))
				{
					if ( this.CalculateDistance(protagonist.Location, (Location)antagonists[0]) < this.CheckSpeed(protagonist, interaction))
					{
						List<string> failures = this.TestInteraction(interaction, protagonist);
						if (failures.Count==0)
						{
							this.ApplyInteraction(interaction, protagonist, (Location)antagonists[0]);
							interaction.Succeeded("Moved");	
						}
						else interaction.Failed(failures);	
					}
					else interaction.Illegal("Out of range");
				}
				else interaction.Illegal("Invalid destination");
			}
			return null;	
		}
		
		private double CalculateDistance(Location source, Location destination)
		{
			int deltaX = source.X - destination.X;
			int deltaY = source.Y - destination.Y;
			int deltaZ = source.Z - destination.Z;
			return Math.Sqrt(deltaX * deltaX + deltaY * deltaY + deltaZ * deltaZ);
		}
		
		private double CheckSpeed(Actor actor, Interaction interaction)
		{
			//find whatever attributes in an actor which determine how fast it can move
			//apply and interaction data that could affect it
			//and then return the speed
			if (true)//actor.HasStat("speed"))
			{
				double boost = 0;
				double slow = 0;
				if (interaction.Transaction.ContainsKey("boostSpeed")) boost = (double)interaction.Transaction["boostSpeed"];
				if (interaction.Transaction.ContainsKey("reduceSpeed")) slow = (double)interaction.Transaction["reduceSpeed"];
				return -1;//actor.GetStat("speed") + boost - slow;
			}
			else return -1;
		}
		
		private List<string> TestInteraction (Interaction interaction, Actor actor)
		{
			//no charge for this interaction at present, so it's always going to be doable
			return new List<string>();	
		}
		
		private void ApplyInteraction (Interaction interaction, Actor actor, Location target)
		{
			//we would apply any charges built up in interaction here, but there are none at present so
			//it's not going to be used.
			if (actor is Avatar)
			{
				actor.Location.Inhabitants.Remove((Avatar)actor);
				actor.Location = target;
				actor.Location.Inhabitants.Add((Avatar)actor);
			}
			else if (actor is Npc)
			{
				actor.Location.Fauna.Remove((Npc)actor);
				actor.Location = target;
				actor.Location.Fauna.Add((Npc)actor);

			}
				
		}
	}
}
