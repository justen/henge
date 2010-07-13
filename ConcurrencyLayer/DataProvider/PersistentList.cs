using System;
using System.Linq;
using System.Threading;
using System.Collections.Generic;



namespace ConcurrencyLayer
{
	internal class PersistentList<T> : PersistentBase, IPersistence//, IList<T>
	{
		protected IList<T> source		= null;
		protected IList<T> persistent	= null;
		private bool isClass;
		private Type type;
		
		
		public PersistentList(long id, IList<T> source, PersistenceCache cache) : base(id, source, cache)
		{
			this.type 				= typeof(T);
			this.source				= source;
			this.isClass			= this.type.IsClass && this.type != typeof(string);
			this.PersistentObject	= this;
			
			if (this.isClass)
			{
				this.persistent	= new List<T>(source.Count);
				foreach (T item in this.source)
				{
					this.persistent.Add((T)this.cache.GetPersistent(this.type, item));
				}
			}
		}
		
		
		public object GetSource()
		{
			return this.Object;
		}
		
		
		public object GetBase()
		{
			return this;
		}
		
		
	#region IList<T> Members
		int IndexOf(T item)
		{
			return this.persistent.IndexOf(item);
		}
		

		void Insert(int index, T item)
		{
			this.AssertWrite();
			
			if (index < 0) throw new IndexOutOfRangeException("Negative Index");

			if (this.isClass)
			{
				IPersistence persistent = item as IPersistence;
				PersistentBase pb 		= (persistent == null) ? this.cache.GetBase(this.type, item) : persistent.GetBase() as PersistentBase;

				this.source.Insert(index, (T)pb.Object);
				this.persistent.Insert(index, (T)pb.PersistentObject);
			}
			else this.source.Insert(index, item);
		}
		

		T this[int index]
		{
			get
			{
				throw new NotImplementedException();
				
				return default(T);
//				if (index < 0)
//				{
//					throw new IndexOutOfRangeException("negative index");
//				}
//				object result = ReadElementByIndex(index);
//				if (result == Unknown)
//				{
//					return glist[index];
//				}
//				else
//				{
//					return (T) result;
//				}
			}
			set
			{
				this.AssertWrite();
				throw new NotImplementedException();
//				if (index < 0)
//				{
//					throw new IndexOutOfRangeException("negative index");
//				}
//				object old = PutQueueEnabled ? ReadElementByIndex(index) : Unknown;
//				if (old == Unknown)
//				{
//					Write();
//					glist[index] = value;
//				}
//				else
//				{
//					QueueOperation(new SetDelayedOperation(this, index, value, old));
//				}
			}
		}
	#endregion
		
		
	}//*/
}
