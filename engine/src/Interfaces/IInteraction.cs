using System;
using System.Collections.Generic;

using Henge.Data.Entities;


namespace Henge.Rules
{
	public interface IInteraction
	{
		Component Subject				{ get; }
		Component Antagonist			{ get; set; }
		Actor Protagonist				{ get; set; }
		//IList<Func<bool, bool>> Deltas 	{ get; }
		IList<Component> Interferers	{ get; }
		//List<Entity> PendingDeletions	{ get; }
		string Conclusion				{ get; }
		bool Finished					{ get; }
		bool Succeeded					{ get; }
		bool Illegal					{ get; }
		string Chain					{ get; }
		Dictionary<string, object> Arguments		{ get;}
		
		IInteraction Conclude();
		IInteraction Success(string message);
		IInteraction Failure(string message, bool illegal);

		void SetSubject(Component subject);
		void Delete(Entity target);
		
		IDisposable Lock(params object [] entities);
	}
}
