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

        // ����Ʈ�� ��ȯ�Ǹ� �����ð� �ڿ� �ı�
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


