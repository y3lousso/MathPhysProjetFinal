using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyAABBCollider : MyCollider
{
	public MyVector3 localCenter = MyVector3.Zero;
	public MyVector3 size = MyVector3.One;
	[Space(10)]
	public bool adaptAABB = true;
	public MyVector3 aabbCalculatedRotation;

    // For quick gizmo
	private MyVector3 point1 = MyVector3.Zero;
	private MyVector3 point2 = MyVector3.Zero;
	private MyVector3 point3 = MyVector3.Zero;
	private MyVector3 point4 = MyVector3.Zero;
	private MyVector3 point5 = MyVector3.Zero;
	private MyVector3 point6 = MyVector3.Zero;
	private MyVector3 point7 = MyVector3.Zero;
	private MyVector3 point8 = MyVector3.Zero;

    public void FixedUpdate()
    {
		if (adaptAABB && aabbCalculatedRotation != myTransform.rotation)
        {
            CalculateAABB();
        }
    }

    public override void CalculateInertiaTensor()
    {
		float sizeXCarre = myTransform.localScale.x * myTransform.localScale.x;
		float sizeYCarre = myTransform.localScale.y * myTransform.localScale.y;
		float sizeZCarre = myTransform.localScale.z * myTransform.localScale.z;

        // box aligned inertia tensor
        MyMatrix3x3 alignedInertiaTensor = new MyMatrix3x3(new float[,] {
            { rb.masse * (sizeYCarre+sizeZCarre), 0.0f, 0.0f },
            { 0.0f, rb.masse * (sizeXCarre+sizeZCarre), 0.0f },
            { 0.0f, 0.0f, rb.masse * (sizeXCarre+sizeYCarre) } });

        // Update local axis
		MyVector3 orientation = transform.rotation.eulerAngles;
        float conversionDegRad = Mathf.PI / 180;

        float thetaX = (conversionDegRad * orientation.x) % (2 * Mathf.PI);
        float thetaY = (conversionDegRad * orientation.y) % (2 * Mathf.PI);
        float thetaZ = (conversionDegRad * orientation.z) % (2 * Mathf.PI);
        // Matrix YXZ else doesn't work : Ry*Rx*Rz * vect because unity is zxy order ...
        MyMatrix3x3 rotationMatrix = MathsUtility.RotationMatrixY(thetaY) * MathsUtility.RotationMatrixX(thetaX) * MathsUtility.RotationMatrixZ(thetaZ);

        inertiaTensor = rotationMatrix * alignedInertiaTensor;
    }

    public void CalculateAABB()
    {
		MyVector3 orientation = myTransform.rotation;
        float conversionDegRad = Mathf.PI / 180;

        float thetaX = (conversionDegRad * orientation.x) % (2 * Mathf.PI);
        float thetaY = (conversionDegRad * orientation.y) % (2 * Mathf.PI);
        float thetaZ = (conversionDegRad * orientation.z) % (2 * Mathf.PI);

        // Matrix YXZ else doesn't work : Ry*Rx*Rz * vect because unity is zxy order ...
        MyMatrix3x3 rotationMatrix = MathsUtility.RotationMatrixY(thetaY) * MathsUtility.RotationMatrixX(thetaX) *MathsUtility.RotationMatrixZ(thetaZ);
       
		point1 = rotationMatrix * new MyVector3(myTransform.localScale.x, myTransform.localScale.y , myTransform.localScale.z )/2;
		point2 = rotationMatrix * new MyVector3(myTransform.localScale.x , myTransform.localScale.y , -myTransform.localScale.z )/2;
		point3 = rotationMatrix * new MyVector3(myTransform.localScale.x , -myTransform.localScale.y , myTransform.localScale.z )/2;
		point4 = rotationMatrix * new MyVector3(myTransform.localScale.x , -myTransform.localScale.y , -myTransform.localScale.z )/2;
		point5 = rotationMatrix * new MyVector3(-myTransform.localScale.x, myTransform.localScale.y, myTransform.localScale.z)/2;
		point6 = rotationMatrix * new MyVector3(-myTransform.localScale.x, myTransform.localScale.y, -myTransform.localScale.z)/2;
		point7 = rotationMatrix * new MyVector3(-myTransform.localScale.x, -myTransform.localScale.y, myTransform.localScale.z)/2;
		point8 = rotationMatrix * new MyVector3(-myTransform.localScale.x, -myTransform.localScale.y, -myTransform.localScale.z)/2;

        

        float maxX = Mathf.Max(point1.x, point2.x, point3.x, point4.x, point5.x, point6.x, point7.x, point8.x) - localCenter.x;
        float minX = Mathf.Min(point1.x, point2.x, point3.x, point4.x, point5.x, point6.x, point7.x, point8.x) - localCenter.x;
        float maxY = Mathf.Max(point1.y, point2.y, point3.y, point4.y, point5.y, point6.y, point7.y, point8.y) - localCenter.y;
        float minY = Mathf.Min(point1.y, point2.y, point3.y, point4.y, point5.y, point6.y, point7.y, point8.y) - localCenter.y;
        float maxZ = Mathf.Max(point1.z, point2.z, point3.z, point4.z, point5.z, point6.z, point7.z, point8.z) - localCenter.z;
        float minZ = Mathf.Min(point1.z, point2.z, point3.z, point4.z, point5.z, point6.z, point7.z, point8.z) - localCenter.z;

		size.x = Mathf.Lerp(size.x, maxX + Mathf.Abs(minX), Time.deltaTime);
		size.y = Mathf.Lerp(size.y, maxY + Mathf.Abs(minY), Time.deltaTime);
		size.z = Mathf.Lerp(size.z, maxZ + Mathf.Abs(minZ), Time.deltaTime);

		aabbCalculatedRotation = myTransform.rotation;
    }


    public override CollisionData isColliding(MySphereCollider c)
    {
		CollisionData cd = c.isColliding(this);

		if (cd != null) {
			cd.contactPoint = -cd.contactPoint;
			cd.n = -cd.n;
		}

		return cd;
    }

    public override CollisionData isColliding(MyAABBCollider c)
    {
		MyVector3 center1 = myTransform.position + localCenter;
		MyVector3 center2 = c.myTransform.position + c.localCenter;

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
			cd.n = -cd.n;
		}

		return cd;
    }

    /*
    * Draw
    */
	public override void OnDrawGizmos()
    {
		base.OnDrawGizmos ();

        Gizmos.color = new Color(0f, 1f, 0f, 1f);

		Gizmos.DrawWireCube(transform.position + (Vector3)localCenter, (Vector3)size);

        Gizmos.color = new Color(1f, 0f, 0f, 1f);
		Gizmos.DrawWireCube(transform.position + (Vector3)point1, new Vector3(.1f, .1f, .1f));
		Gizmos.DrawWireCube(transform.position + (Vector3)point2, new Vector3(.1f, .1f, .1f));
		Gizmos.DrawWireCube(transform.position + (Vector3)point3, new Vector3(.1f, .1f, .1f));
		Gizmos.DrawWireCube(transform.position + (Vector3)point4, new Vector3(.1f, .1f, .1f));
		Gizmos.DrawWireCube(transform.position + (Vector3)point5, new Vector3(.1f, .1f, .1f));
		Gizmos.DrawWireCube(transform.position + (Vector3)point6, new Vector3(.1f, .1f, .1f));
		Gizmos.DrawWireCube(transform.position + (Vector3)point7, new Vector3(.1f, .1f, .1f));
		Gizmos.DrawWireCube(transform.position + (Vector3)point8, new Vector3(.1f, .1f, .1f));
    }
}


