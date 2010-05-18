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
		private IList<HengeEntity> antagonists	= null;
		private IList<HengeEntity> interferers	= null;

		public Actor Protagonist						{ get; set; }
		public string Conclusion						{ get; private set; }
		public bool Finished							{ get; private set; }
		public bool Succeeded							{ get; private set; }
		public bool Illegal								{ get; private set; }
		public HengeEntity Antagonist					{ get; private set; }
		public HengeEntity Interferer					{ get; private set; }
		public Dictionary<string, object> Transaction 	{ get; private set; }
		
		
		public Interaction ()
		{
			this.Transaction = new Dictionary<string, object>();
		}
		
		
		public IList<HengeEntity> Antagonists	
		{ 
			get { return this.antagonists; }
			
			set
			{
				this.antagonists 	= value;
				this.Antagonist		= (value != null && value.Count > 0) ? value[0] : null;
			}
		}
		
		
		public IList<HengeEntity> Interferers	
		{ 
			get { return this.interferers; }
			
			set
			{
				this.interferers	= value;
				this.Interferer		= (value != null && value.Count > 0) ? value[0] : null;
			}
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
		
		
		public void Next(Involvement involment)
		{
			switch (involment)
			{
				case Involvement.Antagonist:	this.Antagonist = this.Next(this.Antagonist, this.antagonists);		break;
				case Involvement.Interferer:	this.Interferer	= this.Next(this.Interferer, this.interferers);		break;
			}
		}
		
		
		public void Reset(Involvement involment)
		{
			switch (involment)
			{
				case Involvement.Antagonist:	this.Antagonist = (this.antagonists != null && this.antagonists.Count > 0) ? this.antagonists[0] : null;	break;
				case Involvement.Interferer:	this.Interferer	= (this.interferers != null && this.interferers.Count > 0) ? this.interferers[0] : null;	break;
			}
		}
		
		
		private HengeEntity Next(HengeEntity current, IList<HengeEntity> subjects)
		{
			HengeEntity result = null;
			
			if (current != null && subjects != null)
			{
				int index = subjects.IndexOf(current) + 1;	
				if (index > 0 && index < subjects.Count) result = subjects[index];
			}
			
			return result;
		}
	}
}
