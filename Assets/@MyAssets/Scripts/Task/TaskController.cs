using System;
using UnityEngine;

public abstract class TaskController : MonoBehaviour
{
    public Customer storedCustomer;
    public Transform taskPoint, shootPoint;

    public abstract void StartTask();

    private void Start()
    {
        TicketController.instance.allTaskControllers.Add(this);
    }


}