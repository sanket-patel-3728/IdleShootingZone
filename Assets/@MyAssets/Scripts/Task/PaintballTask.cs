using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintballTask : TaskController
{
    public bool isBarrel;
    public override void StartTask()
    {
        storedCustomer.transform.position = taskPoint.position;
        if (isBarrel)
        {
            storedCustomer.SetAnimation("BarrelIdle", true);
        }
        else
        {
            storedCustomer.SetAnimation("WallIdle", true);
        }
        StartCoroutine(PlayTask());
    }

    IEnumerator PlayTask()
    {
        yield return new WaitForSeconds(Random.Range(3, 5));
        if (isBarrel)
        {
            storedCustomer.SetAnimation("BarrelShoot");
        }
        else
        {
            storedCustomer.SetAnimation("WallShoot");
        }
        yield return new WaitForSeconds(Random.Range(5, 7));
        if (isBarrel)
        {
            storedCustomer.SetAnimation("BarrelShoot");
        }
        else
        {
            storedCustomer.SetAnimation("WallShoot");
        }
        yield return new WaitForSeconds(Random.Range(5, 7));
        if (isBarrel)
        {
            storedCustomer.SetAnimation("BarrelShoot");
        }
        else
        {
            storedCustomer.SetAnimation("WallShoot");
        }
        yield return new WaitForSeconds(Random.Range(3, 5));
        if (isBarrel)
        {
            storedCustomer.SetAnimation("BarrelIdle", false);
        }
        else
        {
            storedCustomer.SetAnimation("WallIdle", false);
        }
        storedCustomer.ExitCustomer();
        storedCustomer = null;
    }
}
