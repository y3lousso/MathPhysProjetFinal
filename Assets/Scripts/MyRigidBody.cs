using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyRigidBody : MonoBehaviour {

	public float masse = 1f;

	public float gravity = 9.81f;
	public bool useGravity;
	public bool isStatic;

	public Vector3 velocity;
	public Vector3 angVelocity;

	public List<Vector3> forces = new List<Vector3>();

	public Transform lastTransform;

	void Start () {
		lastTransform = transform;
	}

	void FixedUpdate () {
		if (useGravity)
			forces.Add(new Vector3(0, -gravity * masse, 0));

		forces.Add(-velocity * 0.05f);

		Vector3 sum = Vector3.zero;
		foreach (Vector3 f in forces) {
			sum += f;
		}

		forces.Clear ();

		velocity += sum * (Time.fixedDeltaTime / masse);

		angVelocity *= 0.95f;

		if (!isStatic) {
			lastTransform = transform;

			transform.Translate (transform.InverseTransformDirection(velocity * Time.fixedDeltaTime));
			transform.Rotate (angVelocity * Time.fixedDeltaTime);
		}
	}

}
