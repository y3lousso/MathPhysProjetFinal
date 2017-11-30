using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MathsPhys
{
    public class MyRectangle3DCollider : Collider
    {

        // Use this for initialization
        public override void Init(BaseObject obj)
        {
            base.Init(obj);
            CalculateAABB();
        }

        public override void CalculateAABB()
        {
            aabb.Calculate(((Rectangle3D)baseObject));
           // Debug.Log(baseObject.name + "  aabb updated");
        }
    }
}
