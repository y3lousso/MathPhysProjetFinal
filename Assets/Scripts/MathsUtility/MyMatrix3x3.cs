using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MyMatrix3x3 : MyMatrix
{
    public MyMatrix3x3()
        : base(3, 3)
    {
    }

    public MyMatrix3x3(float[,] matrix)
        : base(matrix)
    {
        if (rows != 3 || cols != 3)
        {
            // throw new ArgumentException();
            Debug.LogError("Error");
        }
    }

    public static MyMatrix3x3 I()
    {
        return new MyMatrix3x3(new float[,] {
    { 1.0f, 0.0f, 0.0f },
    { 0.0f, 1.0f, 0.0f },
    { 0.0f, 0.0f, 1.0f } });
    }

    public MyMatrix3x3 Invert()
    {
        float det = this.matrix[0, 0] * (this.matrix[1, 1] * this.matrix[2, 2] - this.matrix[2, 1] * this.matrix[1, 2]) -
         this.matrix[0, 1] * (this.matrix[1, 0] * this.matrix[2, 2] - this.matrix[1, 2] * this.matrix[2, 0]) +
         this.matrix[0, 2] * (this.matrix[1, 0] * this.matrix[2, 1] - this.matrix[1, 1] * this.matrix[2, 0]);

        float invdet = 1 / det;

        float[,] inverse = new float[3, 3];

        inverse[0, 0] = (this.matrix[1, 1] * this.matrix[2, 2] - this.matrix[2, 1] * this.matrix[1, 2]) * invdet;
        inverse[0, 1] = (this.matrix[0, 2] * this.matrix[2, 1] - this.matrix[0, 1] * this.matrix[2, 2]) * invdet;
        inverse[0, 2] = (this.matrix[0, 1] * this.matrix[1, 2] - this.matrix[0, 2] * this.matrix[1, 1]) * invdet;
        inverse[1, 0] = (this.matrix[1, 2] * this.matrix[2, 0] - this.matrix[1, 0] * this.matrix[2, 2]) * invdet;
        inverse[1, 1] = (this.matrix[0, 0] * this.matrix[2, 2] - this.matrix[0, 2] * this.matrix[2, 0]) * invdet;
        inverse[1, 2] = (this.matrix[1, 0] * this.matrix[0, 2] - this.matrix[0, 0] * this.matrix[1, 2]) * invdet;
        inverse[2, 0] = (this.matrix[1, 0] * this.matrix[2, 1] - this.matrix[2, 0] * this.matrix[1, 1]) * invdet;
        inverse[2, 1] = (this.matrix[2, 0] * this.matrix[0, 1] - this.matrix[0, 0] * this.matrix[2, 1]) * invdet;
        inverse[2, 2] = (this.matrix[0, 0] * this.matrix[1, 1] - this.matrix[1, 0] * this.matrix[0, 1]) * invdet;

        return new MyMatrix3x3(inverse);
    }

    public static MyVector3 operator *(MyMatrix3x3 MyMatrix3x3, MyVector3 v)
    {
        float[,] m = MyMatrix3x3.matrix;
        return new MyVector3(
            m[0, 0] * v.x + m[0, 1] * v.y + m[0, 2] * v.z,
            m[1, 0] * v.x + m[1, 1] * v.y + m[1, 2] * v.z,
            m[2, 0] * v.x + m[2, 1] * v.y + m[2, 2] * v.z);
    }

    public static MyMatrix3x3 operator *(MyMatrix3x3 mat1, MyMatrix3x3 mat2)
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
        return new MyMatrix3x3(m3);
    }

    public static MyMatrix3x3 operator *(MyMatrix3x3 m, float scalar)
    {
        return new MyMatrix3x3(Multiply(m, scalar));
    }

} 

