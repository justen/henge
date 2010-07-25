/*using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

using Db4objects.Db4o;
using Db4objects.Db4o.CS;
using Db4objects.Db4o.TA;
using Db4objects.Db4o.Linq;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.CS.Config;
using Db4objects.Db4o.Config;

using Henge.Data.Context;
using Henge.Data.Entities;

namespace Henge.Data
{
	internal class ObjectDataProvider
	{
		private IContext context		= null;
		private IObjectServer server	= null;
		
		
		//public bool Initialise(string storageType, string connectionString, bool web)
		public bool Initialise(string connectionString, bool web)
		{
			if (this.server == null)
			{
				IServerConfiguration config = Db4oClientServer.NewServerConfiguration();
				config.Common.Add(new TransparentPersistenceSupport());
				config.Common.Add(new TransparentActivationSupport());
				
				//config.Common.IndexClass<User>();
				
				this.context	= web ? (IContext)new WebContext() : (IContext)new ThreadContext();
				this.server		= Db4oClientServer.OpenServer(config, connectionString, 0);
			
				return true;
			}
			
			return false;
		}
		
		
		public void Dispose()
		{
			if (this.server != null) this.server.Close();
		}
		
		
		private T CastAs<T>(Entity entity, T target) where T: Entity
		{
			return entity as T;
		}

		
		public void Bootstrap( List<Entity> data )
		{
			if (this.server != null)
			{
				IObjectContainer container = this.server.OpenClient();
				var all = from Entity a in container select a;
				foreach (var e in all) container.Delete(e);
				foreach(Entity entity in data)
				{
					container.Store(entity);
				}
				container.Commit();
				container.Close();
				container.Dispose();
			}
		}
//		public void Bootstrap()
//		{
//			if (this.server != null)
//			{
//				IObjectContainer container = this.server.OpenClient();
//				
//				var all = from Entity a in container select a;
//				foreach (var e in all) container.Delete(e);
//				
//				Appearance ap				= new Appearance { Type = "Nondescript Wasteland", Description = "looks like the Creator simply couldn''t be bothered to do anything with it. It is totally unremarkable in every way", ShortDescription = "a thoroughly boring spot" };
//				ComponentType type 			= new ComponentType(ap);
//				Appearance apa				= new Appearance { Type = "Oz", Description = "looks like you're not in Kansas any more", ShortDescription = "somewhat magical" };
//				ComponentType typea			= new ComponentType(apa);
//				Henge.Data.Entities.User u	= new Henge.Data.Entities.User { Name = "test", Password = "A94A8FE5CCB19BA61C4C0873D391E987982FBBD3", Clan = "Test" };
//				Map m 						= new Map { Name = "Main" };
//				Location l					= new Location (0, 0, 0);
//				Appearance avatarAppearance = new Appearance {Type = "Person", ShortDescription = "another person", Description = "another person"};
//				ComponentType avatar		= new ComponentType(avatarAppearance);
//				Avatar av					= new Avatar { Name = "Og", User = u, Location = l, Type = avatar };
//				
//				av.Skills.Add("Strength", new Skill { Value = 0.5 });
//				av.Traits.Add("Energy", new Trait { Value = 10, Minimum = -10, Maximum = 10 });
//				av.Traits.Add("Weight", new Trait { Value = 70, Minimum = 0 });
//				
//				u.Avatars.Add(av);
//			//	m.Locations.Add(l.Coordinates, l);
//				l.Inhabitants.Add(av);
//				
//				for (int y = -2; y < 3; y++)
//				{
//					for (int x = -2; x < 3; x++) 
//					{
//						if (x == 0 && y == 0) continue;
//						
//						l = new Location (x, y, 0) { Map = m, Type = type }; 
//			//			m.Locations.Add(l.Coordinates, l);
//					}
//				}
//				
//				container.Store(u);
//				container.Store(m);
//				
//				container.Commit();
//				container.Close();
//				container.Dispose();
//			}
//			
//		}
		
		
		public bool RegisterContext()
		{	
			if (this.server != null)
			{
				this.context.Container = this.server.OpenClient();
				return true;
			}
			
			return false;
		}
		
		
		public bool ReleaseContext()
		{
			if (this.server != null && this.context.Container != null) 
			{
				this.context.Container.Commit();
				this.context.Container.Close();
				this.context.Container = null;
				return true;
			}
			
			return false;
		}
		
		
		public T Store<T>(T entity) where T : Entity
		{
			IObjectContainer container = this.GetContainer();
			
			if (container != null)
			{
				container.Store(entity);
				container.Commit();
				
				return entity;
			}	
			
			return null;
		}
		
		
		public bool Delete(ObjectEntity entity)
		{
			IObjectContainer container = this.GetContainer();
			
			if (container != null)
			{
				container.Delete(entity);
				container.Commit();
				
				return true;
			}
			
			return false;
		}
		
		
		public bool Delete<T>(IList<T> entities) where T : Entity
		{
			IObjectContainer container = this.GetContainer();
			
			if (container != null && entities != null)
			{
				foreach (T entity in entities) container.Delete(entity);
				container.Commit();
				
				return true;
			}
			
			return false;
		}
		
		
		public IQueryable<T> Query<T>() where T : Entity
		{
			IObjectContainer container = this.GetContainer();
			
			return (container == null) ? null : container.AsQueryable<T>(); //(from T item in container select item).AsQueryable();
		}
		
		
		public T Get<T>(System.Linq.Expressions.Expression<Func<T, bool>> expression) where T : Entity
		{
			IObjectContainer container = this.GetContainer();
			
			return (container == null) ? null : container.AsQueryable<T>().SingleOrDefault(expression); //(from T item in container select item).AsQueryable().SingleOrDefault(expression);
		}

		
		public void Flush()
		{
			IObjectContainer container = this.GetContainer();
			if (container != null) container.Commit();
		}
		
		
		
		private IObjectContainer GetContainer()
		{
			return (this.server == null) ? null : this.context.Container;
		}
	}

}*/