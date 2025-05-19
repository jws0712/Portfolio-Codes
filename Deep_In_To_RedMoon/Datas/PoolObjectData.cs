namespace OTO.Manager
{
    //System
    using System;
    using System.Collections.Generic;
    
    //UnityEngine
    using UnityEngine;


    [Serializable]
    //Ǯ ������Ʈ ������ Ŭ����
    public class PoolObjectData
    {
        public GameObject poolPrefabObject = null;
        public int poolCount = default;
        [HideInInspector] public Queue<GameObject> poolObjectContainer = new Queue<GameObject>();
    }
}
