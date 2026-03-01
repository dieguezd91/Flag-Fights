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
        if (GameManager.instance.currentTime > GameManager.instance.lossTimer) return;

        _playerModel.HasFlag = false;
        _playerView.SetFlagVisibility(false);
        GameManager.instance.EndRound(true);
    }
}
