using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyRigidBody : MonoBehaviour {

	public MyTransform myTransform;

	public float masse = 1f;

	public float gravity = 9.81f;
	public bool useGravity;
	public bool isStatic;

	public MyVector3 velocity;
	public MyVector3 angVelocity;

	public List<MyVector3> forces = new List<MyVector3>();

	public MyVector3 lastPosition;
	public MyVector3 lastRotation;

	void Start () {
		myTransform = GetComponent<MyTransform> ();

		lastPosition = myTransform.position;
		lastRotation = myTransform.rotation;
	}

	void FixedUpdate () {
		if (useGravity)
			forces.Add(new Vector3(0, -gravity * masse, 0));

		forces.Add(-velocity * 0.05f);

		MyVector3 sum = MyVector3.Zero;
		foreach (MyVector3 f in forces) {
			sum += f;
		}

		forces.Clear ();

		velocity += sum * (Time.fixedDeltaTime / masse);

		angVelocity *= 0.95f;

		lastPosition = myTransform.position;
		lastRotation = myTransform.rotation;

		if (!isStatic) {
			myTransform.Translate (velocity * Time.fixedDeltaTime);
			myTransform.Rotate (angVelocity * 360/6.28f * Time.fixedDeltaTime);
		}
	}

	public virtual void OnDrawGizmos()
	{
		if (myTransform == null)
			myTransform = GetComponent<MyTransform> ();
	}
}
