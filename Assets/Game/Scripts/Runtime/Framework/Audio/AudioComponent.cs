using System;
using GameMain;
using JSAM;
using UnityEngine;

namespace Game.Scripts.Runtime.Audio
{
    public class AudioComponent : UnityGameFramework.Runtime.GameFrameworkComponent
    {
        public MusicChannelHelper MainMusicHelper => AudioManager.MainMusicHelper;

        #region PlaySound

        public SoundChannelHelper PlaySound<T>(T sound, Transform transform = null, SoundChannelHelper helper = null)
            where T : Enum
            => AudioManager.PlaySound(sound, transform, helper);

        public SoundChannelHelper PlaySound<T>(T sound, Vector3 position, SoundChannelHelper helper = null)
            where T : Enum
            => AudioManager.PlaySound(sound, position, helper);

        #endregion

        #region StopSound

        public void StopSound<T>(T sound, Transform transform = null, bool stopInstantly = true) where T : Enum =>
            AudioManager.StopSound(sound, transform, stopInstantly);

        /// <summary>
        /// <inheritdoc cref="StopSound{T}(T, Transform)"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sound">The sound to be stopped</param>
        /// <param name="position">The sound's playback position in world space. 
        /// Passing this property will limit playback stopping 
        /// to only the sound playing at this specific position</param>
        /// <param name="stopInstantly">Optional: If true, stop the sound immediately, you may want to leave this false for looping sounds</param>
        public void StopSound<T>(T sound, Vector3 position, bool stopInstantly = true) where T : Enum =>
            AudioManager.StopSound(sound, position, stopInstantly);


        /// <summary>
        /// Stops all playing sounds maintained by AudioManager
        /// <param name="stopInstantly">Optional: If true, stop all sounds immediately</param>
        /// </summary>
        public void StopAllSounds(bool stopInstantly = true) =>
            AudioManager.StopAllSounds(stopInstantly);

        /// <summary>
        /// A shorthand for wrapping StopSound in an IsSoundPlaying if-statement
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sound">The sound to be stopped</param>
        /// <param name="transform">Optional: If the sound was initially passed a reference to 
        /// a transform in PlaySound, passing the same Transform reference will stop that specific playing instance</param>
        /// <param name="stopInstantly">Optional: If true, stop the sound immediately, you may want to leave this false for looping sounds</param>
        /// <returns>True if sound was stopped successfully, false if sound wasn't playing</returns>
        public bool StopSoundIfPlaying<T>(T sound, Transform transform = null, bool stopInstantly = true)
            where T : Enum =>
            AudioManager.StopSoundIfPlaying(sound, transform, stopInstantly);

        /// <summary>
        /// <inheritdoc cref="StopSoundIfPlaying{T}(T, Transform)"/>
        /// </summary>
        /// <param name="sound">The sound to be stopped</param>
        /// <param name="position">The sound's playback position in world space. 
        /// Passing this property will limit playback stopping 
        /// to only the sound playing at this specific position</param>
        /// <param name="stopInstantly">Optional: If true, stop the sound immediately, you may want to leave this false for looping sounds</param>
        /// <returns>True if sound was stopped successfully, false if sound wasn't playing</returns>
        public bool StopSoundIfPlaying<T>(T sound, Vector3 position, bool stopInstantly = true) where T : Enum =>
            AudioManager.StopSoundIfPlaying(sound, position, stopInstantly);

        #endregion

        #region IsSoundPlaying

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sound">The enum value for the sound in question. Check AudioManager to see what Enum you should use</param>
        /// <param name="transform">Optional: Only return true if the sound is playing from this transform</param>
        /// <returns>True if a sound that was played using PlaySound is currently playing</returns>
        public bool IsSoundPlaying<T>(T sound, Transform transform = null) where T : Enum =>
            AudioManager.IsSoundPlaying(sound, transform);

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sound">The enum value for the sound in question. Check AudioManager to see what Enum you should use</param>
        /// <param name="position">Only return true if the sound is played at this position</param>
        /// <returns><inheritdoc cref="IsSoundPlaying{T}(T, Transform)" path="/returns"/></returns>
        public bool IsSoundPlaying<T>(T sound, Vector3 position) where T : Enum =>
            AudioManager.IsSoundPlaying(sound, position);


        /// <summary>
        /// Very similar use case as TryGetComponent
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sound">The enum of the music in question, check AudioManager to see what enums you can use</param>
        /// <param name="helper">This helper reference will be given a value if the method returns true</param>
        /// <returns>The first Sound Helper that's currently playing the specified music</returns>
        public bool TryGetPlayingSound<T>(T sound, out SoundChannelHelper helper) where T : Enum =>
            AudioManager.TryGetPlayingSound(sound, out helper);

