using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Базовый класс для объектов
/// </summary>
public abstract class BaseObject : MonoBehaviour
{

#region Fields

    protected Transform _GameObjectTransform;
    protected GameObject _GameObject;
    protected string _name;
    protected bool _isVisible;
    protected int _layer;
    protected int _childCount;

    protected Vector3 _position;
    protected Vector3 _scale;
    protected Quaternion _rotation;

    protected Material _material;
    protected Color _color;

    protected Rigidbody _rigidbody;
    protected Animator _animator;

    protected Camera _mainCamera;

#endregion

#region UnityFunction

    protected virtual void Awake()
    {
        _GameObject = gameObject;
        _GameObjectTransform = _GameObject.transform;
        _name = _GameObject.name;
        _layer = _GameObject.layer;

        _mainCamera = Camera.main;

        if(GetComponent<Rigidbody>())
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        if(GetComponent<Animator>())
        {
            _animator = GetComponent<Animator>();
        }

        if(GetComponent<Renderer>())
        {
            _material = GetComponent<Renderer>().material;
        }
        
    }

#endregion

#region Property

    /// <summary>
    /// Ссылка на количество потомков
    /// </summary>
    public int ChildCount
    {
        get { return _GameObjectTransform.childCount; }
    }

    /// <summary>
    /// Ссылка на объект
    /// </summary>
    public GameObject InstanceGameObject
    {
        get { return _GameObject; }
    }

    /// <summary>
    /// Ссылка на имя объекта
    /// </summary>
    public string Name
    {
        get { return _name; }
        set
        {
            _name = value;
            InstanceGameObject.name = _name;
        }
    }

    /// <summary>
    /// Видимость объекта
    /// </summary>
    public bool Visible
    {
        get { return _isVisible; }
        set
        {
            _isVisible = value;
            if (_GameObject.GetComponent<MeshRenderer>())
            {
                _GameObject.GetComponent<MeshRenderer>().enabled = _isVisible;
            }
        }
    }

    /// <summary>
    /// Ссылка на позицию объекта
    /// </summary>
    public Vector3 Position
    {
        get
        {
            if (_GameObject)
            {
                _position = _GameObjectTransform.position;
            }
            return _position;
        }
        set
        {
            _position = value;
            if (_GameObject)
            {
                _GameObjectTransform.position = _position;
            }
        }
    }

    /// <summary>
    /// Ссылка на масштаб объекта
    /// </summary>
    public Vector3 Scale
    {
        get
        {
            if (_GameObject)
            {
                _scale = _GameObjectTransform.localScale;
            }
            return _scale;
        }
        set
        {
            _scale = value;
            if (_GameObject)
            {
                _GameObjectTransform.localScale = _scale;
            }
        }
    }

    /// <summary>
    /// Ссылка на поворот объекта
    /// </summary>
    public Quaternion Rotation
    {
        get
        {
            if (_GameObject)
            {
                _rotation = _GameObjectTransform.rotation;
            }
            return _rotation;
        }
        set
        {
            _rotation = value;
            if (_GameObject)
            {
                _GameObjectTransform.rotation = _rotation;
            }
        }
    }

    /// <summary>
    /// Ссылка на материал объекта
    /// </summary>
    public Material GetMaterial
    {
        get { return _material; }
    }

    /// <summary>
    /// Ссылка на RigidBody объекта
    /// </summary>
    public Rigidbody GetRigidBody
    {
        get { return _rigidbody; }
    }

    /// <summary>
    /// Ссылка на Animator объекта
    /// </summary>
    public Animator GetAnimator
    {
        get { return _animator; }
    }

    /// <summary>
    /// Ссылка на mainCamera
    /// </summary>
    public Camera GetMainCamera
    {
        get { return _mainCamera; }
    }

    /// <summary>
    /// Ссылка на слой объекта в сцене 
    /// </summary>
    public int Layer
    {
        get
        {
            if (_GameObject)
            {
                _layer = _GameObject.layer;
            }
            return _layer;
        }
        set
        {
            _layer = value;
            if (_GameObject)
            {
                _GameObject.layer = _layer;
            }
        }
    }

#endregion

#region Functions
    /// <summary>
    /// включение/выключение физики объекта
    /// </summary>
    public void UseGravity(bool value)
    {
        if (_rigidbody)
        {
            _rigidbody.useGravity = value;
        }
    }

#endregion
}