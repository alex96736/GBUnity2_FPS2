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

    // состояния бота
    private bool _patrol;
    private bool _shooting;

    private float _maxAngle = 30;
    private float _maxRadius = 20;

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


    void Update()
    {
        _patrol = true;
        foreach (Collider collision in Physics.OverlapSphere(_GameObjectTransform.position, _maxRadius))
        {
            if (collision.tag == "Player")
            {
                _patrol = false;
            }
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
                _agent.SetDestination(_playerPos.position);

                Vector3 RayPos = new Vector3(Position.x, Position.y + 1, Position.z);
                Ray ray = new Ray(RayPos, transform.forward);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, _maxRadius))
                {
                    if (hit.collider.tag == "Player")
                    {
                        Debug.Log("Hit Player");
                    }
                }

            }
        }
    }
}
