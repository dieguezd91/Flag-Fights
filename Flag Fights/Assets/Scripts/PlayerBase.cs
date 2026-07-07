using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    PlayerController _playerController;
    PlayerFlagCarryVisual _flagCarryVisual;
    PlayerModel _playerModel;
    PlayerView _playerView;

    void Awake()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            _playerController = player.GetComponent<PlayerController>();
            _flagCarryVisual = player.GetComponent<PlayerFlagCarryVisual>();
            _playerModel = player.GetComponent<PlayerModel>();
            _playerView  = player.GetComponent<PlayerView>();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (_playerModel == null || !_playerModel.HasFlag) return;

        _playerModel.HasFlag = false;

        GameEvents.RaiseFlagCarryChanged(_playerController, false);

        // TODO: Remove this fallback after all player prefabs require PlayerFlagCarryVisual.
        if (ShouldUseLegacyFlagVisibility())
            _playerView.SetFlagVisibility(false);

        GameEvents.RaiseFlagCaptured();
    }

    bool ShouldUseLegacyFlagVisibility()
    {
        return _flagCarryVisual == null || !_flagCarryVisual.isActiveAndEnabled;
    }
}
