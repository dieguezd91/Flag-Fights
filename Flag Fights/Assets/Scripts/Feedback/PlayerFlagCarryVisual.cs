using UnityEngine;

public class PlayerFlagCarryVisual : MonoBehaviour
{
    [SerializeField] PlayerController player;
    [SerializeField] GameObject carriedFlagVisual;

    bool _warnedMissingVisual;

    void Awake()
    {
        if (player == null)
            player = GetComponentInParent<PlayerController>();

        SetCarryingFlag(false);
    }

    void OnEnable()
    {
        if (carriedFlagVisual == null && !_warnedMissingVisual)
        {
            Debug.LogWarning($"{nameof(PlayerFlagCarryVisual)} on {name} has no carried flag visual assigned.", this);
            _warnedMissingVisual = true;
        }

        GameEvents.OnFlagCarryChanged += OnFlagCarryChanged;
    }

    void OnDisable()
    {
        GameEvents.OnFlagCarryChanged -= OnFlagCarryChanged;
    }

    public void SetCarryingFlag(bool isCarrying)
    {
        if (carriedFlagVisual == null) return;

        carriedFlagVisual.SetActive(isCarrying);
        carriedFlagVisual.transform.localScale = Vector3.one;
    }

    void OnFlagCarryChanged(PlayerController eventPlayer, bool hasFlag)
    {
        if (player != null && eventPlayer != player) return;

        SetCarryingFlag(hasFlag);
    }
}
