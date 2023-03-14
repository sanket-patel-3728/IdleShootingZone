using System.Collections;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable]
public class Unlockable : MonoBehaviour
{
    [Header("Unlockable Data")] public string id;
    public float price;
    public float fillPrice;
    public TextMeshProUGUI priceText;
    public GameObject unlockableObject;

    public UnityEvent unlockFinish;
    public Image fillImage;

    bool _isPlayer;

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        priceText.text = (price + "$").ToString();
    }
#endif
    private void Awake()
    {
        fillPrice = price;
        LoadData();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (_isPlayer) return;
            _isPlayer = true;
            StartCoroutine(Unlocking());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!_isPlayer) return;
            _isPlayer = false;
            //MoneyManager.instance.moneySpending.gameObject.SetActive(false);
            StopCoroutine(Unlocking());
            //DOTween.KillAll();
        }
    }

    int _moneyCounter = 1;

    IEnumerator Unlocking()
    {
        yield return new WaitForSeconds(0.5f);
        PlayDoSpeed();
        _moneyCounter = 1;
        while (_isPlayer)
        {
            var money = PlayerPrefs.GetInt(PlayerPrefsKey.Money, 0);
            if (money >= 1 && price > 0)
            {
                //MoneyManager.instance.moneySpending.transform.LookAt(transform.position);
                //MoneyManager.instance.moneySpending.gameObject.SetActive(true);
                if (money >= _moneyCounter)
                {
                    if (price - _moneyCounter < 0)
                    {
                        PlayerPrefs.SetInt(PlayerPrefsKey.Money, (money - (int)price));
                        price = 0;
                    }
                    else
                    {
                        price -= _moneyCounter;
                        PlayerPrefs.SetInt(PlayerPrefsKey.Money, (money - _moneyCounter));
                    }
                }
                else
                {
                    if (price - money < 0)
                    {
                        PlayerPrefs.SetInt(PlayerPrefsKey.Money, (money - (int)price));
                        price = 0;
                    }
                    else
                    {
                        price -= money;
                        PlayerPrefs.SetInt(PlayerPrefsKey.Money, (money - money));
                    }
                }

                //price--;
                MoneyManager.instance.moneyScore.text = (PlayerPrefs.GetInt(PlayerPrefsKey.Money, 0) + "$").ToString();
                priceText.text = (price + "$").ToString();
                PlayerPrefs.SetFloat(id, price);
                DOTween.To(() => fillImage.fillAmount, x => fillImage.fillAmount = x, 1 - (price / fillPrice), 0.1f);
                if (price.Equals(0))
                {
                    PlayerPrefs.SetInt(PlayerPrefsKey.UnlockCount,
                        (PlayerPrefs.GetInt(PlayerPrefsKey.UnlockCount, 0) + 1));
                    //MoneyManager.instance.moneySpending.gameObject.SetActive(false);
                    Unlock();
                    //CustomerManager.instance.ConfettiBlast.transform.position = transform.position.With(3);
                    //CustomerManager.instance.ConfettiBlast.Play();
                    yield break;
                }
            }
            else
            {
                //MoneyManager.instance.moneySpending.gameObject.SetActive(false);
            }

            if (price % 2 == 0)
            {
                _moneyCounter += 1;
            }

            yield return new WaitForSeconds(_unlockSpeed);
            //yield return new WaitForSecondsRealtime(unlockSpeed);
        }
    }

    float _unlockSpeed = 0.2f;

    public void PlayDoSpeed()
    {
        DOTween.To(() => _unlockSpeed, x => _unlockSpeed = x, 0f, 3).From(0.2f);
    }

    public void Unlock()
    {
        if (unlockableObject)
            unlockableObject.transform.DOScale(Vector3.one, 0.2f).From(Vector3.zero)
                .OnStart(() => { unlockableObject.SetActive(true); })
                .OnComplete((() => unlockFinish?.Invoke()));
        //unlockFinishTutorial?.Invoke();
        //TutorialController.instance.ShowStandToBuy();
        gameObject.SetActive(false);
    }


    public void LoadData()
    {
        var p = PlayerPrefs.GetFloat(id, price);
        price = p;
        priceText.text = (p + "$").ToString();
        PlayerPrefs.SetFloat(id, price);
        DOTween.To(() => fillImage.fillAmount, x => fillImage.fillAmount = x, 1 - (price / fillPrice), 0.1f);
        if (p <= 0)
        {
            Unlock();
        }
    }
}