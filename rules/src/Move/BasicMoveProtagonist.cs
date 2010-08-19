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
		
		private void AddTracks(Location location, Actor actor, bool inbound, string flavour, double strength, HengeInteraction interaction)
		{
			IList<Trait> list = inbound? location.TracesIn : location.TracesOut;		
			if (strength > 0)
			{
				if  (list.Count >= Constants.MaximumTracks)
				{
					Constants.Randomise(list);
					Trait overwritten = list[0];
					list.RemoveAt(0);
					interaction.Delete(overwritten);
				}
				list.Add(new Trait(Constants.MaximumTrackValue, 0, strength){ Subject=actor, Flavour = flavour, Expiry = DateTime.Now + Constants.TraceLife });
			}
					
		}
		
		protected void ApplyInteraction (HengeInteraction interaction, Actor actor, Location target)
		{
			int dx					= actor.Location.X - target.X;
			int dy 					= actor.Location.Y - target.Y;
			char [] inDirection		= new char [] {
				(dx > 0) ? 'e' : (dx < 0) ? 'w' : '-',
				(dy > 0) ? 's' : (dy < 0) ? 'n' : '-'
			};
			
			char [] outDirection		= new char [] {
				(dx < 0) ? 'e' : (dx > 0) ? 'w' : '-',
				(dy < 0) ? 's' : (dy > 0) ? 'n' : '-'
			};
			double baseTrack =  actor.Traits.ContainsKey("Tracks")? actor.Traits["Tracks"].Value : Constants.BaseTrack;
			double trackOut = baseTrack * (actor.Location.Traits.ContainsKey("Tracks")?actor.Location.Traits["Tracks"].Value : 0);
			double trackIn = baseTrack * (target.Traits.ContainsKey("Tracks")?target.Traits["Tracks"].Value : 0);

			if (actor is Avatar)
			{
				Dictionary<string, int> timesIn = new Dictionary<string, int>();
				Dictionary<string, int> timesOut = new Dictionary<string, int>();
				foreach(Trait track in target.TracesOut)
				{
					Actor person = track.Subject as Actor;
					if (timesOut.ContainsKey(person.Name))
					{
						timesOut[person.Name]++;
					}
					else timesOut.Add(person.Name, 1);
				}
				foreach(Trait track in target.TracesIn)
				{
					Actor person = track.Subject as Actor;
					if (timesIn.ContainsKey(actor.Name))
					{
						timesIn[person.Name]++;
					}
					else timesIn.Add(person.Name, 1);
				}
				bool beenHere = false;
				foreach(string name in timesIn.Keys)
				{
					if (name!=actor.Name)
					{
						int outTimes = timesOut.ContainsKey(name)?timesOut[name] : 0;
						int totalTraffic = timesIn[name] + outTimes;
						if (totalTraffic <3)
						{
							interaction.Log += (name + " has been here. ");	
						}
						else 
						{
							if (totalTraffic < 6)
							{
								interaction.Log += (name + " has been here more than once. ");
							}
							else
							{
								interaction.Log += (name + " has been here frequently. ");	
							}
						}
						if (outTimes < timesIn[name]) interaction.Log += "You cannot see any of their tracks leading away. ";
					}
					else beenHere = true;
				}
				if (timesOut.ContainsKey(actor.Name))
				{
					beenHere = true;	
				}
				if (beenHere) interaction.Log += " It looks like you've been here before. ";
				Avatar avatar = actor as Avatar;
								//potential bottleneck here - may want to do smarter locking
				using (interaction.Lock(avatar, avatar.Location.Inhabitants, target.Inhabitants, avatar.Location.TracesOut, target.TracesIn))
				{
					this.AddTracks(avatar.Location, avatar, false, outDirection.ToString(), trackOut, interaction);
					this.AddTracks(target, avatar, true, inDirection.ToString(), trackIn, interaction);
					avatar.Location.Inhabitants.Remove(avatar);
					target.Inhabitants.Add(avatar);
					avatar.Location = target;
				
					
				}
				interaction.Success(string.Format("You reach your destination, a {0}", target.Inspect(avatar).ShortDescription));	
			}
			else
			{
				Npc npc = actor as Npc;
				
				using (interaction.Lock(npc, npc.Location.Fauna, target.Fauna, npc.Location.TracesOut, target.TracesIn))
				{
					this.AddTracks(npc.Location, npc, false, outDirection.ToString(), trackOut, interaction);
					this.AddTracks(target, npc, true, inDirection.ToString(), trackIn, interaction);
					npc.Location.Fauna.Remove(npc);
					target.Fauna.Add(npc);
					npc.Location = target;
				}
					
				interaction.Success("Moved");	
			}
		}	
	}
}
