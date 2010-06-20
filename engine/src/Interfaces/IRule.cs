using System;

using Henge.Data.Entities;


namespace Henge.Rules
{
	public interface IRule
	{
		string Interaction	{ get; }
		double Priority(Component subject);
		IInteraction Apply(IInteraction interaction);
	}
}
