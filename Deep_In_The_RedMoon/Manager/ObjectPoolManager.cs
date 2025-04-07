namespace OTO.Manager
{
    using System;
    //System
    using System.Collections;
    using System.Collections.Generic;

    //UnityEninge
    using UnityEngine;

    public class ObjectPoolManager : MonoSingleton<ObjectPoolManager>
    {
        [SerializeField] private PoolObjectData[] poolObjectDataArray = null;

        private Dictionary<string, Queue<GameObject>> poolObjectDataDictionary = new Dictionary<string, Queue<GameObject>>();

        public override void Awake()
        {
            base.Awake();

            InitPool();
        }

        //������ƮǮ �ʱ�ȭ
        private void InitPool()
        {
            foreach (PoolObjectData poolObjectData in poolObjectDataArray)
            {
                poolObjectDataDictionary.Add(poolObjectData.poolPrefabObject.name, GetPoolDataQueue(poolObjectData.poolObjectContainer, poolObjectData.poolPrefabObject, poolObjectData.poolCount));
            }
        }

        //������Ʈ�� ����� ť�� ��ȯ��
        private Queue<GameObject> GetPoolDataQueue(Queue<GameObject> poolObjectQueue, GameObject poolObject, int poolCount)
        {
            for (int i = 0; i < poolCount; i++)
            {
                poolObjectQueue.Enqueue(CreateObjcet(poolObject));
            }

            return poolObjectQueue;
        }

        //������Ʈ ����
        private GameObject CreateObjcet(GameObject poolObject)
        {
            GameObject obj = Instantiate(poolObject);
            obj.name = poolObject.name;
            obj.gameObject.SetActive(false);
            obj.transform.SetParent(transform);
            return obj;
        }

        //������Ʈ�� Ǯ���� ������
        public GameObject GetPoolObject(GameObject prefabObject)
        {
            string objectName = prefabObject.name;

            if (poolObjectDataDictionary[objectName].Count > 0)
            {
                var returnObject = poolObjectDataDictionary[objectName].Dequeue();
                returnObject.transform.SetParent(null);
                returnObject.gameObject.SetActive(true);
                return returnObject;
            }
            else
            {
                PoolObjectData findObject = Array.Find(poolObjectDataArray, x => x.poolPrefabObject.name == objectName);

                if (findObject != null)
                {
                    var returnObject = CreateObjcet(findObject.poolPrefabObject);
                    returnObject.transform.SetParent(null);
                    returnObject.gameObject.SetActive(true);
                    return returnObject;
                }
                else
                {
                    Debug.LogError("ERROR : OBJCET NOT FOUND");
                    return null;
                }

            }
        }

        //������Ʈ�� Ǯ�� ��ȯ��
        public void ReturnObject(GameObject obj)
        {
            obj.gameObject.SetActive(false);
            obj.transform.SetParent(transform);
            poolObjectDataDictionary[obj.name].Enqueue(obj);
        }

        //������Ʈ Ǯ �ʱ�ȭ
        public void ClearPool()
        {
            poolObjectDataDictionary.Clear();

            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }
    }
}

