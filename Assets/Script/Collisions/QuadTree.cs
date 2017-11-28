using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MathsPhys {
	public class QuadTree : MonoBehaviour {

		static Vector3[] cubeSubdiv = new Vector3[] {
			new Vector3 (-1f, -1f, -1f),
			new Vector3 (-1f, 1f, -1f),
			new Vector3 (1f, 1f, -1f),
			new Vector3 (1f, -1f, -1f),
			new Vector3 (-1f, -1f, 1f),
			new Vector3 (-1f, 1f, 1f),
			new Vector3 (1f, 1f, 1f),
			new Vector3 (1f, -1f, 1f)
		};

		public Vector3 position = new Vector3();
		public Vector3 size = new Vector3();
		public QuadTree[] childrens;

		public List<BaseObject> objects;

		public void Init (List<BaseObject> o) {
			objects = o;
		}

		public void UpdateCollisions () {
			if (childrens != null && childrens.Length > 10) {
				childrens = new QuadTree[8];

				for (int i = 0 ; i < 8 ; i++) {
					childrens [i] = Instantiate (new GameObject (), transform).AddComponent<QuadTree> ();
					childrens [i].position = position + size / 4 * cubeSubdiv [i];
					childrens [i].size = size / 2;
					childrens [i].Init (objects);
				}

				foreach (BaseObject o in objects) {
					// add objects to the sub quad
				}
			}

			foreach (QuadTree quad in childrens) {
				quad.UpdateCollisions ();
			}

		}

		void OnDrawGizmos() {
			if (childrens == null || childrens.Length == 0) {
				Gizmos.color = new Color (1.0f, 0f, 0f, 0.4f);

				Gizmos.DrawWireCube (position, size);
			}
		}
	}
}
