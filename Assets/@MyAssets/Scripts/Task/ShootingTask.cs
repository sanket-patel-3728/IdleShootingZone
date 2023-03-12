
using System.Collections;
using UnityEngine;

public class ShootingTask : TaskController
{
    public override void StartTask()
    {
        StartCoroutine (PlayTask());
    }

    IEnumerator PlayTask() 
    {
        yield return new WaitForSeconds (1);
        storedCustomer.SetAnimation ("Idle", true);
        yield return new WaitForSeconds (5);
        storedCustomer.SetAnimation ("Shoot");
        yield return new WaitForSeconds (5);
        storedCustomer.SetAnimation ("Shoot");
        yield return new WaitForSeconds (5);
        storedCustomer.SetAnimation ("Idle", false);
        yield return new WaitForSeconds (1);
        storedCustomer.ExitCustomer ();
        storedCustomer = null;
    }
}