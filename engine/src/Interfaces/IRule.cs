using System;

using Henge.Data.Entities;


namespace Henge.Rules
{
	public interface IRule
	{
		string Interaction	{ get; }
		
		bool Valid(Component subject);
		
		IInteraction Apply(IInteraction interaction);
	}
}