        #endregion

        #region PlayMusic

        /// <summary>
        /// Play Music globally without spatialization
        /// Supports built-in music transition operations
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="music">Enum value for the music to be played. You can find this in the AudioLibrary</param>
        /// <param name="isMainMusic">If true, defines the music as the "Main Music", making future operations easier</param>
        /// <returns>The Music Channel helper playing the sound, useful for transitions, like copying the playback position to the next music</returns>
        public MusicChannelHelper PlayMusic<T>(T music, bool isMainMusic) where T : Enum
        {
            return AudioManager.PlayMusic(music, isMainMusic);
        }

        public MusicChannelHelper PlayMusic(MusicFileObject music, bool isMainMusic)
        {
            return AudioManager.PlayMusic(music, isMainMusic);
        }

        /// <summary>
        /// Plays the specified music using the settings provided in the Music File Object. 
        /// Supports spatialization
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="music">Enum value for the music to be played. You can find this in the AudioLibrary</param>
        /// <param name="transform">Optional: The transform of the music's source</param>
        /// <param name="helper">Optional: The specific channel you want to play the sound from. 
        /// <para>Good if you want an entity to only play a single music at any time</para></param>
        /// <returns><inheritdoc cref="PlayMusic{T}(T, bool)" path="/returns"/></returns>
        public MusicChannelHelper PlayMusic<T>(T music, Transform transform = null,
            MusicChannelHelper helper = null) where T : Enum
        {
            return AudioManager.PlayMusic(music, transform, helper);
        }

        /// <summary>
        /// <inheritdoc cref="PlayMusic{T}(T, Transform, MusicChannelHelper)"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="music">Enum value for the music to be played. You can find this in the AudioLibrary</param>
        /// <param name="position">The world position you want the music to play from</param>
        /// <param name="helper">Optional: The specific channel you want to play the sound from. 
        /// <para>Good if you want an entity to only play a single music at any time</para></param>
        /// <returns><inheritdoc cref="PlayMusic{T}(T, bool)" path="/returns"/></returns>
        public MusicChannelHelper PlayMusic<T>(T music, Vector3 position, MusicChannelHelper helper = null)
            where T : Enum
        {
            return AudioManager.PlayMusic(music, position, helper);
        }

        #endregion


        #region FadeMusic

        /// <summary>
        /// Play and fade in a new music
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="music">Enum value for the music to be played. You can find this in the AudioLibrary</param>
        /// <param name="fadeInTime">Amount of time in seconds the fade will last</param>
        /// <param name="isMainmusic">If true, defines the music as the "Main music", making future operations easier</param>
        /// <returns></returns>
        public MusicChannelHelper FadeMusicIn<T>(T music, float fadeInTime, bool isMainmusic = false)
            where T : Enum
        {
            return AudioManager.FadeMusicIn(music, fadeInTime, isMainmusic);
        }


        /// <summary>
        /// Fades out the currently designated "Main Music"
        /// </summary>
        /// <param name="fadeOutTime">Amount of time in seconds the fade will last</param>
        /// <returns></returns>
        public MusicChannelHelper FadeMainMusicOut(float fadeOutTime)
        {
            return AudioManager.FadeMainMusicOut(fadeOutTime);
        }

        /// <summary>
        /// Fades music out
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="music"></param>
        /// <param name="fadeOutTime">Amount of time in seconds the fade will last</param>
        /// <returns></returns>
        public MusicChannelHelper FadeMusicOut<T>(T music, float fadeOutTime) where T : Enum
        {
            return AudioManager.FadeMusicOut(music, fadeOutTime);
        }

        #endregion

        #region IsMusicPlaying

        /// <summary>
        /// </summary>
        /// <param name="music">The enum of the music in question, check AudioManager to see what enums you can use</param>
        /// <returns>True if music that was played through PlayMusic is currently playing</returns>
        public bool IsMusicPlaying<T>(T music) where T : Enum =>
            AudioManager.IsMusicPlaying(music);

        /// <summary>
        /// Very similar use case as TryGetComponent
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="music">The enum of the music in question, check AudioManager to see what enums you can use</param>
        /// <param name="helper">This helper reference will be given a value if the method returns true</param>
        /// <returns>The first Music Helper that's currently playing the specified music</returns>
        public bool TryGetPlayingMusic<T>(T music, out MusicChannelHelper helper) where T : Enum =>
            AudioManager.TryGetPlayingMusic(music, out helper);

