using System;


namespace Henge.Engine
{
	public static class Generator
	{
		private static long id 			= 0;
		private static object locker 	= new object();
		
		
		public static long Id
		{
			get
			{
				lock(locker) return ++id;
			}
		}
	}
}
