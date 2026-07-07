using UnityEngine;

public class PlayerView : MonoBehaviour
{
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
}
