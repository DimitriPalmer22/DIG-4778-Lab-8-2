using System;
using System.IO;
using System.Text;
using UnityEngine;

[Serializable]
public class TransformSaver
{
    private readonly Transform _transform;

    [SerializeField] private Vector3 position;
    [SerializeField] private Quaternion rotation;
    [SerializeField] private Vector3 scale;

    #region Getters

    public Vector3 Position => position;

    public Quaternion Rotation => rotation;

    public Vector3 Scale => scale;

    #endregion

    public TransformSaver(Transform transform)
    {
        _transform = transform;

        SetValues();
    }

    private void SetValues()
    {
        position = _transform.position;
        rotation = _transform.rotation;
        scale = _transform.localScale;
    }

    public static explicit operator TransformSaver(Transform transform)
    {
        return new TransformSaver(transform);
    }
}