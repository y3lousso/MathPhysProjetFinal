using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MathsPhys
{
    public class GameEngine : MonoBehaviour
    {
        public static GameEngine instance;

        int nbFramePerSecond = 60;

        List<BaseObject> _objects;

        CollisionSystem collisionSystem;

        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }
            else
            {
                throw new System.Exception("Can't have multiple game engine instances.");
            }
        }

        // Use this for initialization
        void Start()
        {
            // Init du FixedUpdate à nbFramePerSecond
            Time.fixedDeltaTime = 1f / nbFramePerSecond;

            // Get all object of the scene
            _objects = new List<BaseObject>();
            _objects.AddRange(FindObjectsOfType<BaseObject>());

            // Create all systems
            collisionSystem = new CollisionSystem();

            // Init all systems
            collisionSystem.StartSystem();
            
            // Init objects and their components
            foreach (BaseObject obj in _objects)
            {
                obj.Init();
            }

            // Pour tester
            foreach (BaseObject obj in _objects)
            {
                obj.currentPosition = obj.GetPositionFromEditor();
                obj.currentOrientation = MathsUtility.GetEulerAngleFromQuaternion(obj.GetOrientationFromEditor());
                obj.velocity = new Vector3(0, 20, 40);
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
            foreach (BaseObject obj in _objects)
            {
				obj.rotationRate = Vector3.NewZero ();
				obj.UpdateVelocity (Time.fixedDeltaTime);                
            }

            // Detect collision
            collisionSystem.UpdateSystem();

            // Apply new position
            foreach (BaseObject obj in _objects)
            {
                obj.ApplyPositionOrientation();
            }
        }

        public List<BaseObject> GetAllObjects()
        {
            return _objects;
        }
    }

}
