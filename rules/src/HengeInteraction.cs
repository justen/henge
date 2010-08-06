using System;
using System.Linq;
using System.Collections.Generic;

using Henge.Data;
using Henge.Data.Entities;


namespace Henge.Rules
{
	public class HengeInteraction : Interaction
	{
		public double Impedance		{ get; set; }
		public double Difficulty	{ get; set; }
		public string Log 			{ get; set; }
		
		public PropertyCache ProtagonistCache	{ get; private set; }
		public PropertyCache AntagonistCache	{ get; private set; }
		public PropertyCache SubjectCache		{ get; private set; }
		
		public IList<Action> Actions			{get; private set;}
		
		
		public HengeInteraction(DataProvider db, Actor protagonist, Component antagonist, Dictionary<string, object> arguments) : base(db, protagonist, antagonist, arguments)
		{
			this.Actions = new List<Action>();
			this.ProtagonistCache	= new PropertyCache(db, protagonist);
			this.AntagonistCache	= new PropertyCache(db, antagonist);
			this.Difficulty = 0;
			this.Impedance = 0;
		}
		
		
		public override void SetSubject(Component subject)
		{
			if (this.SubjectCache != null && this.Subject is Actor) this.ApplyBonuses(this.SubjectCache.SkillBonuses, this.Subject as Actor);
			
			this.Subject		= subject;
			this.SubjectCache	= new PropertyCache(this.db, subject);
		}	
		
		
		public bool TraitCheck(Component subject, string traitName)
		{
			bool result = false;
			if (subject.Traits.ContainsKey(traitName))
			{
				Trait trait = subject.Traits[traitName];
				if ( trait.Expiry.HasValue && trait.Expiry.Value < DateTime.Now)
				{
					using (this.db.Lock(subject.Traits))
					{
						subject.Traits.Remove(traitName);
					}
				}
				else result = true;
			}
			return result;
		}
		
		
		public override IInteraction Conclude()
		{
			if (this.Antagonist is Actor) 							this.ApplyBonuses(this.AntagonistCache.SkillBonuses, this.Antagonist as Actor);
			if (this.SubjectCache != null && this.Subject is Actor)	this.ApplyBonuses(this.SubjectCache.SkillBonuses, this.Subject as Actor);
			
			this.ApplyBonuses(this.ProtagonistCache.SkillBonuses, this.Protagonist as Actor);
		
			return this as IInteraction;
		}
		
		public new IInteraction Failure(string message, bool illegal)
		{
			return base.Failure(string.Format("{0}{1}", this.Log, message), illegal);	
		}
		
		public new IInteraction Success(string message)
		{
			return base.Success(string.Format("{0}{1}", this.Log, message));	
		}
		
		private void ApplyBonuses(Dictionary<Skill, double> bonuses, Actor actor)
		{
			if (actor != null && bonuses != null)
			{
				List<string> skillsToAdd = new List<string>();
				
				using (this.db.Lock(bonuses.Keys.ToArray()))
				{
					foreach (var item in bonuses)
					{
						item.Key.Add(item.Value);
						
						if (item.Key.Value == 1.0) skillsToAdd.AddRange(item.Key.Children);
					}
				}
			
				if (skillsToAdd.Any())
				{
					using (this.db.Lock(actor.Skills))
					{
						foreach (string skill in skillsToAdd)
						{
							if (!actor.Skills.ContainsKey(skill)) actor.Skills.Add(skill, new Skill { Value = Constants.SkillGrantDefault });
						}
					}
				}
			}
		}
	}
}
