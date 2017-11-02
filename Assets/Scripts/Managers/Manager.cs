using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Manager : MonoBehaviour
{
    // Some scripts might be calling one of the managers in their OnDisable methods after the application has started closing
    // and after the managers themselves have already been disabled. This variable is used in order to prevent exception throwing
    // if anyone calls an Instance property of a disabled manager.
    protected static bool isApplicationClosing;

    protected virtual void OnDisable()
    {
        isApplicationClosing = true;
    }
}
