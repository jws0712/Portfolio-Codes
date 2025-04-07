namespace OTO.Object
{
    //UnityEngine
    using UnityEngine;

    //Project
    using OTO.Manager;

    // 자동차의 기능을 구현한 클래스
    public class Car : MonoBehaviour
    {
        [SerializeField] GameObject playerObject = null;
        [SerializeField] private float spawnPower = default;

        // 플레이어 오브젝트를 소환시키는 함수
        public void SpawnPlayer()
        {
            AudioManager.Instance.PlaySFX("PlayerJump");
            GameObject player = Instantiate(playerObject, transform.position, Quaternion.identity);

            player.GetComponent<Rigidbody2D>().AddForce(Vector2.up * spawnPower, ForceMode2D.Impulse);
        }
    }
}


