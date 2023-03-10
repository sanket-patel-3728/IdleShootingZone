using TMPro;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public GameObject moneyPrefab;

    public GameObject moneyImage;
    //public ParticleSystem moneySpending;

    [Header("Money")] public Transform moneyIcon;
    public TextMeshProUGUI moneyScore;

    public static MoneyManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        LoadData();
    }

    private void LoadData()
    {
        var money = PlayerPrefs.GetInt("Money");
        moneyScore.text = (money + "$").ToString();
    }

    public void AddMoney()
    {
        var m = PlayerPrefs.GetInt("Money");
        PlayerPrefs.SetInt("Money", m + 100);
        moneyScore.text = (m + 100 + "$").ToString();
    }
}