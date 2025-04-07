namespace OTO.Object
{
    //UnityEngine
    using UnityEngine;

    public class Coin : MonoBehaviour
    {
        [Header("Coin Info")]
        [SerializeField] private int coinValue = default;

        public int CoinValue => coinValue;

        private Rigidbody2D rb = null;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        //���� ���������� �̲������� �ʰ� �ϴ� �ڵ�
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if(collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                rb.velocity = Vector2.zero;
            }
        }
    }
}
