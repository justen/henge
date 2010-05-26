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
		private Interactor() {}
		
		
		public void Initialise(string applicationPath)
		{
			this.rulebook = Loader.LoadRules(applicationPath);
		}
		

		public Interaction Interact(Actor protagonist, HengeEntity antagonist, string interaction)
		{
			return this.rulebook.Section(interaction).ApplyRules(new Interaction { Antagonist = antagonist, Protagonist = protagonist });
		}
	}
}
