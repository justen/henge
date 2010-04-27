
using System;
using System.Text;
using System.Collections.Generic;

namespace Henge.Engine
{

	//this will be the transaction buffer for a single interaction
	public class Interaction
	{
		private Dictionary<string, object> transaction = new Dictionary<string, object>();
		private bool succeeded = false;
		private bool illegal = false;
		private string conclusion;
		
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
		public Interaction ()
		{
			this.conclusion = "";
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
	}
}
