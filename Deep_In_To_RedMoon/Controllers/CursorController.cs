namespace OTO.Controller
{
    //UnityEngine
    using UnityEngine;

    //Project
    using OTO.Manager;

    public class CursorController : MonoBehaviour
    {
        [Header("Cursor Object")]
        [SerializeField] GameObject cursorObject;
        private Camera mainCamera;


        //private ����
        private Vector2 mousePos;

        // �ʱ� ���콺Ŀ���� �� ����
        private void Start()
        {
            mainCamera = Camera.main;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;

        }

        private void Update()
        {
            //Ŀ�� ������Ʈ�� ���콺 Ŀ���� �����
            if(GameManager.Instance.IsGameOver)
            {
                Cursor.visible = true;
                cursorObject.SetActive(false);
            }

            mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            cursorObject.transform.position = mousePos;
        }
    }
}