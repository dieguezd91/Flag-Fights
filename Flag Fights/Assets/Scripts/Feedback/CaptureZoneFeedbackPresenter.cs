using UnityEngine;

public class CaptureZoneFeedbackPresenter : MonoBehaviour
{
    [SerializeField] PlayerController player;
    [SerializeField] GameObject captureReadyIndicator;

    void Awake()
    {
        SetCaptureReady(false);
    }

    void OnEnable()
    {
        GameEvents.OnFlagCarryChanged += OnFlagCarryChanged;
    }

    void OnDisable()
    {
        GameEvents.OnFlagCarryChanged -= OnFlagCarryChanged;
    }

    void OnFlagCarryChanged(PlayerController eventPlayer, bool hasFlag)
    {
        if (player != null && eventPlayer != player) return;

        SetCaptureReady(hasFlag);
    }

    void SetCaptureReady(bool isReady)
    {
        if (captureReadyIndicator == null) return;

        captureReadyIndicator.SetActive(isReady);
    }
}
