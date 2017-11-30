using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MathsPhys
{ 
    public class Matrix3x3 : Matrix
    {
        public Matrix3x3()
            : base(3, 3)
        {
        }

        public Matrix3x3(float[,] matrix)
            : base(matrix)
        {
            if (rows != 3 || cols != 3)
            {
                // throw new ArgumentException();
                Debug.LogError("Error");
            }
        }

        public static Matrix3x3 I()
        {
            return new Matrix3x3(new float[,] {
        { 1.0f, 0.0f, 0.0f },
        { 0.0f, 1.0f, 0.0f },
        { 0.0f, 0.0f, 1.0f } });
        }

        public static Vector3 operator *(Matrix3x3 Matrix3x3, Vector3 v)
        {
            float[,] m = Matrix3x3.matrix;
            return new Vector3(
                m[0, 0] * v.x + m[0, 1] * v.y + m[0, 2] * v.z,
                m[1, 0] * v.x + m[1, 1] * v.y + m[1, 2] * v.z,
                m[2, 0] * v.x + m[2, 1] * v.y + m[2, 2] * v.z);
        }

        public static Matrix3x3 operator *(Matrix3x3 mat1, Matrix3x3 mat2)
        {
            float[,] m1 = mat1.matrix;
            float[,] m2 = mat2.matrix;
            float[,] m3 = new float[3, 3];
            m3[0, 0] = m1[0, 0] * m2[0, 0] + m1[0, 1] * m2[1, 0] + m1[0, 2] * m2[2, 0];
            m3[0, 1] = m1[0, 0] * m2[0, 1] + m1[0, 1] * m2[1, 1] + m1[0, 2] * m2[2, 1];
            m3[0, 2] = m1[0, 0] * m2[0, 2] + m1[0, 1] * m2[1, 2] + m1[0, 2] * m2[2, 2];
            m3[1, 0] = m1[1, 0] * m2[0, 0] + m1[1, 1] * m2[1, 0] + m1[1, 2] * m2[2, 0];
            m3[1, 1] = m1[1, 0] * m2[0, 1] + m1[1, 1] * m2[1, 1] + m1[1, 2] * m2[2, 1];
            m3[1, 2] = m1[1, 0] * m2[0, 2] + m1[1, 1] * m2[1, 2] + m1[1, 2] * m2[2, 2];
            m3[2, 0] = m1[2, 0] * m2[0, 0] + m1[2, 1] * m2[1, 0] + m1[2, 2] * m2[2, 0];
            m3[2, 1] = m1[2, 0] * m2[0, 1] + m1[2, 1] * m2[1, 1] + m1[2, 2] * m2[2, 1];
            m3[2, 2] = m1[2, 0] * m2[0, 2] + m1[2, 1] * m2[1, 2] + m1[2, 2] * m2[2, 2];
            return new Matrix3x3(m3);
        }

        public static Matrix3x3 operator *(Matrix3x3 m, float scalar)
        {
            return new Matrix3x3(Multiply(m, scalar));
        }

    } 

}