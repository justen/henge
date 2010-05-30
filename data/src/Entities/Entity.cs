using System;
using Db4objects.Db4o;
using Db4objects.Db4o.Activation;
using Db4objects.Db4o.TA;



namespace Henge.Data.Entities
{
	public abstract class Entity //: IActivatable
	{
		//public virtual long Id { get; protected set; }
		
		
		/*[Transient]
		private IActivator activator = null;
		
		public void Bind (IActivator activator)
		{
			this.activator = activator;
		}
		
		
		public void Activate (ActivationPurpose purpose)
		{
			if (this.activator != null) this.activator.Activate(purpose);
		}*/
	}
}
