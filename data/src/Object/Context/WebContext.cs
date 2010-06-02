using System;
using System.Web;
using Db4objects.Db4o;


namespace Henge.Data.Context
{
	internal class WebContext : IContext
	{
		private const string webContextKey = "Henge.Data.Context.WebContext";
		
		
		public IObjectContainer Container
		{
			get { return HttpContext.Current.Items[webContextKey] as IObjectContainer;	}
			set { HttpContext.Current.Items[webContextKey] = value;						}
		}
	}
}
