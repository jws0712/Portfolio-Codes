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

        // 불기둥이 파괴되는 함수
        private void DestroyObject()
        {
            Destroy(gameObject);
        }

        // 불기둥이 특정 테그와 부딛쳤을때 실행되는 코드
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                collision.GetComponent<PlayerManager>().TakeDamage(fireDamage);
            }
        }
    }
}
