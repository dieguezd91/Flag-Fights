using UnityEngine;

public class KnightRoundDefeatPresenter : MonoBehaviour
{
    [SerializeField] private KnightController _knightController;

    private void Awake()
    {
        if (_knightController == null)
            _knightController = GetComponent<KnightController>();
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
        if (!gameObject.activeInHierarchy || !enabled) return;

        if (_knightController != null)
        {
            _knightController.EnterRoundDefeat();
        }
    }

    private void OnRoundStarted(int round)
    {
        if (_knightController != null)
        {
            _knightController.ResetForRound();
        }
    }
}
