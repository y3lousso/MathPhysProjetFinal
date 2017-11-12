using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MathsPhys {
	public class Force : Vector3 {

		public Force(): base()
		{
			
		}

		public Force(float x, float y, float z): base(x, y, z)
		{
			
		}

		public Force(float xyz): base(xyz)
		{
			
		}

		public Force(Vector3 v): base(v)
		{
			
		}
	}
}