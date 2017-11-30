using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MySphereCollider : MyCollider {

	public Vector3 center;
	public float radius = 1f;

	public override CollisionData isColliding (MySphereCollider c) {
		Vector3 dist = (c.transform.position + c.center) - (transform.position + center);

		if (dist.magnitude < radius + c.radius) {
			CollisionData cd = new CollisionData();

			cd.contactPoint = dist / 2;

			return cd;
		}
		return null;
	}

	public override CollisionData isColliding (MyAABBCollider c) {
		Vector3 closestPoint = center - c.center;
		closestPoint.Normalize ();
		closestPoint *= radius;

		Vector3 AABBCenter = c.transform.position + c.center;

		bool overLapX = closestPoint.x > AABBCenter.x - c.size.x && closestPoint.x < AABBCenter.x + c.size.x;
		bool overLapY = closestPoint.y > AABBCenter.y - c.size.y && closestPoint.y < AABBCenter.y + c.size.y;
		bool overLapZ = closestPoint.z > AABBCenter.z - c.size.z && closestPoint.z < AABBCenter.z + c.size.z;

		if (overLapX && overLapY && overLapZ) {
			CollisionData cd = new CollisionData();

			cd.contactPoint = (c.center - center) / 2;

			return cd;
		}
		return null;
	}

	/*
	 * Draw
	 */
	void OnDrawGizmos() {
		Gizmos.color = new Color (0f, 1f, 0f, 1f);

		Gizmos.DrawWireSphere (transform.position + center, radius);
	}
}
