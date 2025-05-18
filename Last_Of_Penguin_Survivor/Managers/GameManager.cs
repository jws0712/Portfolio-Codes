//Unity
using UnityEngine;

//TMP
using TMPro;

//Mirror
using Mirror;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private GameObject resultPenel = null;

    [Space(10)]
    [Header("resultPenelText")]
    [SerializeField] private TextMeshProUGUI surviveDayCount = null;
    [SerializeField] private TextMeshProUGUI getItemCount = null;
    [SerializeField] private TextMeshProUGUI craftItemCount = null;

    public float surviveDayCountVel = default;
    public float getItemCountVel = default;
    public float craftItemCountVel = default;

    private bool isGameOver = default;

    //�̱���
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        isGameOver = false;

        resultPenel.gameObject.SetActive(false);

        surviveDayCountVel = 0;
        getItemCountVel = 0;
        craftItemCountVel = 0;
    }

    private void Update()
    {
        if (Input.anyKey && isGameOver)
        {
            //���ӿ����� ���̵����� �Ǹ鼭 �۵�
            FadeManager.Instance.FadeIn(() => 
            {
                if (LopNetworkManager.singleton.isNetworkActive)
                {
                    if (NetworkClient.isConnected)
                    {
                        LopNetworkManager.singleton.StopClient();
                    }
                    if (NetworkServer.active)
                    {
                        LopNetworkManager.singleton.StopServer();
                    }
                }
            } );
        }
    }

    //���ȭ���� ������
    public void OnResultPenel()
    {
        FadeManager.Instance.FadeOut(() => { isGameOver = true; });

        resultPenel.gameObject.SetActive(true);

        surviveDayCount.text = GameManager.Instance.surviveDayCountVel.ToString("N0");
        getItemCount.text = GameManager.Instance.getItemCountVel.ToString("N0");
        craftItemCount.text = GameManager.Instance.craftItemCountVel.ToString("N0");
    }
}



