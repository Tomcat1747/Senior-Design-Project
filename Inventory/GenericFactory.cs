using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericFactory<T> : MonoBehaviour where T: MonoBehaviour
{
    /*private T usedPrefab;//Stores the reference to the prefab that will be instantiated

    /// <summary>
    /// Returns the new instance of usedPrefab
    /// </summary>
    /// <returns></returns>
    public virtual T NewInstantiation()
    {
        return Instantiate(usedPrefab);
    }*/
}
