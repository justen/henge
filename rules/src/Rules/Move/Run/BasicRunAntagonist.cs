
using System;
using System.Collections.Generic;
using Henge.Data.Entities;
using Henge.Rules;

namespace Henge.Rules.Antagonist.Move.Run
{
	public class BasicRun : Rule, IAntagonistRule
	{
		public BasicRun()
		{
		}
		
		
		public  Interaction ConcludeInteraction(Interaction interaction)
		{
			//structure of this rule is
			/*
			 *  IF (ConditionsMet(protagonist, antagonists, interaction))
			 *  THEN ApplyChanges (protagonist, antagonists, interaction)
			 */
			if (interaction.Antagonists.Count == 1)
			{
				if (!interaction.Finished)
				{
					if ((interaction.Antagonist is Location) && (interaction.Protagonist.Location.Map == ((Location)interaction.Antagonist).Map))
					{
						if ( this.CalculateDistance(interaction.Protagonist.Location, (Location)interaction.Antagonist) < this.CheckSpeed(interaction.Protagonist, interaction))
						{
							string failures = this.TestInteraction(interaction, interaction.Protagonist);
							
							if (failures == null)
							{
								this.ApplyInteraction(interaction, interaction.Protagonist, (Location)interaction.Antagonist);
								
								interaction.Success("Moved");	
							}
							else interaction.Failure(failures, false);	
						}
						else interaction.Failure("Out of range", true);
					}
					else interaction.Failure("Invalid destination", true);
				}
			}
			else interaction.Failure("Multiple targets", true);
			
			return interaction;	
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
		
		
		private string TestInteraction(Interaction interaction, Actor actor)
		{
			//no charge for this interaction at present, so it's always going to be doable
			return null;
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
