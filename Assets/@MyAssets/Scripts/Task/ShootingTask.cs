using System.Collections;
using UnityEngine;
using DG.Tweening;

public class ShootingTask : TaskController
{
    public override void StartTask()
    {
        StartCoroutine(PlayTask());
    }

    Transform pos;
    IEnumerator PlayTask()
    {
        yield return new WaitForSeconds(1);
        storedCustomer.SetAnimation("Idle", true);
        yield return new WaitForSeconds(5);
        storedCustomer.SetAnimation("Shoot");
        yield return new WaitForSeconds(0.3f);
        storedCustomer.bubulletTrail.transform.parent = null;
        storedCustomer.bubulletTrail.Show();
        storedCustomer.bubulletTrail.transform.DOMove(shootPoint.position, 1)
            .OnComplete(() => storedCustomer.bubulletTrail.Hide());
        yield return new WaitForSeconds(5);
        storedCustomer.SetAnimation("Shoot");
        yield return new WaitForSeconds(0.3f);
        storedCustomer.bubulletTrail.Show();
        storedCustomer.bubulletTrail.transform.position = storedCustomer.bubulletStartPoint.position;
        storedCustomer.bubulletTrail.transform.DOMove(shootPoint.position, 1)
            .OnComplete(() => storedCustomer.bubulletTrail.Hide());
        yield return new WaitForSeconds(5);
        storedCustomer.SetAnimation("Shoot");
        yield return new WaitForSeconds(0.3f);
        storedCustomer.bubulletTrail.Show();
        storedCustomer.bubulletTrail.transform.position = storedCustomer.bubulletStartPoint.position;
        storedCustomer.bubulletTrail.transform.DOMove(shootPoint.position, 1)
            .OnComplete(() => storedCustomer.bubulletTrail.Hide());
        yield return new WaitForSeconds(5);
        storedCustomer.SetAnimation("Idle", false);
        yield return new WaitForSeconds(1);
        storedCustomer.ExitCustomer();
        storedCustomer = null;
    }
}