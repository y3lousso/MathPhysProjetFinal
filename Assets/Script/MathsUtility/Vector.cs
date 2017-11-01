using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MathsPhys
{
    public class Vector3
    {
        public static Vector3 Zero = NewZero();
        public static Vector3 One = NewOne();

        public float x;
        public float y;
        public float z;

        public Vector3()
        {
            x = 0.0f;
            y = 0.0f;
            z = 0.0f;
        }

        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector3(float xyz)
        {
            this.x = xyz;
            this.y = xyz;
            this.z = xyz;
        }

        public Vector3(Vector3 v)
        {
            this.x = v.x;
            this.y = v.y;
            this.z = v.z;
        }

        public static Vector3 NewZero()
        {
            return new Vector3(0.0f);
        }

        public static Vector3 NewOne()
        {
            return new Vector3(1.0f);
        }

        public float DotProduct(Vector3 other)
        {
            return x * other.x + y * other.y + z * other.z;
        }

        public static Vector3 operator +(Vector3 v1, Vector3 v2)
        {           
            return new Vector3(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
        }

        public static Vector3 operator -(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
        }

        public static Vector3 operator -(Vector3 v)
        {
            return new Vector3(-v.x, -v.y, -v.z);
        }

        public static Vector3 operator *(Vector3 v, float scalar)
        {
            return new Vector3(v.x * scalar, v.y * scalar, v.z * scalar);
        }

        public static Vector3 operator /(Vector3 v, float scalar)
        {
            return new Vector3(v.x / scalar, v.y / scalar, v.z / scalar);
        }

        public static bool operator ==(Vector3 v1, Vector3 v2)
        {
            return v1.x == v2.x && v1.y == v2.y && v1.z == v2.z;
        }

        public static bool operator !=(Vector3 v1, Vector3 v2)
        {
            return v1.x != v2.x || v1.y != v2.y || v1.z != v2.z;
        }

        // Cast from MathsPhys Vect3 to Unity Vect3 
        public static implicit operator UnityEngine.Vector3(Vector3 vector)
        {
            return new UnityEngine.Vector3(vector.x, vector.y, vector.z);
        }

        // Cast from Unity Vect3 to MathsPhys Vect3
        public static implicit operator Vector3(UnityEngine.Vector3 vector)
        {
            return new Vector3(vector.x, vector.y, vector.z);
        }

        public static Vector3 CrossProduct(Vector3 a, Vector3 b)
        {
            return new Vector3(a.y * b.z - a.z * b.y, a.z * b.x - a.x * b.z, a.x * b.y - a.y * b.x);
        }

        public Vector3 Add(Vector3 v)
        {
            x += v.x;
            y += v.y;
            z += v.z;
            return this;
        }

        public float DistanceTo(Vector3 v)
        {
            float dx = this.x - v.x;
            float dy = this.y - v.y;
            float dz = this.z - v.z;
            return (float)Mathf.Sqrt(dx * dx + dy * dy + dz * dz);
        }

        public float Size()
        {
            return DistanceTo(Vector3.Zero);
        }

        public Vector3 Normalize()
        {
            float size = Size();
            this.x /= size;
            this.y /= size;
            this.z /= size;
            return this;
        }

        public Vector3 Clone()
        {
            return new Vector3(this);
        }

        public override string ToString()
        {
            return "(" + x + ", " + y + ", " + z + ")";
        }
    }
}