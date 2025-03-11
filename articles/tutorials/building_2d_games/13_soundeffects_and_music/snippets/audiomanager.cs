#region declaration
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace MonoGameLibrary.Audio;

public class AudioManager : GameComponent { }
#endregion
{
    #region fields
    private readonly Dictionary<string, SoundEffect> _soundEffects;
    private readonly Dictionary<string, Song> _songs;
    private readonly List<SoundEffectInstance> _activeSoundEffectInstances;
    private float _previousMusicVolume;
    private float _previousSoundEffectVolume;
    #endregion

    #region properties
    /// <summary>
    /// Gets a value that indicates if audio is muted.
    /// </summary>
    public bool IsMuted { get; private set; }
    #endregion

    #region ctors
    /// <summary>
    /// Creates a new AudioManager instance.
    /// </summary>
    /// <param name="game">The game this audio manager will belong too..</param>
    public AudioManager(Game game)
        : base(game)
    {
        _soundEffects = new Dictionary<string, SoundEffect>();
        _songs = new Dictionary<string, Song>();
        _activeSoundEffectInstances = new List<SoundEffectInstance>();
    }
    #endregion

    #region methods_gamecomponent
    /// <summary>
    /// Initializes this Audio manager.
    /// </summary>
    public override void Initialize()
    {
        _previousMusicVolume = MediaPlayer.Volume = 1.0f;
        _previousSoundEffectVolume = SoundEffect.MasterVolume = 1.0f;
        base.Initialize();
    }

    /// <summary>
    /// Updates this Audio manager
    /// </summary>
    /// <param name="gameTime">A snapshot of the current timing values for the game.</param>
    public override void Update(GameTime gameTime)
    {
        int index = 0;

        while (index < _activeSoundEffectInstances.Count)
        {
            SoundEffectInstance instance = _activeSoundEffectInstances[index];

            if (instance.State == SoundState.Stopped && !instance.IsDisposed)
            {
                instance.Dispose();
            }

            _activeSoundEffectInstances.RemoveAt(index);
        }

        base.Update(gameTime);
    }

    /// <summary>
    /// Disposes this Audio manager and cleans up resources.
    /// </summary>
    /// <param name="disposing">Indicates whether managed resources should be disposed.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            foreach (SoundEffect soundEffect in _soundEffects.Values)
            {
                soundEffect.Dispose();
            }

            foreach (Song song in _songs.Values)
            {
                song.Dispose();
            }

            _soundEffects.Clear();
            _songs.Clear();
            _activeSoundEffectInstances.Clear();
        }

        base.Dispose(disposing);
    }
    #endregion

    #region methods_content
    /// <summary>
    /// Adds the sound effect with the specified asset name to this audio manager.
    /// </summary>
    /// <param name="assetName">The asset name of the sound effect to add.</param>
    public void AddSoundEffect(string assetName)
    {
        SoundEffect soundEffect = Game.Content.Load<SoundEffect>(assetName);
        _soundEffects.Add(assetName, soundEffect);
    }

    /// <summary>
    /// Adds the song with the specified asset name to this audio manager.
    /// </summary>
    /// <param name="assetName">The asset name of the song to add.</param>
    public void AddSong(string assetName)
    {
        Song song = Game.Content.Load<Song>(assetName);
        _songs.Add(assetName, song);
    }
    #endregion

    #region methods_playback
    /// <summary>
    /// Plays the sound effect with the specified name.
    /// </summary>
    /// <param name="assetName">The asset name of the sound effect to play.</param>
    /// <returns>The sound effect instance created by playing the sound effect.</returns>
    public SoundEffectInstance PlaySoundEffect(string assetName)
    {
        return PlaySoundEffect(assetName, 1.0f, 0.0f, 0.0f, false);
    }

    /// <summary>
    /// Plays the sound effect with the specified asset name, using the specified properties.
    /// </summary>
    /// <param name="assetName">The asset name of the sound effect to play.</param>
    /// <param name="volume">The volume, ranging from 0.0 (silence) to 1.0 (full volume).</param>
    /// <param name="pitch">The pitch adjustment, ranging from -1.0 (down an octave) to 0.0 (no change) to 1.0 (up an octave).</param>
    /// <param name="pan">The panning, ranging from -1.0 (left speaker) to 0.0 (centered), 1.0 (right speaker).</param>
    /// <param name="isLooped">Whether the the sound effect should loop after playback.</param>
    /// <returns>The sound effect instance created by playing the sound effect.</returns>
    public SoundEffectInstance PlaySoundEffect(string assetName, float volume, float pitch, float pan, bool isLooped)
    {
        SoundEffect soundEffect = _soundEffects[assetName];

        SoundEffectInstance soundEffectInstance = soundEffect.CreateInstance();
        soundEffectInstance.Volume = volume;
        soundEffectInstance.Pitch = pitch;
        soundEffectInstance.Pan = pan;
        soundEffectInstance.IsLooped = isLooped;

        soundEffectInstance.Play();

        return soundEffectInstance;
    }

    /// <summary>
    /// Plays the song with the specified asset name.
    /// </summary>
    /// <param name="assetName">The asset name of the song to play.</param>
    public void PlaySong(string assetName)
    {
        Song song = _songs[assetName];

        // Check if the media player is already playing, if so, stop it
        if (MediaPlayer.State == MediaState.Playing)
        {
            MediaPlayer.Stop();
        }

        MediaPlayer.Play(song);
    }
    #endregion

    #region methods_state
    /// <summary>
    /// Pauses all audio.
    /// </summary>
    public void PauseAudio()
    {
        // Pause any active songs playing
        MediaPlayer.Pause();

        // Pause any active sound effects
        foreach (SoundEffectInstance soundEffectInstance in _activeSoundEffectInstances)
        {
            soundEffectInstance.Pause();
        }
    }

    /// <summary>
    /// Resumes play of all previous paused audio.
    /// </summary>
    public void ResumeAudio()
    {
        // Resume paused music
        MediaPlayer.Resume();

        // Resume any active sound effects
        foreach (SoundEffectInstance soundEffectInstance in _activeSoundEffectInstances)
        {
            soundEffectInstance.Resume();
        }
    }

    /// <summary>
    /// Mutes all audio.
    /// </summary>
    public void MuteAudio()
    {
        // Store the volume so they can be restored during ResumeAudio
        _previousMusicVolume = MediaPlayer.Volume;
        _previousSoundEffectVolume = SoundEffect.MasterVolume;

        // Set all volumes to 0
        MediaPlayer.Volume = 0.0f;
        SoundEffect.MasterVolume = 0.0f;

        IsMuted = true;
    }

    /// <summary>
    /// Unmutes all audio to the volume level prior to muting.
    /// </summary>
    public void UnmuteAudio()
    {
        // Restore the previous volume values
        MediaPlayer.Volume = _previousMusicVolume;
        SoundEffect.MasterVolume = _previousSoundEffectVolume;

        IsMuted = false;
    }

    /// <summary>
    /// Toggles the current audio mute state.
    /// </summary>
    public void ToggleMute()
    {
        if (IsMuted)
        {
            UnmuteAudio();
        }
        else
        {
            MuteAudio();
        }
    }
    #endregion

    #region methods_volume
    /// <summary>
    /// Increases volume of all audio by the specified amount.
    /// </summary>
    /// <param name="amount">The amount to increase the audio by.</param>
    public void IncreaseVolume(float amount)
    {
        if (!IsMuted)
        {
            MediaPlayer.Volume = Math.Min(MediaPlayer.Volume + amount, 1.0f);
            SoundEffect.MasterVolume = Math.Min(SoundEffect.MasterVolume + amount, 1.0f);
        }
    }

    /// <summary>
    /// Decreases the volume of all audio by the specified amount.
    /// </summary>
    /// <param name="amount">The amount to decrease the audio by.</param>
    public void DecreaseVolume(float amount)
    {
        if (!IsMuted)
        {
            MediaPlayer.Volume = Math.Max(MediaPlayer.Volume - amount, 0.0f);
            SoundEffect.MasterVolume = Math.Max(SoundEffect.MasterVolume - amount, 0.0f);
        }
    }
    #endregion
}
