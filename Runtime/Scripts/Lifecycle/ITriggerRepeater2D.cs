using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BOC.BTagged
{
	public interface ITriggerRepeater2D
	{
		Action<Collider2D> OnTriggerEnter { get; set; }
		Action<Collider2D> OnTriggerExit { get; set; }
		Action<Collider2D> OnTriggerStay { get; set; }
	}
}