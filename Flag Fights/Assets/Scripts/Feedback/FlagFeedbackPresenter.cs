using System.Collections;
using UnityEngine;

public class FlagFeedbackPresenter : MonoBehaviour
{
    [SerializeField] GameObject observedFlag;
    [SerializeField] ParticleSystem pickupParticles;
    [SerializeField] GameObject pickupFeedbackObject;
    [SerializeField] float pickupFeedbackDuration = 1f;

    Coroutine _pickupFeedbackRoutine;

    void Awake()
    {
        if (observedFlag == null)
            observedFlag = gameObject;

        SetPickupFeedback(false);
    }

    void OnEnable()
    {
        GameEvents.OnFlagPickedUp += OnFlagPickedUp;
    }

    void OnDisable()
    {
        GameEvents.OnFlagPickedUp -= OnFlagPickedUp;

        if (_pickupFeedbackRoutine != null)
        {
            StopCoroutine(_pickupFeedbackRoutine);
            _pickupFeedbackRoutine = null;
        }

        SetPickupFeedback(false);
    }

    void OnFlagPickedUp(PlayerController player, GameObject pickedFlag)
    {
        if (pickedFlag != observedFlag) return;

        if (pickupFeedbackObject != null)
        {
            if (_pickupFeedbackRoutine != null)
                StopCoroutine(_pickupFeedbackRoutine);

            _pickupFeedbackRoutine = StartCoroutine(ShowPickupFeedback());
        }

        if (pickupParticles != null)
            pickupParticles.Play(true);
    }

    IEnumerator ShowPickupFeedback()
    {
        SetPickupFeedback(true);
        yield return new WaitForSecondsRealtime(Mathf.Max(0f, pickupFeedbackDuration));
        SetPickupFeedback(false);
        _pickupFeedbackRoutine = null;
    }

    void SetPickupFeedback(bool isActive)
    {
        if (pickupFeedbackObject == null) return;

        pickupFeedbackObject.SetActive(isActive);
    }
}
