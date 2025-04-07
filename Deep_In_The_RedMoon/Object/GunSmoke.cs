namespace OTO.Object
{
    using OTO.Manager;
    //System
    using System.Collections;

    //UnityEngine
    using UnityEngine;

    public class GunSmoke : MonoBehaviour
    {
        [SerializeField] private float destoryTime = default;

        //소환되면 연기를 파괴시키는 코루틴 실행
        private void OnEnable()
        {
            StartCoroutine(Co_DestroySmoke());
        }

        // 연기를 파괴하는 코루틴
        private IEnumerator Co_DestroySmoke()
        {
            yield return new WaitForSeconds(destoryTime);
            ObjectPoolManager.Instance.ReturnObject(gameObject);
        }
    }
}


