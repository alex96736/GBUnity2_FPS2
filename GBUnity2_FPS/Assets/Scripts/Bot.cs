using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.AI;

// проверка необходимых компонентов
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(ThirdPersonCharacter))]


public class Bot : Unit
{
    private NavMeshAgent _agent;
    private ThirdPersonCharacter _controller;
    private Transform _playerPos;
    private Vector3 _StartPos;
    private Transform _traget;

    private int _activeDistance = 10;
    private int _stoppingDistance = 1;

    private float _activeAngle = 30;

    private bool _isTarget;
    private bool _isDie;

    // точки для патрулирования
    private List<Vector3> _wayPoints = new List<Vector3>();

    private int _wayCount;

    // объект, хранящий точки патрулирования 
    private GameObject wayPoints;

    // время ожидания на точке
    private float _timeWait = 2f;
    private float _timeout;

    [Header("Состояние бота")]
    // состояния бота
    [SerializeField] private bool _patrol;
    private bool _shooting;

    [Header("Параметры поля зрения бота")]
    [Range(0, 90)]
    [SerializeField] private float _maxAngle = 90;
    [SerializeField] private float _maxRadius = 20;

    [Header("Массив видимых целей бота")]
    // хранения видимый целей
    [SerializeField] private List<Transform> _visibleTargets = new List<Transform>();


    [Header("Маски слоей объектов для бота")]
    // маски слоев
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private LayerMask obstacleMask;

    [Header("Элементы для выстрелов")]
    [SerializeField] protected ParticleSystem _muzzleFlash;
    [SerializeField] protected GameObject _hitParticle;
    // точка, из которой производится выстрел
    [SerializeField] protected Transform _SpawnBullet;

    private int _damage = 20;
    private float _shootDistance = 1000f;

    Vector3 RayPos;

    


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _maxRadius);

        Vector3 fovLineLeft = Quaternion.AngleAxis(-_maxAngle, transform.up)*transform.forward * _maxRadius;
        Vector3 fovLineRight = Quaternion.AngleAxis(_maxAngle, transform.up)*transform.forward * _maxRadius;

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, fovLineLeft);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, fovLineRight);
    }

    protected override void Awake()
    {
        base.Awake();
        BaseHealth = 100;
        Health = BaseHealth;
        _patrol = true;
        _agent = GetComponent<NavMeshAgent>();
        _controller = GetComponent<ThirdPersonCharacter>();
        _agent.updatePosition = true;
        _agent.updateRotation = true;
        _playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        _StartPos = Position;

        wayPoints = GameObject.FindGameObjectWithTag("Waypoint");
        foreach (Transform t in wayPoints.transform)
        {
            _wayPoints.Add(t.position);
        }

        StartCoroutine("FindTargets", 1f);

        _muzzleFlash = GetComponentInChildren<ParticleSystem>();
        _SpawnBullet = GameObject.FindGameObjectWithTag("GunT").transform;

    }

    private void ChangeWaypoint()
    {
        if (_wayCount < _wayPoints.Count - 1)
        {
            _wayCount++;
        }
        else
        {
            _wayCount = 0;
        }
    }

    private void FindVisibleTargets()
    {
        _visibleTargets.Clear();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(Position, _maxRadius, targetMask);
        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;

            Vector3 dirToTarget = (target.position - Position).normalized;

            if (Vector3.Angle(transform.forward, dirToTarget)<_maxAngle/2)
            {
                float targetDistance = Vector3.Distance(Position, target.position);
                if (!Physics.Raycast(Position, dirToTarget, targetDistance, obstacleMask))
                {
                    _visibleTargets.Add(target);
                }
            }  
        }
    }

    IEnumerator FindTargets(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    void Update()
    {
        RayPos = new Vector3(Position.x, Position.y + 1, Position.z);

        if (_visibleTargets.Count > 0)
        {
            _patrol = false;
        }
        else
        {
            _patrol = true;
        }

        if (_agent)
        {
            if (_agent.remainingDistance > _agent.stoppingDistance)
            {
                _controller.Move(_agent.desiredVelocity, false, false);
                _animator.SetBool("Move", true);
            }
            else
            {
                _controller.Move(Vector3.zero, false, false);
                _animator.SetBool("Move", false);
            }

            if (_patrol)
            {
                if (_wayPoints.Count >= 2 && !_isTarget)
                {
                    _agent.stoppingDistance = _stoppingDistance;
                    _agent.SetDestination(_wayPoints[_wayCount]);
                    if (!_agent.hasPath)
                    {
                        _timeout += 0.1f;
                        if (_timeout > _timeWait)
                        {
                            _timeout = 0;
                            ChangeWaypoint();
                        }
                    }
                }
                else if (_wayPoints.Count == 0)
                {
                    _agent.stoppingDistance = 5f;
                    _agent.SetDestination(_playerPos.position);
                }
            }
            else
            {
                _agent.stoppingDistance = 5f;
                _agent.SetDestination(_visibleTargets[0].position);
                Ray ray = new Ray(RayPos, transform.forward);
                RaycastHit hit;
                Debug.DrawRay(RayPos, transform.forward, Color.blue);
                if (Physics.Raycast(ray, out hit, _shootDistance))
                {
                    if (hit.collider.tag == "Player" && !_shooting)
                    {
                        StartCoroutine(Shoot(hit));
                        _shooting = true;
                    }
                    else
                    {
                        return;
                    }
                }
            }
        }
    }

    IEnumerator Shoot(RaycastHit playerHit)
    {
        yield return new WaitForSeconds(2.5f);
        _muzzleFlash.Play();
        playerHit.collider.GetComponent<ISetDamage>().SetDamage(_damage);
        GameObject tempHit = Instantiate(_hitParticle, playerHit.point, Quaternion.LookRotation(playerHit.normal));
        tempHit.transform.parent = playerHit.transform;
        Destroy(tempHit, 0.5f);
        _shooting = false;
    }
}
