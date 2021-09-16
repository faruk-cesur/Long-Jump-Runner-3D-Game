using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public static AnimationController Instance;

    [SerializeField] private Animator _animator;

    private void Awake()
    {
        if (Instance != this)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void IdleAnimation()
    {
        _animator.SetBool("Idle", true);
        _animator.SetBool("Run", false);
    }

    public void WalkAnimation()
    {
        _animator.SetBool("Idle", false);
        _animator.SetBool("Walk", true);
        _animator.SetBool("SlowRun", false);
        _animator.SetBool("Run", false);
    }
    
    public void SlowRunAnimation()
    {
        _animator.SetBool("Idle", false);
        _animator.SetBool("Walk", false);
        _animator.SetBool("SlowRun", true);
        _animator.SetBool("Run", false);
    }
    
    public void RunAnimation()
    {
        _animator.SetBool("Idle", false);
        _animator.SetBool("Walk", false);
        _animator.SetBool("SlowRun", false);
        _animator.SetBool("Run", true);
    }

    public void DeathAnimation()
    {
        _animator.SetBool("Run", false);
        _animator.SetBool("Death", true);
    }

    public void WinAnimation()
    {
        _animator.SetBool("Run", false);
        _animator.SetBool("Win", true);
    }
}