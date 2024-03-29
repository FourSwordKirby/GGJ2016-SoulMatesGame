﻿using UnityEngine;
using System.Collections;

public class DrumPuzzle : Puzzle {

   //Need to hit a drum x number of times within some constrained time frame
   //Every error leads to a small stunned penalty

    public float timeLimit;
    private float timeRemaining;
    public int beatsGoal;
    private int beatsRemaining;

    public DrumPuzzlePlayer player1;
    public DrumPuzzlePlayer player2;

	public AudioClip drumFailClip;
	public AudioClip drumSuccessClip;
	private AudioSource drumFailSound;
	private AudioSource drumSuccessSound;

    private bool P1_PRESSED;
    private bool P2_PRESSED;
    private SpriteRenderer beat1;
    private SpriteRenderer beat2;
    private Animator drum;

	private AudioSource makeAudioSource(AudioClip clip, float volume) {
		AudioSource source = gameObject.AddComponent<AudioSource>();
		source.clip = clip;
		source.loop = false;
		source.playOnAwake = false;
		source.volume = volume;
		return source;
	}

	public void Awake() {
		this.drumFailSound = this.makeAudioSource (this.drumFailClip, 0.5f);
		this.drumSuccessSound = this.makeAudioSource (this.drumSuccessClip, 0.8f);
	}

    override public void P1_ButA()
    {
        if (player1.playerStatus == DrumPuzzlePlayer.PlayerStatus.IDLE)
        {
            player1.trigger();
        }
    }

    override public void P2_ButA()
    {
        if (player2.playerStatus == DrumPuzzlePlayer.PlayerStatus.IDLE)
        {
           player2.trigger();
        }
    }

    override public float GetTimeRemaining()
    {
        return timeRemaining;
    }

    override public float GetTimeLimit()
    {
        return timeLimit;
    }

    override public int GetDifficulty()
    {
        return 0;
    }

    /// <summary>
    /// Give the puzzle a name. My be useful later.
    /// </summary>
    override public string GetName()
    {
        return "Drums";
    }

    /// <summary>
    /// This should be called exactly once before actually running a puzzle.
    /// This will initialize all variables needed by the puzzle.
    /// Only put code that can execute in 1 frame here.
    /// </summary>
    override public void Setup()
    {
        timeRemaining = timeLimit;
        beatsRemaining = beatsGoal;
        Vector3 camPosition = this.transform.FindChild("Camera Point").position;
        Camera cam1 = GameObject.Find("P1 Camera").GetComponent<Camera>();
        cam1.transform.position = camPosition;
        Camera cam2 = GameObject.Find("P2 Camera").GetComponent<Camera>();
        cam2.transform.position = camPosition;
        //Initialize position of players?
        //Initialize some stats?

        beat1 = this.transform.FindChild("beat 1").GetComponent<SpriteRenderer>();
        beat2 = this.transform.FindChild("beat 2").GetComponent<SpriteRenderer>();
        drum = this.transform.FindChild("Drum").GetComponent<Animator>();
    }

    /// <summary>
    /// This should be called exactly once when the puzzle ends.
    /// This will do any cleanup for the puzzle (probably not needed??)
    /// Only put code that can execute in 1 frame here.
    /// </summary>
    override public void Cleanup()
    {
        Debug.Log("puzzle finished, should I ramp up the level of this puzzle?");
    }

    /// <summary>
    /// This is the main execution method for a puzzle.
    /// Should be called every Update() to run.
    /// </summary>
    override public void Execute()
    {
        timeRemaining -= Time.deltaTime;
        if (player1.playerStatus == DrumPuzzlePlayer.PlayerStatus.PRIMED && player2.playerStatus == DrumPuzzlePlayer.PlayerStatus.PRIMED)
        {
            beatsRemaining--;
            player1.succeed();
            player2.succeed();
			drumSuccessSound.Play ();
            drum.SetTrigger("Shake");
            if(beatsRemaining == 1)
            {
                beat1.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            }
            else if (beatsRemaining == 0)
            {
                beat2.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            }
        }

        if (player1.playerStatus == DrumPuzzlePlayer.PlayerStatus.PENALIZED || player2.playerStatus == DrumPuzzlePlayer.PlayerStatus.PENALIZED)
        {
            player1.penalize();
			player2.penalize();
			drumFailSound.Play ();
        }
    }

    /// <summary>
    /// This is the fixed execution method for a puzzle.
    /// Should be called in every FixedUpdate().
    /// Put physics related manipulations here.
    /// </summary>
    override public void FixedExecute()
    {
        //NO IDEA????
    }

    /// <summary>
    /// Returns the current puzzle status.
    /// Use this to determine if the puzzle is finished or in progress.
    /// Feel free to use PuzzleStatus.SPECIAL to do weird stuff with
    /// the PuzzleManager.
    /// </summary>
    override public PuzzleStatus Status()
    {
        if (beatsRemaining == 0)
        {
            return PuzzleStatus.SUCCESS;
        }
        else if (timeRemaining < 0)
        {
            return PuzzleStatus.FAIL;
        }
        else
        {
            return PuzzleStatus.INPROGRESS;
        }
    }
    
    private const string INSTRUCTIONS = "Hit on the same beat!";
    private const string CONTROLS = "A";

    public override string GetP1Instructions()
    {
        return INSTRUCTIONS;
    }

    public override string GetP2Instructions()
    {
        return INSTRUCTIONS;
    }

    public override string GetP1Controls()
    {
        return CONTROLS;
    }

    public override string GetP2Controls()
    {
        return CONTROLS;
    }
}
