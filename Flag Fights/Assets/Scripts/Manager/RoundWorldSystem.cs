using UnityEngine;

public class RoundWorldSystem : MonoBehaviour
{
    [SerializeField] PlayerController _playerController;
    [SerializeField] Transform _playerInitialTransform;
    [SerializeField] EnemyBase _enemyBase;
    [SerializeField] FlagSpawner _flagSpawner;

    GameObject[] _enemies;

    public void SetupRound()
    {
        if (_playerController != null && _playerInitialTransform != null)
        {
            _playerController.transform.SetPositionAndRotation(
                _playerInitialTransform.position,
                _playerInitialTransform.rotation);

            Physics.SyncTransforms(); // Sync physics transforms immediately so Cinemachine reads the new position

            GameEvents.RaisePlayerRespawned(_playerController);

            // Reset Cinemachine virtual cameras previous state to prevent interpolation/damping from the old position
            var virtualCameras = FindObjectsOfType<Cinemachine.CinemachineVirtualCameraBase>();
            foreach (var vcam in virtualCameras)
            {
                vcam.PreviousStateIsValid = false;
            }
        }

        _playerController?.ResetRoundState();

        _flagSpawner?.InitializeSpawner();

        if (_enemies == null || _enemies.Length == 0 || _enemies[0] == null)
        {
            _enemyBase?.InitializeBase();
            _enemies = GameObject.FindGameObjectsWithTag("Enemy");
        }
    }

    public void ResetActors()
    {
        if (_enemies == null) return;
        for (int n = 0; n < _enemies.Length; n++)
        {
            if (_enemies[n] == null) continue;
            _enemies[n].GetComponent<GoblinController>()?.ResetEnemy();
            _enemies[n].GetComponent<KnightController>()?.ResetEnemy();
        }
    }
}
