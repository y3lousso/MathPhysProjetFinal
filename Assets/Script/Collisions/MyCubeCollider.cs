using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MathsPhys
{
    public class MyCubeCollider : Collider
    {

        // Use this for initialization
        public override void Init(BaseObject obj)
        {
            base.Init(obj);

        }

        public override void CalculateAABB()
        {
            aabb.Calculate(baseObject.currentPosition, ((Cube)baseObject).size.x/2, ((Cube)baseObject).size.y/2, ((Cube)baseObject).size.z/2);
        }
    }
}
