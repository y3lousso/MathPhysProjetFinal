using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MathsPhys
{
    public class Sphere : BaseObject
    {
        public float radius;

        // Use this for initialization
        public override void Init()
        {
            base.Init();

            if (GetComponent<MySphereCollider>() != null)
            {
                collider = GetComponent<MySphereCollider>();
            }
            else
            {
                collider = new MySphereCollider();
            }

            collider.Init(this);
        }
    }
}
