using System;
using System.Collections.Generic;

using Henge.Rules;
using Henge.Data.Entities;


namespace Henge.Engine
{
	public sealed class Interactor
	{
		private Rulebook rulebook;
		private static readonly Interactor instance = new Interactor();

		public static Interactor Instance { get { return instance; }}
		
		
		static Interactor () {}
		
		
		private Interactor()
		{
			this.rulebook = Loader.LoadRules();			
		}
		

		public Interaction Interact(Actor protagonist, HengeEntity antagonist, string interaction)
		{
			return this.rulebook.Section(interaction).ApplyRules(new Interaction { Antagonist = antagonist, Protagonist = protagonist });
		}
	}
}
