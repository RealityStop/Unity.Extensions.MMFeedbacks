using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BOC.BTagged
{
	public enum TagQueryChangeType { Add, Remove }
	public enum MultiTagQueryStyle { All, Any }

	public struct TagQueryChange<T>
	{
		public readonly TagQueryChangeType ChangeType;
		public readonly T Item;

		public TagQueryChange(TagQueryChangeType changeType, T change)
		{
			ChangeType = changeType;
			Item = change;
		}
	}
	
	public abstract class TagQueryBase : IDisposable
	{
		private readonly BTagged.BTaggedQueryGO _query;

		public abstract int Count { get; }


		public TagQueryBase(BTagged.BTaggedQueryGO query)
		{
			query.AddListenerOnEnabled(OnGameObjectEnabled);
			query.AddListenerOnDisabled(OnGameObjectDisabled);
			var results = query.GetGameObjects();
			foreach (var item in results) OnGameObjectEnabled(item);
		}

		
		
		public TagQueryBase(Tag tag)
		{
			_query = BTagged.Find(tag);
			_query.AddListenerOnEnabled(OnGameObjectEnabled);
			_query.AddListenerOnDisabled(OnGameObjectDisabled);
			var results = _query.GetGameObjects();
			foreach (var item in results) OnGameObjectEnabled(item);
		}
		
		public TagQueryBase(IEnumerable<Tag> tags, MultiTagQueryStyle queryStyle)
		{
			if (queryStyle == MultiTagQueryStyle.All)
				_query = BTagged.FindAll(tags);
			else
				_query = BTagged.FindAny(tags);	
			_query.AddListenerOnEnabled(OnGameObjectEnabled);
			_query.AddListenerOnDisabled(OnGameObjectDisabled);
			var results = _query.GetGameObjects();
			foreach (var item in results) OnGameObjectEnabled(item);
		}


		protected abstract void OnGameObjectEnabled(GameObject obj);

		protected abstract void OnGameObjectDisabled(GameObject obj);

		public virtual void Dispose()
		{
			_query.RemoveListenerOnEnabled();
			_query.RemoveListenerOnDisabled();
		}

		public static bool TryGetComponent<T>(GameObject go, out T component)
		{
			component = go.GetComponent<T>();

			return component != null;
		}
	}




	public class TagQuery<T1> : TagQueryBase, IEnumerable<T1>
	{
		private readonly HashSet<T1> _components = new HashSet<T1>();

		public override int Count => _components.Count;

		public Action<T1> OnItemAdded { get; set; }
		public Action<T1> OnItemRemoved { get; set; }

		public TagQuery(BTagged.BTaggedQueryGO query) : base(query)
		{
		}

		public TagQuery(Tag tag) : base(tag)
		{
		}
		public TagQuery(MultiTagQueryStyle queryStyle, params Tag[] tags) : base(tags, queryStyle)
		{
		}

		public TagQuery(IEnumerable<Tag> tags, MultiTagQueryStyle queryStyle) : base(tags, queryStyle)
		{
		}

		public IDisposable ObserveChanges(Action<TagQueryChange<T1>> onChange)
		{
			void AddFunc(T1 item)
			{
				onChange.Invoke(new TagQueryChange<T1>(TagQueryChangeType.Add, item));
			}

			void RemoveFunc(T1 item)
			{
				onChange.Invoke(new TagQueryChange<T1>(TagQueryChangeType.Remove, item));
			}
			
			foreach (var item in this)
			{
				AddFunc(item);
			}

			OnItemAdded += AddFunc;
			OnItemRemoved += RemoveFunc;

			return Disposable.Create(() =>
			{
				OnItemAdded -= AddFunc;
				OnItemRemoved -= RemoveFunc;
			});
		}
		protected override void OnGameObjectEnabled(GameObject go)
		{
			//Not sure if the backing data would prevent duplicate adds or not.  Contains to prevent throws might not be necessary
			if (Has(go, out T1 c1))
			{
				if (!_components.Contains(c1))
				{
					_components.Add(c1);
					OnItemAdded?.Invoke(c1);
				}
			}
		}

		protected override void OnGameObjectDisabled(GameObject go)
		{
			//Not sure if the backing data would prevent duplicate removes or not.  Contains to prevent throws might not be necessary
			if (Has(go, out T1 c1))
			{
				if (_components.Contains(c1))
				{
					_components.Remove(c1);
					OnItemRemoved?.Invoke(c1);
				}
			}
		}


		public bool Has(GameObject go, out T1 component1)
		{
			component1 = default;

			return go.TryGetComponent(out component1);
		}

		public HashSet<T1>.Enumerator GetEnumerator() => _components.GetEnumerator();

		IEnumerator<T1> IEnumerable<T1>.GetEnumerator() => GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public override void Dispose()
		{
			base.Dispose();
			foreach (var item in this)
			{
				OnItemRemoved?.Invoke(item);
			}
		}
	}




	public class TagQuery<T1, T2> : TagQueryBase, IEnumerable<(T1, T2)>
	{
		private readonly HashSet<(T1, T2)> _components = new HashSet<(T1, T2)>();
		public override int Count => _components.Count;

		public Action<(T1,T2)> OnItemAdded  { get; set; }
		public Action<(T1,T2)> OnItemRemoved  { get; set; }

		public TagQuery(BTagged.BTaggedQueryGO query) : base(query)
		{
		}
		public TagQuery(Tag tag) : base(tag)
		{
		}
		public TagQuery(MultiTagQueryStyle queryStyle, params Tag[] tags) : base(tags, queryStyle)
		{
		}

		public TagQuery(IEnumerable<Tag> tags, MultiTagQueryStyle queryStyle) : base(tags, queryStyle)
		{
		}

		public IDisposable ObserveChanges(Action<TagQueryChange<(T1,T2)>> onChange)
		{
			void AddFunc((T1,T2) item)
			{
				onChange.Invoke(new TagQueryChange<(T1,T2)>(TagQueryChangeType.Add, item));
			}

			void RemoveFunc((T1,T2) item)
			{
				onChange.Invoke(new TagQueryChange<(T1,T2)>(TagQueryChangeType.Remove, item));
			}
			
			foreach (var item in this)
			{
				AddFunc(item);
			}

			OnItemAdded += AddFunc;
			OnItemRemoved += RemoveFunc;

			return Disposable.Create(() =>
			{
				OnItemAdded -= AddFunc;
				OnItemRemoved -= RemoveFunc;
			});
		}
		
		protected override void OnGameObjectEnabled(GameObject go)
		{
			//Not sure if the backing data would prevent duplicate adds or not.  Contains to prevent throws might not be necessary
			if (Has(go, out T1 c1, out T2 c2))
			{
				if (!_components.Contains((c1, c2)))
				{
					_components.Add((c1, c2));
					OnItemAdded?.Invoke((c1, c2));
				}
			}
		}

		protected override void OnGameObjectDisabled(GameObject go)
		{
			//Not sure if the backing data would prevent duplicate removes or not.  Contains to prevent throws might not be necessary
			if (Has(go, out T1 c1, out T2 c2))
			{
				if (_components.Contains((c1, c2)))
				{
					_components.Remove((c1, c2));
					OnItemRemoved?.Invoke((c1, c2));
				}
			}
		}


		public bool Has(GameObject go, out T1 component1, out T2 component2)
		{
			component1 = default;
			component2 = default;

			return go.TryGetComponent(out component1) &&
				go.TryGetComponent(out component2);
		}

		public HashSet<(T1, T2)>.Enumerator GetEnumerator() => _components.GetEnumerator();

		IEnumerator<(T1, T2)> IEnumerable<(T1, T2)>.GetEnumerator() => GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
		
		public override void Dispose()
		{
			base.Dispose();
			foreach (var item in this)
			{
				OnItemRemoved?.Invoke(item);
			}
		}
	}



	public class TagQuery<T1, T2, T3> : TagQueryBase, IEnumerable<(T1, T2, T3)>
	{
		private readonly HashSet<(T1, T2, T3)> _components = new HashSet<(T1, T2, T3)>();
		public override int Count => _components.Count;

		public Action<(T1, T2, T3)> OnItemAdded  { get; set; }
		public Action<(T1, T2, T3)> OnItemRemoved { get; set; }

		public TagQuery(Tag tag) : base(tag)
		{
		}
		public TagQuery(MultiTagQueryStyle queryStyle, params Tag[] tags) : base(tags, queryStyle)
		{
		}

		public TagQuery(IEnumerable<Tag> tags, MultiTagQueryStyle queryStyle) : base(tags, queryStyle)
		{
		}

		public IDisposable ObserveChanges(Action<TagQueryChange<(T1,T2,T3)>> onChange)
		{
			void AddFunc((T1,T2,T3) item)
			{
				onChange.Invoke(new TagQueryChange<(T1,T2,T3)>(TagQueryChangeType.Add, item));
			}

			void RemoveFunc((T1,T2,T3) item)
			{
				onChange.Invoke(new TagQueryChange<(T1,T2,T3)>(TagQueryChangeType.Remove, item));
			}
			
			foreach (var item in this)
			{
				AddFunc(item);
			}

			OnItemAdded += AddFunc;
			OnItemRemoved += RemoveFunc;

			return Disposable.Create(() =>
			{
				OnItemAdded -= AddFunc;
				OnItemRemoved -= RemoveFunc;
			});
		}
		
		protected override void OnGameObjectEnabled(GameObject go)
		{
			//Not sure if the backing data would prevent duplicate adds or not.  Contains to prevent throws might not be necessary
			if (Has(go, out T1 c1, out T2 c2, out T3 c3))
			{
				if (!_components.Contains((c1, c2, c3)))
				{
					_components.Add((c1, c2, c3));
					OnItemAdded?.Invoke((c1, c2, c3));
				}
			}
		}

		protected override void OnGameObjectDisabled(GameObject go)
		{
			//Not sure if the backing data would prevent duplicate removes or not.  Contains to prevent throws might not be necessary
			if (Has(go, out T1 c1, out T2 c2, out T3 c3))
			{
				if (_components.Contains((c1, c2, c3)))
				{
					_components.Remove((c1, c2, c3));
					OnItemRemoved?.Invoke((c1, c2, c3));
				}
			}
		}


		public bool Has(GameObject go, out T1 component1, out T2 component2, out T3 component3)
		{
			component1 = default;
			component2 = default;
			component3 = default;

			return go.TryGetComponent(out component1) &&
				go.TryGetComponent(out component2) &&
				go.TryGetComponent(out component3);
		}

		public HashSet<(T1, T2, T3)>.Enumerator GetEnumerator() => _components.GetEnumerator();

		IEnumerator<(T1, T2, T3)> IEnumerable<(T1, T2, T3)>.GetEnumerator() => GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
		
		public override void Dispose()
		{
			base.Dispose();
			foreach (var item in this)
			{
				OnItemRemoved?.Invoke(item);
			}
		}
	}




	public class TagQuery<T1, T2, T3, T4> : TagQueryBase, IEnumerable<(T1, T2, T3, T4)>
	{
		private readonly HashSet<(T1, T2, T3, T4)> _components = new HashSet<(T1, T2, T3, T4)>();
		public override int Count => _components.Count;


		public Action<(T1, T2, T3, T4)> OnItemAdded { get; set; }
		public Action<(T1, T2, T3, T4)> OnItemRemoved { get; set; }

		public TagQuery(BTagged.BTaggedQueryGO query) : base(query)
		{
		}
		public TagQuery(Tag tag) : base(tag)
		{
		}
		public TagQuery(MultiTagQueryStyle queryStyle, params Tag[] tags) : base(tags, queryStyle)
		{
		}

		public TagQuery(IEnumerable<Tag> tags, MultiTagQueryStyle queryStyle) : base(tags, queryStyle)
		{
		}
		
		public IDisposable ObserveChanges(Action<TagQueryChange<(T1,T2,T3,T4)>> onChange)
		{
			void AddFunc((T1,T2,T3,T4) item)
			{
				onChange.Invoke(new TagQueryChange<(T1,T2,T3,T4)>(TagQueryChangeType.Add, item));
			}

			void RemoveFunc((T1,T2,T3,T4) item)
			{
				onChange.Invoke(new TagQueryChange<(T1,T2,T3,T4)>(TagQueryChangeType.Remove, item));
			}
			
			foreach (var item in this)
			{
				AddFunc(item);
			}

			OnItemAdded += AddFunc;
			OnItemRemoved += RemoveFunc;

			return Disposable.Create(() =>
			{
				OnItemAdded -= AddFunc;
				OnItemRemoved -= RemoveFunc;
			});
		}

		protected override void OnGameObjectEnabled(GameObject go)
		{
			//Not sure if the backing data would prevent duplicate adds or not.  Contains to prevent throws might not be necessary
			if (Has(go, out T1 c1, out T2 c2, out T3 c3, out T4 c4))
			{
				if (!_components.Contains((c1, c2, c3, c4)))
				{
					_components.Add((c1, c2, c3, c4));
					OnItemAdded?.Invoke((c1, c2, c3, c4));
				}
			}
		}

		protected override void OnGameObjectDisabled(GameObject go)
		{
			//Not sure if the backing data would prevent duplicate removes or not.  Contains to prevent throws might not be necessary
			if (Has(go, out T1 c1, out T2 c2, out T3 c3, out T4 c4))
			{
				if (_components.Contains((c1, c2, c3, c4)))
				{
					_components.Remove((c1, c2, c3, c4));
					OnItemRemoved?.Invoke((c1, c2, c3, c4));
				}
			}
		}


		public bool Has(GameObject go, out T1 component1, out T2 component2, out T3 component3, out T4 component4)
		{
			component1 = default;
			component2 = default;
			component3 = default;
			component4 = default;

			return go.TryGetComponent(out component1) &&
				go.TryGetComponent(out component2) &&
				go.TryGetComponent(out component3) &&
				go.TryGetComponent(out component4);
		}

		public HashSet<(T1, T2, T3, T4)>.Enumerator GetEnumerator() => _components.GetEnumerator();

		IEnumerator<(T1, T2, T3, T4)> IEnumerable<(T1, T2, T3, T4)>.GetEnumerator() => GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
		
		public override void Dispose()
		{
			base.Dispose();
			foreach (var item in this)
			{
				OnItemRemoved?.Invoke(item);
			}
		}
	}



	public class TagQuery<T1, T2, T3, T4, T5> : TagQueryBase, IEnumerable<(T1, T2, T3, T4, T5)>
	{
		private readonly HashSet<(T1, T2, T3, T4, T5)> _components = new HashSet<(T1, T2, T3, T4, T5)>();
		public override int Count => _components.Count;

		public Action<(T1, T2, T3, T4, T5)> OnItemAdded { get; set; }
		public Action<(T1, T2, T3, T4, T5)> OnItemRemoved { get; set; }

		public TagQuery(BTagged.BTaggedQueryGO query) : base(query)
		{
		}
		public TagQuery(Tag tag) : base(tag)
		{
		}
		public TagQuery(MultiTagQueryStyle queryStyle, params Tag[] tags) : base(tags, queryStyle)
		{
		}

		public TagQuery(IEnumerable<Tag> tags, MultiTagQueryStyle queryStyle) : base(tags, queryStyle)
		{
		}

		public IDisposable ObserveChanges(Action<TagQueryChange<(T1,T2,T3,T4,T5)>> onChange)
		{
			void AddFunc((T1,T2,T3,T4,T5) item)
			{
				onChange.Invoke(new TagQueryChange<(T1,T2,T3,T4,T5)>(TagQueryChangeType.Add, item));
			}

			void RemoveFunc((T1,T2,T3,T4,T5) item)
			{
				onChange.Invoke(new TagQueryChange<(T1,T2,T3,T4,T5)>(TagQueryChangeType.Remove, item));
			}
			
			foreach (var item in this)
			{
				AddFunc(item);
			}

			OnItemAdded += AddFunc;
			OnItemRemoved += RemoveFunc;

			return Disposable.Create(() =>
			{
				OnItemAdded -= AddFunc;
				OnItemRemoved -= RemoveFunc;
			});
		}
		
		protected override void OnGameObjectEnabled(GameObject go)
		{
			//Not sure if the backing data would prevent duplicate adds or not.  Contains to prevent throws might not be necessary
			if (Has(go, out T1 c1, out T2 c2, out T3 c3, out T4 c4, out T5 c5))
			{
				if (!_components.Contains((c1, c2, c3, c4, c5)))
				{
					_components.Add((c1, c2, c3, c4, c5));
					OnItemAdded?.Invoke((c1, c2, c3, c4, c5));
				}
			}
		}

		protected override void OnGameObjectDisabled(GameObject go)
		{
			//Not sure if the backing data would prevent duplicate removes or not.  Contains to prevent throws might not be necessary
			if (Has(go, out T1 c1, out T2 c2, out T3 c3, out T4 c4, out T5 c5))
			{
				if (_components.Contains((c1, c2, c3, c4, c5)))
				{
					_components.Remove((c1, c2, c3, c4, c5));
					OnItemRemoved?.Invoke((c1, c2, c3, c4, c5));
				}
			}
		}


		public bool Has(GameObject go, out T1 component1, out T2 component2, out T3 component3, out T4 component4, out T5 component5)
		{
			component1 = default;
			component2 = default;
			component3 = default;
			component4 = default;
			component5 = default;

			return go.TryGetComponent(out component1) &&
				go.TryGetComponent(out component2) &&
				go.TryGetComponent(out component3) &&
				go.TryGetComponent(out component4) &&
				go.TryGetComponent(out component5);
		}

		public HashSet<(T1, T2, T3, T4, T5)>.Enumerator GetEnumerator() => _components.GetEnumerator();

		IEnumerator<(T1, T2, T3, T4, T5)> IEnumerable<(T1, T2, T3, T4, T5)>.GetEnumerator() => GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
		
		public override void Dispose()
		{
			base.Dispose();
			foreach (var item in this)
			{
				OnItemRemoved?.Invoke(item);
			}
		}
	}




	public class TagQuery<T1, T2, T3, T4, T5, T6> : TagQueryBase, IEnumerable<(T1, T2, T3, T4, T5, T6)>
	{
		private readonly HashSet<(T1, T2, T3, T4, T5, T6)> _components = new HashSet<(T1, T2, T3, T4, T5, T6)>();
		public override int Count => _components.Count;


		public Action<(T1, T2, T3, T4, T5, T6)> OnItemAdded { get; set; }
		public Action<(T1, T2, T3, T4, T5, T6)> OnItemRemoved { get; set; }

		public TagQuery(BTagged.BTaggedQueryGO query) : base(query)
		{
		}
		public TagQuery(Tag tag) : base(tag)
		{
		}
		public TagQuery(MultiTagQueryStyle queryStyle, params Tag[] tags) : base(tags, queryStyle)
		{
		}

		public TagQuery(IEnumerable<Tag> tags, MultiTagQueryStyle queryStyle) : base(tags, queryStyle)
		{
		}

		public IDisposable ObserveChanges(Action<TagQueryChange<(T1,T2,T3,T4,T5,T6)>> onChange)
		{
			void AddFunc((T1,T2,T3,T4,T5,T6) item)
			{
				onChange.Invoke(new TagQueryChange<(T1,T2,T3,T4,T5,T6)>(TagQueryChangeType.Add, item));
			}

			void RemoveFunc((T1,T2,T3,T4,T5,T6) item)
			{
				onChange.Invoke(new TagQueryChange<(T1,T2,T3,T4,T5,T6)>(TagQueryChangeType.Remove, item));
			}
			
			foreach (var item in this)
			{
				AddFunc(item);
			}

			OnItemAdded += AddFunc;
			OnItemRemoved += RemoveFunc;

			return Disposable.Create(() =>
			{
				OnItemAdded -= AddFunc;
				OnItemRemoved -= RemoveFunc;
			});
		}
		
		protected override void OnGameObjectEnabled(GameObject go)
		{
			//Not sure if the backing data would prevent duplicate adds or not.  Contains to prevent throws might not be necessary
			if (Has(go, out T1 c1, out T2 c2, out T3 c3, out T4 c4, out T5 c5, out T6 c6))
			{
				if (!_components.Contains((c1, c2, c3, c4, c5, c6)))
				{
					_components.Add((c1, c2, c3, c4, c5, c6));
					OnItemAdded((c1, c2, c3, c4, c5, c6));
				}
			}
		}

		protected override void OnGameObjectDisabled(GameObject go)
		{
			//Not sure if the backing data would prevent duplicate removes or not.  Contains to prevent throws might not be necessary
			if (Has(go, out T1 c1, out T2 c2, out T3 c3, out T4 c4, out T5 c5, out T6 c6))
			{
				if (_components.Contains((c1, c2, c3, c4, c5, c6)))
				{
					_components.Remove((c1, c2, c3, c4, c5, c6));
					OnItemRemoved((c1, c2, c3, c4, c5, c6));
				}
			}
		}


		public bool Has(GameObject go, out T1 component1, out T2 component2, out T3 component3, out T4 component4, out T5 component5, out T6 component6)
		{
			component1 = default;
			component2 = default;
			component3 = default;
			component4 = default;
			component5 = default;
			component6 = default;

			return go.TryGetComponent(out component1) &&
				go.TryGetComponent(out component2) &&
				go.TryGetComponent(out component3) &&
				go.TryGetComponent(out component4) &&
				go.TryGetComponent(out component5) &&
				go.TryGetComponent(out component6);
		}

		public HashSet<(T1, T2, T3, T4, T5, T6)>.Enumerator GetEnumerator() => _components.GetEnumerator();

		IEnumerator<(T1, T2, T3, T4, T5, T6)> IEnumerable<(T1, T2, T3, T4, T5, T6)>.GetEnumerator() => GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
		
		public override void Dispose()
		{
			base.Dispose();
			foreach (var item in this)
			{
				OnItemRemoved?.Invoke(item);
			}
		}
	}
}