using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MyRigidBody))]

public abstract class MyCollider : MonoBehaviour {

	public MyRigidBody rb;
    public MyMatrix3x3 inertiaTensor;

    // Use this for initialization
    void Start () {
		rb = GetComponent<MyRigidBody> ();
	}

	public CollisionData isColliding (MyCollider c) {
		if (c is MySphereCollider)
			return isColliding ((MySphereCollider)c);
		if (c is MyAABBCollider)
			return isColliding ((MyAABBCollider)c);
        if (c is MyOBBCollider)
            return isColliding((MyOBBCollider)c);
		
		return null;
	}

    public abstract void CalculateInertiaTensor();

    public abstract CollisionData isColliding (MySphereCollider c);
	public abstract CollisionData isColliding (MyAABBCollider c);
    public abstract CollisionData isColliding (MyOBBCollider c);
}

public class CollisionData {
	public Vector3 contactPoint;
    public Vector3 n; // normal vector
}
