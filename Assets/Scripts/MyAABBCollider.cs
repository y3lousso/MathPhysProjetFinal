using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyAABBCollider : MyCollider
{
    public Vector3 localCenter;
    public Vector3 size = Vector3.one;
    public Vector3 aabbCalculatedRotation;

    public void FixedUpdate()
    {
        if (aabbCalculatedRotation != transform.rotation.eulerAngles)
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

        // Matrix XYZ 
        MyMatrix3x3 rotationMatrix = MathsUtility.RotationMatrixX(thetaX) * MathsUtility.RotationMatrixY(thetaY) * MathsUtility.RotationMatrixZ(thetaZ);
       
        Vector3 point1 = rotationMatrix * new MyVector3(transform.localScale.x, transform.localScale.y , transform.localScale.z );
        Vector3 point2 = rotationMatrix * new MyVector3(transform.localScale.x , transform.localScale.y , -transform.localScale.z );
        Vector3 point3 = rotationMatrix * new MyVector3(transform.localScale.x , -transform.localScale.y , transform.localScale.z );
        Vector3 point4 = rotationMatrix * new MyVector3(transform.localScale.x , -transform.localScale.y , -transform.localScale.z );
        Vector3 point5 = rotationMatrix * new MyVector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        Vector3 point6 = rotationMatrix * new MyVector3(-transform.localScale.x, transform.localScale.y, -transform.localScale.z);
        Vector3 point7 = rotationMatrix * new MyVector3(-transform.localScale.x, -transform.localScale.y, transform.localScale.z);
        Vector3 point8 = rotationMatrix * new MyVector3(-transform.localScale.x, -transform.localScale.y, -transform.localScale.z);

        float maxX = Mathf.Max(point1.x, point2.x, point3.x, point4.x, point5.x, point6.x, point7.x, point8.x) - localCenter.x;
        float minX = Mathf.Min(point1.x, point2.x, point3.x, point4.x, point5.x, point6.x, point7.x, point8.x) - localCenter.x;
        float maxY = Mathf.Max(point1.y, point2.y, point3.y, point4.y, point5.y, point6.y, point7.y, point8.y) - localCenter.y;
        float minY = Mathf.Min(point1.y, point2.y, point3.y, point4.y, point5.y, point6.y, point7.y, point8.y) - localCenter.y;
        float maxZ = Mathf.Max(point1.z, point2.z, point3.z, point4.z, point5.z, point6.z, point7.z, point8.z) - localCenter.z;
        float minZ = Mathf.Min(point1.z, point2.z, point3.z, point4.z, point5.z, point6.z, point7.z, point8.z) - localCenter.z;

        size.x = maxX ;
        size.y = maxY ;
        size.z = maxZ ;

        aabbCalculatedRotation = transform.rotation.eulerAngles;
    }


    public override CollisionData isColliding(MySphereCollider c)
    {
        return c.isColliding(this);
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

    /*
        * Draw
        */
    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0f, 1f, 0f, 1f);

        Gizmos.DrawWireCube(transform.position + localCenter, size);
    }
}


