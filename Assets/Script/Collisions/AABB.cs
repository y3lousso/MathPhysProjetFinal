using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MathsPhys
{
    public class AABB : MonoBehaviour
    {
        public float maxPosX;
        public float minPosX;
        public float maxPosY;
        public float minPosY;
        public float maxPosZ;
        public float minPosZ;

        // Use this for initialization
        public void Calculate(Rectangle3D rectangle3D)
        {            
            Vector3 position = rectangle3D.currentPosition;
            float sizeX = rectangle3D.size.x;
            float sizeY = rectangle3D.size.y;
            float sizeZ = rectangle3D.size.z;

            float conversion = Mathf.PI/180;
            float thetaX = (conversion * rectangle3D.currentOrientation.x) % (2 * Mathf.PI);
            float thetaY = (conversion * rectangle3D.currentOrientation.y) % (2 * Mathf.PI);
            float thetaZ = (conversion * rectangle3D.currentOrientation.z) % (2 * Mathf.PI);
 
            // Matrix ZXY for unity marche pas
            //Matrix3x3 rotationMatrix = MathsUtility.RotationMatrixZ(thetaZ)*MathsUtility.RotationMatrixX(thetaX) * MathsUtility.RotationMatrixY(thetaY) ;

            // Matrix XYZ 
            Matrix3x3 rotationMatrix = MathsUtility.RotationMatrixX(thetaX) * MathsUtility.RotationMatrixY(thetaY) * MathsUtility.RotationMatrixZ(thetaZ);

            Vector3 point1 = rotationMatrix * new Vector3(sizeX / 2, sizeY / 2, sizeZ / 2); 
            Vector3 point2 = rotationMatrix * new Vector3(sizeX / 2, -sizeY / 2, sizeZ / 2); 
            Vector3 point3 = rotationMatrix * new Vector3(-sizeX / 2, sizeY / 2, sizeZ / 2); 
            Vector3 point4 = rotationMatrix * new Vector3(-sizeX / 2, -sizeY / 2, sizeZ / 2);

            /*Debug.Log("point1 x : " + point1.x + " y : " + point1.y + " z : " + point1.z);
            Debug.Log("point2 x : " + point2.x + " y : " + point2.y + " z : " + point2.z);
            Debug.Log("point3 x : " + point3.x + " y : " + point3.y + " z : " + point3.z);
            Debug.Log("point4 x : " + point4.x + " y : " + point4.y + " z : " + point4.z);*/


            float projectionX = Mathf.Max(Mathf.Abs(point1.x), Mathf.Abs(point2.x), Mathf.Abs(point3.x), Mathf.Abs(point4.x));
           // Debug.Log("Proj X : " + projectionX);
            float projectionY = Mathf.Max(Mathf.Abs(point1.y), Mathf.Abs(point2.y), Mathf.Abs(point3.y), Mathf.Abs(point4.y));
            float projectionZ = Mathf.Max(Mathf.Abs(point1.z), Mathf.Abs(point2.z), Mathf.Abs(point3.z), Mathf.Abs(point4.z));
            Debug.Log("Proj X : " + projectionZ);

            maxPosX = position.x + projectionX;
            minPosX = position.x - projectionX;
            maxPosY = position.y + projectionY;
            minPosY = position.y - projectionY;
            maxPosZ = position.z + projectionZ;
            minPosZ = position.z - projectionZ;
        }

        // Use this for initialization
        public void Calculate(Sphere sphere)
        {
            Vector3 position = sphere.currentPosition;
            float radius = sphere.radius;
            maxPosX = position.x + radius;
            minPosX = position.x - radius;
            maxPosY = position.y + radius;
            minPosY = position.y - radius;
            maxPosZ = position.z + radius;
            minPosZ = position.z - radius;
        }
    }
}
