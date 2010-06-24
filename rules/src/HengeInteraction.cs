using System;
using System.Linq;
using System.Collections.Generic;

using Henge.Data.Entities;


namespace Henge.Rules
{
	public class HengeInteraction : Interaction
	{
		public double Impedance { get; set; }
		
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
	}
}
