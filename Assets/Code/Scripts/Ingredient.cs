using UnityEngine;

public class Ingredient : MonoBehaviour
{
    public bool IsBread => _isBread;
    [SerializeField] private bool _isBread;

    public string Name => _name;
    [SerializeField] private string _name;
    
    public Vector3 Extends => _extends;
    [SerializeField] private Vector3 _extends;

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, _extends);
    }
#endif

}
