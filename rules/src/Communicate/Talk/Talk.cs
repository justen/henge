using System;
using System.Collections.Generic;

using Henge.Data.Entities;

namespace Henge.Rules.Protagonist.Communicate.Talk
{
	public class Talk : HengeRule, IProtagonist
	{
		public override bool Valid (Component subject)
		{
			//Only Actors can talk
			return (subject is Actor);
		}
		
		
		protected override double Visibility(HengeInteraction interaction, out Component subject)
		{
			subject = interaction.Protagonist;
			return (Constants.StandardVisibility * interaction.SubjectCache.Conspicuousness);
		}
		

		protected override IInteraction Apply(HengeInteraction interaction)
		{
			if (!interaction.Finished)
			{
				if (interaction.ProtagonistCache.UseEnergy(Constants.TalkCost, EnergyType.None))
				{
					if (interaction.Arguments.ContainsKey("Message"))
					{
						List<Avatar> recipients	= interaction.Arguments["Recipients"] as List<Avatar>;
						if (recipients==null) recipients = new List<Avatar>();
						
						string target		= (recipients.Count > 1) ? "out loud" : "to you";
						string message		= string.Format("{0} says \"{1}\" {2}.", interaction.Protagonist.Name, interaction.Arguments["Message"] as string, target);
						DateTime occurred	= DateTime.Now;
						foreach (Avatar recipient in recipients)
						{
							//Log the speech
							using (interaction.Lock(recipient.Log))
							{
								// Possibly inefficient to lock each recipient's log individually. Also, shouldn't
								// we be using the relational DB for this sort of thing?
								recipient.Log.Add(new LogEntry(){ Entry = message, Occurred = occurred});
							}
						}
						if (recipients.Count == 1)
						{
							target = string.Format("to {0}", recipients[0].Name);	
						}
						interaction.Success(string.Format("You say \"{0}\" {0}.", interaction.Arguments["Message"], target));
					} 
					else interaction.Failure("You have nothing to say", false);
				}
				else interaction.Failure("You are too tired even to speak", false);
			}
			return interaction;
		}

		
		protected virtual double CalculateDifficulty(Item item)
		{
			return (1 - item.Traits["Visibility"].Value / Constants.SearchDifficulty);
		}
	}
}
