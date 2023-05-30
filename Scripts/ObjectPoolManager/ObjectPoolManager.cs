namespace GameFoundation.Scripts.ObjectPoolManager
{
    using System.Collections.Generic;
    using UnityEngine;

    public class ObjectPoolManager 
    {
        public GameObject                         objectRoot;
        public Dictionary<GameObject, ObjectPool> Pools;
        public Dictionary<GameObject, ObjectPool> SpawnedGameObject;

        public ObjectPoolManager()
        {
            this.Pools             = new();
            this.SpawnedGameObject = new();
        }

        private GameObject Spawn(GameObject prefab,Transform parent,Vector3 position,Quaternion rotation)
        {
            if (this.Pools.TryGetValue(prefab, out var objectPool))
            {
                var spawnedObject = objectPool.Spawn(prefab,parent,position,rotation);
                if (!this.SpawnedGameObject.ContainsKey(spawnedObject))
                {
                    this.SpawnedGameObject.Add(spawnedObject,objectPool);
                }
                return spawnedObject;
            }

            if (this.objectRoot == null)
            {
                this.objectRoot      = new GameObject();
                this.objectRoot.name = "ObjectPools";
            }
            var newPool = new ObjectPool(prefab,this.objectRoot.transform);
            this.Pools.Add(prefab,newPool);
            return this.Spawn(prefab,parent,position,rotation);
        }

        public GameObject Spawn(GameObject prefab) => this.Spawn(prefab,null, Vector3.zero, Quaternion.identity);
        
        public GameObject Spawn(GameObject prefab, Transform parent) => this.Spawn(prefab, parent, Vector3.zero, Quaternion.identity);

        public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation) => this.Spawn(prefab, null, position, rotation);
        

        public void Recycle(GameObject gameObject)
        {
            if (this.SpawnedGameObject.TryGetValue(gameObject, out var objectPool))
            {
                objectPool.Recycle(gameObject);
            }
        }


    }
}