namespace OTO.Manager
{
    //System
    using System;

    //UnityEngine
    using UnityEngine;
    using UnityEngine.Audio;

    public class AudioManager : MonoSingleton<AudioManager>
    {
        public SoundData[] musicSounds, sfxSounds;
        public AudioSource musicSource, sfxSource;

        public override void Awake()
        {
            base.Awake();
        }

        //���� ���
        public void PlayMusic(string name)
        {
            SoundData s = Array.Find(musicSounds, x => x.name == name);

            if (s == null)
            {
                Debug.Log("Music Not Found");
            }
            else
            {
                musicSource.clip = s.clip;
                musicSource.Play();
            }
        }

        //���� ��� ����
        public void StopMusic()
        {
            musicSource.Stop();
        }

        //ȿ���� ���
        public void PlaySFX(string name)
        {
            SoundData s = Array.Find(sfxSounds, x => x.name == name);

            if (s == null)
            {
                Debug.Log("Sount Not Found");
            }
            else
            {
                sfxSource.PlayOneShot(s.clip);
            }
        }

        //������ �Ҹ��� ����
        public void SetMusicVolume(float volume, AudioMixer audioMixer)
        {
            audioMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
            PlayerPrefs.SetFloat("musicVolume", volume);
        }

        //ȿ������ �Ҹ��� ����
        public void SetSFXVolume(float volume, AudioMixer audioMixer)
        {
            audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
            PlayerPrefs.SetFloat("SFXVolume", volume);
        }

        //�Ҹ��� ������ ���� �޾ƿͼ� ����
        public void LoadVolume(float musicVolume, float sfxVolume, AudioMixer audioMixer)
        {
            musicVolume = PlayerPrefs.GetFloat("musicVolume");
            sfxVolume = PlayerPrefs.GetFloat("SFXVolume");

            SetMusicVolume(musicVolume, audioMixer);
            SetSFXVolume(sfxVolume, audioMixer);
        }

    }
}
