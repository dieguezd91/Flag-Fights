using UnityEngine;

public class PlayerFootstepFeedbackPresenter : MonoBehaviour
{
    [SerializeField, Tooltip("Referencia al modelo del jugador para verificar su estado e input.")] 
    private PlayerModel playerModel;

    [Header("Audio")]
    [SerializeField, Tooltip("AudioSource dedicado exclusivamente a reproducir los sonidos de pasos.")] 
    private AudioSource footstepSource;

    [SerializeField, Tooltip("Array de clips de audio para los pasos. Se seleccionan de forma aleatoria sin repetir el último.")] 
    private AudioClip[] footstepClips;

    [SerializeField, Tooltip("Rango de modulación aleatoria del pitch (tono) para cada paso.")] 
    private Vector2 pitchRange = new Vector2(0.95f, 1.05f);

    [Header("Dust")]
    [SerializeField] private ParticleSystem footstepDust;
    [SerializeField, Min(1)] private int particlesPerStep = 10;

    [Header("Step Detection")]
    [SerializeField, Min(0.1f), Tooltip("Distancia horizontal en metros que debe recorrer el jugador para producir un paso.")] 
    private float distancePerStep = 1f;

    [SerializeField, Min(0f), Tooltip("Umbral de input de movimiento mínimo para permitir la reproducción de pasos.")] 
    private float movementThreshold = 0.1f;

    [SerializeField, Min(0.1f), Tooltip("Distancia máxima permitida en un frame. Si se supera, se considera teleport/respawn y no suena paso.")] 
    private float teleportThreshold = 2f;

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
        TriggerFootstepFeedback();
    }

    private bool CanPlayFootsteps()
    {
        return isActiveAndEnabled
            && playerModel != null
            && !playerModel.IsDead
            && playerModel.CurrentMoveInput >= movementThreshold;
    }

    private void TriggerFootstepFeedback()
    {
        PlayFootstepSound();
        EmitFootstepDust();
    }

    private void PlayFootstepSound()
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

    private void EmitFootstepDust()
    {
        if (footstepDust == null)
            return;

        footstepDust.Emit(particlesPerStep);
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
