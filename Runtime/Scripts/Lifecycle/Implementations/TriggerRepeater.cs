using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BOC.BTagged
{
	class TriggerRepeater : MonoBehaviour, ITriggerRepeater
	{
		Action<Collider> ITriggerRepeater.OnTriggerEnter { get; set; }
		Action<Collider> ITriggerRepeater.OnTriggerExit { get; set; }
		Action<Collider> ITriggerRepeater.OnTriggerStay { get; set; }

		private void OnTriggerEnter(Collider collision)
		{
			((ITriggerRepeater)this).OnTriggerEnter?.Invoke(collision);
		}

		private void OnTriggerStay(Collider collision)
		{
			((ITriggerRepeater)this).OnTriggerStay?.Invoke(collision);
		}

		private void OnTriggerExit(Collider collision)
		{
			((ITriggerRepeater)this).OnTriggerExit?.Invoke(collision);
		}
	}
}