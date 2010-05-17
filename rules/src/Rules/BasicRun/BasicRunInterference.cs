
using System;
using Henge.Rules;

namespace Henge.Rules.Core
{


	public class BasicRunInterference : BasicRun, IInterferenceRule
	{

		public BasicRunInterference ()
		{
			this.ruletype = "interference";
		}
		
		public Interaction ContinueInteraction(Interaction interaction)
		{
			return interaction;	
		}

	}
}
