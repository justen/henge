using System;
using System.Linq;
using System.Threading;


namespace ConcurrencyLayer
{
	internal abstract class PersistentBase
	{
		private static int LOCK_TIMEOUT = 1;  // Milliseconds
		private static int CACHE_LIFE	= 600;	// Seconds

		protected PersistenceCache cache;
		protected ReaderWriterLockSlim objectLock = new ReaderWriterLockSlim();
		
		public long Id					{ get; set; }
		public bool Dirty				{ get; set; }
		public DateTime Access			{ get; set; }
		public object Object			{ get; set; }	
		public object PersistentObject	{ get; set; }
		
		
		public PersistentBase(long id, object source, PersistenceCache cache)
		{
			this.Id					= id;
			this.Dirty				= false;
			this.Object				= source;
			this.PersistentObject	= this;
			this.Access				= DateTime.Now;
			this.cache				= cache;
		}
		
		
		public bool Expired
		{
			get { return (DateTime.Now - this.Access).Seconds > CACHE_LIFE; }
		}
		
		
		public bool Lock()
		{
			return this.objectLock.TryEnterWriteLock(LOCK_TIMEOUT);	
		}
		
		
		public void Unlock()
		{
			if (this.objectLock.IsWriteLockHeld) this.objectLock.ExitWriteLock();
		}
		
		
		protected void AssertWrite()
		{
			if (!this.objectLock.IsWriteLockHeld) throw new Exception("Attempted to modify an unlocked persistent object");
			
			this.Dirty = true;
		}
		
		
		protected T Read<T>(Func<T> operation)
		{
			T result = default(T);
			
			if (!this.objectLock.IsWriteLockHeld)
			{
				this.objectLock.EnterReadLock();
				
				try 	{ result = operation();				}
				finally	{ this.objectLock.ExitReadLock();	}
			}
			else result = operation();
			
			return result;
		}
		
		
		protected PersistentBase GetBase(Type type, object item)
		{
			return (item is IPersistence) ? (item as IPersistence).GetBase() as PersistentBase : this.cache.GetBase(type, item);
		}
	}
}
