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


        //private 변수
        private Vector2 mousePos;

        // 초기 마우스커서의 값 설정
        private void Start()
        {
            mainCamera = Camera.main;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;

        }

        private void Update()
        {
            //커서 오브젝트가 마우스 커서를 따라옴
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