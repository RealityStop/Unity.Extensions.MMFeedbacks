using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BOC.BTagged
{
	class DestroyRepeater : MonoBehaviour, IDestroyRepeater
	{
		Action<GameObject> IDestroyRepeater.OnDestroy { get; set; }


		private void OnDestroy()
		{
			((IDestroyRepeater)this).OnDestroy?.Invoke(gameObject);    
		}
	}
}