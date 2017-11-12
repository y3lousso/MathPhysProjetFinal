using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MathsPhys
{
    public class GameEngine : MonoBehaviour
    {
        int nbFramePerSecond = 60;

        List<BaseObject> objects;

        // Use this for initialization
        void Start()
        {
            // Init du FixedUpdate à nbFramePerSecond
            Time.fixedDeltaTime = 1f/ nbFramePerSecond;

            // On init
            objects = new List<BaseObject>();
            objects.AddRange(FindObjectsOfType<BaseObject>());

            // Pour tester
            foreach (BaseObject obj in objects)
            {
                obj.currentPosition = obj.GetPositionFromEditor();
                obj.currentOrientation = MathsUtility.GetEulerAngleFromQuaternion(obj.GetOrientationFromEditor());
                obj.velocity = new Vector3(Random.RandomRange(-1f,1f), Random.RandomRange(-1f, 1f), Random.RandomRange(-1f, 1f));
            }


            // Convert euler -> quat -> euler : OK
          /*  Vector3 testEulerAngle = new Vector3(30, 25, 66);
            Debug.Log(testEulerAngle);
            Quaternion testQuat = new Quaternion();
            testQuat = MathsUtility.GetQuaternionFromEulerAngle(testEulerAngle.x, testEulerAngle.y, testEulerAngle.z);
            Debug.Log(testQuat);
            testEulerAngle = MathsUtility.GetEulerAngleFromQuaternion(testQuat);
            Debug.Log(testEulerAngle);*/


        }

        // Update is called once per frame
        void FixedUpdate()
        {
            // On fait tourner de manière random
            // Vector3 rotationRateEulerAngle = new Vector3(Random.RandomRange(-89f, 89f), Random.RandomRange(-89f, 89f), Random.RandomRange(-89f, 89f));

            // On update tous les objets suivant la procédure habituelle d'un moteur 
            foreach (BaseObject obj in objects)
            {
				obj.rotationRate = Vector3.NewZero ();
				obj.UpdateVelocity ();

                // Apply
                obj.ApplyNextFramePositionOrientation();
            }
        }
    }

}
