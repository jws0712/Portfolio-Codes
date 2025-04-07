namespace OTO.Charactor.Player
{
    //System
    using System.Collections;
    using System.Collections.Generic;

    //UnityEngine
    using UnityEngine;
    using UnityEngine.UI;

    //TMP
    using TMPro;

    //Project
    using OTO.Object;
    using OTO.Controller;
    using OTO.Manager;

    public class PlayerController : Subject
    {
        #region variables
        [Header("Move")]
        [SerializeField] private float moveSpeed;

        [Header("Jump")]
        [SerializeField] private float jumpPower;
        [SerializeField] private Transform groundCheckPos;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float oringGravitySacle = default;
        [SerializeField] private float fallGravitySacle = default;

        [Header("Dash")]
        [SerializeField] private float dashPower;
        [SerializeField] private float dashingTime;
        [SerializeField] private float dashingTimeCoolTime;

        [Header("GhostEffect")]
        [SerializeField] private PlayerGhost playerGhost = null;

        [Header("Camera")]
        [SerializeField] private Camera mainCamera = null;

        [Header("Gun")]
        [SerializeField] private Animator gunAnimator = null;


        [Header("Positions")]
        [SerializeField] private Transform handPos = null;
        [SerializeField] private Transform firePos = null;
        [SerializeField] private Transform spinPos = null;

        [Header("Bullet")]
        [SerializeField] private GameObject bullet = null;
        [SerializeField] private GameObject gunSmoke = null;
        [SerializeField] private float bulletDamage = default;
        [SerializeField] private int maxAmmo = default;

        [Header("CoolTime")]
        [SerializeField] private float fireCoolTime = default;
        [SerializeField] private float reloadCoolTime = default;

        [Header("Bullet Spread")]
        [SerializeField] private float maxSpreadAngle = default;
        [SerializeField] private float minSpreadAngle = default;

        [Header("Spin")]
        [SerializeField] private float RotZ = default;

        [Header("UI")]
        [SerializeField] private Slider reroadTimeSlider = null;

        //private variables
        private Quaternion bulletAngle = default;

        private Vector3 mousePos = default;

        private Vector2 dir;

        private LayerMask monsterLayer = default;
        private LayerMask playerLayer = default;

        private int currentAmmo = default;
        private int currentCoin = default;

        private float horizontal = default;
        private float currentReroadCoolTIme = default;

        private bool isShot = default;
        private bool isReload = default;
        private bool isFilp = default;
        private bool isDash = default;
        private bool canDash = true;

        private string cameraShakeType = default;

        private GameObject GunObject = null;

        private Animator anim = null;

        private Rigidbody2D rb = null;

        private PlayerManager playerManager = null;

        private CameraController cameraController = null;
        private GameUIController gameUIController = null;

        //property
        public int CurrentCoin => currentCoin;
        public int CurrentAmmo => currentAmmo;
        public string CameraShakeType { get { return cameraShakeType; } set { cameraShakeType = value; } }
        public PlayerManager PlayerManager => playerManager;
        #endregion

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
            playerManager = GetComponent<PlayerManager>();
        }
        private void OnEnable()
        {
            GameManager.Instance.SetController(this);

            cameraController = GameManager.Instance.CameraController;
            gameUIController = GameManager.Instance.GameUIController;

            mainCamera = Camera.main;

            if (cameraController)
            {
                Attach(cameraController);
            }

            if (gameUIController)
            {
                Attach(gameUIController);
            }

            currentAmmo = maxAmmo;

            NotifyObservers();
        }
        private void OnDisable()
        {
            if(cameraController)
            {
                Detach(cameraController);
            }

            if (gameUIController)
            {
                Detach(gameUIController);
            }

        }

        private void Start()
        {
            isFilp = true;

            monsterLayer = LayerMask.NameToLayer("Monster");
            playerLayer = LayerMask.NameToLayer("Player");
        }

        private void Update()
        {
            if (Time.timeScale == 0)
            {
                return;
            }

            GunObject = handPos.GetChild(0).gameObject;

            reroadTimeSlider.gameObject.SetActive(false);
            reroadTimeSlider.value = currentReroadCoolTIme / reloadCoolTime;

            Spin();
            Shoot();
            Reload();
            PlayerInput();
            PlayerJump();
            Filp();
            Dash();
            PlayerAnimation();
        }

        private void FixedUpdate()
        {
            if (isDash)
            {
                return;
            }

            PlayerMove();
        }

        //플레이어의 입력을 관리하는 함수
        private void PlayerInput()
        {
            horizontal = Input.GetAxisRaw("Horizontal");
        }
        
        //플레이어의 움직임을 관리하는 함수
        private void PlayerMove()
        {
            rb.velocity = new Vector2(horizontal * moveSpeed, rb.velocity.y);
        }
        
        //플레이어의 점프 기능을 구현한 함수
        private void PlayerJump()
        {
            if (rb.velocity.y < 0)
            {
                rb.gravityScale = fallGravitySacle;
            }
            else
            {
                rb.gravityScale = oringGravitySacle;
            }

            if (Input.GetButtonDown("Jump") && CheckGround() == true && !isDash)
            {

                AudioManager.Instance.PlaySFX("PlayerJump");
                rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);

            }
            
            if(CheckGround() == true)
            {
                anim.SetBool("isJump", false);
            }
            else
            {
                anim.SetBool("isJump", true);
            }

        }
        
        //플레이어 오브젝트를 뒤집는 함수
        private void Filp()
        {
            if (!isDash)
            {
                if (Mathf.Abs(RotZ) >= 100f && isFilp)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                    handPos.localScale = new Vector3(-1, -1, 1);
                    isFilp = false;
                }
                if (Mathf.Abs(RotZ) <= 80f && !isFilp)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                    handPos.localScale = new Vector3(1, 1, 1);
                    isFilp = true;
                }
            }
            else
            {
                if (horizontal < 0)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                    isFilp = false;
                }
                else if (horizontal > 0)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                    isFilp = true;
                }
            }

        }

        //대시를 실행하는 함수
        private void Dash()
        {
            if(Input.GetMouseButtonDown(1) && canDash && horizontal != 0 && CheckGround() == true)
            {
                StartCoroutine(Dashing());
            }
        }

        //대시의 기능을 구현한 함수
        private IEnumerator Dashing()
        {
            rb.velocity = Vector2.zero;

            GunObject.SetActive(false);

            canDash = false;
            isDash = true;

            float orginGravity = rb.gravityScale;

            rb.gravityScale = 0f;
            rb.AddForce(Vector2.right * horizontal * dashPower, ForceMode2D.Impulse);

            playerGhost.makeGhost = true;

            Physics2D.IgnoreLayerCollision(playerLayer, monsterLayer, true);

            yield return new WaitForSeconds(dashingTime);

            rb.gravityScale = orginGravity;
            isDash = false;

            yield return new WaitForSeconds(dashingTimeCoolTime);

            Physics2D.IgnoreLayerCollision(playerLayer, monsterLayer, false);

            playerGhost.makeGhost = false;
            canDash = true;

            GunObject.SetActive(true);
        }

        //플레이어의 애니매이션 상태를 관리하는 함수
        private void PlayerAnimation()
        {
            if(horizontal != 0)
            {
                anim.SetBool("isWalk", true);
            }
            else
            {
                anim.SetBool("isWalk", false);
            }

            anim.SetBool("isDash", isDash);
        }

        //땅에 닿았는지 검사하는 함수
        private bool CheckGround()
        {
            return Physics2D.OverlapCircle(groundCheckPos.position, 0.1f, groundLayer);
        }

        //총을 재장전할때 실행되는 함수
        private void Reload()
        {
            if (!isShot && currentAmmo != maxAmmo && Input.GetKeyDown(KeyCode.R) && isReload == false && isDash == false)
            {
                AudioManager.Instance.PlaySFX("Reload");
                isReload = true;
                StartCoroutine(Co_Reload());
            }
        }

        //총을 재장전하는 코루틴
        private IEnumerator Co_Reload()
        {
            while (true)
            {
                reroadTimeSlider.gameObject.SetActive(true);
                currentReroadCoolTIme += Time.deltaTime;
                if (currentReroadCoolTIme >= reloadCoolTime)
                {
                    currentAmmo = maxAmmo;
                    NotifyObservers();
                    currentReroadCoolTIme = 0;
                    isReload = false;

                    yield break;
                }
                yield return null;
            }
        }
        
        //마우스 코드
        private void Spin()
        {
            mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);

            Vector3 rotation = mousePos - spinPos.position;

            RotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

            handPos.rotation = Quaternion.Euler(0, 0, RotZ);
        }

        //총알을 발사하는 함수
        private void Shoot()
        {
            if (isDash)
            {
                return;
            }

            if (Input.GetButton("Fire1") && isShot == false && currentAmmo > 0)
            {
                StartCoroutine(Co_Shooting());
            }
        }
        
        //총알을 발사하는 코루틴
        private IEnumerator Co_Shooting()
        {
            float SpreadAngle = RotZ + Random.Range(minSpreadAngle, maxSpreadAngle);
            bulletAngle = Quaternion.Euler(0, 0, SpreadAngle);

            currentAmmo --;
            AudioManager.Instance.PlaySFX("GunFire");
            cameraShakeType = "GunFire";
            NotifyObservers();

            GameObject bullet = ObjectPoolManager.Instance.GetPoolObject(this.bullet);
            bullet.transform.position = firePos.transform.position;
            bullet.transform.rotation = bulletAngle;
            bullet.GetComponent<Bullet>().BulletDamage = bulletDamage;

            GameObject smoke = ObjectPoolManager.Instance.GetPoolObject(gunSmoke);
            smoke.transform.position = firePos.transform.position;
            smoke.transform.rotation = bulletAngle;

            gunAnimator.SetTrigger("Fire");

            isShot = true;
            yield return new WaitForSeconds(fireCoolTime);
            isShot = false;
        }

        //아이템과 부딛쳤을때를 실행되는 코드
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Coin"))
            {
                AudioManager.Instance.PlaySFX("GetCoin");

                currentCoin += collision.gameObject.GetComponent<Coin>().CoinValue;

                NotifyObservers();

                ObjectPoolManager.Instance.ReturnObject(collision.gameObject);
            }
        }
    }
}
