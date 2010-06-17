using System;
using System.Collections.Generic;
using Henge.Data.Entities;


namespace Henge.Rules.Protagonist.Move.Run
{
	public class BasicRun : ProtagonistRule
	{
		public BasicRun()
		{
		}
		
		
		public override Interaction Apply(Interaction interaction)
		{
			// structure of this rule is
			//
			//  IF (ConditionsMet(protagonist, antagonists, interaction))
			//  THEN ApplyChanges (protagonist, antagonists, interaction)
			//
			if (!interaction.Finished)
			{
				Actor protagonist = interaction.Protagonist as Actor;
				Location antagonist	= interaction.Antagonist as Location;
				
				if (protagonist != null && antagonist != null)
				{

					if (this.CalculateDistance(protagonist.Location, antagonist) < 2)
					{
						if (Common.UseEnergy(protagonist, (double)interaction.Transaction["impedance"]))
						{
							this.ApplyInteraction(interaction, protagonist, antagonist);
							//now everything that was trying to impede progress is going to have to take damage I suppose...?
						}
					}
					else interaction.Failure("Out of range", true);
				}
			}
	
			return interaction;
		}
		
		
		public override double Priority (Component subject)
		{
			return (subject is Actor) ? 1 : -1;
		}
		
		
		private double CalculateDistance(Location source, Location destination)
		{
			double deltaX = source.Coordinates.X - destination.Coordinates.X;
			double deltaY = source.Coordinates.Y - destination.Coordinates.Y;
			// currently can't run in z, so don't bother calculating it.
			// int deltaZ = source.Coordinates.Z - destination.Coordinates.Z;
			
			return deltaX * deltaX + deltaY * deltaY;
		}
		
		
		private void ApplyInteraction (Interaction interaction, Actor actor, Location target)
		{
			// We would apply any charges built up in interaction here, but there are none at present so
			// it's not going to be used.
			if (actor is Avatar)
			{
				Avatar avatar = actor as Avatar;
				
				interaction.Deltas.Add((success) =>
				{
					//if (success)
					//{
						avatar.Location.Inhabitants.Remove(avatar);
						avatar.Location = target;
						avatar.Location.Inhabitants.Add(avatar);
					//}
					return true;
				});
					
				interaction.Success("Moved");	
			}
			else if (actor is Npc)
			{
				Npc npc = actor as Npc;
				
				interaction.Deltas.Add((success) =>
				{
					//if (success)
					//{
						npc.Location.Fauna.Remove(npc);
						npc.Location = target;
						npc.Location.Fauna.Add(npc);
					//}
					return true;
				});
					
				interaction.Success("Moved");	
			}
			else interaction.Failure("Antagonist cannot move", true);
		}
		
	}
}
