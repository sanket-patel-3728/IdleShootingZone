using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Customer : MonoBehaviour
{
    [SerializeField] NavMeshAgent _navMeshAgent;
    [SerializeField] Animator _anim;

    public Transform bubulletStartPoint;
    public GameObject bubulletTrail;
    public GameObject headPhone;
    public GameObject gun, gun2;

    public Transform arrowStartPoint;
    public GameObject arrow, arrow2, arrow3;
    public GameObject archery,bow;

    public bool isGirl;
    public bool isSiting;
    CustomerManager _customerManager;
    Action _action;
    Vector3 _target;
    Vector3 _rotation;
    bool _isStop;

    public TaskController taskController;
    public bool _isExit;

    private void Awake()
    {
        _customerManager = CustomerManager.instance;
    }

    private void Start()
    {
        bubulletTrail.Hide();
        StartCoroutine(EditUpdate());
        StartCoroutine(PlaySitingAnimation());
    }

    IEnumerator EditUpdate()
    {
        yield return new WaitForSeconds(1);
        if (_navMeshAgent.enabled == false && _isStop == true)
        {
            if ((_target - transform.position).magnitude > 0)
            {
                SetTarget(_target, _rotation, _action);
            }
        }

        if (_navMeshAgent.enabled == true)
        {
            var dist = Vector3.Distance(_target, transform.position);
            if (dist <= 0.1)
            {
                if (_isExit)
                {
                    StopAgent();
                    _customerManager.instanceSpawing();
                    Destroy(this.gameObject);
                }
                else
                {
                    //StopAgent();
                    StopAgentForTask();
                    _action?.Invoke();
                    _action = null;
                }
            }
        }

        StartCoroutine(EditUpdate());
    }

    public void ExitCustomer()
    {
        //if (isCustomerReady) _navMeshObstacle.enabled = false; 
        _target = CustomerManager.instance.customerInstantiatePoint.position;
        _navMeshAgent.enabled = true;
        _navMeshAgent.SetDestination(CustomerManager.instance.customerInstantiatePoint.position);
        _anim.SetBool("Walk", true);
        _isExit = true;
    }

    public void SetTarget(Vector3 target, Vector3 rotation, Action endTask = null)
    {
        _action = endTask;
        _target = target;
        _rotation = rotation;
        //if (isCustomerReady) _navMeshObstacle.enabled = false;
        _navMeshAgent.enabled = true;
        _navMeshAgent.SetDestination(target);
        _anim.SetBool("Walk", true);
    }

    public void StopAgent()
    {
        _isStop = false;
        _navMeshAgent.enabled = false;
        //if (isCustomerReady) _navMeshObstacle.enabled = true;
        _anim.SetBool("Walk", false);
    }

    public void StopAgentForTask()
    {
        transform.eulerAngles = _rotation;
        _isStop = true;
        _navMeshAgent.enabled = false;
        //if (isCustomerReady) _navMeshObstacle.enabled = true;
        _anim.SetBool("Walk", false);
    }

    public void SetAnimation(String key, bool state)
    {
        _anim.SetBool(key, state);
    }

    public void SetAnimation(String key)
    {
        _anim.SetTrigger(key);
    }

    public void ShowHappyEmoji()
    {
        var par = CustomerManager.instance.happyEmoji[Helper.RandomInt(0, CustomerManager.instance.happyEmoji.Length)];
        var pos = transform.position;
        pos.y += 3;
        var temp = Instantiate(par.gameObject, pos, Quaternion.identity, transform);
        temp.GetComponent<ParticleSystem>().Play();
    }

    public void ShowSadEmoji()
    {
        var par = CustomerManager.instance.sadEmoji[Helper.RandomInt(0, CustomerManager.instance.sadEmoji.Length)];
        var pos = transform.position;
        pos.y += 3;
        var temp = Instantiate(par.gameObject, pos, Quaternion.identity, transform);
        temp.GetComponent<ParticleSystem>().Play();
    }

    private List<string> aniprm = new List<string> { "Task_1", "Task_2", "Task_3", "Task_4", "Task_5" };

    IEnumerator PlaySitingAnimation()
    {
        if (isSiting)
        {
            SetAnimation(aniprm[Helper.RandomInt(0, aniprm.Count)]);
        }

        yield return new WaitForSeconds(Helper.RandomInt(10, 25));
        StartCoroutine(PlaySitingAnimation());
    }
}