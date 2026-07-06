using UnityEngine;

public class PlayerView : MonoBehaviour
{
    public Animator Animator { get; private set; }
    public GameObject flag;
    public bool HasFlagReference => flag != null;

    private void Awake()
    {
        Animator = GetComponent<Animator>();

        if (Animator == null)
        {
            Debug.Log("Animator null");
        }

        EnsureFlagReference();
    }

    public void SetFlagVisibility(bool isVisible)
    {
        EnsureFlagReference();

        if (flag == null)
        {
            Debug.LogError($"[{nameof(PlayerView)}] Cannot set hand flag visibility because flag is not assigned on {name}.", this);
            return;
        }

        flag.SetActive(isVisible);
        Debug.Log($"[{nameof(PlayerView)}] Hand flag visibility set to {isVisible}.", this);

        if (isVisible)
        {
            flag.transform.PopIn();
        }
    }

    private void EnsureFlagReference()
    {
        if (flag != null) return;

        foreach (Transform child in GetComponentsInChildren<Transform>(true))
        {
            if (child != transform && child.CompareTag("Flag"))
            {
                flag = child.gameObject;
                Debug.LogWarning($"[{nameof(PlayerView)}] Recovered missing hand flag reference from child {flag.name}.", this);
                return;
            }
        }

        Debug.LogError($"[{nameof(PlayerView)}] Hand flag reference is missing on {name}. Assign the disabled flag child in the PlayerView inspector.", this);
    }
}