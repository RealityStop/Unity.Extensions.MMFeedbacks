using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BOC.BTagged
{
	class TriggerRepeater2D : MonoBehaviour, ITriggerRepeater2D
	{
		Action<Collider2D> ITriggerRepeater2D.OnTriggerEnter { get; set; }
		Action<Collider2D> ITriggerRepeater2D.OnTriggerExit { get; set; }
		Action<Collider2D> ITriggerRepeater2D.OnTriggerStay { get; set; }

		private void OnTriggerEnter2D(Collider2D collision)
		{
			((ITriggerRepeater2D)this).OnTriggerEnter?.Invoke(collision);
		}

		private void OnTriggerStay2D(Collider2D collision)
		{
			((ITriggerRepeater2D)this).OnTriggerStay?.Invoke(collision);
		}

		private void OnTriggerExit2D(Collider2D collision)
		{
			((ITriggerRepeater2D)this).OnTriggerExit?.Invoke(collision);
		}
	}
}