using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadPhoneShop : MonoBehaviour
{
    public Transform stadingPoint;
    public List<Customer> allWaitingCustomer;
    public FillObject fillObject;
    Customer _storedCustomer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Customer tcustomer))
        {
            if (allWaitingCustomer != null)
            {
                if (tcustomer == allWaitingCustomer[0])
                {
                    _storedCustomer = tcustomer;
                    PickUpBaseball();
                    /*var temp = fillObject.allFillObject.Find(x => x.activeSelf == true);
                    if (temp != null)
                    {
                        temp.Hide();
                        allWaitingCustomer.Remove(tcustomer);
                        tcustomer.baseball.Show();
                        tcustomer.SetTarget(tcustomer.taskController.gamePlayPoint.position);
                        ArrangePosition();
                    }*/
                }
            }
        }
    }

    public void ArrangePosition()
    {
        for (byte i = 0; i < allWaitingCustomer.Count; i++)
        {
            var pos = stadingPoint.position;
            if (!i.Equals(0))
            {
                pos.x -= (i * 2);
            }

            var i1 = i;
            allWaitingCustomer[i]
                .SetTarget(pos, stadingPoint.eulerAngles);
        }
    }

    public void PickUpBaseball()
    {
        StartCoroutine(Baseball());
    }

    IEnumerator Baseball()
    {
        yield return new WaitForSeconds(1.5f);
        var temp = fillObject.allFillObject.Find(x => x.activeSelf == true);
        /*if (temp == null)
        {
            if (PlayerPrefs.GetInt(PlayerPrefsKey.TutorialCount, 0).Equals(0))
            {
                TutorialControler.Instance.targetPoint = TutorialControler.Instance.baseballPoint;
            }
        }*/

        if (temp != null && _storedCustomer != null)
        {
            temp.Hide();
            _storedCustomer.headPhone.Show();
            allWaitingCustomer.Remove(_storedCustomer);
            var task = _storedCustomer.taskController;
            _storedCustomer.SetTarget(task.taskPoint.position, task.taskPoint.eulerAngles, () => task.StartTask());
            ArrangePosition();
        }
    }
}
