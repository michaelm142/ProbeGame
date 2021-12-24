using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActionDoer : MonoBehaviour
{
    public UnityEvent Action;

    public void DoAction()
    {
        Action.Invoke();
    }
}
