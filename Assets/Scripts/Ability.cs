using UnityEngine;
using System.Collections;

public class Ability : MonoBehaviour
{
    public AbilityType type;

    void Start()
    {
        Animator ani = GetComponent<Animator>();
        if (type == AbilityType.Red)
        {
            ani.SetInteger("type", 0);
        }
        else if (type == AbilityType.Blue)
        {
            ani.SetInteger("type", 1);
        }
        else if (type == AbilityType.Green)
        {
            ani.SetInteger("type", 4);
        }
        else if (type == AbilityType.Orange)
        {
            ani.SetInteger("type", 5);
        }
        else if (type == AbilityType.Purple)
        {
            ani.SetInteger("type", 3);
        }
        else if (type == AbilityType.Yellow)
        {
            ani.SetInteger("type", 2);
        }

    }
}
