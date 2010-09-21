using System;
using System.Web;

using NHibernate;
using NHibernate.Engine;
using NHibernate.Context;


namespace Henge.Data
{
	internal class HybridSessionContext : CurrentSessionContext
	{
		[ThreadStatic]
		private static ISession threadSession = null;
		private const string webSession = "Henge.Data.Context.WebContext";
		
		
		public HybridSessionContext(ISessionFactoryImplementor factory)
		{
		}
		
		
		protected override ISession Session
		{
			get 
			{ 
				return (HttpContext.Current != null) ? HttpContext.Current.Items[webSession] as ISession : threadSession;		
			}
			
			set 
			{ 
				if (HttpContext.Current != null) 	HttpContext.Current.Items[webSession] 	= value;
				else 								threadSession 							= value;	
			}
		}
	}
}
