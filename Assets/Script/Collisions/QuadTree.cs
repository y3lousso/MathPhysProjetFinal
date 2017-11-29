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

		public List<BaseObject> _objects = new List<BaseObject>();

		public void Init (List<BaseObject> o) {
            for (int j = 0 ; j < o.Count ; j++) { 
				if (o [j].nextFramePosition.x < position.x + size.x / 2 && o [j].nextFramePosition.x > position.x - size.x / 2 &&
				    o [j].nextFramePosition.y < position.y + size.y / 2 && o [j].nextFramePosition.y > position.y - size.y / 2 &&
				    o [j].nextFramePosition.z < position.z + size.z / 2 && o [j].nextFramePosition.z > position.z - size.z / 2) {

                    _objects.Add (o[j]);
				}
			}
		}
        
		public void UpdateSpaceDistribution () {
			// Create subdivisions if needed
			if ((childrens == null || childrens.Length == 0) && _objects.Count > nbMax) {
				childrens = new QuadTree[8];

				for (int i = 0 ; i < 8 ; i++) {
					childrens [i] = Instantiate (new GameObject (), transform).AddComponent<QuadTree> ();
					childrens [i].parent = this;
					childrens [i].position = position + size / 4 * cubeSubdiv [i];
					childrens [i].size = size / 2;
					childrens [i].Init (_objects);
				}
			} 
			// Destroy subdivision if not needed anymore
			else if (childrens != null && childrens.Length > 0 && _objects.Count < nbMin) {
				for (int j = childrens.Length - 1; j >= 0; j--) { 
					Destroy (childrens[j].gameObject);
				}

				childrens = null;
			}

			// Update Childrens
			if (childrens != null && childrens.Length > 0) {
				for (int i = 0; i < 8; i++) {
					childrens [i].UpdateSpaceDistribution();
				}
			} 
			// Update objects collisions
			else {
                SpacePartition spacePartition = new SpacePartition();
                spacePartition.GetObjects().AddRange(_objects);
                CollisionSystem.instance.AddSpacePartition(spacePartition);
			}

			// Handle objects leaving quadtree
			for (int j = _objects.Count-1; j >= 0 ; j--) { 
				if (_objects[j].nextFramePosition.x > position.x + size.x / 2 || _objects[j].nextFramePosition.x < position.x - size.x / 2 ||
                    _objects[j].nextFramePosition.y > position.y + size.y / 2 || _objects[j].nextFramePosition.y < position.y - size.y / 2 ||
                    _objects[j].nextFramePosition.z > position.z + size.z / 2 || _objects[j].nextFramePosition.z < position.z - size.z / 2) {

					if (parent != null)
						parent.AssignObject (_objects[j]);

                    _objects.RemoveAt (j);
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

                    _objects.Add (o);
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

    public class SpacePartition
    {
        private List<BaseObject> _object = new List<BaseObject>();

        public List<BaseObject> GetObjects()
        {
            return _object;       
        }

        public BaseObject GetObjectByIndex(int index)
        {
            return _object[index];
        }
    }
}
