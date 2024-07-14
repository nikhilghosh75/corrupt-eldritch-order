using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using WSoft.Audio;

public class UIAudioListener : MonoBehaviour
{
    [Header("Enemy Events")]
    public AudioEvent enemyDeath;

    [Header("Player Events")]
    public AudioEvent playerShoot;
    public AudioEvent playerDash;
    public AudioEvent playerJump;
    public AudioEvent playerDoubleJump;
    public AudioEvent currency;
    public AudioEvent playerLowHealth;

    [Header("UI Events")]
    public AudioEvent placeholderPowerup;
    public AudioEvent placeholderPause;
    public AudioEvent placeholderResume; //not sure if we need a resume event

    [Header("Ambience")]
    public AudioEvent overgrownPlay;
    public AudioEvent overgrownStop;
    public AudioEvent industrialPlay;
    public AudioEvent industrialStop;
    public AudioEvent organicPlay;
    public AudioEvent organicStop;

    [Header("Music")]
    public AudioEvent levelPlay;
    public AudioEvent levelStop;
    public AudioEvent bossPlay;
    public AudioEvent bossStop;

    public GameObject player;

    public GameObject globalLevel;

    private LevelTheme currentTheme = LevelTheme.Default;

    private bool bossPlaying = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        overgrownPlay.PlayAudio(gameObject);
        levelPlay.PlayAudio(gameObject);

        EventBus.Subscribe((EnemyKilledEvent e) =>
        {
            //enemyDeath?.PlayAudio(gameObject);
        });

        EventBus.Subscribe((ProjectileShotEvent e) =>
        {
            playerShoot?.PlayAudio(gameObject);
        });

        EventBus.Subscribe((DashEvent e) =>
        {
            playerDash?.PlayAudio(gameObject);
        });

        EventBus.Subscribe((JumpEvent e) =>
        {
            if (player.GetComponent<PlayerController>().airController.jumpsRemaining == 1)
            {
                playerDoubleJump?.PlayAudio(gameObject);
            }
            else
            {
                playerJump?.PlayAudio(gameObject);
            }
        });


        EventBus.Subscribe((PowerupEvent e) =>
        {
            placeholderPowerup?.PlayAudio(gameObject);
        });

        EventBus.Subscribe((PauseEvent e) =>
        {
            placeholderPause?.PlayAudio(gameObject);
        });

        EventBus.Subscribe((ResumeEvent e) =>
        {
            placeholderResume?.PlayAudio(gameObject);
        });

        EventBus.Subscribe((CurrencyAddedEvent e) => 
        {
            currency?.PlayAudio(gameObject);
        });

        EventBus.Subscribe((UpdateHealthEvent e) =>
        {
            if (e.health == 1)
            {
                playerLowHealth?.PlayAudio(gameObject);
            }
        });

        EventBus.Subscribe((RoomEnterEvent e) =>
        {
            CompareRoomThemes(e.levelTheme);
            CheckBoss(e.isBossRoom);
        });

        EventBus.Subscribe((RestartLevelEvent e) =>
        {
            overgrownStop.PlayAudio(gameObject);
            industrialStop.PlayAudio(gameObject);
            organicStop.PlayAudio(gameObject);
            levelStop.PlayAudio(gameObject);
            bossStop.PlayAudio(gameObject);
        }
        );
    }

    void Update()
    {
        
    }

    void CompareRoomThemes(LevelTheme newTheme)
    {
        if (newTheme != currentTheme)
        {
            switch (currentTheme)
            {
                case LevelTheme.Default:
                    overgrownStop.PlayAudio(gameObject);
                    break;
                case LevelTheme.Industrial:
                    industrialStop.PlayAudio(gameObject);
                    break;
                case LevelTheme.Organic:
                    organicStop.PlayAudio(gameObject);
                    break;
                default:
                    break;
            }
            currentTheme = newTheme;
            switch (currentTheme)
            {
                case LevelTheme.Default:
                    overgrownPlay.PlayAudio(gameObject);
                    break;
                case LevelTheme.Industrial:
                    industrialPlay.PlayAudio(gameObject);
                    break;
                case LevelTheme.Organic:
                    organicPlay.PlayAudio(gameObject);
                    break;
                default:
                    break;
            }
        }
    }

    void CheckBoss(bool boss)
    {
        if (boss && !bossPlaying)
        {
            levelStop.PlayAudio(gameObject);
            bossPlay.PlayAudio(gameObject);
            bossPlaying = true;
        }
    }

    private void OnDestroy()
    {
        overgrownStop.PlayAudio(gameObject);
        industrialStop.PlayAudio(gameObject);
        organicStop.PlayAudio(gameObject);
        levelStop.PlayAudio(gameObject);
        bossStop.PlayAudio(gameObject);
    }
}

public class RoomEnterEvent
{
    public LevelTheme levelTheme;
    public bool isBossRoom = false;

    public RoomEnterEvent(LevelTheme ltheme, bool isBoss)
    {
        levelTheme = ltheme;
        isBossRoom = isBoss;
    }

}
