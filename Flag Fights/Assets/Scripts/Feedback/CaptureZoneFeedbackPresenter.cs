using System.Collections;
using UnityEngine;

public class CaptureZoneFeedbackPresenter : MonoBehaviour
{
    [SerializeField] PlayerController player;
    [SerializeField] GameObject captureReadyIndicator;
    [SerializeField] ParticleSystem captureParticles;
    [SerializeField] GameObject captureFeedbackObject;
    [SerializeField] float captureFeedbackDuration = 1f;

    Coroutine _captureFeedbackRoutine;

    void Awake()
    {
        SetCaptureReady(false);
        SetCaptureFeedback(false);
    }

    void OnEnable()
    {
        GameEvents.OnFlagCarryChanged += OnFlagCarryChanged;
        GameEvents.OnFlagCaptured += OnFlagCaptured;
        GameEvents.OnRoundStarted += OnRoundStarted;
        GameEvents.OnRoundEnded += OnRoundEnded;
    }

    void OnDisable()
    {
        GameEvents.OnFlagCarryChanged -= OnFlagCarryChanged;
        GameEvents.OnFlagCaptured -= OnFlagCaptured;
        GameEvents.OnRoundStarted -= OnRoundStarted;
        GameEvents.OnRoundEnded -= OnRoundEnded;

        if (_captureFeedbackRoutine != null)
        {
            StopCoroutine(_captureFeedbackRoutine);
            _captureFeedbackRoutine = null;
        }

        SetCaptureReady(false);
        SetCaptureFeedback(false);
    }

    void OnRoundStarted(int round)
    {
        SetCaptureReady(false);
        if (_captureFeedbackRoutine != null)
        {
            StopCoroutine(_captureFeedbackRoutine);
            _captureFeedbackRoutine = null;
        }
        SetCaptureFeedback(false);
    }

    void OnRoundEnded(bool playerWon)
    {
        SetCaptureReady(false);
        if (_captureFeedbackRoutine != null)
        {
            StopCoroutine(_captureFeedbackRoutine);
            _captureFeedbackRoutine = null;
        }
        SetCaptureFeedback(false);
    }

    void OnFlagCarryChanged(PlayerController eventPlayer, bool hasFlag)
    {
        if (player != null && eventPlayer != player) return;

        SetCaptureReady(hasFlag);
    }

    void OnFlagCaptured()
    {
        SetCaptureReady(false);

        if (captureFeedbackObject != null)
        {
            if (_captureFeedbackRoutine != null)
                StopCoroutine(_captureFeedbackRoutine);

            _captureFeedbackRoutine = StartCoroutine(ShowCaptureFeedback());
        }

        if (captureParticles != null)
            captureParticles.Play(true);
    }

    IEnumerator ShowCaptureFeedback()
    {
        SetCaptureFeedback(true);
        yield return new WaitForSecondsRealtime(Mathf.Max(0f, captureFeedbackDuration));
        SetCaptureFeedback(false);
        _captureFeedbackRoutine = null;
    }

    void SetCaptureReady(bool isReady)
    {
        if (captureReadyIndicator == null) return;

        captureReadyIndicator.SetActive(isReady);
    }

    void SetCaptureFeedback(bool isActive)
    {
        if (captureFeedbackObject == null) return;

        captureFeedbackObject.SetActive(isActive);
    }
}

