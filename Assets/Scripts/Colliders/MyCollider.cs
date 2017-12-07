using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MyTransform))]
[RequireComponent(typeof(MyRigidBody))]

public abstract class MyCollider : MonoBehaviour {

	public MyTransform myTransform;
	public MyRigidBody rb;
    public MyMatrix3x3 inertiaTensor = null;

    // Use this for initialization
    void Start () {
		myTransform = GetComponent<MyTransform> ();
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

	public virtual void OnDrawGizmos()
	{
		if (myTransform == null)
			myTransform = GetComponent<MyTransform> ();
		if (rb == null)
			rb = GetComponent<MyRigidBody> ();
	}
}

public class CollisionData {
	public MyVector3 contactPoint;
	public MyVector3 n; // normal vector
}
