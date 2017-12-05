using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MySphereCollider : MyCollider {

	public Vector3 localCenter;
	public float radius = 1f;

    public override void CalculateInertiaTensor()
    {
        float rCarre = transform.localScale.x * transform.localScale.x;
        float J = (2f / 5) * rb.masse * rCarre;

        inertiaTensor = new MyMatrix3x3( new float[,] {
            { J, 0.0f, 0.0f },
            { 0.0f, J, 0.0f },
            { 0.0f, 0.0f, J } });
    }

    public override CollisionData isColliding (MySphereCollider c) {
		Vector3 dist = (c.transform.position + c.localCenter) - (transform.position + localCenter);

		if (dist.magnitude < radius + c.radius) {
			CollisionData cd = new CollisionData();

			cd.contactPoint = dist / 2;
            cd.n = Vector3.Normalize(cd.contactPoint);
            return cd;
		}
		return null;
	}

	public override CollisionData isColliding (MyAABBCollider c) {
		Vector3 closestPoint = localCenter - c.localCenter;
		closestPoint.Normalize ();
		closestPoint *= radius;

		Vector3 AABBCenter = c.transform.position + c.localCenter;

		bool overLapX = closestPoint.x > AABBCenter.x - c.size.x && closestPoint.x < AABBCenter.x + c.size.x;
		bool overLapY = closestPoint.y > AABBCenter.y - c.size.y && closestPoint.y < AABBCenter.y + c.size.y;
		bool overLapZ = closestPoint.z > AABBCenter.z - c.size.z && closestPoint.z < AABBCenter.z + c.size.z;

		if (overLapX && overLapY && overLapZ) {
			CollisionData cd = new CollisionData();

			cd.contactPoint = (c.localCenter - localCenter) / 2;
            // NEED CHANGE : detect which cube face normal is colliding
            cd.n = Vector3.Normalize(cd.contactPoint);
            return cd;
		}
		return null;
	}

    public override CollisionData isColliding(MyOBBCollider c)
    {
		CollisionData cd = c.isColliding(this);

		if (cd != null) {
			cd.contactPoint = -cd.contactPoint;
		}

		return cd;
    }

    /*
	 * Draw
	 */
    void OnDrawGizmos() {
		Gizmos.color = new Color (0f, 1f, 0f, 1f);

		Gizmos.DrawWireSphere (transform.position + localCenter, radius);       
	}
}
