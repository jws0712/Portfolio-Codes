// # System
using System;
using System.Collections.Generic;

// # UnityEngine
using UnityEngine;


// # Projects
using Lop.Survivor.Island.Buildingbase;

public class TemperatureManager : MonoBehaviour
{
    public static TemperatureManager Instance;

    public List<DayTemperatureData> dayTemperatureDataList;

    private Map map;
    private MapSettingManager mapSettingManager;


    
    private void Awake()
    {
        //�̱���
        Instance = this;

        mapSettingManager = FindObjectOfType<MapSettingManager>();
        map = mapSettingManager.Map;
    }

    //pos ��ġ�� �µ��� ��ȯ��Ŵ
    public int GetTemperature(Vector3 pos)
    {
        int x = Mathf.FloorToInt(pos.x);
        int y = Mathf.FloorToInt(pos.y);
        int z = Mathf.FloorToInt(pos.z);

        //���� ������ ����� Nomal�µ��� ��ȯ��Ŵ
        if (x < 0 || x > (mapSettingManager.MapWidthChunkValue * ChunkData.ChunkWidthValue) ||
            z < 0 || z > (mapSettingManager.MapLengthChunkValue * ChunkData.ChunkLengthValue) ||
            y <= mapSettingManager.ParentChunkYPos || y > ChunkData.ChunkHeightValue)
        {
            return (int)TemperatureType.Nomal; //Noaml �µ��� ��ȯ
        }

        return (int)map.MapBlock[x, y, z].blockTemperatureType;
    }

    //���� �µ��� ��ȯ ��Ű�� �Լ�
    public void UpdateTemperature()
    {
        //��ں� �������� ã�� ���ں� ��ó ���� �µ��� ������
        GameObject[] lightBuildings = GameObject.FindGameObjectsWithTag("Bonfire");

        foreach(var lightBuilding in lightBuildings)
        {
            lightBuilding.GetComponent<AreaEffectManager>().TemperatrueUp();
        }

        //���� ��� ���� �˻���
        for (int y = 0; y < ChunkData.ChunkHeightValue; y++)
        {
            for (int x = 0; x < mapSettingManager.MapWidthChunkValue * ChunkData.ChunkWidthValue; x++)
            {
                for (int z = 0; z < mapSettingManager.MapLengthChunkValue * ChunkData.ChunkLengthValue; z++)
                {
                    //�˻��� ���� �����̸� �µ��� �������� ����
                    if (map.MapBlock[x, y, z].id == BlockType.Air || x < 0 || z < 0)
                    {
                        continue;
                    }

                    //�˻��� ���� ���̶�� ������ �µ��� VeryCold�� ������
                    if (map.MapBlock[x, y, z].id == BlockType.Water)
                    {
                        map.MapBlock[x, y, z].blockTemperatureType = BlockTemperatureType.VeryCold;
                    }
                    else
                    {
                        //�˻��� ���� ������ �ƴ϶�� �µ��� ��ü�µ��� ���� �µ��� ������
                        switch (mapSettingManager.globalTemperature)
                        {
                            case TemperatureType.VeryCold:
                                {
                                    map.MapBlock[x, y, z].blockTemperatureType = BlockTemperatureType.VeryCold;
                                    break;
                                }

                            case TemperatureType.Cold:
                                {
                                    map.MapBlock[x, y, z].blockTemperatureType = BlockTemperatureType.Cold;

                                    break;
                                }

                            case TemperatureType.Nomal:
                                {
                                    map.MapBlock[x, y, z].blockTemperatureType = BlockTemperatureType.Nomal;
                                    break;
                                }

                            case TemperatureType.Heat:
                                {
                                    map.MapBlock[x, y, z].blockTemperatureType = BlockTemperatureType.Heat;
                                    break;
                                }

                            case TemperatureType.VeryHeat:
                                {
                                    map.MapBlock[x, y, z].blockTemperatureType = BlockTemperatureType.VeryHeat;
                                    break;
                                }
                        }
                    }
                }
            }
        }
    }
}

[Serializable]
public class DayTemperatureData
{
    public TemperatureType[] temperatureTypesArray;
}
