using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PoolManager : MonoBehaviour
{
    public static PoolManager instance;
    //public Ball ballPrefab;
    public List<Money> allHideMoney;
    public List<GameObject> allHideMoneyImage;
    //public List<Ball> allHideBall;
    MoneyManager _moneyManager;

    protected void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        _moneyManager = MoneyManager.instance;
        for (byte i = 0; i < 20; i++)
        {
            var m = Instantiate(_moneyManager.moneyPrefab, transform).GetComponent<Money>();
            var mi = Instantiate(_moneyManager.moneyImage, transform);
            //var ball = Instantiate(ballPrefab, transform);
            PoolMoney(m);
            PoolMoneyImage(mi);
            //PoolBall(ball);
        }
    }

    public Money GetMoney()
    {
        if (allHideMoney.Count > 0)
        {
            var m = allHideMoney[0];
            allHideMoney.Remove(allHideMoney[0]);
            m.Show();
            return m;
        }
        else
        {
            var m = Instantiate(_moneyManager.moneyPrefab, transform);
            return m.GetComponent<Money>();
        }
    }

    public GameObject GetMoneyImage()
    {
        if (allHideMoneyImage.Count > 0)
        {
            var m = allHideMoneyImage[0];
            allHideMoneyImage.Remove(allHideMoneyImage[0]);
            m.Show();
            return m;
        }
        else
        {
            var m = Instantiate(_moneyManager.moneyImage, transform);
            return m;
        }
    }

    /*public Ball GetBall()
    {
        if (allHideBall.Count > 0)
        {
            var ball = allHideBall[0];
            allHideBall.Remove(allHideBall[0]);
            ball.Show();
            return ball;
        }
        else
        {
            var ball = Instantiate(ballPrefab, transform);
            ball.rb.isKinematic = true;
            return ball;
        }
    }*/

    public void PoolMoney(Money money)
    {
        if (allHideMoney.Count > 25)
        {
            Destroy(money.gameObject);
        }
        else
        {
            money.Hide();
            money.transform.SetParent(transform);
            allHideMoney.Add(money);
        }
    }

    public void PoolMoneyImage(GameObject moneyimage)
    {
        if (allHideMoneyImage.Count > 25)
        {
            Destroy(moneyimage.gameObject);
        }
        else
        {
            moneyimage.Hide();
            moneyimage.transform.SetParent(transform);
            allHideMoneyImage.Add(moneyimage);
        }
    }

    /*public void PoolBall(Ball ball)
    {
        if (allHideBall.Count > 25)
        {
            Destroy(ball.gameObject);
        }
        else
        {
            ball.rb.isKinematic = true;
            ball.Hide();
            ball.transform.SetParent(transform);
            allHideBall.Add(ball);
        }
    }*/
}