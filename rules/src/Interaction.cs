using System;
using System.Text;
using System.Collections.Generic;

using Henge.Data.Entities;


namespace Henge.Rules
{
	public enum Involvement
	{
		Protagonist,
		Antagonist,
		Interferer
	}
	
	
	// This will be the transaction buffer for a single interaction
	public class Interaction
	{
		public IList<HengeEntity> Interferers			{ get; set; }
		public HengeEntity Antagonist					{ get; set; }
		public Actor Protagonist						{ get; set; }
		public string Conclusion						{ get; private set; }
		public bool Finished							{ get; private set; }
		public bool Succeeded							{ get; private set; }
		public bool Illegal								{ get; private set; }
		public HengeEntity Interferer					{ get; private set; }
		public Dictionary<string, object> Transaction 	{ get; private set; }
		public HengeEntity Subject						{ get; set; }
		
		
		public Interaction ()
		{
			this.Transaction = new Dictionary<string, object>();
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
