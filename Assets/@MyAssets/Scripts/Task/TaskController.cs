using System;
using UnityEngine;

public abstract class TaskController : MonoBehaviour
{
    public Transform taskPoint, shootPoint;
    public Customer storedCustomer;

    public abstract void StartTask();

    private void Start()
    {
        TicketController.instance.allTaskControllers.Add(this);
    }


}