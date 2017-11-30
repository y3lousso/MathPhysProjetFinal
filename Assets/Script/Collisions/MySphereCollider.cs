using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MathsPhys
{
    public class MySphereCollider : Collider
    {
        // Use this for initialization
        public override void Init(BaseObject obj)
        {
            base.Init(obj);
            CalculateAABB();
        }

        public override void CalculateAABB()
        {
            aabb.Calculate(((Sphere)baseObject));
        }
    }
}
