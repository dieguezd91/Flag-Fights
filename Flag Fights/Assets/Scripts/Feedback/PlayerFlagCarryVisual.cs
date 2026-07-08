using UnityEngine;

public class PlayerFlagCarryVisual : MonoBehaviour
{
    static readonly int IsCarryingFlagHash = Animator.StringToHash("HasFlag");

    [SerializeField] PlayerController player;
    [SerializeField] GameObject carriedFlagVisual;
    [SerializeField] GameObject carryIndicator;
    [SerializeField] Animator animator;
    [SerializeField] bool updateAnimatorCarryParameter;

    bool _warnedMissingVisual;
    bool _hasAnimatorCarryParameter;
    bool _ignoreCarryIndicator;

    void Awake()
    {
        if (player == null)
            player = GetComponentInParent<PlayerController>();

        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        _hasAnimatorCarryParameter = HasAnimatorBoolParameter(animator, IsCarryingFlagHash);

        if (carriedFlagVisual != null && carryIndicator != null && carriedFlagVisual == carryIndicator)
        {
            Debug.LogWarning($"{nameof(PlayerFlagCarryVisual)} on {name} has carriedFlagVisual and carryIndicator pointing to the same GameObject. Ignoring carryIndicator to avoid double control.", this);
            _ignoreCarryIndicator = true;
        }

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

        bool initialHasFlag = false;
        if (player != null)
        {
            var model = player.GetComponent<PlayerModel>();
            if (model != null)
                initialHasFlag = model.HasFlag;
        }
        else
        {
            var model = GetComponentInParent<PlayerModel>();
            if (model != null)
                initialHasFlag = model.HasFlag;
        }

        SetCarryingFlag(initialHasFlag);
    }

    void OnDisable()
    {
        GameEvents.OnFlagCarryChanged -= OnFlagCarryChanged;
    }

    public void SetCarryingFlag(bool isCarrying)
    {
        if (carriedFlagVisual != null)
        {
            carriedFlagVisual.SetActive(isCarrying);
            carriedFlagVisual.transform.localScale = Vector3.one;
        }

        if (carryIndicator != null && !_ignoreCarryIndicator)
            carryIndicator.SetActive(isCarrying);

        if (updateAnimatorCarryParameter && animator != null && _hasAnimatorCarryParameter)
            animator.SetBool(IsCarryingFlagHash, isCarrying);
    }

    void OnFlagCarryChanged(PlayerController eventPlayer, bool hasFlag)
    {
        if (player != null && eventPlayer != player) return;

        SetCarryingFlag(hasFlag);
    }

    static bool HasAnimatorBoolParameter(Animator targetAnimator, int parameterHash)
    {
        if (targetAnimator == null) return false;

        foreach (AnimatorControllerParameter parameter in targetAnimator.parameters)
        {
            if (parameter.nameHash == parameterHash && parameter.type == AnimatorControllerParameterType.Bool)
                return true;
        }

        return false;
    }
}

