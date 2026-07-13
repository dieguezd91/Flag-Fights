using UnityEngine;

public class PlayerFootstepFeedbackPresenter : MonoBehaviour
{
    [SerializeField] private PlayerModel playerModel;

    [Header("Audio")]
    [SerializeField] private AudioSource footstepSource;
    [SerializeField] private AudioClip[] footstepClips;
    [SerializeField] private Vector2 pitchRange = new Vector2(0.95f, 1.05f);

    [Header("Step Detection")]
    [SerializeField, Min(0.1f)] private float distancePerStep = 1f;
    [SerializeField, Min(0f)] private float movementThreshold = 0.1f;
    [SerializeField, Min(0.1f)] private float teleportThreshold = 2f;

    private Vector3 _lastPosition;
    private float _accumulatedDistance;
    private int _lastClipIndex = -1;

    private void Awake()
    {
        if (playerModel == null)
        {
            playerModel = GetComponent<PlayerModel>();
        }

        _lastPosition = transform.position;
    }

    private void OnEnable()
    {
        _lastPosition = transform.position;
        _accumulatedDistance = 0f;
    }

    private void Update()
    {
        Vector3 currentPosition = transform.position;
        Vector3 displacement = currentPosition - _lastPosition;
        displacement.y = 0f;

        _lastPosition = currentPosition;

        if (!CanPlayFootsteps())
        {
            _accumulatedDistance = 0f;
            return;
        }

        float travelledDistance = displacement.magnitude;

        if (travelledDistance >= teleportThreshold)
        {
            _accumulatedDistance = 0f;
            return;
        }

        _accumulatedDistance += travelledDistance;

        if (_accumulatedDistance < distancePerStep)
            return;

        _accumulatedDistance %= distancePerStep;
        PlayFootstep();
    }

    private bool CanPlayFootsteps()
    {
        return isActiveAndEnabled
            && playerModel != null
            && !playerModel.IsDead
            && playerModel.CurrentMoveInput >= movementThreshold;
    }

    private void PlayFootstep()
    {
        if (footstepSource == null)
            return;

        if (footstepClips == null || footstepClips.Length == 0)
            return;

        int clipIndex = SelectClipIndex();
        if (clipIndex < 0)
            return;

        float minPitch = Mathf.Min(pitchRange.x, pitchRange.y);
        float maxPitch = Mathf.Max(pitchRange.x, pitchRange.y);

        footstepSource.pitch = Random.Range(minPitch, maxPitch);
        footstepSource.PlayOneShot(footstepClips[clipIndex]);

        _lastClipIndex = clipIndex;
    }

    private int SelectClipIndex()
    {
        int totalValid = 0;
        for (int i = 0; i < footstepClips.Length; i++)
        {
            if (footstepClips[i] != null)
            {
                totalValid++;
            }
        }

        if (totalValid == 0)
            return -1;

        if (totalValid == 1)
        {
            for (int i = 0; i < footstepClips.Length; i++)
            {
                if (footstepClips[i] != null)
                    return i;
            }
        }

        // totalValid >= 2
        bool lastClipIsValid = _lastClipIndex >= 0 
            && _lastClipIndex < footstepClips.Length 
            && footstepClips[_lastClipIndex] != null;

        int candidatesCount = lastClipIsValid ? totalValid - 1 : totalValid;
        int targetCandidateIndex = Random.Range(0, candidatesCount);

        int validIndexSeen = 0;
        for (int i = 0; i < footstepClips.Length; i++)
        {
            if (footstepClips[i] == null)
                continue;

            if (lastClipIsValid && i == _lastClipIndex)
                continue;

            if (validIndexSeen == targetCandidateIndex)
            {
                return i;
            }
            validIndexSeen++;
        }

        return -1;
    }
}
