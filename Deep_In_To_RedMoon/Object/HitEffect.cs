namespace OTO.Object
{
    //System
    using System.Collections;

    //UnityEngine
    using UnityEngine;

    //Project
    using OTO.Manager;

    //총알이 파괴될때 소환되는 이펙트의 기능을 구현한 함수
    public class HitEffect : MonoBehaviour
    {
        [SerializeField] private float destoryTime = default;

        // 이펙트가 소환되면 일정시간 뒤에 파괴되게 하는 코드
        private void OnEnable()
        {
            StartCoroutine(Co_DestroySmoke());
        }

        // 이펙트를 파괴하는 코루틴
        private IEnumerator Co_DestroySmoke()
        {
            yield return new WaitForSeconds(destoryTime);
            ObjectPoolManager.Instance.ReturnObject(gameObject);
        }

    }
}


