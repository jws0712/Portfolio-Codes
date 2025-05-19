namespace OTO.Object
{
    //System
    using System.Collections;

    //UnityEngine
    using UnityEngine;

    //Project
    using OTO.Manager;

    public class HitEffect : MonoBehaviour
    {
        [SerializeField] private float destoryTime = default;

        // 이펙트가 소환되면 일정시간 뒤에 파괴
        private void OnEnable()
        {
            StartCoroutine(Co_DestroySmoke());
        }

        private IEnumerator Co_DestroySmoke()
        {
            yield return new WaitForSeconds(destoryTime);
            ObjectPoolManager.Instance.ReturnObject(gameObject);
        }

    }
}


