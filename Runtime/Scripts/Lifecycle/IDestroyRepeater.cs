using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BOC.BTagged
{
	public interface IDestroyRepeater
	{
		Action<GameObject> OnDestroy { get; set; }
	}
}