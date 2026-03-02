using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    PlayerModel _playerModel;
    PlayerView _playerView;

    void Awake()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            _playerModel = player.GetComponent<PlayerModel>();
            _playerView  = player.GetComponent<PlayerView>();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (_playerModel == null || !_playerModel.HasFlag) return;

        _playerModel.HasFlag = false;
        _playerView.SetFlagVisibility(false);
        GameEvents.OnFlagCaptured?.Invoke();
    }
}
