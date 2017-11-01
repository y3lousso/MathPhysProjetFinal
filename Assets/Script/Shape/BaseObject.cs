using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MathsPhys
{
    public class BaseObject : MonoBehaviour
    {

        public Vector3 currentPosition;
        public Vector3 nextFramePosition;

        public Vector3 currentOrientation;
        public Vector3 nextFrameOrientation;

        public Vector3 scale;

        public Vector3 velocity;

        // pour tester
        public Vector3 rotationRate;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void CalculateNextFramePositionOrientation(float deltaTime)
        {
            // Calculate NextFramePosition
            nextFramePosition = currentPosition + velocity * deltaTime;

            // Calculate NextFrameOrientation
            // pour tester
            nextFrameOrientation = currentOrientation + rotationRate * deltaTime;

        }

        public void ApplyNextFramePositionOrientation()
        {
            currentPosition = nextFramePosition;
            transform.position = new UnityEngine.Vector3(currentPosition.x, currentPosition.y, currentPosition.z);

            currentOrientation = nextFrameOrientation;
            transform.rotation = MathsUtility.GetQuaternionFromEulerAngle(currentOrientation.x, currentOrientation.y, currentOrientation.z);
        }

        public void ApplyRotationToTransform()
        {
            Vector3 eulerAngles;
            // eulerAngles = GetEulerAngleFromMatrice(baseObject.rotation);
            //   baseObject.transform.rotation.SetEulerAngles(eulerAngles.x, eulerAngles.y, eulerAngles.z); 

        }

        public void ApplyScaleToTransform()
        {
            //baseObject.transform.localScale = baseObject.scale;

        }

        public Vector3 GetPositionFromEditor()
        {
            return transform.position;
        }

        public Quaternion GetOrientationFromEditor()
        {
            return transform.rotation;
        }
    }
}

