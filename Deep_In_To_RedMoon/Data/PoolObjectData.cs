namespace OTO.Manager
{
    //System
    using System;
    using System.Collections.Generic;
    
    //UnityEngine
    using UnityEngine;


    //Ǯ ������Ʈ ������ Ŭ����
    [Serializable]
    public class PoolObjectData
    {
        public GameObject poolPrefabObject = null;
        public int poolCount = default;
        [HideInInspector] public Queue<GameObject> poolObjectContainer = new Queue<GameObject>();
    }
}
