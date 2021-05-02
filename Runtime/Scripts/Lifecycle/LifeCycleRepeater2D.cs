using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BOC.BTagged
{
	public class LifeCycleRepeater2D : MonoBehaviour
	{
		private IDestroyRepeater _ObjectDestroy;
		public IDestroyRepeater ObjectDestroy
		{
			get
			{
				if (_ObjectDestroy == null)
					_ObjectDestroy = gameObject.AddComponent<DestroyRepeater>();
				return _ObjectDestroy;
			}
		}

		private ICollisionRepeater2D _ObjectCollision;
		public ICollisionRepeater2D ObjectCollision
		{
			get
			{
				if (_ObjectCollision == null)
					_ObjectCollision = gameObject.AddComponent<CollisionRepeater2D>();
				return _ObjectCollision;
			}
		}

		private ITriggerRepeater2D _ObjectTriggers;
		public ITriggerRepeater2D ObjectTriggers
		{
			get
			{
				if (_ObjectTriggers == null)
					_ObjectTriggers = gameObject.AddComponent<TriggerRepeater2D>();
				return _ObjectTriggers;
			}
		}
	}
}