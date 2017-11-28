using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MathsPhys {
	public class collisionData {
		public Vector3[] contactPoints;
	}

	public class Collider : MonoBehaviour {

		public virtual collisionData isColliding(Collider col) {
			return null;
		}
	}
}
