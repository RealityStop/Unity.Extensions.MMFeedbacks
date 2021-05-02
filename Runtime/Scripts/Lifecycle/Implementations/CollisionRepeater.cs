using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BOC.BTagged
{
	class CollisionRepeater : MonoBehaviour, ICollisionRepeater
	{
		Action<Collision> ICollisionRepeater.OnCollisionEnter { get; set; }
		Action<Collision> ICollisionRepeater.OnCollisionExit { get; set; }
		Action<Collision> ICollisionRepeater.OnCollisionStay { get; set; }

		private void OnCollisionEnter(Collision collision)
		{
			((ICollisionRepeater)this).OnCollisionEnter?.Invoke(collision);
		}

		private void OnCollisionExit(Collision collision)
		{
			((ICollisionRepeater)this).OnCollisionExit?.Invoke(collision);
		}

		private void OnCollisionStay(Collision collision)
		{
			((ICollisionRepeater)this).OnCollisionStay?.Invoke(collision);
		}
	}
}