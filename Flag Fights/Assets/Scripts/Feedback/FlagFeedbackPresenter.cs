using UnityEngine;

public class FlagFeedbackPresenter : MonoBehaviour
{
    [SerializeField] GameObject observedFlag;
    [SerializeField] ParticleSystem pickupParticles;
    [SerializeField] GameObject pickupFeedbackObject;

    void Awake()
    {
        if (observedFlag == null)
            observedFlag = gameObject;
    }

    void OnEnable()
    {
        GameEvents.OnFlagPickedUp += OnFlagPickedUp;
    }

    void OnDisable()
    {
        GameEvents.OnFlagPickedUp -= OnFlagPickedUp;
    }

    void OnFlagPickedUp(PlayerController player, GameObject pickedFlag)
    {
        if (pickedFlag != observedFlag) return;

        if (pickupFeedbackObject != null)
            pickupFeedbackObject.SetActive(true);

        if (pickupParticles != null)
            pickupParticles.Play(true);
    }
}
