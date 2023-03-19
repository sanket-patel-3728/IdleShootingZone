using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TicketController : MonoBehaviour
{
    public static TicketController instance;
    public Image triggerIn;
    public Image triggerOut;
    public Collider playerCollider;
    public MoneyStacker moneyStacker;
    public Transform stadingPoint;
    public List<TaskController> allTaskControllers;
    public HeadPhoneShop headPhoneShop;

    bool _verify;
    bool _isCustomer;
    bool _isPlayer;

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
        CodeMonkey.Utils.FunctionTimer.Create(() => { CustomerManager.instance.instanceSpawing(); }, 1);
        _isPlayer = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Customer tcustomer))
        {
            if (tcustomer == CustomerManager.instance.allWaitingCustomers[0])
            {
                _isCustomer = true;
                _verify = true;
                if (_isPlayer)
                {
                    //AggryPermission();
                }
            }
        }

        if (other.TryGetComponent(out PlayerController player))
        {
            if (_isPlayer) return;
            if (other.bounds.Intersects(playerCollider.bounds))
            {
                _isPlayer = true;
                triggerOut.Hide();
                triggerIn.Show();
                if (_verify && _isCustomer)
                {
                    StartCoroutine(AggryPermission());
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerController player))
        {
            if (!_isPlayer) return;
            _isPlayer = false;
            triggerIn.Hide();
            triggerOut.Show();
            StopCoroutine(AggryPermission());
        }

        if (other.TryGetComponent(out Customer tcustomer))
        {
            if (tcustomer == CustomerManager.instance.allWaitingCustomers[0])
            {
                _isCustomer = false;
                _verify = false;
            }
        }
    }

    IEnumerator AggryPermission()
    {
        yield return new WaitForSeconds(1);
        if ((_isCustomer && _isPlayer && _verify))
        {
            var customer = CustomerManager.instance.allWaitingCustomers[0];
            var task = allTaskControllers.Find(x => x.storedCustomer == null);

            if (task != null && headPhoneShop.allWaitingCustomer.Count < 3)
            {
                task.storedCustomer = customer;
                task.storedCustomer.taskController = task;

                headPhoneShop.allWaitingCustomer.Add(customer);
                headPhoneShop.ArrangePosition();
                //customer.SetTarget(task.taskPoint.position, task.taskPoint.eulerAngles, () => task.StartTask());
                NextCustomer(customer);
            }
        }
        StartCoroutine(AggryPermission());
    }

    private void NextCustomer(Customer customer)
    {
        moneyStacker.GiveMoney(customer.transform, 5);
        CustomerManager.instance.allWaitingCustomers.Remove(customer);
        _isCustomer = false;
        _verify = false;
        CodeMonkey.Utils.FunctionTimer.Create(() =>
        {
            CustomerManager.instance.instanceSpawing();
            CustomerManager.instance.ArrangePosition();
        }, 2);
    }

    /*public void CheckWaitingCustomer()
    {
        var waitingCustomer = chairs.Find(x => x.storedCustomer != null);
        if (waitingCustomer != null)
        {
            var customer = waitingCustomer.storedCustomer;
            var task = _baseballController.allTaskControllers.Find(x => x.storedCustomer == null);
            if (task != null && _baseballController.helmetShop.allWaitingCustomer.Count < 3)
            {
                waitingCustomer.storedCustomer = null;
                customer.isSiting = false;
                customer.SetAnimation("Sit", false);
                customer.SetAnimation("Idle");
                task.storedCustomer = customer;
                task.storedCustomer.taskController = task;

                _baseballController.helmetShop.allWaitingCustomer.Add(customer);
                _baseballController.helmetShop.ArrangePosition();
            }
        }
    }*/
}