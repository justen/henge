using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using Coincidental;
using Henge.Data.Entities;


namespace Henge.Data
{
	public class DataProvider
	{
		private static int ACTIVATION_DEPTH					= 2;
		private Type objectEntityType						= typeof(ObjectEntity);
		private Provider objectProvider						= new Provider();
		private RelationalDataProvider relationalProvider	= new RelationalDataProvider();
		
		
		public bool Initialise(string objectConnection, string relationalType, string relationalConnection, bool web)
		{
			CoincidentalConfiguration config = Provider.Configure
				.Connection(objectConnection)
				.ActivationDepth(ACTIVATION_DEPTH)
				.Debugging(true)
				.Indexing(i => i.AssemblyOf<Entity>().Where(t => t.IsSubclassOf(typeof(ObjectEntity))));
					       
			return 	this.objectProvider.Initialise(config) && 
					this.relationalProvider.Initialise(relationalType, relationalConnection, web);
		}
		
		
		public void Dispose()
		{
			this.objectProvider.Dispose();
			this.relationalProvider.Dispose();
		}
		
		
		public void Bootstrap(List<Entity> data)
		{
			foreach(Entity entity in data) 
			{
				this.objectProvider.Store(entity);
			}
		}
		
		
		public bool UpdateSchema()
		{
			return this.relationalProvider.UpdateSchema();
		}
		
		
		public bool RegisterContext()
		{	
			return this.relationalProvider.RegisterContext();
		}
		
		
		public bool ReleaseContext()
		{
			return this.relationalProvider.ReleaseContext();
		}
		
		
		public T Store<T>(T entity) where T : Entity
		{
			return (entity is ObjectEntity) ? this.objectProvider.Store<T>(entity) : this.relationalProvider.Store<T>(entity);
		}
		
		
		
		public bool Delete(Entity entity)
		{
			return (entity is ObjectEntity) ? this.objectProvider.Delete(entity as ObjectEntity) : this.relationalProvider.Delete(entity as RelationalEntity);
		}
		
		
		public bool Delete<T>(IEnumerable<T> entities) where T : Entity
		{
			return typeof(T).IsSubclassOf(objectEntityType) ? this.objectProvider.Delete(entities) : this.relationalProvider.Delete(entities);
		}
		
		
		public IQueryable<T> Query<T>() where T : Entity
		{
			return typeof(T).IsSubclassOf(objectEntityType) ? this.objectProvider.Query<T>() : this.relationalProvider.Query<T>();
		}
		
		
		// Getting by ID is only valid for a relational database
		public T Get<T>(object id) where T : RelationalEntity
		{
			return this.relationalProvider.Get<T>(id);
		}
		
		
		public T Get<T>(System.Linq.Expressions.Expression<Func<T, bool>> expression) where T : Entity
		{
			return typeof(T).IsSubclassOf(objectEntityType) ? this.objectProvider.Get<T>(expression) : this.relationalProvider.Get<T>(expression);
		}
		
		
		public IDisposable Lock(params object [] entities)
		{
			return this.objectProvider.Lock(entities);
		}
		
		
		public void Flush()
		{
			this.objectProvider.Flush();
			this.relationalProvider.Flush();
		}
	}

}