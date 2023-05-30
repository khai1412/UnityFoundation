namespace GameFoundation.Scripts.ObjectPoolManager
{
    using System.Collections.Generic;
    using UnityEngine;

    public class ObjectPool
    {
        private GameObject       rootObject;
        private List<GameObject> pooledGameObjects;
        private List<GameObject> spawnedGameObject;
        public  int              PoolSize                => this.pooledGameObjects.Count + this.spawnedGameObject.Count;
        private bool             CheckHasObjectInPool    => this.pooledGameObjects.Count > 0;
        private bool             CheckHasObjectInSpawned => this.spawnedGameObject.Count > 0;
        public ObjectPool(GameObject prefab,Transform root)
        {
            this.SetupPool(prefab,root);
        }
        
        private void SetupPool(GameObject prefab,Transform root)
        {
            this.rootObject        = new GameObject();
            this.rootObject.transform.SetParent(root);
            this.rootObject.name   = "Pool" + prefab.name;
            this.pooledGameObjects = new();
            this.spawnedGameObject = new();
        }

        public GameObject Spawn(GameObject prefab,Transform parent,Vector3 position, Quaternion rotation)
        {
            if (!prefab) return null;
            GameObject gameObject;
            if (this.CheckHasObjectInPool)
            {
                gameObject = this.pooledGameObjects[^1];
                this.spawnedGameObject.Add(gameObject);
                this.pooledGameObjects.Remove(gameObject);
                gameObject.SetActive(true);
            }
            else
            {
                gameObject                         = Object.Instantiate(prefab);
                this.spawnedGameObject.Add(gameObject);
            }
            gameObject.transform.position = position;
            gameObject.transform.rotation = rotation;
            this.SetParentObject(gameObject,parent!=null?parent:this.rootObject.transform);
            return gameObject;
        }
        
        private void SetParentObject(GameObject gameObject, Transform parent)
        {
            if(!gameObject) return;
            gameObject.transform.SetParent(parent);
        }

        public void Recycle(GameObject gameObject)
        {
            if(!gameObject||!this.spawnedGameObject.Contains(gameObject)) return;
            GameObject recycleObject = null;
            recycleObject = gameObject;
            recycleObject.SetActive(false);
            this.spawnedGameObject.Remove(recycleObject);
            this.pooledGameObjects.Add(recycleObject);
        }
        
    }
}