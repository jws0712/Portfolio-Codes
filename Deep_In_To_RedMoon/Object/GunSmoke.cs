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

        //��ȯ�Ǹ� ���⸦ �ı���Ű�� �ڷ�ƾ ����
        private void OnEnable()
        {
            StartCoroutine(Co_DestroySmoke());
        }

        // ���⸦ �ı��ϴ� �ڷ�ƾ
        private IEnumerator Co_DestroySmoke()
        {
            yield return new WaitForSeconds(destoryTime);
            ObjectPoolManager.Instance.ReturnObject(gameObject);
        }
    }
}


