using System;
using System.Text;
using System.Collections.Generic;

using Henge.Data.Entities;


namespace Henge.Rules
{
	// This will be the transaction buffer for a single interaction
	public class Interaction
	{
		private Dictionary<string, object> transaction = new Dictionary<string, object>();
		
		private string conclusion;
		private bool succeeded					= false;
		private bool illegal 					= false;
		private Actor protagonist				= null;
		private int antagonist					= -1;
		private int interferer					= -1;
		private IList<HengeEntity> antagonists	= null;
		private IList<HengeEntity> interferers	= null;
		
		
		public Interaction ()
		{
			this.conclusion = "";
		}
		
		
		public Actor Protagonist
		{
			get
			{
				return this.protagonist;
			}
			
			set
			{
				this.protagonist = value;
			}
		}
		
		
		public HengeEntity Antagonist
		{
			get
			{
				if ((this.antagonist>=0) && (this.antagonist<this.antagonists.Count))
					return this.antagonists[this.antagonist];
				else return null;
			}
			
			set 
			{
				this.antagonists = new List<HengeEntity>();
				this.antagonists.Add(value);
				this.antagonist = 0;
			}	
		}
		
		
		public HengeEntity Interferer
		{
			get
			{
				if ((this.interferer>=0) && (this.interferer<this.interferers.Count))
					return this.interferers[this.interferer];
				else return null;
			}
			
			set 
			{
				this.interferers = new List<HengeEntity>();
				this.interferers.Add(value);
				this.interferer = 0;
			}	
		}
		
		
		public IList<HengeEntity> Antagonists
		{
			get
			{
				return this.antagonists;
			}
			set
			{
				this.antagonists = value;
				if (this.Antagonists != null)
				{
					this.antagonist = 0;
				}
				else this.antagonist = -1;
			}
		}	
		
		
		public IList<HengeEntity> Interferers
		{
			get
			{
				return this.interferers;
			}
			set
			{
				this.interferers = value;
				if (this.interferers != null)
				{
					this.interferer = 0;
				}
				else this.interferer = -1;
			}
		}
		
		
		public bool Concluded
		{
			get
			{
				return (!this.conclusion.Equals(""));
			}
		}
		
		
		public Dictionary<string, object> Transaction
		{
			get
			{
				return this.transaction;	
			}
		}
		
		
		public void Succeeded(string message)
		{
			this.succeeded = true;
			this.conclusion = message;
		}
		
		
		public void Failed (IList<string> failures)
		{
			this.succeeded = false;
			StringBuilder messenger = new StringBuilder(failures[0]);
			for (int i = 1; i < failures.Count; i++)
			{
				messenger.Append(", ");
				messenger.Append(failures[i]);
			}
			this.conclusion = messenger.ToString();
		}
		
		
		public void Illegal (string message)
		{
			this.illegal = true;
			this.conclusion = message;		
		}
		
		
		public bool CycleInterferers()
		{
			bool result = false;
			if ((this.interferer!=-1)&&(this.interferer+1<this.antagonists.Count))
			{
				this.interferer++;
				result = true;
			}
			return result;
		}
		
		
		public bool ResetInterferers()
		{
			if ((this.interferers!=null)&&(this.interferers.Count>0))
			{
				this.interferer = 0;
				return true;
			}
			this.interferer = -1;
			return false;
		}	
		
		
		public bool ResetAntagonists()
		{
			if ((this.antagonists!=null)&&(this.antagonists.Count>0))
			{
				this.antagonist = 0;
				return true;
			}
			this.antagonist = -1;
			return false;
		}
		
		
		public bool CycleAntagonists()
		{
			bool result = false;
			if ((this.antagonist!=-1)&&(this.antagonist+1<this.antagonists.Count))
			{
				this.antagonist++;
				result = true;
			}
			return result;			
		}
	}
}
