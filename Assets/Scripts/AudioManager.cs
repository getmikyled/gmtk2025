using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public class MusicClipInfo
    {
        public AudioClip audioClip;
        public bool loop;

        public MusicClipInfo(AudioClip argAudioClip, bool argLoop)
        {
            audioClip = argAudioClip;
            loop = argLoop;
        }
    }

    private Queue<MusicClipInfo> _musicQueue = new Queue<MusicClipInfo>();
    private AudioSource _musicSource;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        
        GameObject musicObject = new GameObject("MusicSource");
        _musicSource = musicObject.AddComponent<AudioSource>();
        DontDestroyOnLoad(musicObject);
        
        QueueMusicClip(Resources.Load<AudioClip>("Music/background_music_intro"), false);
        QueueMusicClip(Resources.Load<AudioClip>("Music/background_music_loop"), true);
    }

    public void QueueMusicClip(AudioClip audioClip, bool loop)
    {
        MusicClipInfo musicClipInfo = new MusicClipInfo(audioClip, loop);
        _musicQueue.Enqueue(musicClipInfo);
    }

    private void Update()
    {
        if (_musicSource.isPlaying == false && _musicQueue.Count > 0)
        {
            MusicClipInfo musicClip = _musicQueue.Dequeue();
            _musicSource.clip = musicClip.audioClip;
            _musicSource.loop = musicClip.loop;

            _musicSource.Play();
        }
    }
}