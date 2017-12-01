using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MyOBBCollider : MyCollider
{
	public MyVector3 localCenter = MyVector3.Zero;
    public MyVector3[] localAxis = new MyVector3[] { new MyVector3(1,0,0), new MyVector3(0, 1, 0), new MyVector3(0, 0, 1) }; // 0 : x // 1 : y // 2 : z
	public MyVector3 halfExtends = new MyVector3(0.5f,0.5f,0.5f);

    public void FixedUpdate()
    {
        // Update local axis
        Vector3 orientation = transform.rotation.eulerAngles;
        float conversionDegRad = Mathf.PI / 180;

        float thetaX = (conversionDegRad * orientation.x) % (2 * Mathf.PI);
        float thetaY = (conversionDegRad * orientation.y) % (2 * Mathf.PI);
        float thetaZ = (conversionDegRad * orientation.z) % (2 * Mathf.PI);

        // Matrix YXZ else doesn't work : Ry*Rx*Rz * vect because unity is zxy order ...
        MyMatrix3x3 rotationMatrix = MathsUtility.RotationMatrixY(thetaY) * MathsUtility.RotationMatrixX(thetaX) * MathsUtility.RotationMatrixZ(thetaZ);

        localAxis[0] = rotationMatrix * new Vector3(1, 0, 0);
        localAxis[1] = rotationMatrix * new Vector3(0, 1, 0);
        localAxis[2] = rotationMatrix * new Vector3(0, 0, 1);

        // If the form can change size
        halfExtends.x = transform.localScale.x/2;
        halfExtends.y = transform.localScale.y/2;
        halfExtends.z = transform.localScale.z/2;
    }

    public override CollisionData isColliding(MySphereCollider c)
    {
        return null;
    }

    public override CollisionData isColliding(MyAABBCollider c)
    {
       	return null;
    }

    public override CollisionData isColliding(MyOBBCollider c)
    {
        float ra, rb;
        MyMatrix3x3 R = new MyMatrix3x3();
        MyMatrix3x3 AbsR = new MyMatrix3x3();

        CollisionData cd = new CollisionData();

        // Matrice de changement de repère de c vers this
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                R.matrix[i,j] = MyVector3.DotProduct(localAxis[i], c.localAxis[j]);
            }
        }

        // Translation vector translationVector
        MyVector3 translationVector = transform.position - c.transform.position;
        MyVector3 translationLocalVector = new MyVector3();

        // Bring translation into this coordinate frame
        translationLocalVector.x = MyVector3.DotProduct(translationVector, localAxis[0]);
        translationLocalVector.y = MyVector3.DotProduct(translationVector, localAxis[1]);
        translationLocalVector.z = MyVector3.DotProduct(translationVector, localAxis[2]);

        // Add epsilon to avoid parallelism for cross product
        float epsilon = 0.000001f;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                AbsR.matrix[i, j] = Mathf.Abs(R.matrix[i, j]) + epsilon;
            }
        }

        // Test this local axis (X1, Y1, Z1)
        for (int i = 0; i < 3; i++)
        {
            ra = halfExtends.Get(i);
            rb = c.halfExtends.Get(i) * AbsR.matrix[i,0] + c.halfExtends.Get(1) * AbsR.matrix[i,1] + c.halfExtends.Get(2) * AbsR.matrix[i, 2];
            if (Mathf.Abs(translationLocalVector.Get(i)) > ra + rb)
            {
                return null;
            }
        }

        // Test c local axis (X2, Y2, Z2)
        for (int i = 0; i < 3; i++)
        {
            ra = halfExtends.Get(i) * AbsR.matrix[i, 0] + halfExtends.Get(1) * AbsR.matrix[i, 1] + halfExtends.Get(2) * AbsR.matrix[i, 2];
            rb = c.halfExtends.Get(i);
            if (Mathf.Abs(translationLocalVector.Get(0) * R.matrix[0, i] + translationLocalVector.Get(1) * R.matrix[1, i] + translationLocalVector.Get(2) * R.matrix[2, i]) > ra + rb)
            {
                return null;
            }
        }

        // Test axis L = A0 x B0
        ra = halfExtends.Get(1) * AbsR.matrix[2, 0] + halfExtends.Get(2) * AbsR.matrix[1, 0];
        rb = c.halfExtends.Get(1) * AbsR.matrix[0, 2] + c.halfExtends.Get(2) * AbsR.matrix[0, 1];
        if (Mathf.Abs(translationLocalVector.Get(2) * R.matrix[1, 0] - translationLocalVector.Get(1) * R.matrix[2, 0]) > ra + rb)
            return null;

        // Test axis L = A0 x B1
        ra = halfExtends.Get(1) * AbsR.matrix[2, 1] + halfExtends.Get(2) * AbsR.matrix[1, 1];
        rb = c.halfExtends.Get(0) * AbsR.matrix[0, 2] + c.halfExtends.Get(2) * AbsR.matrix[0, 0];
        if (Mathf.Abs(translationLocalVector.Get(2) * R.matrix[1, 1] - translationLocalVector.Get(1) * R.matrix[2, 1]) > ra + rb)
            return null;

        // Test axis L = A0 x B2
        ra = halfExtends.Get(1) * AbsR.matrix[2, 2] + halfExtends.Get(2) * AbsR.matrix[1, 2];
        rb = c.halfExtends.Get(0) * AbsR.matrix[0, 1] + c.halfExtends.Get(1) * AbsR.matrix[0, 0];
        if (Mathf.Abs(translationLocalVector.Get(2) * R.matrix[1, 2] - translationLocalVector.Get(1) * R.matrix[2, 2]) > ra + rb)
            return null;

        // Test axis L = A1 x B0
        ra = halfExtends.Get(0) * AbsR.matrix[2, 0] + halfExtends.Get(2) * AbsR.matrix[0, 0];
        rb = c.halfExtends.Get(1) * AbsR.matrix[1, 2] + c.halfExtends.Get(2) * AbsR.matrix[1, 1];
        if (Mathf.Abs(translationLocalVector.Get(0) * R.matrix[2, 0] - translationLocalVector.Get(2) * R.matrix[0, 0]) > ra + rb)
            return null;

        // Test axis L = A1 x B1
        ra = halfExtends.Get(0) * AbsR.matrix[2, 1] + halfExtends.Get(2) * AbsR.matrix[0, 1];
        rb = c.halfExtends.Get(0) * AbsR.matrix[1, 2] + c.halfExtends.Get(2) * AbsR.matrix[1, 0];
        if (Mathf.Abs(translationLocalVector.Get(0) * R.matrix[2, 1] - translationLocalVector.Get(2) * R.matrix[0, 1]) > ra + rb)
            return null;

        // Test axis L = A1 x B2
        ra = halfExtends.Get(0) * AbsR.matrix[2, 2] + halfExtends.Get(2) * AbsR.matrix[0, 2];
        rb = c.halfExtends.Get(0) * AbsR.matrix[1, 1] + c.halfExtends.Get(1) * AbsR.matrix[1, 0];
        if (Mathf.Abs(translationLocalVector.Get(0) * R.matrix[2, 2] - translationLocalVector.Get(2) * R.matrix[0, 2]) > ra + rb)
            return null;

        // Test axis L = A2 x B0
        ra = halfExtends.Get(0) * AbsR.matrix[1, 0] + halfExtends.Get(1) * AbsR.matrix[0, 0];
        rb = c.halfExtends.Get(1) * AbsR.matrix[2, 2] + c.halfExtends.Get(2) * AbsR.matrix[2, 1];
        if (Mathf.Abs(translationLocalVector.Get(1) * R.matrix[0, 0] - translationLocalVector.Get(0) * R.matrix[1, 0]) > ra + rb)
            return null;

        // Test axis L = A2 x B1
        ra = halfExtends.Get(0) * AbsR.matrix[1, 1] + halfExtends.Get(1) * AbsR.matrix[0, 1];
        rb = c.halfExtends.Get(0) * AbsR.matrix[2, 2] + c.halfExtends.Get(2) * AbsR.matrix[2, 0];
        if (Mathf.Abs(translationLocalVector.Get(1) * R.matrix[0, 1] - translationLocalVector.Get(0) * R.matrix[1, 1]) > ra + rb)
            return null;

        // Test axis L = A2 x B2
        ra = halfExtends.Get(0) * AbsR.matrix[1, 2] + halfExtends.Get(1) * AbsR.matrix[0, 2];
        rb = c.halfExtends.Get(0) * AbsR.matrix[2, 1] + c.halfExtends.Get(1) * AbsR.matrix[2, 0];
        if (Mathf.Abs(translationLocalVector.Get(1) * R.matrix[0, 2] - translationLocalVector.Get(0) * R.matrix[1, 2]) > ra + rb)
            return null;

        // Since no separating axis is found, the OBBs must be intersecting
        // Need to adjust the contact point location
        cd.contactPoint = -translationLocalVector / 2;
        return cd;
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 1f);
        Gizmos.DrawLine(transform.position + (Vector3)localCenter, transform.position + (Vector3)localCenter + (Vector3)localAxis[0]);
        Gizmos.DrawLine(transform.position + (Vector3)localCenter, transform.position + (Vector3)localCenter + (Vector3)localAxis[1]);
        Gizmos.DrawLine(transform.position + (Vector3)localCenter, transform.position + (Vector3)localCenter + (Vector3)localAxis[2]);

		Gizmos.color = new Color(0f, 1f, 0f, 1f);
		Vector3 A = transform.position + (Vector3)localCenter + Vector3.Scale ((Vector3)localAxis [0], (Vector3)halfExtends)
			- Vector3.Scale ((Vector3)localAxis [1], (Vector3)halfExtends)
			- Vector3.Scale ((Vector3)localAxis [2], (Vector3)halfExtends);
		Vector3 B = transform.position + (Vector3)localCenter - Vector3.Scale ((Vector3)localAxis [0], (Vector3)halfExtends)
			- Vector3.Scale ((Vector3)localAxis [1], (Vector3)halfExtends)
			- Vector3.Scale ((Vector3)localAxis [2], (Vector3)halfExtends);

		Vector3 C = transform.position + (Vector3)localCenter - Vector3.Scale ((Vector3)localAxis [0], (Vector3)halfExtends)
			+ Vector3.Scale ((Vector3)localAxis [1], (Vector3)halfExtends)
			- Vector3.Scale ((Vector3)localAxis [2], (Vector3)halfExtends);
		Vector3 D = transform.position + (Vector3)localCenter - Vector3.Scale ((Vector3)localAxis [0], (Vector3)halfExtends)
			- Vector3.Scale ((Vector3)localAxis [1], (Vector3)halfExtends)
			- Vector3.Scale ((Vector3)localAxis [2], (Vector3)halfExtends);

		Vector3 E = transform.position + (Vector3)localCenter - Vector3.Scale ((Vector3)localAxis [0], (Vector3)halfExtends)
			- Vector3.Scale ((Vector3)localAxis [1], (Vector3)halfExtends)
			+ Vector3.Scale ((Vector3)localAxis [2], (Vector3)halfExtends);
		Vector3 F = transform.position + (Vector3)localCenter - Vector3.Scale ((Vector3)localAxis [0], (Vector3)halfExtends)
			- Vector3.Scale ((Vector3)localAxis [1], (Vector3)halfExtends)
			- Vector3.Scale ((Vector3)localAxis [2], (Vector3)halfExtends);

		Gizmos.DrawLine (A, B);
		Gizmos.DrawLine (A + Vector3.Scale ((Vector3)localAxis [1], (Vector3)halfExtends*2), B + Vector3.Scale ((Vector3)localAxis [1], (Vector3)halfExtends*2));
		Gizmos.DrawLine (A + Vector3.Scale ((Vector3)localAxis [1], (Vector3)halfExtends*2) + Vector3.Scale ((Vector3)localAxis [2], (Vector3)halfExtends*2), B + Vector3.Scale ((Vector3)localAxis [1], (Vector3)halfExtends*2) + Vector3.Scale ((Vector3)localAxis [2], (Vector3)halfExtends*2));
		Gizmos.DrawLine (A + Vector3.Scale ((Vector3)localAxis [2], (Vector3)halfExtends*2), B + Vector3.Scale ((Vector3)localAxis [2], (Vector3)halfExtends*2));

		Gizmos.DrawLine (C, D);
		Gizmos.DrawLine (C + Vector3.Scale ((Vector3)localAxis [0], (Vector3)halfExtends*2), D + Vector3.Scale ((Vector3)localAxis [0], (Vector3)halfExtends*2));
		Gizmos.DrawLine (C + Vector3.Scale ((Vector3)localAxis [0], (Vector3)halfExtends*2) + Vector3.Scale ((Vector3)localAxis [2], (Vector3)halfExtends*2), D + Vector3.Scale ((Vector3)localAxis [0], (Vector3)halfExtends*2) + Vector3.Scale ((Vector3)localAxis [2], (Vector3)halfExtends*2));
		Gizmos.DrawLine (C + Vector3.Scale ((Vector3)localAxis [2], (Vector3)halfExtends*2), D + Vector3.Scale ((Vector3)localAxis [2], (Vector3)halfExtends*2));

		Gizmos.DrawLine (E, F);
		Gizmos.DrawLine (E + Vector3.Scale ((Vector3)localAxis [0], (Vector3)halfExtends*2), F + Vector3.Scale ((Vector3)localAxis [0], (Vector3)halfExtends*2));
		Gizmos.DrawLine (E + Vector3.Scale ((Vector3)localAxis [0], (Vector3)halfExtends*2) + Vector3.Scale ((Vector3)localAxis [1], (Vector3)halfExtends*2), F + Vector3.Scale ((Vector3)localAxis [0], (Vector3)halfExtends*2) + Vector3.Scale ((Vector3)localAxis [1], (Vector3)halfExtends*2));
		Gizmos.DrawLine (E + Vector3.Scale ((Vector3)localAxis [1], (Vector3)halfExtends*2), F + Vector3.Scale ((Vector3)localAxis [1], (Vector3)halfExtends*2));


    }
}
