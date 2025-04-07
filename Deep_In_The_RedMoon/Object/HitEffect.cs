namespace OTO.Object
{
    //System
    using System.Collections;

    //UnityEngine
    using UnityEngine;

    //Project
    using OTO.Manager;

    //�Ѿ��� �ı��ɶ� ��ȯ�Ǵ� ����Ʈ�� ����� ������ �Լ�
    public class HitEffect : MonoBehaviour
    {
        [SerializeField] private float destoryTime = default;

        // ����Ʈ�� ��ȯ�Ǹ� �����ð� �ڿ� �ı��ǰ� �ϴ� �ڵ�
        private void OnEnable()
        {
            StartCoroutine(Co_DestroySmoke());
        }

        // ����Ʈ�� �ı��ϴ� �ڷ�ƾ
        private IEnumerator Co_DestroySmoke()
        {
            yield return new WaitForSeconds(destoryTime);
            ObjectPoolManager.Instance.ReturnObject(gameObject);
        }

    }
}


