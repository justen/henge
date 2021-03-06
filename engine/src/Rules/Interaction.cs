using System;
using System.Text;
using System.Collections.Generic;

using Henge.Data;
using Henge.Data.Entities;


namespace Henge.Rules
{
	// This will be the transaction buffer for a single interaction
	public abstract class Interaction : IInteraction
	{
		public Component Subject						{ get; protected set; }
		public Component Antagonist						{ get; set; }
		public Actor Protagonist						{ get; set; }
		public IList<Component> Interferers				{ get; private set; }
		public string Conclusion						{ get; private set; }
		public bool Finished							{ get; private set; }
		public bool Succeeded							{ get; private set; }
		public bool Illegal								{ get; private set; }
		public Dictionary<string, object> Arguments		{ get; private set; }
		public Dictionary<string, object> Results		{ get; set; }
		public string Chain								{ get; set; }
		//public List<Entity>	PendingDeletions			{ get; private set; }
		
		public DataProvider db;
		
		
		public Interaction(DataProvider db, Actor protagonist, Component antagonist, Dictionary<string, object> arguments)
		{
			this.Chain				= string.Empty;
			this.db					= db;
			this.Interferers		= new List<Component>();
			this.Protagonist		= protagonist;
			this.Antagonist			= antagonist;
			this.Arguments			= (arguments==null)? new Dictionary<string, object>() : arguments;
			this.Results 			= new Dictionary<string, object>();
			//this.PendingDeletions	= new List<Entity>();
		}		
		
		
		public IInteraction Success(string message)
		{
			this.Finished	= true;
			this.Succeeded	= true;
			this.Illegal	= false;
			this.Conclusion = message;
			
			return this;
		}
		
		
		public IInteraction Failure(string message, bool illegal)
		{
			this.Finished	= true;
			this.Succeeded 	= false;
			this.Illegal	= illegal;
			this.Conclusion	= message;
			
			return this;
		}
		
		
		public virtual void SetSubject(Component subject)
		{
			this.Subject = subject;
		}
		
		
		public virtual IInteraction Conclude()
		{
			return this as IInteraction;	
		}
		
		
		public virtual void Delete(Entity target)
		{
			//if (!this.PendingDeletions.Contains(target)) this.PendingDeletions.Add(target);
			this.db.Delete(target);
		}
		
		
		public IDisposable Lock(params object [] entities)
		{
			return this.db.Lock(entities);
		}
	}
}
