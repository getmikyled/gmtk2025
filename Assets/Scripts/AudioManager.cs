using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    /// <summary>
    /// MusicClipInfo is used for storing queued music clips.
    /// </summary>
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
        // Audio manager persists between scenes
        DontDestroyOnLoad(gameObject);
                                                  
        // Create source object for queueing music
        GameObject musicObject = new GameObject("MusicSource");
        _musicSource = musicObject.AddComponent<AudioSource>();
        DontDestroyOnLoad(musicObject);
        
        // Temporarily queue audio
        QueueMusicClip(Resources.Load<AudioClip>("Music/background_music_intro"), false);
        QueueMusicClip(Resources.Load<AudioClip>("Music/background_music_loop"), true);
    }

    /// <summary>
    /// Queue music clip
    /// </summary>
    /// <param name="audioClip">Audio clip object</param>
    /// <param name="loop">Does the audio clip loop</param>
    public void QueueMusicClip(AudioClip audioClip, bool loop)
    {
        MusicClipInfo musicClipInfo = new MusicClipInfo(audioClip, loop);
        _musicQueue.Enqueue(musicClipInfo);
    }

    /// <summary>
    /// </summary>
    private void Update()
    {
        // If the music source isn't playing and the queue is not empty, queue the next music clip.
        if (_musicSource.isPlaying == false && _musicQueue.Count > 0)
        {
            MusicClipInfo musicClip = _musicQueue.Dequeue();
            _musicSource.clip = musicClip.audioClip;
            _musicSource.loop = musicClip.loop;

            _musicSource.Play();
        }
    }
}