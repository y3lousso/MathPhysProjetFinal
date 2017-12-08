using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionEngine : MonoBehaviour {

    public bool impulseResponse = true;

    [Range(0,1)]
    public float elasticityCoef = 0.5f;

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
			// Draw contact point for debug
			Debug.DrawLine ((Vector3)c1.myTransform.position, (Vector3)c1.myTransform.position + (Vector3)cd.n, Color.blue);
			Debug.DrawLine ((Vector3)c2.myTransform.position, (Vector3)c2.myTransform.position - (Vector3)cd.n, Color.blue);

			if (impulseResponse) {
				if (c1.inertiaTensor == null)
					c1.CalculateInertiaTensor ();
				if (c2.inertiaTensor == null)
					c2.CalculateInertiaTensor ();

				MyVector3 radius1 = cd.contactPoint - c1.myTransform.position;
				MyVector3 v1 = c1.rb.velocity;// + c1.rb.angVelocity * radius1.Magnitude ();
				MyVector3 radius2 = cd.contactPoint - c2.myTransform.position;
				MyVector3 v2 = c2.rb.velocity;// + c2.rb.angVelocity * radius2.Magnitude ();

				MyVector3 deltaVelocityAtImpactPoint = v1 - v2;

				float impulsionKNumerator = (elasticityCoef + 1) * MyVector3.DotProduct (deltaVelocityAtImpactPoint, cd.n);
				float denominatorMasse = /*cd.n */ ((1 / c1.rb.masse) + (1 / c2.rb.masse));
				//MyVector3 denominatorInertie1 = MyVector3.CrossProduct (c1.inertiaTensor.Invert () * MyVector3.CrossProduct (radius1, cd.n), radius1);
				//MyVector3 denominatorInertie2 = MyVector3.CrossProduct (c2.inertiaTensor.Invert () * MyVector3.CrossProduct (radius2, cd.n), radius2);

				//float impulsionKDenominator = MyVector3.DotProduct (denominatorMasse + denominatorInertie1 + denominatorInertie2, cd.n);
				float impulsionKDenominator = denominatorMasse * MyVector3.DotProduct (cd.n, cd.n);

				if (impulsionKDenominator == 0)
					return;

				float impulsionK = impulsionKNumerator / impulsionKDenominator;

				c1.rb.velocity -= impulsionK * cd.n / c1.rb.masse; 
				c2.rb.velocity += impulsionK * cd.n / c2.rb.masse;
				c1.rb.angVelocity += (c1.inertiaTensor.Invert () * MyVector3.CrossProduct (radius1 * impulsionK, cd.n));
				c2.rb.angVelocity += (c2.inertiaTensor.Invert () * MyVector3.CrossProduct (radius2 * impulsionK, cd.n));
			} else {
            	// Estimate resulting velocities
	            float vi = (c1.rb.velocity.magnitude * (c1.rb.masse - c2.rb.masse) + (c2.rb.velocity.magnitude * 2 * c2.rb.masse) / (c1.rb.masse + c2.rb.masse));
				float vj = (c2.rb.velocity.magnitude * (c2.rb.masse - c1.rb.masse) + (c1.rb.velocity.magnitude * 2 * c1.rb.masse) / (c1.rb.masse + c2.rb.masse));

				// Estimate angular velocity
				c1.rb.angVelocity = MyVector3.CrossProduct (cd.contactPoint, c1.rb.velocity * c1.rb.masse) * 360/6.28f;
				c2.rb.angVelocity = MyVector3.CrossProduct (-cd.contactPoint, c2.rb.velocity * c2.rb.masse) * 360/6.28f;

				// Apply velocity
				c1.rb.velocity = vi * cd.n;
				c2.rb.velocity = vj * -cd.n;
			}

            // tranfer gravity on contact otherwise stacked object won't fall if bottom one has no gravity
            if (c1.rb.useGravity)
				c2.rb.forces.Add(new MyVector3(0, -c1.rb.gravity * c1.rb.masse, 0));

			if (c2.rb.useGravity)
				c1.rb.forces.Add(new MyVector3(0, -c2.rb.gravity * c2.rb.masse, 0));

			// if one is static it stops the other
			if (c1.rb.isStatic) {
				c2.rb.velocity = MyVector3.Zero;
				c2.rb.angVelocity = MyVector3.Zero;
			}

			if (c2.rb.isStatic) {
				c1.rb.velocity = MyVector3.Zero;
				c1.rb.angVelocity = MyVector3.Zero;
			}
		}
	}
}
