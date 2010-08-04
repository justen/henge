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
		
		protected IInteraction Move(HengeInteraction interaction)
		{
			// structure of this rule is
			//
			//  IF (ConditionsMet(protagonist, antagonists, interaction))
			//  THEN ApplyChanges (protagonist, antagonists, interaction)
			//
			if (!interaction.Finished)
			{
				Location antagonist	= interaction.Antagonist as Location;
				Location source		= interaction.Protagonist.Location;
				
				if (interaction.Protagonist != null && antagonist != null)
				{
					if (this.CalculateDistance(source, antagonist) <= 2)
					{
						//For standard movement, we should only be able to move up and down a limited range of slopes
						double gradient = (double)(antagonist.Z - source.Z)/Constants.MaximumMoveZ;
						if (gradient != 0)
						{
							if (gradient > 0) 
							{
								if (gradient > 1) interaction.Failure("The slope is too steep to walk up", false);
								else
								{
									if (gradient >0.5) interaction.Log += "You struggle to climb the steep incline";
									interaction.Impedance += interaction.Impedance * gradient;
								}
							}
							else
							{
								gradient = -gradient;
								if (gradient > 1) interaction.Failure("The slope is too steep to walk down", false);
								else
								{
									if (gradient >0.5)
									{
										interaction.Log += "You struggle to descend the steep slope";
									}
									interaction.Impedance += interaction.Impedance * (gradient-0.5);
								}
							}
						}
						if (!interaction.Finished)
						{
							//interaction.Log+=string.Format("Total move cost: {0} ", interaction.Impedance);
							if (interaction.ProtagonistCache.UseEnergy(interaction.Impedance, EnergyType.Fitness))
							{
								this.ApplyInteraction(interaction, interaction.Protagonist, antagonist);
								// Now everything that was trying to impede progress is going to have to take damage I suppose...?
							}
							else 
							{
								if (interaction.Impedance > interaction.ProtagonistCache.Strength * interaction.Protagonist.Traits["Energy"].Maximum)
								{
									interaction.Failure(string.Format("{0}Your chosen route seems impassable.", interaction.Log), false);
								}
								else interaction.Failure(string.Format("{0}You are unable to summon sufficient energy to make it to your destination.", interaction.Log), false);
							}
						}
					}
					else interaction.Failure("You cannot move that far", true);
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
			// We would apply any charges built up in interaction here, but there are none at present so
			// it's not going to be used.
			if (actor is Avatar)
			{
				Avatar avatar = actor as Avatar;
				
				using (interaction.Lock(avatar, avatar.Location.Inhabitants, target.Inhabitants))
				{
					avatar.Location.Inhabitants.Remove(avatar);
					target.Inhabitants.Add(avatar);
					avatar.Location = target;
				}
					
				interaction.Success(string.Format("{0}You reach your destination, a {1}", interaction.Log, target.Inspect(avatar).ShortDescription));	
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
