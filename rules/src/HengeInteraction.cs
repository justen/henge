using System;
using System.Linq;
using System.Collections.Generic;

using Henge.Data.Entities;


namespace Henge.Rules
{
	public class HengeInteraction : Interaction
	{
		public double Impedance { get; set; }
		public string Log {get; set;}
		
		public PropertyCache ProtagonistCache	{ get; private set; }
		public PropertyCache AntagonistCache	{ get; private set; }
		public PropertyCache SubjectCache		{ get; private set; }
		
		
		public HengeInteraction(Actor protagonist, Component antagonist, Dictionary<string, object> arguments) : base(protagonist, antagonist, arguments)
		{
			this.ProtagonistCache	= new PropertyCache(this.Deltas, protagonist);
			this.AntagonistCache	= new PropertyCache(this.Deltas, antagonist);
		}
		
		
		public override void SetSubject(Component subject)
		{

			if ((this.SubjectCache!=null)&&(this.Subject is Actor )) 
			{
				this.ApplyBonuses(this.SubjectCache.SkillBonuses, this.Subject as Actor, this.SubjectCache);
			}
			this.Subject		= subject;
			this.SubjectCache	= new PropertyCache(this.Deltas, subject);
		}	
		
		public bool TraitCheck(Component subject, string traitName)
		{
			bool result = false;
			if (subject.Traits.ContainsKey(traitName))
			{
				Trait trait = subject.Traits[traitName];
				if ( trait.Expiry.HasValue && trait.Expiry.Value < DateTime.Now)
				{
					this.Deltas.Add((success) => {
						subject.Traits.Remove(traitName);
						return true;
					});
				}
				else result = true;
			}
			return result;
		}
		
		public override IInteraction Conclude()
		{

			if (this.Antagonist is Actor) this.ApplyBonuses(this.AntagonistCache.SkillBonuses, this.Antagonist as Actor, this.AntagonistCache);
			if ((this.SubjectCache!=null)&&(this.Subject is Actor )) 
			{
				this.ApplyBonuses(this.SubjectCache.SkillBonuses, this.Subject as Actor, this.SubjectCache);
			}
			this.ApplyBonuses(this.ProtagonistCache.SkillBonuses, this.Protagonist as Actor, this.ProtagonistCache);
		
			return this as IInteraction;
		}
		
		private void ApplyBonuses(Dictionary<Skill, double> bonuses, Actor actor, PropertyCache cache)
		{
			if (cache!=null && actor!=null && bonuses!=null)
			{
				this.Deltas.Add((success) => {
					foreach (Skill skill in bonuses.Keys)
					{
						skill.Add(bonuses[skill]);
						
						if (skill.Value == 1.0)
						{
							foreach (string s in skill.Children)
							{
								if (!actor.Skills.ContainsKey(s)) actor.Skills.Add(s, new Skill { Value = Constants.SkillGrantDefault });
							}
						}
					}
					return true;
				});
			}
		}
	}
}
