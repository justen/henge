using System;
using System.Linq;
using System.Threading;
using System.Collections.Generic;



namespace ConcurrencyLayer
{
	/*internal class PersistentList<T> : IList<T>
	{
		private ContainerCache cache;
		private ReaderWriterLockSlim objectLock = new ReaderWriterLockSlim();

		public bool Dirty	{ get; set; }
		
		protected IList<T> source;
		protected IList<T> persistent;
		
		private bool active;
		private bool isClass;
		private bool isGeneric;
		private Type type;
		
		
		public PersistentList(IList<T> source, ContainerCache cache)
		{
			this.type 		= typeof(T);
			this.source		= source;
			this.cache		= cache;
			this.Dirty		= false;
			this.active		= false;
			this.isClass	= this.type.IsClass && this.type != typeof(string);
			this.isGeneric	= this.type.IsGenericType;
			this.persistent	= this.isClass ? new List<T>(source.Count) : null;
		}
		
		
		private void Activate()
		{
			// Much quicker to check this without entering a lock, but if two threads
			// happen to enter the conditional at the same time you have to avoid problems
			// by checking the active flag again within a lock.
			if (!this.active)
			{
				this.objectLock.EnterWriteLock();
					if (!this.active)
					{
						this.cache.Activate(this.source);
					
						if (this.isClass)
						{
							if (this.isGeneric)
							{
								
							}
							else
							{		
								foreach (T item in this.source)
								{
									this.persistent.Add((T)this.cache.GetPersistent(this.type, item));
								}
							}
						}
						this.active = true;
					}
				this.objectLock.ExitWriteLock();
			}
		}
		
		
	#region IList<T> Members
		int IList<T>.IndexOf(T item)
		{
			this.Activate();
			
			return this.persistent.IndexOf(item);
		}
		

		void IList<T>.Insert(int index, T item)
		{
			if (index < 0) throw new IndexOutOfRangeException("Negative Index");
			
			this.Activate();
			
			
			if (this.isClass)
			{
				if (this.isGeneric)
				{
					// Handle lists of lists/dictionaries here
				}
				else
				{
					IPersistence persistent = item as IPersistence;
					ConcurrencyContainer cc = (persistent == null) ? this.cache.GetContainer(this.type, item) : persistent.GetContainer() as ConcurrencyContainer;
					
					this.objectLock.EnterWriteLock();
					try
					{
						this.source.Insert(index, (T)cc.Object);
						this.persistent.Insert(index, (T)cc.PersistentObject);
					}
					finally
					{
						this.objectLock.ExitWriteLock();
					}
				}
			}
			else
			{
				this.objectLock.EnterWriteLock();
				try
				{
					this.source.Insert(index, item);
				}
				finally
				{
					this.objectLock.ExitWriteLock();
				}
			}
		}
		

		T IList<T>.this[int index]
		{
			get
			{
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
