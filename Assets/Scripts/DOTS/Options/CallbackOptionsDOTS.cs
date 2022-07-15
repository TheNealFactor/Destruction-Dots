using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class CallbackOptionsDOTS
{
    [Tooltip("This callback is invoked when the fracturing/slicing process has been completed.")]
    public UnityEvent onCompleted;

    public CallbackOptionsDOTS()
    {
        this.onCompleted = null;
    }
}