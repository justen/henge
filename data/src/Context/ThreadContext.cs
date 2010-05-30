using System;
using Db4objects.Db4o;

namespace Henge.Data.Context
{
	internal class ThreadContext : IContext
	{
		[ThreadStatic]
		private static IObjectContainer container = null;
		
		
		public IObjectContainer Container
		{
			get { return container; 	}
			set { container = value;	}
		}
	}
}