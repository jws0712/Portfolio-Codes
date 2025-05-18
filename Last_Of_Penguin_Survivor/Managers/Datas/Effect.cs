namespace Lop.Survivor.Island.Effect
{

    // # System
    using System.Collections;
    using System.Collections.Generic;

    // # UnityEngine
    using UnityEngine;
    using UnityEngine.Pool;

    public class Effect : MonoBehaviour
    {
        [Header("ParticleInfo")]
        [SerializeField] private string sfxName = default;
        [SerializeField] private float destroyTime = default;
        

        private IObjectPool<Effect> managedPool;

        //����Ʈ�� �����ɶ� ����
        private void OnEnable()
        {
            Invoke("DestroyObject", destroyTime);
        }

        //����Ʈ ��ü�� ���� ������Ʈ Ǯ�� ����
        public void SetManagedPool(IObjectPool<Effect> pool)
        {
            managedPool = pool;
        }

        //����Ʈ ������Ʈ �ı�
        public void DestroyObject()
        {
            managedPool.Release(this);
        }
    }
}
