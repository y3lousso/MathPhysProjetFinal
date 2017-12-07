using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MySphereCollider : MyCollider {

	public MyVector3 localCenter;
	public float radius = 1f;

    public override void CalculateInertiaTensor()
    {
		float rCarre = myTransform.localScale.x * myTransform.localScale.x;
        float J = (2f / 5) * rb.masse * rCarre;

        inertiaTensor = new MyMatrix3x3( new float[,] {
            { J, 0.0f, 0.0f },
            { 0.0f, J, 0.0f },
            { 0.0f, 0.0f, J } });
    }

    public override CollisionData isColliding (MySphereCollider c) {
		MyVector3 dist = (c.myTransform.position + c.localCenter) - (myTransform.position + localCenter);

		if (dist.magnitude < radius + c.radius) {
			CollisionData cd = new CollisionData();

			cd.contactPoint = dist / 2;
			cd.n = cd.contactPoint.Normalize();
            return cd;
		}
		return null;
	}

	public override CollisionData isColliding (MyAABBCollider c) {
		MyVector3 closestPoint = localCenter - c.localCenter;
		closestPoint = closestPoint.Normalize ();
		closestPoint *= radius;

		MyVector3 AABBCenter = c.myTransform.position + c.localCenter;

		bool overLapX = closestPoint.x > AABBCenter.x - c.size.x && closestPoint.x < AABBCenter.x + c.size.x;
		bool overLapY = closestPoint.y > AABBCenter.y - c.size.y && closestPoint.y < AABBCenter.y + c.size.y;
		bool overLapZ = closestPoint.z > AABBCenter.z - c.size.z && closestPoint.z < AABBCenter.z + c.size.z;

		if (overLapX && overLapY && overLapZ) {
			CollisionData cd = new CollisionData();

			cd.contactPoint = (c.localCenter - localCenter) / 2;
            // NEED CHANGE : detect which cube face normal is colliding
			cd.n = cd.contactPoint.Normalize();
            return cd;
		}
		return null;
	}

    public override CollisionData isColliding(MyOBBCollider c)
    {
		CollisionData cd = c.isColliding(this);

		if (cd != null) {
			cd.contactPoint = -cd.contactPoint;
			cd.n = -cd.n;
		}

		return cd;
    }

    /*
	 * Draw
	 */
	public override void OnDrawGizmos() {
		base.OnDrawGizmos ();

		radius = transform.localScale.x/2;

		Gizmos.color = new Color (0f, 1f, 0f, 1f);

		Gizmos.DrawWireSphere (transform.position + (Vector3)localCenter, radius);

        

    }
}
