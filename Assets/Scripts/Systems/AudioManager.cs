using UnityEngine;
using AfterLife.Dimensions;
using AfterLife.Interactables;
using AfterLife.Player;

namespace AfterLife.Systems
{
    /// <summary>Plays the music and sound effects. One instance survives scene changes.</summary>
    public class AudioManager : MonoBehaviour
    {
        [Header("Music")]
        [Tooltip("Looping background track.")]
        [SerializeField]
        private AudioClip music;

        [Tooltip("Loudness of the music.")]
        [SerializeField, Range(0f, 1f)]
        private float musicVolume = 0.4f;

        [Header("Effects")]
        [Tooltip("Loudness of all effects.")]
        [SerializeField, Range(0f, 1f)]
        private float effectsVolume = 0.8f;

        [Tooltip("Spirit orb pickup.")]
        [SerializeField]
        private AudioClip orbClip;

        [Tooltip("Key pickup.")]
        [SerializeField]
        private AudioClip keyClip;

        [Tooltip("Heart pickup.")]
        [SerializeField]
        private AudioClip heartClip;

        [Tooltip("Dimension switch.")]
        [SerializeField]
        private AudioClip switchClip;

        [Tooltip("Reaching the level exit.")]
        [SerializeField]
        private AudioClip exitClip;

        [Tooltip("Player jump.")]
        [SerializeField]
        private AudioClip jumpClip;

        [Tooltip("Taking damage.")]
        [SerializeField]
        private AudioClip hurtClip;

        [Tooltip("Dying.")]
        [SerializeField]
        private AudioClip deathClip;

        [Tooltip("Reaching a checkpoint.")]
        [SerializeField]
        private AudioClip checkpointClip;

        [Tooltip("Switches, locks and pressure plates.")]
        [SerializeField]
        private AudioClip mechanismClip;

        private static AudioManager instance;
        private AudioSource effectsSource;
        private AudioSource musicSource;

        /// <summary>Keeps the first instance and builds its two audio sources.</summary>
        private void Awake()
        {
            if (instance != null)
            {
                gameObject.SetActive(false); // Stops this copy from subscribing.
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);

            effectsSource = gameObject.AddComponent<AudioSource>();
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.clip = music;
            musicSource.volume = musicVolume;
            musicSource.loop = true;
        }

        /// <summary>Listens for game events.</summary>
        private void OnEnable()
        {
            Collectible.Collected += OnCollected;
            DimensionManager.PlayerSwitched += OnSwitched;
            LevelExit.PlayerReachedExit += OnExitReached;
            PlayerController.Jumped += OnJumped;
            Health.Damaged += OnDamaged;
            Checkpoint.Touched += OnCheckpointTouched;
            Interactable.Used += OnMechanismUsed;
            PressurePlate.Pressed += OnMechanismUsed;
        }

        /// <summary>Starts the music.</summary>
        private void Start()
        {
            if (music != null)
            {
                musicSource.Play();
            }
        }

        /// <summary>Stops listening.</summary>
        private void OnDisable()
        {
            Collectible.Collected -= OnCollected;
            DimensionManager.PlayerSwitched -= OnSwitched;
            LevelExit.PlayerReachedExit -= OnExitReached;
            PlayerController.Jumped -= OnJumped;
            Health.Damaged -= OnDamaged;
            Checkpoint.Touched -= OnCheckpointTouched;
            Interactable.Used -= OnMechanismUsed;
            PressurePlate.Pressed -= OnMechanismUsed;
        }

        /// <summary>Plays the sound for the kind of pickup.</summary>
        /// <param name="item">The collected item.</param>
        private void OnCollected(Collectible item)
        {
            switch (item.Type)
            {
                case CollectibleType.Orb:
                    Play(orbClip);
                    break;
                case CollectibleType.Key:
                    Play(keyClip);
                    break;
                case CollectibleType.Heart:
                    Play(heartClip);
                    break;
            }
        }

        /// <summary>Plays the dimension switch sound.</summary>
        private void OnSwitched()
        {
            Play(switchClip);
        }

        /// <summary>Plays the exit sound.</summary>
        private void OnExitReached()
        {
            Play(exitClip);
        }

        /// <summary>Plays the jump sound.</summary>
        private void OnJumped()
        {
            Play(jumpClip);
        }

        /// <summary>Plays the hurt sound, or the death sound for a fatal hit.</summary>
        /// <param name="health">The health that was damaged.</param>
        private void OnDamaged(Health health)
        {
            Play(health.IsDead ? deathClip : hurtClip);
        }

        /// <summary>Plays the checkpoint sound.</summary>
        private void OnCheckpointTouched()
        {
            Play(checkpointClip);
        }

        /// <summary>Plays the mechanism sound.</summary>
        private void OnMechanismUsed()
        {
            Play(mechanismClip);
        }

        /// <summary>Plays one effect. An empty clip slot stays silent.</summary>
        /// <param name="clip">The sound to play.</param>
        private void Play(AudioClip clip)
        {
            if (clip != null)
            {
                effectsSource.PlayOneShot(clip, effectsVolume);
            }
        }
    }
}