        #endregion


        #region StopMusic

        /// <summary>
        /// Stops all playing music maintained by AudioManager
        /// </summary>
        public void StopAllMusic(bool stopInstantly = true) =>
            AudioManager.StopAllMusic(stopInstantly);

        /// <summary>
        /// Instantly stops the playback of the specified playing music music
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="music">The enum corresponding to the music music</param>
        /// <param name="transform">Optional: The transform of the music's source</param>
        /// <param name="stopInstantly">Optional: If false, will allow music to transition out using it's transition settings. 
        /// Otherwise, will immediately end playback</param>
        /// <returns>The Music Channel helper playing the sound, useful for transitions, like copying the playback position to the next music</returns>
        public MusicChannelHelper StopMusic<T>(T music, Transform transform = null, bool stopInstantly = true)
            where T : Enum
        {
            return AudioManager.StopMusic(music, transform, stopInstantly);
        }

        /// <summary>
        /// <inheritdoc cref="StopMusic(MusicFileObject, Transform){T}(T, Transform, JSAMMusicChannelHelper)"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="music">The enum corresponding to the music music</param>
        /// <param name="position">The world position the music is playing from</param>
        /// <param name="stopInstantly">Optional: If false, will allow music to transition out using it's transition settings. 
        /// Otherwise, will immediately end playback</param>
        /// <returns><inheritdoc cref="StopMusic{T}(T, Transform, bool)"/></returns>
        public MusicChannelHelper StopMusic<T>(T music, Vector3 position, bool stopInstantly = true)
            where T : Enum
        {
            return AudioManager.StopMusic(music, position, stopInstantly);
        }

        /// <summary>
        /// A shorthand for wrapping StopMusic in an IsMusicPlaying if-statement
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="music">The enum corresponding to the music music</param>
        /// <param name="transform">Optional: The transform of the music's source</param>
        /// <param name="stopInstantly">Optional: If false, will allow music to transition out using it's transition settings. 
        /// Otherwise, will immediately end playback</param>
        /// <returns>True if music was stopped successfully, false if music wasn't playing</returns>
        public bool StopMusicIfPlaying<T>(T music, Transform transform = null, bool stopInstantly = true)
            where T : Enum
        {
            return AudioManager.StopMusicIfPlaying(music, transform, stopInstantly);
        }

        /// <summary>
        /// <inheritdoc cref="StopMusicIfPlaying{T}(T, Transform, bool)"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="music">The enum corresponding to the music music</param>
        /// <param name="position">The world position the music is playing from</param>
        /// <param name="stopInstantly">Optional: If false, will allow music to transition out using it's transition settings. 
        /// Otherwise, will immediately end playback</param>
        /// <returns><inheritdoc cref="StopMusicIfPlaying{T}(T, Transform, bool)"/></returns>
        public bool StopMusicIfPlaying<T>(T music, Vector3 position, bool stopInstantly = true) where T : Enum =>
            AudioManager.StopMusicIfPlaying(music, position, stopInstantly);

        #endregion

        #region Volume

        /// <summary>
        /// The current overall volume from 0 to 1
        /// </summary>
        public float MasterVolume
        {
            get => AudioManager.MasterVolume;
            set => AudioManager.MasterVolume = value;
        }

        public bool MasterMuted
        {
            get => AudioManager.MasterMuted;
            set => AudioManager.MasterMuted = value;
        }

        /// <summary>
        /// Get the current volume of Music as a normalized float from 0 to 1
        /// </summary>
        public float MusicVolume
        {
            get => AudioManager.MusicVolume;
            set => AudioManager.MusicVolume = value;
        }

        public bool MusicMuted
        {
            get => AudioManager.MusicMuted;
            set
            {
                AudioManager.MusicMuted = value;
            }
        }

        /// <summary>
        /// Get the current volume of Sounds as a normalized float from 0 to 1
        /// </summary>
        public float SoundVolume
        {
            get => AudioManager.SoundVolume;
            set => AudioManager.SoundVolume = value;
        }

        public bool SoundMuted
        {
            get => AudioManager.SoundMuted;
            set => AudioManager.SoundMuted = value;
        }

        /// <summary>
        /// Get the current volume of Voices as a normalized float from 0 to 1
        /// </summary>
        public float VoiceVolume
        {
            get => AudioManager.VoiceVolume;
            set => AudioManager.VoiceVolume = value;
        }

        public bool VoiceMuted
        {
            get => AudioManager.VoiceMuted;
            set => AudioManager.VoiceMuted = value;
        }

        #endregion
    }
}