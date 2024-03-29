﻿using UnityEngine;
using System.Collections;

public class KissPuzzlePlayer : MonoBehaviour {
    public float penalty;

    private const float PENALTY_DURATION = 1.0f;
    private const float PRIME_DURATION = 0.5f;
    private const float START_DURATION = 1.0f;

    private float primeTime;
    private float penaltyTimer;
    private float startTime;

    public enum PlayerStatus
    {
        IDLE,
        STARTING,
        PRIMED,
        SUCCESS,
        PENALIZED,
        RECOVERING
    }
    public PlayerStatus playerStatus { get; private set; }

    /*references to components*/
	private Animator anim;

	private AudioSource makeAudioSource(AudioClip clip, float volume) {
		AudioSource source = gameObject.AddComponent<AudioSource>();
		source.clip = clip;
		source.loop = false;
		source.playOnAwake = false;
		source.volume = volume;
		return source;
	}

    void Start()
    {
        playerStatus = PlayerStatus.IDLE;

        this.anim = this.GetComponent<Animator>();
    }

    public void Trigger()
    {
        anim.SetTrigger("Start");
        anim.speed = 1 / START_DURATION;
        playerStatus = PlayerStatus.STARTING;
        startTime = START_DURATION;
    }

    public void Primed()
    {
        anim.speed = 1;
        playerStatus = PlayerStatus.PRIMED;
        anim.SetTrigger("Prime");
        primeTime = PRIME_DURATION;
    }

    public void Succeed()
    {
        playerStatus = PlayerStatus.SUCCESS;
        anim.SetTrigger("Succeed");
    }

    public void Penalize()
    {
        playerStatus = PlayerStatus.PENALIZED;
		this.transform.parent.GetComponent<KissPuzzle> ().kissFailSound.Play ();
        anim.SetTrigger("Penalize");
        penaltyTimer = PENALTY_DURATION;
    }
    
    public void Update()
    {
        switch(playerStatus)
        {
            case PlayerStatus.STARTING:
                startTime -= Time.deltaTime;
                if(startTime < 0.0f)
                {
                    Primed();
                }
                break;
            case PlayerStatus.PRIMED:
                primeTime -= Time.deltaTime;
                if(primeTime < 0.0f)
                {
                    Penalize();
                }
                break;

            case PlayerStatus.PENALIZED:
                penaltyTimer -= Time.deltaTime;
                if (penaltyTimer <= 0)
                {
                    anim.SetTrigger("Idle");
                    playerStatus = PlayerStatus.IDLE;
                }
                break;

            default:
                break;
        }
    }
}
