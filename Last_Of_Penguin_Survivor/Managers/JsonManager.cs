// # System
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

// # UnityEninge
using UnityEngine;

// # Newtonsoft
using Newtonsoft.Json;


public class JsonManager : MonoBehaviour
{
    public static JsonManager Instance;

    private string directoryPath = default;

    private SerializableChunkData serializableChunkData;
    private SerializableChunkData loadSeralizableChunkData;


    private void Awake()
    {
        #region Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(Instance.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
        #endregion

        serializableChunkData = new SerializableChunkData(); //������ ûũ�� ������(chunkData)�� ������ ����ȭ Ŭ������ ���� ����� ������
        loadSeralizableChunkData = new SerializableChunkData(); //�ε��� �����͸� ���� Ŭ����
        directoryPath = Path.Combine(Application.persistentDataPath, "Maps");   //"Maps" �̶�� ������ ������ ���� ��θ� �Ҵ���
    }

    public void Save(Chunk chunkData)
    {
        //X-Y.chunk ������ �����̸� ����
        string saveFileName = $"{chunkData.Coord.x}-{chunkData.Coord.y}.chunk";         
        string savePath = Path.Combine(directoryPath, saveFileName);

        //��ΰ� ���ٸ� ��θ� ������
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        //��ο� �Ȱ��� ������ �ִ��� �˻�
        //��ο� �����Ͱ� ������ �Ǿ� ���� �ʴٸ� ������
        //��ο� �̹� �����Ͱ� ������ �Ǿ� �ִٸ� �������� ����
        if (!File.Exists(savePath))
        {
            serializableChunkData.blockInChunk = chunkData.BlockInChunk;

            try
            {
                string data = JsonConvert.SerializeObject(serializableChunkData, Formatting.Indented);
                File.WriteAllText(savePath, data);
            }
            catch (JsonSerializationException e)
            {
                Debug.LogError($"����ȭ ���� �߻�: {e.Message} \n StackTrace : {e.StackTrace}");
            }
            catch (JsonWriterException e)
            {
                Debug.LogError($"Json �ۼ� ���� �߻�: {e.Message} \n StackTrace : {e.StackTrace}");
            }
            catch (Exception e)
            {
                Debug.LogError($"�� �� ���� ���� �߻�: {e.Message} \n StackTrace : {e.StackTrace}");
            }
        }
        else
        {

            return;
        }
    }

    //���� �����͸� ���ο� �����ͷ� ���� ��Ŵ
    public void UpdateSave(Chunk chunkData)
    {
        string saveFileName = $"{chunkData.Coord.x}-{chunkData.Coord.y}.chunk";
        string savePath = Path.Combine(directoryPath, saveFileName);

        //��ο� �̹� �����͸� ������
        if (File.Exists(savePath))
        {
            serializableChunkData.blockInChunk = chunkData.BlockInChunk;

            try
            {
                string data = JsonConvert.SerializeObject(serializableChunkData, Formatting.Indented);
                File.WriteAllText(savePath, data);
            }
            catch (JsonSerializationException e)
            {
                Debug.LogError($"������ȭ ���� �߻�: {e.Message} \n StackTrace : {e.StackTrace}");
            }
            catch (JsonReaderException e)
            {
                Debug.LogError($"Json �б� ���� �߻�: {e.Message} \n StackTrace : {e.StackTrace}");
            }
            catch (Exception e)
            {
                Debug.LogError($"�� �� ���� ���� �߻�: {e.Message} \n StackTrace : {e.StackTrace}");
            }
        }
    }

    //������ �ε�
    public BlockData[,,] Load(Vector2Int chunkCoord)
    {
        string saveFileName = $"{chunkCoord.x}-{chunkCoord.y}.chunk";
        string savePath = Path.Combine(directoryPath, saveFileName);

        if (File.Exists(savePath))
        {
            try
            {
                string data = File.ReadAllText(savePath);
                loadSeralizableChunkData = JsonConvert.DeserializeObject<SerializableChunkData>(data);
                return loadSeralizableChunkData.blockInChunk;
            }
            catch (JsonReaderException e)
            {
                Debug.LogError($"JSON �б� ����: {e.Message}");
            }
            catch (JsonSerializationException e)
            {
                Debug.LogError($"JSON ����ȭ ����: {e.Message}");
            }
            catch (JsonException e)
            {
                Debug.LogError($"�Ϲ����� JSON ����: {e.Message}");
            }
            catch (Exception e)
            {
                Debug.Log($"Message: {e.Message}\nStackTrace: {e.StackTrace}");
            }
        }

        return null;
    }
}

[System.Serializable]
public class SerializableChunkData
{
    public BlockData[,,] blockInChunk;
}

