namespace OTO.Manager
{
    //System
    using System.Collections;
    using System.Collections.Generic;

    //UnityEngine
    using UnityEngine;

    //TMP
    using TMPro;

    //Project
    using OTO.Controller;

    public class StageController : MonoBehaviour
    {
        [Header("Stage Info")]
        [SerializeField] private WaveData[] waveArray = null;
        [SerializeField] private Transform[] spawnPosArray = null;
        
        [SerializeField] private float spawnDelayTime = default;
        [SerializeField] private float waveDelayTime = default;

        private void Start()
        {
            StageEventBus.Subscribe(StageEventType.Ready, () => { StartCoroutine(Co_WaitWave(waveDelayTime)); });
            StageEventBus.Subscribe(StageEventType.WaveStart, () => { StartCoroutine(Co_SpawnMonster(GameManager.Instance.CurrentWaveCount, spawnDelayTime)); });
            StageEventBus.Subscribe(StageEventType.WaveClear, WaveClear);
        }

        //웨이브를 클리어 했을때 실행
        private void WaveClear()
        {
            if(GameManager.Instance.CurrentWaveCount == waveArray.Length - 1)
            {
                StageEventBus.Publish(StageEventType.GameClear);
            }
            else
            {
                GameManager.Instance.CurrentWaveCount++;
                StageEventBus.Publish(StageEventType.Ready);
            }
        }

        //웨이브 시작 대기
        private IEnumerator Co_WaitWave(float durationTime)
        {
            yield return new WaitForSeconds(durationTime);
            StageEventBus.Publish(StageEventType.WaveStart);
        }

        //몬스터 스폰
        private IEnumerator Co_SpawnMonster(int currentWaveCount, float delayTime)
        {
            GameManager.Instance.FieldMonsterCount = waveArray[currentWaveCount].monsterArray.Length;
            for (int i = 0; i < waveArray[currentWaveCount].monsterArray.Length; i++)
            {
                int spawnPosNumber = Random.Range(0, spawnPosArray.Length);
                Instantiate(waveArray[currentWaveCount].monsterArray[i], spawnPosArray[spawnPosNumber].position, spawnPosArray[spawnPosNumber].rotation);
                yield return new WaitForSeconds(delayTime);
            }
        }
    }
}

