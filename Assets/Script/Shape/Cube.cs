using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MathsPhys
{
    public class Cube : BaseObject
    {
        public Vector3 size;

        // Use this for initialization
        public override void Init()
        {
            base.Init();

            if(GetComponent<MyCubeCollider>() != null)
            {
                collider = GetComponent<MyCubeCollider>();                
            }
            else
            {
                collider = new MyCubeCollider();
            }
            collider.Init((BaseObject)this);
        }
    }
}
