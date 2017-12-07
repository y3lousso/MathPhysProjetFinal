using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MyVector3
{
    public static MyVector3 Zero = NewZero();
	public static MyVector3 One = NewOne();
	public static MyVector3 Up = new MyVector3(0, 1, 0);
	public static MyVector3 Left = new MyVector3(1, 0, 0);
	public static MyVector3 forward = new MyVector3(0, 0, 1);

    public float x;
    public float y;
    public float z;

	public float magnitude { get { return Mathf.Sqrt (x * x + y * y + z * z); } }

    public MyVector3()
    {
        x = 0.0f;
        y = 0.0f;
        z = 0.0f;
    }

    public MyVector3(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public MyVector3(float xyz)
    {
        this.x = xyz;
        this.y = xyz;
        this.z = xyz;
    }

    public MyVector3(MyVector3 v)
    {
        this.x = v.x;
        this.y = v.y;
        this.z = v.z;
    }

    public static MyVector3 NewZero()
    {
        return new MyVector3(0.0f);
    }

    public static MyVector3 NewOne()
    {
        return new MyVector3(1.0f);
    }
   

    public static MyVector3 operator +(MyVector3 v1, MyVector3 v2)
    {           
        return new MyVector3(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
    }

    public static MyVector3 operator -(MyVector3 v1, MyVector3 v2)
    {
        return new MyVector3(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
    }

    public static MyVector3 operator -(MyVector3 v)
    {
        return new MyVector3(-v.x, -v.y, -v.z);
    }

    public static MyVector3 operator *(MyVector3 v, float scalar)
    {
        return new MyVector3(v.x * scalar, v.y * scalar, v.z * scalar);
    }

	public static MyVector3 operator *(float scalar, MyVector3 v)
	{
		return new MyVector3(v.x * scalar, v.y * scalar, v.z * scalar);
	}

	public static MyVector3 operator *(MyVector3 v1, MyVector3 v2)
	{
		return new MyVector3(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
	}

    public static MyVector3 operator /(MyVector3 v, float scalar)
    {
        return new MyVector3(v.x / scalar, v.y / scalar, v.z / scalar);
    }

    public static bool operator ==(MyVector3 v1, MyVector3 v2)
    {
        return v1.x == v2.x && v1.y == v2.y && v1.z == v2.z;
    }

    public static bool operator !=(MyVector3 v1, MyVector3 v2)
    {
        return v1.x != v2.x || v1.y != v2.y || v1.z != v2.z;
    }

    public override bool Equals(object v1)
    {  
       return (object)this == v1;  
    }  

    public override int GetHashCode()
    {
        return 0;
    }

    // Cast from MyVector to Unity Vector3 
    public static implicit operator Vector3(MyVector3 vector)
    {
        return new Vector3(vector.x, vector.y, vector.z);
    }

    // Cast from Unity Vector3 to MyVector3
    public static implicit operator MyVector3(Vector3 vector)
    {
        return new MyVector3(vector.x, vector.y, vector.z);
    }

    public static float DotProduct(MyVector3 v1, MyVector3 v2)
    {
        return v1.x * v2.x + v1.y * v2.y + v1.z * v2.z;
    }

    public static MyVector3 CrossProduct(MyVector3 v1, MyVector3 v2)
    {
        return new MyVector3(v1.y * v2.z - v1.z * v2.y, v1.z * v2.x - v1.x * v2.z, v1.x * v2.y - v1.y * v2.x);
    }

	public static MyVector3 Clamp(MyVector3 v1, MyVector3 v2, MyVector3 v3)
	{
		return new MyVector3(Mathf.Clamp(v1.x, v2.x, v3.x), Mathf.Clamp(v1.y, v2.y, v3.y), Mathf.Clamp(v1.z, v2.z, v3.z));
	}

    public float Get(int index)
    {
        if (index == 0)
            return x;
        else if (index == 1)
            return y;
        else if (index == 2)
            return z;
        else
            throw new System.Exception("Incorrect index.");
    }

    public MyVector3 Add(MyVector3 v)
    {
        x += v.x;
        y += v.y;
        z += v.z;
        return this;
    }

    public float DistanceTo(MyVector3 v)
    {
        float dx = this.x - v.x;
        float dy = this.y - v.y;
        float dz = this.z - v.z;
        return (float)Mathf.Sqrt(dx * dx + dy * dy + dz * dz);
    }

    public float Size()
    {
        return DistanceTo(MyVector3.Zero);
    }

    public MyVector3 Normalize()
    {
        float size = Size();
        this.x /= size;
        this.y /= size;
        this.z /= size;
        return this;
    }

	public float Magnitude()
	{
		return Mathf.Sqrt (x*x + y*y + z*z);
	}

    public MyVector3 Clone()
    {
        return new MyVector3(this);
    }

    public override string ToString()
    {
        return "(" + x + ", " + y + ", " + z + ")";
    }
}
