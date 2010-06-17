using System;
using System.Text;
using System.Collections.Generic;

using Henge.Data.Entities;


namespace Henge.Rules
{
	public interface IInteraction
	{
		string Conclusion	{ get; }
		bool Finished		{ get; }
		bool Succeeded		{ get; }
		bool Illegal		{ get; }
		
	}
	
	// This will be the transaction buffer for a single interaction
	public class Interaction : IInteraction
	{
		public IList<Component> Interferers				{ get; set; }
		public Component Subject						{ get; set; }
		public Component Antagonist						{ get; set; }
		public Actor Protagonist						{ get; set; }
		public string Conclusion						{ get; private set; }
		public bool Finished							{ get; private set; }
		public bool Succeeded							{ get; private set; }
		public bool Illegal								{ get; private set; }
		public Dictionary<string, object> Transaction 	{ get; private set; }
		public IList<Func<bool, bool>> Deltas 			{ get; private set; }
		
		
		public Interaction (Actor protagonist, Component antagonist)
		{
			this.Transaction 	= new Dictionary<string, object>();
			this.Deltas			= new List<Func<bool, bool>>();
			this.Interferers	= new List<Component>();
			this.Protagonist	= protagonist;
			this.Antagonist		= antagonist;
		}		
		
		
		public void Success(string message)
		{
			this.Finished	= true;
			this.Succeeded	= true;
			this.Illegal	= false;
			this.Conclusion = message;
		}
		
		
		public void Failure(string message, bool illegal)
		{
			this.Finished	= true;
			this.Succeeded 	= false;
			this.Illegal	= illegal;
			this.Conclusion	= message;
		}
	}
}
