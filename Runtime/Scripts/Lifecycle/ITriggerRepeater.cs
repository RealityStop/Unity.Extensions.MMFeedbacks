using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BOC.BTagged
{
	public interface ITriggerRepeater
	{
		Action<Collider> OnTriggerEnter { get; set; }
		Action<Collider> OnTriggerExit { get; set; }
		Action<Collider> OnTriggerStay { get; set; }
	}
}