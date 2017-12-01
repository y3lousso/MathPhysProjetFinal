using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyAABBCollider : MyCollider
{
    public Vector3 localCenter;
    public Vector3 size = Vector3.one;
	[Space(10)]
	public bool adapatAABB;
    public Vector3 aabbCalculatedRotation;

    // For quick gizmo
	private Vector3 point1;
	private Vector3 point2;
	private Vector3 point3;
	private Vector3 point4;
	private Vector3 point5;
	private Vector3 point6;
	private Vector3 point7;
	private Vector3 point8;

    public void FixedUpdate()
    {
		if (adapatAABB && aabbCalculatedRotation != transform.rotation.eulerAngles)
        {
            CalculateAABB();
        }
    }

    public void CalculateAABB()
    {
        Vector3 orientation = transform.rotation.eulerAngles;
        float conversionDegRad = Mathf.PI / 180;

        float thetaX = (conversionDegRad * orientation.x) % (2 * Mathf.PI);
        float thetaY = (conversionDegRad * orientation.y) % (2 * Mathf.PI);
        float thetaZ = (conversionDegRad * orientation.z) % (2 * Mathf.PI);

        // Matrix YXZ else doesn't work : Ry*Rx*Rz * vect because unity is zxy order ...
        MyMatrix3x3 rotationMatrix = MathsUtility.RotationMatrixY(thetaY) * MathsUtility.RotationMatrixX(thetaX) *MathsUtility.RotationMatrixZ(thetaZ);
       
        point1 = rotationMatrix * new MyVector3(transform.localScale.x, transform.localScale.y , transform.localScale.z )/2;
        point2 = rotationMatrix * new MyVector3(transform.localScale.x , transform.localScale.y , -transform.localScale.z )/2;
        point3 = rotationMatrix * new MyVector3(transform.localScale.x , -transform.localScale.y , transform.localScale.z )/2;
        point4 = rotationMatrix * new MyVector3(transform.localScale.x , -transform.localScale.y , -transform.localScale.z )/2;
        point5 = rotationMatrix * new MyVector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z)/2;
        point6 = rotationMatrix * new MyVector3(-transform.localScale.x, transform.localScale.y, -transform.localScale.z)/2;
        point7 = rotationMatrix * new MyVector3(-transform.localScale.x, -transform.localScale.y, transform.localScale.z)/2;
        point8 = rotationMatrix * new MyVector3(-transform.localScale.x, -transform.localScale.y, -transform.localScale.z)/2;

        

        float maxX = Mathf.Max(point1.x, point2.x, point3.x, point4.x, point5.x, point6.x, point7.x, point8.x) - localCenter.x;
        float minX = Mathf.Min(point1.x, point2.x, point3.x, point4.x, point5.x, point6.x, point7.x, point8.x) - localCenter.x;
        float maxY = Mathf.Max(point1.y, point2.y, point3.y, point4.y, point5.y, point6.y, point7.y, point8.y) - localCenter.y;
        float minY = Mathf.Min(point1.y, point2.y, point3.y, point4.y, point5.y, point6.y, point7.y, point8.y) - localCenter.y;
        float maxZ = Mathf.Max(point1.z, point2.z, point3.z, point4.z, point5.z, point6.z, point7.z, point8.z) - localCenter.z;
        float minZ = Mathf.Min(point1.z, point2.z, point3.z, point4.z, point5.z, point6.z, point7.z, point8.z) - localCenter.z;

		size.x = Mathf.Lerp(size.x, maxX + Mathf.Abs(minX), Time.deltaTime);
		size.y = Mathf.Lerp(size.y, maxY + Mathf.Abs(minY), Time.deltaTime);
		size.z = Mathf.Lerp(size.z, maxZ + Mathf.Abs(minZ), Time.deltaTime);

        aabbCalculatedRotation = transform.rotation.eulerAngles;
    }


    public override CollisionData isColliding(MySphereCollider c)
    {
		CollisionData cd = c.isColliding(this);

		if (cd != null) {
			cd.contactPoint = -cd.contactPoint;
		}

		return cd;
    }

    public override CollisionData isColliding(MyAABBCollider c)
    {
        Vector3 center1 = transform.position + localCenter;
        Vector3 center2 = c.transform.position + c.localCenter;

        bool overlapX = center1.x + size.x / 2 <= center2.x + c.size.x / 2 && center1.x + size.x / 2 >= center2.x - c.size.x / 2;
        overlapX |= center1.x - size.x / 2 <= center2.x + c.size.x / 2 && center1.x - size.x / 2 >= center2.x - c.size.x / 2;
        overlapX |= center2.x + c.size.x / 2 <= center1.x + size.x / 2 && center2.x + c.size.x / 2 >= center1.x - size.x / 2;
        overlapX |= center2.x - c.size.x / 2 <= center1.x + size.x / 2 && center2.x - c.size.x / 2 >= center1.x - size.x / 2;

        bool overlapY = center1.y + size.y / 2 <= center2.y + c.size.y / 2 && center1.y + size.y / 2 >= center2.y - c.size.y / 2;
        overlapY |= center1.y - size.y / 2 <= center2.y + c.size.y / 2 && center1.y - size.y / 2 >= center2.y - c.size.y / 2;
        overlapY |= center2.y + c.size.y / 2 <= center1.y + size.y / 2 && center2.y + c.size.y / 2 >= center1.y - size.y / 2;
        overlapY |= center2.y - c.size.y / 2 <= center1.y + size.y / 2 && center2.y - c.size.y / 2 >= center1.y - size.y / 2;

        bool overlapZ = center1.z + size.z / 2 <= center2.z + c.size.z / 2 && center1.z + size.z / 2 >= center2.z - c.size.z / 2;
        overlapZ |= center1.z - size.z / 2 <= center2.z + c.size.z / 2 && center1.z - size.z / 2 > center2.z - c.size.z / 2;
        overlapZ |= center2.z + c.size.z / 2 <= center1.z + size.z / 2 && center2.z + c.size.z / 2 >= center1.z - size.z / 2;
        overlapZ |= center2.z - c.size.z / 2 <= center1.z + size.z / 2 && center2.z - c.size.z / 2 >= center1.z - size.z / 2;

        if (overlapX && overlapY && overlapZ)
        {
            CollisionData cd = new CollisionData();

            cd.contactPoint = (center2 - center1) / 2;

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
    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0f, 1f, 0f, 1f);

        Gizmos.DrawWireCube(transform.position + localCenter, size);

        Gizmos.color = new Color(1f, 0f, 0f, 1f);
        Gizmos.DrawWireCube(transform.position + point1, new Vector3(.1f, .1f, .1f));
        Gizmos.DrawWireCube(transform.position + point2, new Vector3(.1f, .1f, .1f));
        Gizmos.DrawWireCube(transform.position + point3, new Vector3(.1f, .1f, .1f));
        Gizmos.DrawWireCube(transform.position + point4, new Vector3(.1f, .1f, .1f));
        Gizmos.DrawWireCube(transform.position + point5, new Vector3(.1f, .1f, .1f));
        Gizmos.DrawWireCube(transform.position + point6, new Vector3(.1f, .1f, .1f));
        Gizmos.DrawWireCube(transform.position + point7, new Vector3(.1f, .1f, .1f));
        Gizmos.DrawWireCube(transform.position + point8, new Vector3(.1f, .1f, .1f));
    }
}


