using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;

public class FillObject : MonoBehaviour
{
    bool _isPLayer;
    public Image fillImage;
    public List<GameObject> allFillObject;
    public UnityEvent playerTriggerEnter;
    public Vector3 position;
    public bool isHelmetShop;
    public Image notification;

    private void LateUpdate()
    {
        if (allFillObject.FindAll(x => x.activeSelf == true).Count > 0)
        {
            notification.Hide();
        }
        else
        {
            notification.Show();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (_isPLayer) return;
            _isPLayer = true;
            if (allFillObject.FindAll(x => x.activeSelf == false).Count > 0)
            {
                DOTween.To(() => fillImage.fillAmount, x => fillImage.fillAmount = x, 0, 1f).From(1)
                    .OnComplete(() =>
                    {
                        CameraFollow.instance.MoveTo(position, 0.5f);
                        StartCoroutine(StartFilling());
                        /*var tutorial = TutorialControler.Instance;
                        if (isHelmetShop)
                        {
                            if (tutorial.helmetPoint != null)
                            {
                                tutorial.helmetPoint = null;
                                tutorial.targetPoint = null;
                            }
                        }
                        else
                        {
                            if (tutorial.baseballPoint != null)
                            {
                                tutorial.baseballPoint = null;
                                tutorial.targetPoint = null;
                            }
                        }*/
                    })
                    .SetId(fillImage);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!_isPLayer) return;
        _isPLayer = false;
        DOTween.Kill(fillImage);
        DOTween.To(() => fillImage.fillAmount, x => fillImage.fillAmount = x, 1, 0.5f);
        //StopCoroutine(StartFilling());
    }

    IEnumerator StartFilling()
    {
        yield return new WaitForSeconds(1f);
        var temp = allFillObject.FindAll(x => x.activeSelf == false);
        for (byte i = 0; i < temp.Count; i++)
        {
            var i1 = i;

            temp[i1].Show();
            temp[i1].transform.DOScale(Vector3.one, 0.75f).SetEase(Ease.OutBack).From(Vector3.zero);
            yield return new WaitForSeconds(0.1f);
            if (i == temp.Count - 1)
            {
                CameraFollow.instance.ReturnToNormal();
                DOTween.Kill(fillImage);
                DOTween.To(() => fillImage.fillAmount, x => fillImage.fillAmount = x, 1, 0.5f);
                playerTriggerEnter?.Invoke();
            }
        }
    }
}