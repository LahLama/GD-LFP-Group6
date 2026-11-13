 using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("Music")]
    public AudioClip music;
    [Range(0f, 1f)] public float musicVolume = 0.4f;

    [Header("Game sound effects (empty slots are skipped)")]
    public AudioClip step;
    public AudioClip moveAdd;
    public AudioClip pickupIce;
    public AudioClip pickupFire;
    public AudioClip pickupAcid;
    public AudioClip pickupElectro;
    public AudioClip obstacleClear;
    public AudioClip respawn;
    public AudioClip levelComplete;
    public AudioClip acidControlsReversed;
    public AudioClip fireOneWayActive;

    [Header("UI sound effects")]
    public AudioClip buttonClick;
    public AudioClip buttonHover;

    [Range(0f, 1f)] public float sfxVolume = 0.8f;

    AudioSource musicSource, sfxSource, stepSource;
    PlayerProperties player;
    InteractionManager interaction;
    LevelManager levels;
    TileProperties[] tiles = new TileProperties[0];

    bool ready, refreshing;
    int lastLevel, lastCounter, lastObstacles;
    Vector2Int lastCords;
    ElementState lastElement;
    bool lastReversed, lastOneWay;

    void Awake()
    {
        musicSource = gameObject.AddComponent<AudioSource>();
        sfxSource = gameObject.AddComponent<AudioSource>();
        stepSource = gameObject.AddComponent<AudioSource>();
        
        musicSource.playOnAwake = sfxSource.playOnAwake = stepSource.playOnAwake = false;
        musicSource.loop = true;
    }

    void Start()
    {
        player = FindAnyObjectByType<PlayerProperties>();
        interaction = FindAnyObjectByType<InteractionManager>();
        levels = FindAnyObjectByType<LevelManager>();

        if (music != null)
        {
            musicSource.clip = music;
            musicSource.volume = musicVolume;
            musicSource.Play();
        }
        StartCoroutine(RefreshTiles());
    }

    // ---------- UI: call these from Button / Slider events ----------
    public void PlayButtonClick() { Play(buttonClick); }
    public void PlayButtonHover() { Play(buttonHover); }
    
    public void SetMusicVolume(float value) 
    { 
        musicVolume = value; 
        musicSource.volume = value; 
        if (value > 0) musicSource.mute = false; // Auto-unmute if slider moves
    }
    
    public void SetSfxVolume(float value) 
    { 
        sfxVolume = value; 
        if (value > 0) 
        {
            sfxSource.mute = false;
            stepSource.mute = false;
        }
    }

    // --- NEW: DIRECT ON / OFF BUTTON METHODS ---
    
    public void SetMusicOn() { musicSource.mute = false; }
    public void SetMusicOff() { musicSource.mute = true; }
    
    public void SetSFXOn() 
    { 
        sfxSource.mute = false; 
        stepSource.mute = false; 
    }
    
    public void SetSFXOff() 
    { 
        sfxSource.mute = true; 
        stepSource.mute = true; 
    }
    // --------------------------------------------

    // ---------- Game triggers (read-only watching of the game) ----------
    IEnumerator RefreshTiles()
    {
        refreshing = true;
        yield return new WaitForSeconds(0.3f);
        tiles = FindObjectsByType<TileProperties>(FindObjectsSortMode.None);
        Sync();
        refreshing = false;
        ready = true;
    }

    void Sync()
    {
        if (player == null || interaction == null) return;
        lastLevel = CurrentLevel();
        lastCounter = player.RespawnCounter;
        lastCords = player.playerCords;
        lastElement = player.playerElement;
        lastReversed = interaction.isMovementReversed;
        lastOneWay = interaction.isOneWaysActive;
        lastObstacles = CountObstacles();
    }

    void LateUpdate()
    {
        if (!ready || refreshing || player == null || interaction == null) return;

        if (CurrentLevel() != lastLevel)
        {
            Play(levelComplete);
            StartCoroutine(RefreshTiles());
            return;
        }

        if (player.RespawnCounter != lastCounter)
        {
            if (player.RespawnCounter >= 2 && player.RespawnCounter > lastCounter) Play(respawn);
            Sync();
            return;
        }

        if (player.playerCords != lastCords)
        {
            PlayStep();
            TileProperties t = TileAt(player.playerCords);
            if (t != null && t.tileState == TileState.MoveAdd) Play(moveAdd);
            lastCords = player.playerCords;
        }

        if (player.playerElement != lastElement)
        {
            PlayPickup(player.playerElement);
            lastElement = player.playerElement;
        }

        int obstacles = CountObstacles();
        if (obstacles < lastObstacles) Play(obstacleClear);
        lastObstacles = obstacles;

        if (interaction.isMovementReversed && !lastReversed) Play(acidControlsReversed);
        if (interaction.isOneWaysActive && !lastOneWay) Play(fireOneWayActive);
        lastReversed = interaction.isMovementReversed;
        lastOneWay = interaction.isOneWaysActive;
    }

    int CurrentLevel()
    {
        if (levels == null) return 0;
        for (int i = 0; i < levels.GameLevels.Count; i++)
            if (levels.GameLevels[i] != null && levels.GameLevels[i].enabled) return i;
        return -1;
    }

    int CountObstacles()
    {
        int n = 0;
        foreach (var t in tiles)
            if (t != null && t.tileState == TileState.Obstacle) n++;
        return n;
    }

    TileProperties TileAt(Vector2Int cords)
    {
        foreach (var t in tiles)
            if (t != null && t.cords == cords) return t;
        return null;
    }

    void Play(AudioClip clip)
    {
        if (clip != null) sfxSource.PlayOneShot(clip, sfxVolume);
    }

    void PlayStep()
    {
        if (step == null) return;
        stepSource.pitch = Random.Range(0.9f, 1.1f);
        stepSource.PlayOneShot(step, sfxVolume * 0.7f);
    }

    void PlayPickup(ElementState e)
    {
        switch (e)
        {
            case ElementState.Ice: Play(pickupIce); break;
            case ElementState.Fire: Play(pickupFire); break;
            case ElementState.Acid: Play(pickupAcid); break;
            case ElementState.Electro: Play(pickupElectro); break;
        }
    }
}
