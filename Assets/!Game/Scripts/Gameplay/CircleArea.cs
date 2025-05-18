using System;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class CircleArea : MonoBehaviour
{
    #region Fields
    [Header("Area radius."), SerializeField, Range(0f, 10f)] private float _radius = 1f;
    #endregion

    #region Methods
    /// <summary>
    /// Method return true if distance between Position and Circle center less then radius.
    /// </summary>
    /// <param name="Position">Checking Vector3 position.</param>
    /// <param name="CenterApproaching">Return value between 0 and 1 if true, return -1 if false. </param>
    /// <returns>Is distance between circle center and Position is less then radius?</returns>
    public bool PointIsInArea(Vector3 Position, out float CenterApproaching)
    {
        float distance = Vector3.Distance(transform.position, Position);

        if(distance <= _radius)
        {
            CenterApproaching = MathF.Round((distance / _radius), 2);
            return true;
        }
        else
        {
            CenterApproaching = -1;
            return false;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected ()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
#endif
    #endregion
}
