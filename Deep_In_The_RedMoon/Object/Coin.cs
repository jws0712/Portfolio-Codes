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

        //땅에 떨어졌을때 미끄러지지 않게 하는 코드
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if(collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                rb.velocity = Vector2.zero;
            }
        }
    }
}
