using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyAABBCollider : MyCollider {

	public Vector3 center;
	public Vector3 size = Vector3.one;

	public override CollisionData isColliding (MySphereCollider c) {
		return c.isColliding(this);
	}

	public override CollisionData isColliding (MyAABBCollider c) {
		Vector3 center1 = transform.position + center;
		Vector3 center2 = c.transform.position + c.center;

		bool overlapX = center1.x + size.x/2 <= center2.x + c.size.x/2 && center1.x + size.x/2 >= center2.x - c.size.x/2;
		overlapX |= center1.x - size.x/2 <= center2.x + c.size.x/2 && center1.x - size.x/2 >= center2.x - c.size.x/2;
		overlapX |= center2.x + c.size.x/2 <= center1.x + size.x/2 && center2.x + c.size.x/2 >= center1.x - size.x/2;
		overlapX |= center2.x - c.size.x/2 <= center1.x + size.x/2 && center2.x - c.size.x/2 >= center1.x - size.x/2;

		bool overlapY = center1.y + size.y/2 <= center2.y + c.size.y/2 && center1.y + size.y/2 >= center2.y - c.size.y/2;
		overlapY |= center1.y - size.y/2 <= center2.y + c.size.y/2 && center1.y - size.y/2 >= center2.y - c.size.y/2;
		overlapY |= center2.y + c.size.y/2 <= center1.y + size.y/2 && center2.y + c.size.y/2 >= center1.y - size.y/2;
		overlapY |= center2.y - c.size.y/2 <= center1.y + size.y/2 && center2.y - c.size.y/2 >= center1.y - size.y/2;

		bool overlapZ = center1.z + size.z/2 <= center2.z + c.size.z/2 && center1.z + size.z/2 >= center2.z - c.size.z/2;
		overlapZ |= center1.z - size.z/2 <= center2.z + c.size.z/2 && center1.z - size.z/2 > center2.z - c.size.z/2;
		overlapZ |= center2.z + c.size.z/2 <= center1.z + size.z/2 && center2.z + c.size.z/2 >= center1.z - size.z/2;
		overlapZ |= center2.z - c.size.z/2 <= center1.z + size.z/2 && center2.z - c.size.z/2 >= center1.z - size.z/2;

		if (overlapX && overlapY && overlapZ) {
			CollisionData cd = new CollisionData();

			cd.contactPoint = (center2 - center1) / 2;

			return cd;
		}
		return null;
	}

	/*
	 * Draw
	 */
	void OnDrawGizmos() {
		Gizmos.color = new Color (0f, 1f, 0f, 1f);

		Gizmos.DrawWireCube (transform.position + center, size);
	}
}
