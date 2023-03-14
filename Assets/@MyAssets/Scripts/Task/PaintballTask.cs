using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintballTask : TaskController
{
    public bool isBarrel;
    public override void StartTask()
    {
        if (isBarrel)
        {
            storedCustomer.SetAnimation("BarrelIdle", true);
        }
        else
        {
            storedCustomer.SetAnimation("WallIdle", true);
        }
    }
}
