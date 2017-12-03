using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionEngine : MonoBehaviour {

	public bool resetTransformsOnCollision = false;
	public bool rejectionForceOnCollision = false;

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
			// set to true if you have static objects
			if (resetTransformsOnCollision) {
				// Reset to last posisitons
				c1.transform.position = c1.rb.lastPosition;
				c2.transform.position = c2.rb.lastPosition;

				// Reset to last rotations
				c1.transform.rotation = c1.rb.lastRotation;
				c2.transform.rotation = c2.rb.lastRotation;
			}

			// Estimate resulting velocities
			float vi = (c1.rb.velocity.magnitude * (c1.rb.masse - c2.rb.masse) + (c2.rb.velocity.magnitude * 2 * c2.rb.masse) / (c1.rb.masse + c2.rb.masse));
			float vj = (c2.rb.velocity.magnitude * (c2.rb.masse - c1.rb.masse) + (c1.rb.velocity.magnitude * 2 * c1.rb.masse) / (c1.rb.masse + c2.rb.masse));

			// Estimate angular velocity
			c1.rb.angVelocity = Vector3.Cross (cd.contactPoint, c1.rb.velocity * c1.rb.masse) * 360/6.28f;
			c2.rb.angVelocity = Vector3.Cross (-cd.contactPoint, c2.rb.velocity * c2.rb.masse) * 360/6.28f;

			// Draw contact point for debug
			Debug.DrawLine (c1.transform.position, c1.transform.position + cd.contactPoint, Color.blue);
			Debug.DrawLine (c2.transform.position, c2.transform.position - cd.contactPoint, Color.blue);

			// Set velocity direction
			Vector3 velDir = cd.contactPoint;
			velDir.Normalize ();

			// Apply velocity
			c1.rb.velocity = vi * -velDir;
			c2.rb.velocity = vj * velDir;

			if (rejectionForceOnCollision) {
				c1.rb.forces.Add (-velDir);
				c2.rb.forces.Add (velDir);
			}

			// tranfer gravity on contact otherwise stacked object won't fall if bottom one has no gravity
			if (c1.rb.useGravity)
				c2.rb.forces.Add(new Vector3(0, -c1.rb.gravity * c1.rb.masse, 0));

			if (c2.rb.useGravity)
				c1.rb.forces.Add(new Vector3(0, -c2.rb.gravity * c2.rb.masse, 0));

			// if one is static it stops the other
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
