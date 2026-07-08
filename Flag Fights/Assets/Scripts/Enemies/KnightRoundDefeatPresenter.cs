using UnityEngine;

public class KnightRoundDefeatPresenter : MonoBehaviour
{
    private static readonly int DefeatTriggerHash = Animator.StringToHash("Defeat");

    [SerializeField] private KnightController _knightController;
    [SerializeField] private KnightView _knightView;

    private bool _defeatPlayed = false;

    private void Awake()
    {
        if (_knightController == null)
            _knightController = GetComponent<KnightController>();
        if (_knightView == null)
            _knightView = GetComponent<KnightView>();
    }

    private void OnEnable()
    {
        GameEvents.OnRoundEndSequenceStarted += OnRoundEndSequenceStarted;
        GameEvents.OnRoundStarted += OnRoundStarted;
    }

    private void OnDisable()
    {
        GameEvents.OnRoundEndSequenceStarted -= OnRoundEndSequenceStarted;
        GameEvents.OnRoundStarted -= OnRoundStarted;
    }

    private void OnRoundEndSequenceStarted(bool playerWon)
    {
        if (!playerWon) return;
        if (_defeatPlayed) return;

        // If the GameObject or component is inactive, don't execute
        if (!gameObject.activeInHierarchy || !enabled) return;

        _defeatPlayed = true;

        // Stop Knight logic
        if (_knightController != null)
        {
            _knightController.enabled = false;
        }

        // Stop Rigidbody
        if (_knightView != null && _knightView.RB != null)
        {
            _knightView.RB.velocity = Vector3.zero;
            _knightView.RB.angularVelocity = Vector3.zero;
        }

        // Trigger defeat animation
        if (_knightView != null && _knightView._animator != null)
        {
            _knightView._animator.SetTrigger(DefeatTriggerHash);
        }
    }

    private void OnRoundStarted(int round)
    {
        _defeatPlayed = false;

        if (_knightController != null)
        {
            _knightController.enabled = true;
        }

        if (_knightView != null && _knightView.RB != null)
        {
            _knightView.RB.velocity = Vector3.zero;
            _knightView.RB.angularVelocity = Vector3.zero;
        }
    }
}
