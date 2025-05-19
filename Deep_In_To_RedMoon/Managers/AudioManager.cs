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

        //음악 재생
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

        //음악 재생 종료
        public void StopMusic()
        {
            musicSource.Stop();
        }

        //효과음 재생
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

        //음악의 소리를 조절
        public void SetMusicVolume(float volume, AudioMixer audioMixer)
        {
            audioMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
            PlayerPrefs.SetFloat("musicVolume", volume);
        }

        //효과음의 소리를 조절
        public void SetSFXVolume(float volume, AudioMixer audioMixer)
        {
            audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
            PlayerPrefs.SetFloat("SFXVolume", volume);
        }

        //소리를 조절한 값을 받아와서 설정
        public void LoadVolume(float musicVolume, float sfxVolume, AudioMixer audioMixer)
        {
            musicVolume = PlayerPrefs.GetFloat("musicVolume");
            sfxVolume = PlayerPrefs.GetFloat("SFXVolume");

            SetMusicVolume(musicVolume, audioMixer);
            SetSFXVolume(sfxVolume, audioMixer);
        }

    }
}
