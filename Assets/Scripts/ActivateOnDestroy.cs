using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActivateOnDestroy : MonoBehaviour
{
    public UnityEvent OnActivation;

    private void OnDestroy()
    {
        OnActivation.Invoke();
    }
}
