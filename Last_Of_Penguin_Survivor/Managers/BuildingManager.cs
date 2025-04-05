namespace Lop.Survivor.Island
{
    //System
    using System;
    using System.Collections.Generic;

    //Unity
    using UnityEngine;
    using UnityEngine.UI;

    //Project
    using Lop.Survivor.Island.Buildingbase;
    using Structures;
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
        [SerializeField] private float topDirection  = default;
        [SerializeField] private float rightDirection = default;
        [SerializeField] private float bottomDirection = default;
        [SerializeField] private BuildingList[] buildingArray;
        [Space(10f)]
        [SerializeField] private Material disPlaceMaterial = null;
        [SerializeField] private Material placeMaterial = null;
        [Header("BuildingManagerUI_Info")]
        [SerializeField] private GameObject buildingUIPlanel = null;
        [SerializeField] private GameObject checkPoint = null;
        [SerializeField] private Slider buildingPlaceSlider = null;

        [Header("Network")]
        [SerializeField] private BuildingSync buildingSync = null;

        private BuildingDirection buildingDirection = default;

        private Vector3Int builingPlacePosition = default;

        private MapSettingManager mapSettingManager = null;
        private Map map = null;

        private GameObject checkParentObject = null;
        private GameObject checkChildObject = null;

        private string selectBuidling = default;
        private string builingTypeName = default;

        private bool isBuildingMode = default;
        private bool isfirstBuildingCheck = default;
        private bool isCheckBuilding = default;
        private bool isCheckUnderBuilding = default;

        private float buildingRotY = default;
        private float buildingCoolTime = default;

        private List<Structure> structureList = new List<Structure>();

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
            mapSettingManager = FindObjectOfType<MapSettingManager>();
            buildingSync = GetComponent<BuildingSync>();

            map = mapSettingManager.Map;
            buildingRotY = 180f;


            if (buildingSync != null)
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
            //���๰�� �Ʒ��ִ� ���� �˻��ؼ� ���� ���� ���ٸ� ���๰�� �ı���Ŵ
            if (isCheckUnderBuilding)
            {
                for(int i = 0; i < structureList.Count; i++)
                {
                    structureList[i].CheckUnderBuilding();
                }
                isCheckUnderBuilding = false;
            }
        }


        //���๰�� ��ġ�� �˻��ϴ� �Լ�
        public void CheckBuilding(float playerDirection, string buildingType, Vector3 position)
        {
            //��ġ�� ���๰�� �̸��� null�̶�� ��ġ�� �˻����� ����
            if (buildingType == null) return;

            //��ġ�� ���๰ ������Ʈ�� �������
            GameObject buildingObject = FindBuildingObject(buildingType);

            //������Ʈ�� null�̶�� ��ġ�� �˻����� ����
            if (buildingObject == null) return;

            int checkDirectionX = 0;
            int checkDirectionZ = 0;

            int posX = Mathf.FloorToInt(position.x);
            int posY = Mathf.FloorToInt(position.y);
            int posZ = Mathf.FloorToInt(position.z);

            builingTypeName = buildingType;
            isBuildingMode = true;

            //���๰ ������Ʈ�� �����͸� ������
            BuildingBase buildingData = buildingObject.GetComponent<BuildingBase>();
            
            //���๰ ������Ʈ�� ũ�⸦ �Ҵ���
            Vector3Int buildingSize = buildingData.BuilingSize;

            //���๰ ������Ʈ�� ��ġ �ӵ��� �Ҵ���
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

            //���๰�� ���⿡ ���� �˻��ϴ� ������ �ٲ����
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

            //���๰�� ��ġ�� �������� ����
            Vector3Int buildingPlacePos =   Vector3Int.right * (buildingSize.x * checkDirectionX) +
                                            Vector3Int.up * buildingSize.y +
                                            Vector3Int.forward * (buildingSize.z * checkDirectionZ);

            //���๰�� �˻� ������ �ִ밪�� �����
            int buildingMaxX = posX + buildingSize.x * checkDirectionX;
            int buildingMaxZ = posZ + buildingSize.z * checkDirectionZ;
            int buildingMaxY = posY + buildingSize.y;

            //���๰�� �˻� ������ �ִ밪�� Vector3Int ���·� ��ȯ��
            Vector3Int buildingCheckArea =  Vector3Int.right * buildingMaxX +
                                            Vector3Int.up * buildingMaxY +
                                            Vector3Int.forward * buildingMaxZ;

            //���๰�� ��ġ�ɼ� �ִ��� �˻�
            if (CheckBuildingPlaceArea(buildingCheckArea, checkPosition, checkDirectionX, checkDirectionZ))
            {
                CalculateBuildingPlacePosition(buildingPlacePos, checkPosition);
                ChangeBuildingMaterial(placeMaterial, builingTypeName, builingPlacePosition);
                buildingUIPlanel.SetActive(true);
            }
            else
            {
                CalculateBuildingPlacePosition(buildingPlacePos, checkPosition);
                ChangeBuildingMaterial(disPlaceMaterial, builingTypeName, builingPlacePosition);
                buildingUIPlanel.SetActive(false);
            }
        }

        //Ȧ�α׷� �ǹ��� ���͸����� �ٲٴ� �Լ�
        private void ChangeBuildingMaterial(Material setMaterial, string buildingTypeName, Vector3Int placePosition)
        {
            checkParentObject = GameObject.FindGameObjectWithTag(buildingTypeName);
            checkChildObject = checkParentObject.transform.GetChild(0).gameObject;
            checkChildObject.SetActive(true);
            checkChildObject.transform.localPosition = new Vector3(checkChildObject.transform.localPosition.x, mapSettingManager.parentChunkYPos * 2, checkChildObject.transform.localPosition.z); 

            //�ǹ� �˻� �ʱ갪 �ʱ�ȭ
            if (!isfirstBuildingCheck)
            {
                checkParentObject.transform.position = placePosition;
                checkChildObject.transform.rotation = Quaternion.Euler(0, 180, 0);
                isfirstBuildingCheck = true;
            }

            if(checkChildObject.GetComponent<MeshRenderer>() == null)
            {
                checkChildObject.GetComponent<SkinnedMeshRenderer>().material = setMaterial;
            }
            else
            {
                checkChildObject.GetComponent<MeshRenderer>().material = setMaterial;
            }

            checkParentObject.transform.position = placePosition;
        }

        //���๰ ��ġ �����˻�
        private bool CheckBuildingPlaceArea(Vector3Int buildingCheckArea, Vector3Int placePosition, int dirX, int dirZ)
        {

            //���� ����� false ��ȯ
            if (buildingCheckArea.x < 0 || buildingCheckArea.x > (mapSettingManager.MapWidthChunkValue * ChunkData.ChunkWidthValue) ||
                buildingCheckArea.z < 0 || buildingCheckArea.z > (mapSettingManager.MapLengthChunkValue * ChunkData.ChunkLengthValue) ||
                buildingCheckArea.y < mapSettingManager.ParentChunkYPos || buildingCheckArea.y > ChunkData.ChunkHeightValue)
            {
                return false;
            }

            //���๰ ũ�⸸ŭ�� ������ 3�� for������ �˻���
            for (int y = placePosition.y; y <= buildingCheckArea.y; y++)
            {
                for (int x = placePosition.x; (dirX > 0 ? x < buildingCheckArea.x : x > buildingCheckArea.x); x += dirX)
                {
                    for (int z = placePosition.z; (dirZ > 0 ? z < buildingCheckArea.z : z > buildingCheckArea.z); z += dirZ)
                    {
                        if (map.MapBlock[x, y, z].id != Air)
                        {
                            isCheckBuilding = false;
                            return false;
                        }
                        if (map.MapBlock[x, placePosition.y - 1, z].id == Air || map.MapBlock[x, placePosition.y - 1, z].id == Water)
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

        //���๰ ��ġ ��ġ �˻�
        private void CalculateBuildingPlacePosition(Vector3Int buildingPlacePos, Vector3Int placePosition)
        {
            int[] buildingPlaceVector = { buildingPlacePos.x + 1, buildingPlacePos.y, buildingPlacePos.z + 1};
            int[] placePositionVector = {placePosition.x, placePosition.y, placePosition.z };

            for (int i = 0; i < buildingPlaceVector.Length; i++)
            {
                if (buildingPlaceVector[i] < 0)
                {
                    placePositionVector[i] += buildingPlaceVector[i];
                }

                builingPlacePosition =  Vector3Int.right * placePositionVector[0] +
                                        Vector3Int.up * placePositionVector[1] +
                                        Vector3Int.forward * placePositionVector[2];
            }
        }

        //���๰�� ��ġ Ȯ���� ���ߴ� �Լ�
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
            isCheckBuilding = false;
            isfirstBuildingCheck = false;
        }

        //���๰�� ȸ����Ű�� �Լ�
        public void SpinBuilding()
        {
            if(checkChildObject == null) return;
            buildingRotY += 90f;

            if(buildingRotY > 270f)
            {
                buildingRotY = 180;
            }

            checkChildObject.transform.rotation = Quaternion.Euler(0, buildingRotY, 0);
        }

        //���๰�� ������ ��ġ�ϴ� �Լ� (����)
        public GameObject PlaceBuildingOnMapLocal(string builingTypeName, Vector3Int builingPlacePosition)
        {
            GameObject building = PlaceBuildingOnMapSync(builingTypeName, builingPlacePosition, buildingRotY);
            StopCheckBuilding();

            return building;
        }

        //���๰�� ������ ��ġ�ϴ� �Լ� (����ȭ)
        public GameObject PlaceBuildingOnMapSync(string buildingType, Vector3Int position, float rotY)
        {
            if (buildingType == null) return null;

            if (buildingSync.isServer) // ������ ��
            {
                GameObject builingObject = FindBuildingObject(buildingType);
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

        //��ġ�Ǿ� �ִ� ���๰�� �ı��ϴ� �Լ�
        public void DestroyBuilding(GameObject building)
        {
            Vector3 buildingPos = building.transform.position;
            buildingPos.y += 1f;

            map.GetChunkFromVector3(buildingPos).EditVoxel(buildingPos, mapSettingManager.FindBLockType(Air));

            if (buildingSync != null)
                buildingSync.DestroyBuilding(building);
        }

        //��ġ�� ���๰�� �̸��� ã���Լ�
        public void FindBuildingName(string Name)
        {
            selectBuidling = Name;
            StopCheckBuilding();
        }

        //���๰ ������Ʈ�� ã�� �Լ�
        private GameObject FindBuildingObject(string buildingType)
        {
            BuildingList building = Array.Find(buildingArray, x => x.buildingName == buildingType);
            if (building == null)
            {
                return null;
            }

            return building.buildingObject;
        }
    }
}


