using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BOC.BTagged
{
	public class LifeCycleRepeater : MonoBehaviour
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

		private ICollisionRepeater _ObjectCollision;
		public ICollisionRepeater ObjectCollision
		{
			get
			{
				if (_ObjectCollision == null)
					_ObjectCollision = gameObject.AddComponent<CollisionRepeater>();
				return _ObjectCollision;
			}
		}

		private ITriggerRepeater _ObjectTriggers;
		public ITriggerRepeater ObjectTriggers
		{
			get
			{
				if (_ObjectTriggers == null)
					_ObjectTriggers = gameObject.AddComponent<TriggerRepeater>();
				return _ObjectTriggers;
			}
		}
	}
}