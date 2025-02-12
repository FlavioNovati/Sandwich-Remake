using UnityEngine;

public class Ingredient : MonoBehaviour
{
    public bool IsBread => _isBread;
    [SerializeField] private bool _isBread;

}
