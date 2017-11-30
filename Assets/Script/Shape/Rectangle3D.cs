using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MathsPhys
{
    public class Rectangle3D : BaseObject
    {
        public Vector3 size;

        // Use this for initialization
        public override void Init()
        {
            base.Init();

            if(GetComponent<MyRectangle3DCollider>() != null)
            {
                collider = GetComponent<MyRectangle3DCollider>();                
            }
            else
            {
                collider = new MyRectangle3DCollider();
            }
            collider.Init((BaseObject)this);
        }

        public override void SetSizeFromEditor()
        {
            base.SetSizeFromEditor();
            size = transform.localScale;
        }
    }
}
