using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MathsPhys
{
    public class GameEngine : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            float[,] mat = new float[,]{
                { 5, 5, 5, 5 },
                { 5, 5, 5, 5 },
                { 5, 5, 5, 5 },
                { 5, 5, 5, 5 },
            };
            Matrix4x4 matCube = new Matrix4x4(mat);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
