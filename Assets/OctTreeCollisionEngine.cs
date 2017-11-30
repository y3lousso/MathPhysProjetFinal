using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctTreeCollisionEngine : CollisionEngine {

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

	public OctTreeCollisionEngine parent = null;
	public OctTreeCollisionEngine[] childrens;

	protected override void Start() {
		if (transform.parent == null) {
			foreach (MyCollider c in GameObject.FindObjectsOfType<MyCollider> ())
				_objects.Add(c.transform);
		}
	}
	
	protected override void Init (List<Transform> o) {
		for (int j = 0 ; j < o.Count ; j++) { 
			if (o [j].position.x < position.x + size.x / 2 && o [j].position.x > position.x - size.x / 2 &&
				o [j].position.y < position.y + size.y / 2 && o [j].position.y > position.y - size.y / 2 &&
				o [j].position.z < position.z + size.z / 2 && o [j].position.z > position.z - size.z / 2) {

				_objects.Add (o[j]);
			}
		}
	}

	protected override void FixedUpdate () {
		if (transform.parent == null)
			UpdateSpaceDistribution ();
	}

	private void UpdateSpaceDistribution() {
		// Create subdivisions if needed
		if ((childrens == null || childrens.Length == 0) && _objects.Count > nbMax) {
			childrens = new OctTreeCollisionEngine[8];

			for (int i = 0 ; i < 8 ; i++) {
				childrens [i] = Instantiate (new GameObject (), transform).AddComponent<OctTreeCollisionEngine> ();
				childrens [i].parent = this;
				childrens [i].position = position + Vector3.Scale(size / 4, cubeSubdiv [i]);
				childrens [i].size = size / 2;
				childrens [i].nbMax = nbMax;
				childrens [i].nbMin = nbMin;
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
			for (int i = 0 ; i < _objects.Count ; i++) { 
				for (int j = i+1 ; j < _objects.Count ; j++) { 
					HandleCollision (_objects[i].GetComponent<MyCollider>(), _objects[j].GetComponent<MyCollider>());
				}
			}
		}

		// Handle objects leaving quadtree
		for (int j = _objects.Count-1; j >= 0 ; j--) { 
			if (_objects[j].position.x > position.x + size.x / 2 || _objects[j].position.x < position.x - size.x / 2 ||
				_objects[j].position.y > position.y + size.y / 2 || _objects[j].position.y < position.y - size.y / 2 ||
				_objects[j].position.z > position.z + size.z / 2 || _objects[j].position.z < position.z - size.z / 2) {

				if (parent != null)
					parent.AssignObject (_objects[j]);

				_objects.RemoveAt (j);
			}
		}
	}

	/*
	 * Assign object to the correct Quadtree
	 */
	void AssignObject (Transform o) {
		if (childrens == null || childrens.Length == 0) {
			if (o.position.x < position.x + size.x / 2 && o.position.x > position.x - size.x / 2 &&
				o.position.y < position.y + size.y / 2 && o.position.y > position.y - size.y / 2 &&
				o.position.z < position.z + size.z / 2 && o.position.z > position.z - size.z / 2) {

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