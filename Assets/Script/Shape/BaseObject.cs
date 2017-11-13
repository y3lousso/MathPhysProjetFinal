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
        public List<Vector3> _forces = new List<Vector3>();
        public float masse = 1;

        // if the base object is affected by gravity
        public bool UseGravity;
        // realistic value = 9.81 ; but it seems slow from afar
        [Range(1, 100)]
        public float TweekerGravity = 9.81f;
        
        // if the velocity goes under this limit, velocity approximated to 0
        [Range(0.0001f, 1)]
        public float VelocityLowLimit = 0.1f;

        [Range(0,100)]
		public float TweekerAirDrag = 1f;


        // Use this for initialization
        void Start()
        {
			if (masse == 0)
				masse = 0.0001f;
        }

        // Update is called once per frame
		public void UpdateVelocity(float deltaTime)
        {
			if (UseGravity) {
				AddForce (new Vector3(0, -TweekerGravity * masse, 0));
			}
            if(velocity.Size() > VelocityLowLimit)
            {
                AddForce(-velocity * TweekerAirDrag);
            }else
            {
                velocity = Vector3.NewZero();
            }


			Vector3 sum = Vector3.NewZero();
            foreach (Vector3 f in _forces) {
				sum += f;
			}
			 
			_forces.Clear ();
            
            velocity += sum * (deltaTime / masse);
            //Debug.Log(gameObject.name + " : " + velocity);
            CalculateNextFramePositionOrientation (deltaTime);
        }

		public void AddForce(Vector3 f) {
			_forces.Add (f);
		}

        private void CalculateNextFramePositionOrientation(float deltaTime)
        {
            // Calculate NextFramePosition
            nextFramePosition = currentPosition + velocity * deltaTime;

            // Calculate NextFrameOrientation
            nextFrameOrientation = currentOrientation + rotationRate * deltaTime;
        }

        public void ApplyPositionOrientation()
        {
            currentPosition = nextFramePosition;
            transform.position = new UnityEngine.Vector3(currentPosition.x, currentPosition.y, currentPosition.z);

            currentOrientation = nextFrameOrientation;
            transform.rotation = MathsUtility.GetQuaternionFromEulerAngle(currentOrientation.x, currentOrientation.y, currentOrientation.z);
        }

        public void ChangeScaleToTransform(Vector3 newScale)
        {
            transform.localScale = newScale;
            scale = newScale;
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

