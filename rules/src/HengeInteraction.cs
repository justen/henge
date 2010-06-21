using System;
using System.Linq;

using Henge.Data.Entities;


namespace Henge.Rules
{
	public class HengeInteraction : Interaction
	{
		public double Impedance { get; set; }
		
		public PropertyCache ProtagonistCache	{ get; private set; }
		public PropertyCache AntagonistCache	{ get; private set; }
		public PropertyCache SubjectCache		{ get; private set; }
		
		
		public HengeInteraction(Actor protagonist, Component antagonist) : base(protagonist, antagonist)
		{
			this.ProtagonistCache	= new PropertyCache(this.Deltas, protagonist);
			this.AntagonistCache	= new PropertyCache(this.Deltas, antagonist);
		}
		
		
		public override void SetSubject(Component subject)
		{
			this.Subject		= subject;
			this.SubjectCache	= new PropertyCache(this.Deltas, subject);
		}		
	}
}
