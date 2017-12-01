using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionEngine : MonoBehaviour {

	public List<Transform> _objects = new List<Transform>();

	protected virtual void Start () {
		foreach (MyCollider c in GameObject.FindObjectsOfType<MyCollider> ())
			_objects.Add(c.transform);
	}

	protected virtual void Init (ref List<Transform> o) {
		_objects = o;
	}

	// Update is called once per frame
	protected virtual void FixedUpdate () {
		for (int i = 0 ; i < _objects.Count ; i++) {
			for (int j = i+1 ; j < _objects.Count ; j++) {
				HandleCollision (_objects [i].GetComponent<MyCollider>(), _objects [j].GetComponent<MyCollider>());
			}
		}
	}

	protected void HandleCollision(MyCollider c1, MyCollider c2) {
		CollisionData cd = c1.isColliding (c2);

		if (cd != null) {
			c1.transform.position = c1.rb.lastPosition;
			c2.transform.position = c2.rb.lastPosition;

			c1.transform.rotation = c1.rb.lastRotation;
			c2.transform.rotation = c2.rb.lastRotation;

			float vi = (c1.rb.velocity.magnitude * (c1.rb.masse - c2.rb.masse) + (c2.rb.velocity.magnitude * 2 * c2.rb.masse) / (c1.rb.masse + c2.rb.masse));
			float vj = (c2.rb.velocity.magnitude * (c2.rb.masse - c1.rb.masse) + (c1.rb.velocity.magnitude * 2 * c1.rb.masse) / (c1.rb.masse + c2.rb.masse));

			c1.rb.angVelocity = Vector3.Cross (cd.contactPoint, c1.rb.velocity * c1.rb.masse) * 360/6.28f;
			c2.rb.angVelocity = Vector3.Cross (-cd.contactPoint, c2.rb.velocity * c2.rb.masse) * 360/6.28f;

			Vector3 velDir = cd.contactPoint;
			velDir.Normalize ();

			c1.rb.velocity = vi * -velDir;
			c2.rb.velocity = vj * velDir;

			if (c1.rb.useGravity)
				c2.rb.forces.Add(new Vector3(0, -c1.rb.gravity * c1.rb.masse, 0));

			if (c2.rb.useGravity)
				c1.rb.forces.Add(new Vector3(0, -c2.rb.gravity * c2.rb.masse, 0));

			if (c1.rb.isStatic) {
				c2.rb.velocity = Vector3.zero;
				c2.rb.angVelocity = Vector3.zero;
			}

			if (c2.rb.isStatic) {
				c1.rb.velocity = Vector3.zero;
				c1.rb.angVelocity = Vector3.zero;
			}

		}
	}
}
