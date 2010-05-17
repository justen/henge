
using System;
using System.Collections.Generic;
using Henge.Data.Entities;

namespace Henge.Rules
{
	public interface IInterferenceRule : IRule
	{
		Interaction ContinueInteraction(Interaction interaction);
	}
}
