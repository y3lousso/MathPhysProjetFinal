using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MathsPhys {

	public abstract class Collider : MonoBehaviour {

        protected BaseObject baseObject;
        public AABB aabb;

        public virtual void Init(BaseObject obj)
        {
            baseObject = obj;
            aabb = new AABB();
        }

        public abstract void CalculateAABB();
		
	}

}
