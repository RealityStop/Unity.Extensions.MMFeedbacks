using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BOC.BTagged
{
	class CollisionRepeater2D : MonoBehaviour, ICollisionRepeater2D
	{
		Action<Collision2D> ICollisionRepeater2D.OnCollisionEnter { get; set; }
		Action<Collision2D> ICollisionRepeater2D.OnCollisionExit { get; set; }
		Action<Collision2D> ICollisionRepeater2D.OnCollisionStay { get; set; }

		private void OnCollisionEnter2D(Collision2D collision)
		{
			((ICollisionRepeater2D)this).OnCollisionEnter?.Invoke(collision);
		}

		private void OnCollisionExit2D(Collision2D collision)
		{
			((ICollisionRepeater2D)this).OnCollisionExit?.Invoke(collision);
		}

		private void OnCollisionStay2D(Collision2D collision)
		{
			((ICollisionRepeater2D)this).OnCollisionStay?.Invoke(collision);
		}
	}
}