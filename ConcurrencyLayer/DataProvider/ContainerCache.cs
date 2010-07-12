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
		private IDictionary<long, WeakReference> cache 	= new SortedDictionary<long, WeakReference>();
		
		
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
						WeakReference reference = this.cache[id];
						if (reference.Target == null) reference.Target = ConcurrencyContainer.Create(type, id, this.Activate(item), this);
						
						result = reference.Target as ConcurrencyContainer;
					}
					else this.cache.Add(id, new WeakReference(result = ConcurrencyContainer.Create(type, id, this.Activate(item), this)));
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
	}
}
