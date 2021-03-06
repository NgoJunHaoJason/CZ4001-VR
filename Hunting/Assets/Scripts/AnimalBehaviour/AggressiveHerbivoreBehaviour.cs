﻿using UnityEngine;


public class AggressiveHerbivoreBehaviour : AggressiveAnimalBehaviour
{
    void FixedUpdate()
    {
        PreventChildrenDetach();

        if (health.IsDead)
        {
            Die();
        }
        else if (reach.HasPlayerInRange && health.AttackedByPlayer)
        {
            if (health.ShouldFlee)
                Flee();
            else
                Attack(true);
        }
        else if (sight.HasPlayerInRange && health.AttackedByPlayer)
        {
            if (health.ShouldFlee)
                Flee();
            else 
                Chase(sight.PlayerInRange);
        }
        else if (health.IsRecentlyDamaged())
        {
            Flee();
        }
        else if (sight.HasCarnivoreInRange)
        {
            Flee();
        }
        else if (!destinationReached)
        {
            if (fleeing)
                Run();
            else
                Walk();
        }
        else
        {
            RandomIdle();
        }
    }

    protected override void RandomIdle()
    {
        if (actionTimer > 0)
        {
            actionTimer -= Time.deltaTime;
            return;
        }

        fleeing = false;
        switch (Random.Range(0, 3))
        {
            case 0:
                ChangeDestination(null, 1f);
                Walk();
                break;
            case 1:
                Idle();
                break;
            case 2:
                Eat();
                break;
        }

        chaseTimer = idleActionInterval;
        actionTimer = idleActionInterval;

    }

    protected override void ChangeAnimation(AnimalAnimation newAnimation)
    {
        if (currentAnimation != newAnimation)
        {
            currentAnimation = newAnimation;
            animator.SetBool("Attack", false);
            animator.SetBool("Eat", false);
            animator.SetBool("Run", false);
            animator.SetBool("Walk", false);

            switch (currentAnimation)
            {
                case AnimalAnimation.ATTACK:
                    animator.SetBool("Attack", true);
                    break;
                case AnimalAnimation.DIE:
                    animator.SetBool("Die", true);
                    audioSource.PlayOneShot(audioSource.clip);
                    break;
                case AnimalAnimation.EAT:
                    animator.SetBool("Eat", true);
                    break;
                case AnimalAnimation.RUN:
                    animator.SetBool("Run", true);
                    break;
                case AnimalAnimation.WALK:
                    animator.SetBool("Walk", true);
                    break;
                default:
                    break;
            }
        }
    }

}
