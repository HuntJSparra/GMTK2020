﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMBShopping : StateMachineBehaviour
{
    public enum Shopper
    {
        hero
    }
    public enum Action
    {
        RequestItem,
        RefusePurchase,
        AcceptPurchase,
        Leave,
        GiveChoice
    }

    public Shopper shopperName;
    public Action action;

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       if (shopperName == Shopper.hero)
        {
            GameManager.instance.HeroAppears.Invoke();
            switch (action)
            {
                case (Action.RequestItem):
                    if (GameManager.instance.hero.ChooseItem())
                    {
                        GameManager.instance.SetText(GameManager.instance.hero.RequestString());
                    }
                    break;
                case (Action.GiveChoice):
                    animator.SetInteger("ChoiceResponse", 0);
                    GameManager.instance.GiveChoice("Please use it well.", "Um, well…you’re not a high enough level to use that.");
                    break;
                case (Action.RefusePurchase):
                    GameManager.instance.SetText(GameManager.instance.hero.LeaveString());
                    break;
                case (Action.AcceptPurchase):
                    GameManager.instance.hero.BuyItem();
                    GameManager.instance.SetText(GameManager.instance.hero.LeaveString());
                    break;
                case (Action.Leave):
                    animator.SetBool("Sell2Hero", false);
                    GameManager.instance.hero.sprite.enabled = false;
                    GameManager.instance.SetText("");
                    break;
            }


        }   
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
