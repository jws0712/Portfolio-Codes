namespace OTO.Object
{
    //UnityEngine
    using UnityEngine;

    //Project
    using OTO.Manager;

    // �ڵ����� ����� ������ Ŭ����
    public class Car : MonoBehaviour
    {
        [SerializeField] GameObject playerObject = null;
        [SerializeField] private float spawnPower = default;

        // �÷��̾� ������Ʈ�� ��ȯ��Ű�� �Լ�
        public void SpawnPlayer()
        {
            AudioManager.Instance.PlaySFX("PlayerJump");
            GameObject player = Instantiate(playerObject, transform.position, Quaternion.identity);

            player.GetComponent<Rigidbody2D>().AddForce(Vector2.up * spawnPower, ForceMode2D.Impulse);
        }
    }
}


