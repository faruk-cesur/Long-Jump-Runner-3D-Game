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
        _animator.applyRootMotion = true;
        _animator.SetBool("Idle", true);
        _animator.SetBool("Run", false);
    }

    public void WalkAnimation()
    {
        _animator.applyRootMotion = false;
        _animator.SetBool("Idle", false);
        _animator.SetBool("Walk", true);
        _animator.SetBool("SlowRun", false);
        _animator.SetBool("Run", false);
    }

    public void SlowRunAnimation()
    {
        _animator.applyRootMotion = false;
        _animator.SetBool("Idle", false);
        _animator.SetBool("Walk", false);
        _animator.SetBool("SlowRun", true);
        _animator.SetBool("Run", false);
    }

    public void RunAnimation()
    {
        _animator.applyRootMotion = false;
        _animator.SetBool("Idle", false);
        _animator.SetBool("Walk", false);
        _animator.SetBool("SlowRun", false);
        _animator.SetBool("Run", true);
    }

    public IEnumerator InjuredRun()
    {
        _animator.SetBool("InjuredRun", true);

        yield return new WaitForSeconds(2f);
        
        _animator.SetBool("InjuredRun", false);
    }

    public IEnumerator JumpAnimation()
    {
        _animator.applyRootMotion = true;
        _animator.SetBool("Walk", false);
        _animator.SetBool("SlowRun", false);
        _animator.SetBool("Run", false);
        _animator.SetBool("Jump", true);
        yield return new WaitForSeconds(0.833f);
        FallAnimation();
    }

    public void FallAnimation()
    {
        _animator.SetBool("Jump", false);
        _animator.SetBool("Fall", true);
    }

    public void LandAnimation()
    {
        _animator.SetBool("Fall", false);
        _animator.SetBool("Land", true);
    }

    public void WinAnimation()
    {
        _animator.SetBool("Land", false);
        _animator.SetBool("Win", true);
    }

    public void DeathAnimation()
    {
        _animator.applyRootMotion = true;
        _animator.SetBool("Walk", false);
        _animator.SetBool("SlowRun", false);
        _animator.SetBool("Run", false);
        _animator.SetBool("Death", true);
    }
}