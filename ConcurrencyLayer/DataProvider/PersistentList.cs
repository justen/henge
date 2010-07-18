using System;
using System.Linq;
using System.Threading;
using System.Collections;
using System.Collections.Generic;



namespace ConcurrencyLayer
{
	internal class PersistentList<T> : PersistentBase, IPersistence, IList<T>
	{
		protected IList<T> source		= null;
		protected IList<T> persistent	= null;
		private bool isClass;
		private Type type;
		
		
		public PersistentList(long id, IList<T> source, PersistenceCache cache) : base(id, source, cache)
		{
			this.type 				= typeof(T);
			this.source				= source;
			this.isClass			= (this.type.IsClass || this.type.IsGenericType) && this.type != typeof(string);
			this.PersistentObject	= this;
			
			if (this.isClass)
			{
				this.persistent	= new List<T>(source.Count);
				
				foreach (T item in this.source)
				{
					this.persistent.Add((T)this.cache.GetPersistent(this.type, item));
				}
			}
		}
		
		
		public object GetSource()
		{
			return this.Object;
		}
		
		
		public object GetBase()
		{
			return this;
		}
		
		
	#region IList<T> Members
		public int IndexOf(T item)
		{
			return this.Read(() => this.isClass ? this.persistent.IndexOf(item) : this.source.IndexOf(item));
		}
		

		public void Insert(int index, T item)
		{
			this.AssertWrite();
			if (index < 0) throw new IndexOutOfRangeException("Negative index");

			if (this.isClass)
			{
				PersistentBase persistent = this.GetBase(this.type, item);

				this.source.Insert(index, (T)persistent.Object);
				this.persistent.Insert(index, (T)persistent.PersistentObject);
			}
			else this.source.Insert(index, item);
		}
		

		public T this[int index]
		{
			get
			{
				if (index < 0) throw new IndexOutOfRangeException("Negative index");
				
				return this.Read(() => this.isClass ? this.persistent[index] : this.source[index]);
			}
			set
			{
				this.AssertWrite();
				if (index < 0) throw new IndexOutOfRangeException("Negative index");
				
				if (this.isClass)
				{
					PersistentBase persistent	= this.GetBase(this.type, value);
					this.source[index]			= (T)persistent.Object;
					this.persistent[index]		= (T)persistent.PersistentObject;
				}
				else this.source[index] = value;
			}
		}
	#endregion
		
		
	#region ICollection<T> Members
		public void Add(T item)
		{
			this.AssertWrite();
			
			if (this.isClass)
			{
				PersistentBase persistent = this.GetBase(this.type, item);

				this.source.Add((T)persistent.Object);
				this.persistent.Add((T)persistent.PersistentObject);
			}
			else this.source.Add(item);
		}
		
		
		public void Clear()
		{
			this.AssertWrite();
			
			this.source.Clear();
			if (this.isClass) this.persistent.Clear();
		}

		
		public bool Contains(T item)
		{
			return this.Read(() => this.isClass ? this.persistent.Contains(item) : this.source.Contains(item));
		}
		

		public void CopyTo(T[] array, int index)
		{
			this.Read(() => {
				int count = this.source.Count;
				if (this.isClass) 	for (int i=0; i<count; i++) array.SetValue(this.persistent[i], i + index);
				else 				for (int i=0; i<count; i++) array.SetValue(this.source[i], i + index);
				return true;
			});
		}

		
		public bool Remove(T item)
		{
			this.AssertWrite();
			
			int index = this.IndexOf(item);
			
			if (index >= 0)
			{
				this.RemoveAt(index);
				return true;
			}
			
			return false;
		}
		
		
		public void RemoveAt(int index)
		{
			this.AssertWrite();
			
			this.source.RemoveAt(index);
			if (this.isClass) this.persistent.RemoveAt(index);
		}
		

		public int Count
		{
			get { return this.Read(() => this.source.Count); }
		}

		
		public bool IsReadOnly
		{
			get { return false; }
		}
	#endregion

		
	#region IEnumerable<T> Members
		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return new Enumerator(this);
		}
		
		
		public IEnumerator GetEnumerator()
		{
			return new Enumerator(this);
		}
	#endregion

		
		class Enumerator : IEnumerator<T>, IEnumerator
		{
			private PersistentList<T> list;
			private T current = default(T);
			private int index = -1;
			
			
			public Enumerator(PersistentList<T> list)
			{
				this.list = list;
			}
		
		
			public T Current
			{
				get { return this.current; }
			}
			
			
			object IEnumerator.Current
			{
				get { return this.Current; }
			}
			
			
			public bool MoveNext()
			{
				if (++this.index < this.list.Count) 
				{
					this.current = this.list[this.index];
					return true;
				}
				
				return false;
			}
			
			
			public void Reset() 
			{
				this.index		= -1;
				this.current	= default(T);
			}
			
			
			void IDisposable.Dispose() { }
		}
	}
}
