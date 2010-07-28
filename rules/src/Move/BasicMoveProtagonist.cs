using System;
using System.Collections.Generic;

using Henge.Data.Entities;


namespace Henge.Rules.Protagonist.Move
{
	public class BasicMove : HengeRule, IProtagonist
	{
		public override bool Valid (Component subject)
		{
			return subject is Actor;
		}
		
		protected override double Visibility (HengeInteraction interaction, out Component subject)
		{
			//Moving; become more obvious:
			subject = interaction.Protagonist;
			return (Constants.StandardVisibility * interaction.ProtagonistCache.Conspicuousness);
		}
		
		protected override IInteraction Apply(HengeInteraction interaction)
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
						double gradient = (double)(antagonist.Z - source.Z)/255.0;
						if (gradient != 0)
						{
							if (gradient > 0) 
							{
								if (!interaction.ProtagonistCache.SkillCheck("Climb", gradient))
								{
									//it's beyond your climb skill; make this climb much more difficult
									interaction.Impedance+=gradient * interaction.Impedance;
									interaction.Log += "The ascent is punishing. You scramble up as best you can. ";
								}
								else
								{
									//you can climb this without it taking *that* much more energy	
									interaction.Impedance+=gradient * 0.25 * interaction.Impedance;
								}
							}
							else
							{
								if (interaction.ProtagonistCache.SkillCheck("Climb", -gradient))
								{
									//you're comfortable charging down this slope - impedance will drop
									//as a result (max reduction is to halve the impedance)
									interaction.Impedance+=gradient * interaction.Impedance;
								}
								else
								{
									//You're going to have to climb down this
									if (interaction.ProtagonistCache.SkillCheck("Climb", -gradient * 0.75))
									{
										interaction.Impedance -= gradient * interaction.Impedance;
										interaction.Log += "The descent is difficult; you are forced to climb down. ";
									}
									else
									{
										//What are you, insane? I'm not climbing down that!
										interaction.Failure("The descent is too intimidating for you to attempt", false);
									}	
										
								}
							}
						}
						interaction.Log+=string.Format("Total move cost: {0} ", interaction.Impedance);
						if (interaction.ProtagonistCache.UseEnergy(interaction.Impedance))
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
					else interaction.Failure("Out of range", true);
				}
			}
	
			return interaction;
		}	
		
		
		private int CalculateDistance(Location source, Location destination)
		{
			int deltaX = source.X - destination.X;
			int deltaY = source.Y - destination.Y;
			// currently can't run in z, so don't bother calculating it.
			// int deltaZ = source.Coordinates.Z - destination.Coordinates.Z;
			
			return deltaX * deltaX + deltaY * deltaY;
		}
		
		
		private void ApplyInteraction (HengeInteraction interaction, Actor actor, Location target)
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
					
				interaction.Success(string.Format("{0}You reach your destination", interaction.Log));	
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
