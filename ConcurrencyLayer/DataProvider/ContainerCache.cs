using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

using Db4objects.Db4o;


namespace ConcurrencyLayer
{
	internal class ContainerCache
	{
		private IObjectContainer container;
		private IDictionary<long, ConcurrencyContainer> cache 	= new SortedDictionary<long, ConcurrencyContainer>();
		
		
		public ContainerCache(IObjectContainer container)
		{
			this.container = container;
		}
		
		
		public ConcurrencyContainer GetContainer<T>(T item) where T : class
		{
			return this.GetContainer(typeof(T), item);
		}	
		
		
		public ConcurrencyContainer GetContainer(Type type, object item)
		{
			ConcurrencyContainer result = null;
			
			if (item != null)
			{
				long id = this.container.Ext().GetID(item);
				
				if (id == 0)
				{
					// If db4o is not aware of this item then store it
					this.container.Store(item);
					id = this.container.Ext().GetID(item);
				}
				
				lock (this)
				{
					if (this.cache.ContainsKey(id))	
					{
						result			= this.cache[id];
						result.Access	= DateTime.Now;
					}
					else this.cache.Add(id, result = ConcurrencyContainer.Create(type, id, this.Activate(item), this));
				}
			}
			
			return result;	
		}
		
	
		public object GetPersistent(Type type, object item)
		{
			ConcurrencyContainer cc = this.GetContainer(type, item);
			
			return cc != null ? cc.PersistentObject : null;
		}
		
		
		public object GetSource(Type type, object item)
		{
			ConcurrencyContainer cc = this.GetContainer(type, item);
			
			return cc != null ? cc.Object : null;
		}
		
		
		public object Activate(object item)
		{
			if (!this.container.Ext().IsActive(item))
			{
				Console.WriteLine("  Lazy loading:" + item.ToString());
				this.container.Activate(item, 1);
			}
			
			return item;
		}
		
		
		public void Flush()
		{
			// Flush dirty items to db4o
			// Check for objects that have not been accessed for some time and remove them from the cache so that they can be garbage collected	
			List<ConcurrencyContainer> flush	= new List<ConcurrencyContainer>();
			List<long> delete					= new List<long>();
			
			lock(this)
			{
				foreach (KeyValuePair<long, ConcurrencyContainer> kvp in this.cache)
				{
					if (kvp.Value.Dirty)	flush.Add(kvp.Value);
					if (kvp.Value.Expired)	delete.Add(kvp.Key);
				}
				
				foreach (long id in delete) this.cache.Remove(id);
			}
			
			foreach (ConcurrencyContainer cc in flush)
			{
				cc.Lock();
					this.container.Store(cc.Object);
					cc.Dirty = false;
				cc.Unlock();
			}
		}
	}
}
