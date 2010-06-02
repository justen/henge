using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using Henge.Data.Entities;


namespace Henge.Data
{
	public class DataProvider
	{
		private Type objectEntityType						= typeof(ObjectEntity);
		private ObjectDataProvider objectProvider			= new ObjectDataProvider();
		private RelationalDataProvider relationalProvider	= new RelationalDataProvider();
		
		
		public bool Initialise(string objectConnection, string relationalType, string relationalConnection, bool web)
		{
			return this.objectProvider.Initialise(objectConnection, web) && this.relationalProvider.Initialise(relationalType, relationalConnection, web);
		}
		
		
		public void Dispose()
		{
			this.objectProvider.Dispose();
			this.relationalProvider.Dispose();
		}
		
		
		public void Bootstrap()
		{
			this.objectProvider.Bootstrap();
		}
		
		
		public bool UpdateSchema()
		{
			return this.relationalProvider.UpdateSchema();
		}
		
		
		public bool RegisterContext()
		{	
			return this.objectProvider.RegisterContext() && this.relationalProvider.RegisterContext();
		}
		
		
		public bool ReleaseContext()
		{
			return this.objectProvider.ReleaseContext() && this.relationalProvider.ReleaseContext();
		}
		
		
		public T Store<T>(T entity) where T : Entity
		{
			return (entity is ObjectEntity) ? this.objectProvider.Store<T>(entity) : this.relationalProvider.Store<T>(entity);
		}
		
		
		
		public bool Delete(Entity entity)
		{
			return (entity is ObjectEntity) ? this.objectProvider.Delete(entity as ObjectEntity) : this.relationalProvider.Delete(entity as RelationalEntity);
		}
		
		
		public bool Delete(IList<Entity> entities)
		{
			return (entities is IList<ObjectEntity>) ? this.objectProvider.Delete(entities as IList<ObjectEntity>) : this.relationalProvider.Delete(entities as IList<RelationalEntity>);
		}
		
		
		public IQueryable<T> Query<T>() where T : Entity
		{
			return (typeof(T).IsSubclassOf(objectEntityType)) ? this.objectProvider.Query<T>() : this.relationalProvider.Query<T>();
		}
		
		
		public T Get<T>(System.Linq.Expressions.Expression<Func<T, bool>> expression) where T : Entity
		{
			return (typeof(T).IsSubclassOf(objectEntityType)) ? this.objectProvider.Get<T>(expression) : this.relationalProvider.Get<T>(expression);
		}
		
		
		public void Flush()
		{
			this.objectProvider.Flush();
			this.relationalProvider.Flush();
		}
	}

}