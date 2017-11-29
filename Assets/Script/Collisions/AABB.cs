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
        public void Calculate(Vector3 position, float sizeX, float sizeY, float sizeZ)
        {
            maxPosX = position.x + sizeX;
            minPosX = position.x - sizeX;
            maxPosY = position.y + sizeY;
            minPosY = position.y - sizeY;
            maxPosZ = position.z + sizeZ;
            minPosZ = position.z - sizeZ;
        }

        // Use this for initialization
        public void Calculate(Vector3 position, float radius)
        {
            maxPosX = position.x + radius;
            minPosX = position.x - radius;
            maxPosY = position.y + radius;
            minPosY = position.y - radius;
            maxPosZ = position.z + radius;
            minPosZ = position.z - radius;
        }
    }
}
