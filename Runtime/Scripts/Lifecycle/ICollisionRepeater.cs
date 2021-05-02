using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BOC.BTagged
{
	public interface ICollisionRepeater
	{
		Action<Collision> OnCollisionEnter { get; set; }
		Action<Collision> OnCollisionExit { get; set; }
		Action<Collision> OnCollisionStay { get; set; }
	}
}