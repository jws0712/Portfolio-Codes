namespace Lop.Survivor.Island
{
    //System
    using System;
    using System.Collections.Generic;

    //Unity
    using UnityEngine;
    using UnityEngine.UI;

    //Project
    using Lop.Survivor.Island.Buildingdata;
    using static BlockType;


    //�ǹ� ������ ��Ÿ���� Enum
    public enum BuildingDirection
    {
        Top,
        Left,
        Bottom,
        Right
    }

    public class BuildingManager : MonoBehaviour
    {
        public static BuildingManager Instance;

        [Header("BuildingManagerInfo")]
        [SerializeField] private float topDirection = default;
        [SerializeField] private float rightDirection = default;
        [SerializeField] private float bottomDirection = default;
        [SerializeField] private float moveLerpSpeed = default;
        [SerializeField] private float spinSpeed = default;
        public BuildingList[] buildingList;
        [Space(10f)]
        [SerializeField] private Material disPlaceMaterial = null;
        [SerializeField] private Material placeMaterial = null;
        [Header("BuildingManagerUI_Info")]
        [SerializeField] private GameObject buildingUIPlanel = null;
        [SerializeField] private GameObject checkPoint = null;
        public Slider buildingPlaceSlider = null;

        private BuildingDirection buildingDirection;

        private Vector3Int builingPlacePosition;

        private MapSettingManager mapSettingManager;
        private Map map;

        private GameObject checkParentObject;
        private GameObject checkChildObject;

        private string selectBuidling;
        private string builingTypeName;

        public bool isBuildingMode;
        private bool isfirstBuildingCheck;
        public bool isCanBuilding;
        public bool isCheckBuilding;

        public List<BuildingData> buildingDataList = new List<BuildingData>();

        private float buildingRotY;
        private float buildingCoolTime;

        [Header("Network")]
        public BuildingSync buildingSync = null;


        public bool isCheckUnderBuilding = default;

        //������Ƽ
        public string SelectBuildingName => selectBuidling;
        public float BuildingCoolTime => buildingCoolTime;

        public string BuildingTypeName => builingTypeName;

        public Vector3Int BuilingPlacePosition => builingPlacePosition;

        //�̱���
        private void Awake()
        {
            Instance = this;
        }

        
        private void Start()
        {
            //���� �ʱ�ȭ
            {
                map = mapSettingManager.Map;
                buildingRotY = 180f;

                isfirstBuildingCheck = false;
                isCanBuilding = false;
                isCheckUnderBuilding = false;

                mapSettingManager = FindObjectOfType<MapSettingManager>();
                buildingSync = GetComponent<BuildingSync>();
            }

            if(buildingSync != null)
            {
                if (buildingSync.isServer) { mapSettingManager.PlaceTree(); }
            }
            else
            {
                mapSettingManager.PlaceTree();
            }
        }

        private void Update()
        {
            //���๰�� �Ʒ��ִ� ���� �˻��ؼ� ���� ���� ���ٸ� ���๰�� �ı�
            if (isCheckUnderBuilding)
            {
                for(int i = 0; i < buildingDataList.Count; i++)
                {
                    buildingDataList[i].CheckUnderBuilding();
                }
                isCheckUnderBuilding = false;
            }
        }

        //��ġ�� ���๰�� �̸��� ã��
        public void SelectBuilding(string Name)
        {
            selectBuidling = Name;
            StopCheckBuilding();
        }

        //���๰�� ��ġ�� �˻�
        public void Building(float playerDirection, string buildingType, Vector3 position)
        {
            //��ġ�� ���๰�� �̸��� null�̶�� ��ġ�� �˻����� ����
            if (buildingType == null) return;

            //��ġ�� ���๰ ������Ʈ�� �������
            GameObject buildingObject = FindBuilding(buildingType);

            //������Ʈ�� null�̶�� ��ġ�� �˻����� ����
            if (buildingObject == null) return;

            int checkDirectionX = 0;
            int checkDirectionZ = 0;

            int posX = Mathf.FloorToInt(position.x);
            int posY = Mathf.FloorToInt(position.y);
            int posZ = Mathf.FloorToInt(position.z);

            builingTypeName = buildingType;
            isBuildingMode = true;

            //���๰ ������Ʈ�� �����͸� ����
            BuildingData buildingData = buildingObject.GetComponent<BuildingData>();
            
            //���๰ ������Ʈ�� ũ�⸦ �Ҵ�
            Vector3Int buildingSize = buildingData.BuilingSize;

            //���๰ ������Ʈ�� ��ġ �ӵ��� �Ҵ�
            buildingCoolTime = buildingData.BuildingCoolTime;

            //��ġ�� ��ġ
            Vector3Int checkPosition =  Vector3Int.right * posX +
                                        Vector3Int.up * posY +
                                        Vector3Int.forward * posZ;

            //�÷��̾ �ٶ󺸰� �ִ� ���⿡ ���� ���๰�� ������ �ٲ�
            if (playerDirection <= 180)
            {
                if (playerDirection <= topDirection) buildingDirection = BuildingDirection.Top;
                else if (playerDirection >= bottomDirection) buildingDirection = BuildingDirection.Bottom;
                else buildingDirection = BuildingDirection.Right;
            }
            else
            {
                if (playerDirection >= 360 - topDirection) buildingDirection = BuildingDirection.Top;
                else if (playerDirection <= rightDirection) buildingDirection = BuildingDirection.Right;
                else buildingDirection = BuildingDirection.Left;
            }

            //���๰�� ���⿡ ���� �˻��ϴ� ������ �ٲ�
            switch (buildingDirection)
            {
                case BuildingDirection.Top:
                    checkDirectionX = 1;
                    checkDirectionZ = 1;
                    break;
                case BuildingDirection.Right:
                    checkDirectionX = 1;
                    checkDirectionZ = -1;
                    break;
                case BuildingDirection.Bottom:
                    checkDirectionX = -1;
                    checkDirectionZ = -1;
                    break;
                case BuildingDirection.Left:
                    checkDirectionX = -1;
                    checkDirectionZ = 1;
                    break;
            }

            Vector3Int buildingPlacePos =   Vector3Int.right * (buildingSize.x * checkDirectionX) +
                                            Vector3Int.up * buildingSize.y +
                                            Vector3Int.forward * (buildingSize.z * checkDirectionZ);

            //���๰�� �˻� ������ �ִ밪�� ���
            int buildingMaxX = posX + buildingSize.x * checkDirectionX;
            int buildingMaxZ = posZ + buildingSize.z * checkDirectionZ;
            int buildingMaxY = posY + buildingSize.y;

            //���๰�� �˻� ������ �ִ밪�� Vector3Int ���·� ��ȯ
            Vector3Int buildingCheckArea =  Vector3Int.right * buildingMaxX +
                                            Vector3Int.up * buildingMaxY +
                                            Vector3Int.forward * buildingMaxZ;

            if (CheckBuildingArea(buildingCheckArea, checkPosition, checkDirectionX, checkDirectionZ))
            {
                CheckPlacePosition(buildingPlacePos, checkPosition);
                CheckBuilding(placeMaterial, builingTypeName, builingPlacePosition);
                isCanBuilding = true;
                buildingUIPlanel.SetActive(true);
            }
            else
            {
                CheckPlacePosition(buildingPlacePos, checkPosition);
                CheckBuilding(disPlaceMaterial, builingTypeName, builingPlacePosition);
                isCanBuilding = false;
                buildingUIPlanel.SetActive(false);
            }
        }

        //�ǹ��� ��ġ ���� ���͸����� ����
        private void CheckBuilding(Material setMaterial, string buildingTypeName, Vector3Int placePosition)
        {
            checkParentObject = GameObject.FindGameObjectWithTag(buildingTypeName);
            checkChildObject = checkParentObject.transform.GetChild(0).gameObject;
            checkChildObject.SetActive(true);
            checkChildObject.transform.localPosition = new Vector3(checkChildObject.transform.localPosition.x, mapSettingManager.parentChunkYPos * 2, checkChildObject.transform.localPosition.z); 

            if (!isfirstBuildingCheck)
            {
                checkParentObject.transform.position = placePosition;
                checkChildObject.transform.rotation = Quaternion.Euler(0, 180, 0);
                isfirstBuildingCheck = true;
            }

            checkParentObject.transform.position = Vector3.Lerp(checkParentObject.transform.position, placePosition, moveLerpSpeed * Time.deltaTime);
            if(checkChildObject.GetComponent<MeshRenderer>() == null)
            {
                checkChildObject.GetComponent<SkinnedMeshRenderer>().material = setMaterial;
            }
            else
            {
                checkChildObject.GetComponent<MeshRenderer>().material = setMaterial;
            }

            checkChildObject.transform.rotation = Quaternion.Slerp(checkChildObject.transform.rotation, Quaternion.Euler(0, buildingRotY, 0), spinSpeed * Time.deltaTime);
        }

        //�ǹ��� ��ġ �ݰ��� �˻�
        private bool CheckBuildingArea(Vector3Int buildingCheckArea, Vector3Int placePosition, int dirX, int dirZ)
        {
            if (!isCanBuilding)
            {
                isCheckBuilding = false;
                return false;
            }

            //���� ����� false ��ȯ
            if (buildingCheckArea.x < 0 || buildingCheckArea.x > (mapSettingManager.MapWidthChunkValue * ChunkData.ChunkWidthValue) ||
                buildingCheckArea.z < 0 || buildingCheckArea.z > (mapSettingManager.MapLengthChunkValue * ChunkData.ChunkLengthValue) ||
                buildingCheckArea.y < mapSettingManager.ParentChunkYPos || buildingCheckArea.y > ChunkData.ChunkHeightValue)
            {
                return false;
            }

            //���๰ ũ�⸸ŭ�� ������ 3�� for������ �˻�
            for (int y = placePosition.y; y <= buildingCheckArea.y; y++)
            {
                for (int x = placePosition.x; (dirX > 0 ? x < buildingCheckArea.x : x > buildingCheckArea.x); x += dirX)
                {
                    for (int z = placePosition.z; (dirZ > 0 ? z < buildingCheckArea.z : z > buildingCheckArea.z); z += dirZ)
                    {
                        if (map.BlockInMap[x, y, z].id != Air)
                        {
                            isCheckBuilding = false;

                            return false;
                        }
                        if (map.BlockInMap[x, placePosition.y - 1, z].id == Air || map.BlockInMap[x, placePosition.y - 1, z].id == Water)
                        { 
                            isCheckBuilding = false;

                            return false;
                        }
                    }
                }
            }

            isCheckBuilding = true;
            return true;
        }

        //�ǹ��� ��ġ�� ���� ��ġ�� �˻�
        private void CheckPlacePosition(Vector3Int buildingPlacePos, Vector3Int placePosition)
        {
            int[] buildingPlaceVector = { buildingPlacePos.x + 1, buildingPlacePos.y, buildingPlacePos.z + 1};
            int[] placePositionVector = {placePosition.x, placePosition.y, placePosition.z };

            for (int i = 0; i < buildingPlaceVector.Length; i++)
            {
                if (buildingPlaceVector[i] < 0)
                {
                    placePositionVector[i] += buildingPlaceVector[i];
                }

                builingPlacePosition =
                            Vector3Int.right * placePositionVector[0] +
                            Vector3Int.up * placePositionVector[1] +
                            Vector3Int.forward * placePositionVector[2];
            }
        }
        
        //���� ȯ���ؼ� �ǹ��� ��ġ
        public GameObject LocalPlaceBuilding(string builingTypeName, Vector3Int builingPlacePosition)
        {
            GameObject building = PlaceMapOnMap(builingTypeName, builingPlacePosition, buildingRotY);
            StopCheckBuilding();

            return building;
        }

        //�ǹ� ��ġ �˻縦 ����
        public void StopCheckBuilding()
        {
            if (checkChildObject == null) return;

            buildingRotY = 180f;

            checkChildObject.SetActive(false);
            buildingUIPlanel.SetActive(false);

            checkParentObject.transform.position = Vector3.zero;

            checkChildObject = null;
            checkParentObject = null;

            isBuildingMode = false;
            isCanBuilding = false;
            isCheckBuilding = false;
            isfirstBuildingCheck = false;
        }

        //�ǹ��� ȸ��
        public void SpinBuilding()
        {
            if(checkChildObject == null) return;
            buildingRotY += 90f;

            if(buildingRotY > 270f)
            {
                buildingRotY = 180;
            }

            Debug.Log(buildingRotY);
        }

        //�ǹ��� ��ġ
        public GameObject PlaceMapOnMap(string buildingType, Vector3Int position, float rotY)
        {
            if (buildingType == null) return null;

            if (buildingSync.isServer) // ������ ��
            {
                GameObject builingObject = FindBuilding(buildingType);
                GameObject placeBuilding = Instantiate(builingObject, position, Quaternion.identity);

                placeBuilding.transform.GetChild(0).transform.rotation = Quaternion.Euler(0, rotY, 0);
                placeBuilding.transform.GetChild(0).transform.localPosition = new Vector3(placeBuilding.transform.GetChild(0).transform.localPosition.x, mapSettingManager.ParentChunkYPos - 1, placeBuilding.transform.GetChild(0).transform.localPosition.z);
                placeBuilding.GetComponent<BuildingMeshSync>().meshRotY = rotY;
                placeBuilding.GetComponent<BuildingMeshSync>().meshPosY = placeBuilding.transform.GetChild(0).transform.position.y;

                buildingSync.SpawnBuilding(placeBuilding);

                return placeBuilding;
            }
            else // Ŭ���̾�Ʈ�� ��
            {
                buildingSync.SendBuildingSpawnMessage(buildingType, position.x, position.y, position.z, rotY);

                return null;
            }
		}

        //�ǹ��� �ı�
        public void DestroyBuilding(GameObject building)
        {
            Vector3 buildingPos = building.transform.position;
            buildingPos.y += 1f;

            map.GetChunkFromVector3(buildingPos).EditVoxel(buildingPos, mapSettingManager.FindBLockType(Air));

            if (buildingSync != null)
                buildingSync.DestroyBuilding(building);
        }

        //Ư�� �ǹ��� Ž���� ��ȯ
        private GameObject FindBuilding(string buildingType)
        {
            BuildingList building = Array.Find(buildingList, x => x.buildingName == buildingType);
            if (building == null)
            {
                return null;
            }

            return building.buildingObject;
        }
    }
}


