﻿using UnityEngine;
using System.Collections;

public class MovementState : State<Player> {

    private Player player;
    private Vector2 movementInputVector;


    public MovementState(Player playerInstance, StateMachine<Player> fsm)
        : base(playerInstance, fsm)
    {
        player = playerInstance;
    }

    override public void Enter()
    {
        player.anim.SetFloat("MoveSpeed", 1.0f);
        return;
    }

    override public void Execute()
    {
        movementInputVector = Controls.GetDirection(0);

        //Might want to change this stuff later to include transition states
        if (movementInputVector.x == 0)
        {
            player.ActionFsm.ChangeState(new IdleState(player, player.ActionFsm));
            return;
        }

        if (Controls.YInputDown(0) || !player.grounded)
        {
            player.ActionFsm.ChangeState(new AirState(player, player.ActionFsm));
            return;
        }

        //Temporary measures until we get more animations.
        if(movementInputVector.x != 0)
            player.anim.SetFloat("DirX", movementInputVector.x/Mathf.Abs(movementInputVector.x));
        //player.anim.SetFloat("DirY", Mathf.Ceil(Parameters.getVector(player.direction).y));
    }

    override public void FixedExecute()
    {
        player.selfBody.velocity = new Vector2(movementInputVector.x * player.movementSpeed, player.selfBody.velocity.y);
    }

    override public void Exit()
    {
        return;
    }
}
