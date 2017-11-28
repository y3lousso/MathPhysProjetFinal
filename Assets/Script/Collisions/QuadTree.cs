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

		public int nbMax = 20;
		public int nbMin = 10;

		public QuadTree parent = null;
		public QuadTree[] childrens;

		public List<BaseObject> objects = new List<BaseObject>();

		public void Init (List<BaseObject> o) {
			for (int j = 0 ; j < o.Count ; j++) { 
				if (o [j].nextFramePosition.x < position.x + size.x / 2 && o [j].nextFramePosition.x > position.x - size.x / 2 &&
				    o [j].nextFramePosition.y < position.y + size.y / 2 && o [j].nextFramePosition.y > position.y - size.y / 2 &&
				    o [j].nextFramePosition.z < position.z + size.z / 2 && o [j].nextFramePosition.z > position.z - size.z / 2) {

					objects.Add (o[j]);
				}
			}
		}

		public void UpdateCollisions () {
			// Create subdivisions if needed
			if ((childrens == null || childrens.Length == 0) && objects.Count > nbMax) {
				childrens = new QuadTree[8];

				for (int i = 0 ; i < 8 ; i++) {
					childrens [i] = Instantiate (new GameObject (), transform).AddComponent<QuadTree> ();
					childrens [i].parent = this;
					childrens [i].position = position + size / 4 * cubeSubdiv [i];
					childrens [i].size = size / 2;
					childrens [i].Init (objects);
				}
			} 
			// Destroy subdivision if not needed anymore
			else if (childrens != null && childrens.Length > 0 && objects.Count < nbMin) {
				for (int j = childrens.Length - 1; j >= 0; j--) { 
					Destroy (childrens[j].gameObject);
				}

				childrens = null;
			}

			// Update Childrens
			if (childrens != null && childrens.Length > 0) {
				for (int i = 0; i < 8; i++) {
					childrens [i].UpdateCollisions ();
				}
			} 
			// Update objects collisions
			else {
				for (int j = objects.Count - 1; j >= 0; j--) {
					for (int k = objects.Count - 1; k > j; k--) { 
						// Collision entre objects[j] et objects[k] BITE
					}
				}
			}

			// Handle objects leaving quadtree
			for (int j = objects.Count-1; j >= 0 ; j--) { 
				if (objects [j].nextFramePosition.x > position.x + size.x / 2 || objects [j].nextFramePosition.x < position.x - size.x / 2 ||
					objects [j].nextFramePosition.y > position.y + size.y / 2 || objects [j].nextFramePosition.y < position.y - size.y / 2 ||
					objects [j].nextFramePosition.z > position.z + size.z / 2 || objects [j].nextFramePosition.z < position.z - size.z / 2) {

					if (parent != null)
						parent.AssignObject (objects [j]);
					
					objects.RemoveAt (j);
				}
			}
		}

		/*
		 * Assign object to the correct Quadtree
		 */
		void AssignObject (BaseObject o) {
			if (childrens == null || childrens.Length == 0) {
				if (o.nextFramePosition.x < position.x + size.x / 2 && o.nextFramePosition.x > position.x - size.x / 2 &&
					o.nextFramePosition.y < position.y + size.y / 2 && o.nextFramePosition.y > position.y - size.y / 2 &&
					o.nextFramePosition.z < position.z + size.z / 2 && o.nextFramePosition.z > position.z - size.z / 2) {

					objects.Add (o);
				}
			} else {
				for (int i = 0; i < 8; i++) {
					childrens [i].AssignObject (o);
				}
			}
		}

		/*
		 * Draw the Quadtree limits
		 */
		void OnDrawGizmos() {
			if (childrens == null || childrens.Length == 0) {
				Gizmos.color = new Color (1.0f, 0f, 0f, 0.4f);

				Gizmos.DrawWireCube (position, size);
			}
		}
	}
}
