using DG.Tweening;
using UnityEngine;

public class Money : MonoBehaviour
{
    PlayerController _playerController;
    MoneyManager _moneyManager;

    private void Start()
    {
        _moneyManager = MoneyManager.instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerController player))
        {
            _playerController = player;
            CollectingMoney();
        }
    }


    void CollectingMoney()
    {
        GameObject mImage;

        transform.DOJump(_playerController.transform.position, 1, 1, 0.1f).OnComplete(() =>
        {
            Destroy(this.gameObject);
            CodeMonkey.Utils.FunctionTimer.Create(() =>
            {
                var position = CameraFollow.instance.GetComponent<Camera>()
                    .WorldToScreenPoint(_playerController.transform.position);
                mImage = Instantiate(_moneyManager.moneyImage, position, Quaternion.identity,
                    _moneyManager.moneyIcon.parent);
                mImage.transform.DOMove(_moneyManager.moneyIcon.position, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
                {
                    var moneyCount = PlayerPrefs.GetInt(PlayerPrefsKey.Money, 0);
                    PlayerPrefs.SetInt(PlayerPrefsKey.Money, moneyCount + 5);
                    _moneyManager.moneyScore.text = (moneyCount + 5 + "$").ToString();
                    Destroy(mImage.gameObject);
                });
            }, 0.3f);
        });
    }
}