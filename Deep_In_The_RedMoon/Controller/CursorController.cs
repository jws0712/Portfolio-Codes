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


        //private variable
        private Vector2 mousePos;

        // 초기 마우스커서의 값 설정
        private void Start()
        {
            mainCamera = Camera.main;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;

        }

        // 마우스커서 오브젝트가 마우스 위치를 따라오게 만든코드
        private void Update()
        {
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