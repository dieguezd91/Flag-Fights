using UnityEngine;

public class PlayerView : MonoBehaviour
{
    private static readonly int IdleHash = Animator.StringToHash("Idle");
    private static readonly int RunningHash = Animator.StringToHash("Running");
    private static readonly int DeadHash = Animator.StringToHash("Dead");
    private static readonly int HasFlagHash = Animator.StringToHash("HasFlag");

    public Animator Animator { get; private set; }
    public GameObject flag;

    private void Awake()
    {
        Animator = GetComponent<Animator>();
        if (flag != null) flag.SetActive(false);
    }

    public void SetFlagVisibility(bool isVisible)
    {
        if (flag == null) return;

        flag.SetActive(isVisible);
        flag.transform.localScale = Vector3.one;
    }

    public void PlayDeath()
    {
        if (Animator == null) return;

        Animator.SetBool(IdleHash, false);
        Animator.SetBool(RunningHash, false);
        Animator.SetBool(HasFlagHash, false);
        Animator.SetBool(DeadHash, true);
    }

    public void ClearDeath()
    {
        if (Animator == null) return;

        Animator.SetBool(DeadHash, false);
        Animator.SetBool(IdleHash, true);
        Animator.SetBool(RunningHash, false);
        Animator.SetBool(HasFlagHash, false);
    }

    public void ResetToIdle()
    {
        if (Animator == null) return;

        Animator.SetBool(DeadHash, false);
        Animator.SetBool(RunningHash, false);
        Animator.SetBool(HasFlagHash, false);
        Animator.SetBool(IdleHash, true);

        Animator.Play("Idle", 0, 0f);
        Animator.Update(0f);
    }

    public void SetIdle(bool value)
    {
        if (Animator == null) return;
        Animator.SetBool(IdleHash, value);
    }

    public void SetRunning(bool value)
    {
        if (Animator == null) return;
        Animator.SetBool(RunningHash, value);
    }
}