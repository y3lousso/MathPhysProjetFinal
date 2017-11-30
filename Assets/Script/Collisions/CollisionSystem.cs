using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MathsPhys
{
    public class CollisionSystem : MonoBehaviour
    {
        public static CollisionSystem instance;

        QuadTree quadTree;

        List<SpacePartition> _spacePartitions;

        public CollisionSystem()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                throw new System.Exception("Can't have multiple collision system instances.");
            }           
        }

        public void StartSystem()
        {
            // A modifier vu que c'est dégueux
            quadTree = GameEngine.instance.gameObject.GetComponent<QuadTree>();

            quadTree.Init(GameEngine.instance.GetAllObjects());
            _spacePartitions = new List<SpacePartition>();
        }

        public void UpdateSystem()
        {
            _spacePartitions.Clear();
            quadTree.UpdateSpaceDistribution();
            
            foreach (SpacePartition spacePartition in _spacePartitions)
            {               
                foreach (BaseObject baseObject in spacePartition.GetObjects())
                {
                    //if (baseObject.rotationRate != Vector3.Zero)
                    //{
                        baseObject.GetCollider().CalculateAABB();                        
                    //}
                }
                for (int i = 0; i < spacePartition.GetObjects().Count-1; i++)
                {
                    for (int j = 1; j < spacePartition.GetObjects().Count; j++)
                    {
                        if(ObjectOverlapAABB(spacePartition.GetObjectByIndex(i).GetCollider().aabb, spacePartition.GetObjectByIndex(j).GetCollider().aabb))
                        {                          
                            Debug.Log("AABB overlap detected");
                            
                        }
                    }
                }
            }
        }

        public void EndSystem()
        {

        }

        public void AddSpacePartition(SpacePartition spacePartition)
        {
            _spacePartitions.Add(spacePartition);
        }

        public bool ObjectOverlapAABB(AABB aabb1, AABB aabb2)
        {
            bool overlapX = aabb1.minPosX < aabb2.maxPosX && aabb1.maxPosX > aabb2.minPosX ? true : false;
            bool overlapY = aabb1.minPosY < aabb2.maxPosY && aabb1.maxPosY > aabb2.minPosY ? true : false; 
            bool overlapZ = aabb1.minPosZ < aabb2.maxPosZ && aabb1.maxPosZ > aabb2.minPosZ ? true : false;
            Debug.Log("Overlap" + overlapX + "  " + overlapY + "  " + overlapZ);
            return overlapX && overlapY && overlapZ;
        }
    }
}
