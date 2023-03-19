using System.Collections;
using UnityEngine;
using DG.Tweening;

public class ArcheryTask : TaskController
{
    public Transform shootPoint2, shootPoint3;
    public override void StartTask()
    {
        StartCoroutine (PlayTask ());
    }

    IEnumerator PlayTask () 
    {
        yield return new WaitForSeconds (1);
        storedCustomer.archery.Show();
        storedCustomer.bow.Show ();
        storedCustomer.SetAnimation ("Archery", true);
        yield return new WaitForSeconds (5);
        storedCustomer.SetAnimation ("ArcheryShoot");
        yield return new WaitForSeconds (0.3f);
        storedCustomer.arrow.transform.parent = null;
        storedCustomer.arrow.Show ();
        storedCustomer.arrow.transform.DOMove (shootPoint.position, 1);
            //.OnComplete (() => storedCustomer.arrow.Hide ());
        yield return new WaitForSeconds (5);
        storedCustomer.SetAnimation ("ArcheryShoot");
        yield return new WaitForSeconds (0.3f);
        storedCustomer.arrow2.transform.parent = null;
        storedCustomer.arrow2.Show ();
        storedCustomer.arrow2.transform.DOMove (shootPoint2.position, 1);
            //.OnComplete (() => storedCustomer.arrow2.Hide ());
        yield return new WaitForSeconds (5);
        storedCustomer.SetAnimation ("ArcheryShoot");
        yield return new WaitForSeconds (0.3f);
        storedCustomer.arrow3.transform.parent = null;
        storedCustomer.arrow3.Show ();
        storedCustomer.arrow3.transform.DOMove (shootPoint3.position, 1);
            //.OnComplete (() => storedCustomer.arrow3.Hide ());
        yield return new WaitForSeconds (5);
        storedCustomer.SetAnimation ("Archery", false);
        storedCustomer.archery.Hide ();
        storedCustomer.bow.Hide ();
        storedCustomer.arrow.Hide ();
        storedCustomer.arrow2.Hide ();
        storedCustomer.arrow3.Hide ();
        yield return new WaitForSeconds (1);
        storedCustomer.ExitCustomer ();
        storedCustomer = null;
    }
}