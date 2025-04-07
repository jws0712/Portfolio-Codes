namespace OTO.Object
{
    //UnityEngine
    using UnityEngine;

    //Project
    using OTO.Charactor.Player;

    public class Fire : MonoBehaviour
    {
        [Header("FireInfo")]
        [SerializeField] private float fireDamage = default;

        // �ұ���� �ı��Ǵ� �Լ�
        private void DestroyObject()
        {
            Destroy(gameObject);
        }

        // �ұ���� Ư�� �ױ׿� �ε������� ����Ǵ� �ڵ�
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                collision.GetComponent<PlayerManager>().TakeDamage(fireDamage);
            }
        }
    }
}
