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

	[Range(0.1f, 0.9f)]
	public float margin = 0.9f;

	public OctTreeCollisionEngine parent = null;
	public OctTreeCollisionEngine[] childrens;

	protected override void Start() {
		if (transform.parent == null) {
			foreach (MyCollider c in GameObject.FindObjectsOfType<MyCollider> ())
				_objects.Add(c.transform);
		}
	}
	
	protected override void Init (ref List<Transform> o) {
		transform.position = position;

		for (int j = o.Count-1 ; j > 0  ; j--) { 
			if (o [j].position.x < position.x + size.x * margin / 2 && o [j].position.x > position.x - size.x * margin / 2 &&
				o [j].position.y < position.y + size.y * margin / 2 && o [j].position.y > position.y - size.y * margin / 2 &&
				o [j].position.z < position.z + size.z * margin / 2 && o [j].position.z > position.z - size.z * margin / 2) {

				_objects.Add (o[j]);
				o.RemoveAt (j);
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
				GameObject go = new GameObject ();
				go.transform.parent = transform;

				childrens [i] = go.AddComponent<OctTreeCollisionEngine> ();
				childrens [i].parent = this;
				childrens [i].position = position + Vector3.Scale(size / 4, cubeSubdiv [i]);
				childrens [i].size = size / 2;
				childrens [i].nbMax = nbMax;
				childrens [i].nbMin = nbMin;
				childrens [i].margin = margin;
				childrens [i].Init (ref _objects);
			}
		} 
		// Destroy subdivision if not needed anymore
		else if (childrens != null && childrens.Length > 0 && _objects.Count + childrens[0]._objects.Count + childrens[1]._objects.Count + childrens[2]._objects.Count + childrens[3]._objects.Count + childrens[4]._objects.Count + childrens[5]._objects.Count + childrens[6]._objects.Count + childrens[7]._objects.Count < nbMin) {
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
		for (int i = 0 ; i < _objects.Count ; i++) { 
			for (int j = i+1 ; j < _objects.Count ; j++) { 
				HandleCollision (_objects[i].GetComponent<MyCollider>(), _objects[j].GetComponent<MyCollider>());
			}

			if (childrens != null && childrens.Length > 0) {
				for (int k = 0; k < 8; k++) {
					for (int j = i+1 ; j < childrens [k]._objects.Count ; j++) { 
						HandleCollision (_objects[i].GetComponent<MyCollider>(), childrens [k]._objects[j].GetComponent<MyCollider>());
					}
				}
			} 
		}

		// Handle object leaving uncertainty zone
		if (childrens != null && childrens.Length > 0) {
			for (int i = 0; i < _objects.Count; i++) { 
				AssignObject (_objects [i]);
			}
		}

		// Handle objects leaving quadtree
		for (int j = _objects.Count-1; j >= 0 ; j--) { 
			if (_objects[j].position.x > position.x + size.x * margin / 2 || _objects[j].position.x < position.x - size.x * margin / 2 ||
				_objects[j].position.y > position.y + size.y * margin / 2 || _objects[j].position.y < position.y - size.y * margin / 2 ||
				_objects[j].position.z > position.z + size.z * margin / 2 || _objects[j].position.z < position.z - size.z * margin / 2) {

				if (parent != null)
					parent.AssignObject (_objects [j]);

				_objects.RemoveAt (j);
			}
		}
	}

	/*
	 * Assign object to the correct Quadtree
	 */
	void AssignObject (Transform o) {
		if (childrens == null || childrens.Length == 0) {
			if (o.position.x < position.x + size.x * margin / 2 && o.position.x > position.x - size.x * margin / 2 &&
				o.position.y < position.y + size.y * margin / 2 && o.position.y > position.y - size.y * margin / 2 &&
				o.position.z < position.z + size.z * margin / 2 && o.position.z > position.z - size.z * margin / 2) {

				if (!_objects.Contains(o))
					_objects.Add (o);

				if (parent._objects.Contains(o))
					parent._objects.Remove (o);
			}
		} else {
			if (!_objects.Contains(o))
				_objects.Add (o);

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
			Gizmos.color = new Color (1.0f, 0f, 0f, 0.6f);

			Gizmos.DrawWireCube (position, size);

			Gizmos.color = new Color (0f, 0f, 1f, 0.4f);
			Gizmos.DrawWireCube (position, size * margin);
		}
	}
}