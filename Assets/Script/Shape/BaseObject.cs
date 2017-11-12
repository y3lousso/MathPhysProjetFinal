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

		//forces 
		public float masse = 1;
		public bool UseGravity;
		public List<Vector3> _forces = new List<Vector3>();

		public float drag = 0.3f;
		public float StaticDrag = 0.8f;


        // Use this for initialization
        void Start()
        {
			if (masse == 0)
				masse = 0.0001f;
        }

        // Update is called once per frame
		public void UpdateVelocity()
        {
			if (UseGravity) {
				AddForce (new Vector3(0, 1, 0) * -9.81f);
			}
			AddForce (velocity * (velocity.Size() > 0.1f ? -StaticDrag : -drag));

			Vector3 sum = new Vector3(0, 0, 0);
			foreach (Vector3 f in _forces) {
				sum += f;
			}
			 
			_forces.Clear ();

			velocity += sum / masse;
			CalculateNextFramePositionOrientation (Time.fixedDeltaTime);
        }

		public void AddForce(Vector3 f) {
			_forces.Add (f);
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

