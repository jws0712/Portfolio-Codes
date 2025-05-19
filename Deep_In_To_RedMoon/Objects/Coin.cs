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

        private void Start()
        {
            rb.AddForce(Vector2.one * -2f, ForceMode2D.Impulse);
        }


        private void OnCollisionEnter2D(Collision2D collision)
        {
            //속력을 0으로 바꿈
            if(collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                rb.velocity = Vector2.zero;
            }
        }
    }
}
