using UnityEngine;

public class Ingredient : MonoBehaviour
{
    public bool IsBread => _isBread;
    [SerializeField] private bool _isBread;

    public string Name => _name;
    [SerializeField] private string _name;

}
