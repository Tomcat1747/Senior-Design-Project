using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Saving;

//This script is attached to all entities that need to be saved.
[ExecuteAlways]
public class SaveEntity : MonoBehaviour
{
    [SerializeField] string uniqueIdentifier = "";//updated at runtime
    static Dictionary<string, SaveEntity> globalchecker = new Dictionary<string, SaveEntity>();

    public string GetUniqueID()
    {
        return uniqueIdentifier;
    }
    /// <summary>
    /// works on collecting all the snapshots of each object that had a saveEntity script attached to it.
    /// After doing so it assigns it to a dictionary which is what is returned.
    /// </summary>
    /// <returns></returns>
    public object CaptureState()
    {
        Dictionary<string, object> state = new Dictionary<string, object>();
        foreach (Saveable saveable in GetComponents<Saveable>())
        {
            state[saveable.GetType().ToString()] = saveable.CaptureState();
        }
        return state;

    }
    /// <summary>
    /// takes the dictionary created by the capture state function and restores each object to its original
    /// state based on the capturestate parameters.
    /// </summary>
    /// <param name="state"></param>
    public void RestoreState(object state)
    {
        Dictionary<string, object> stateDict = (Dictionary<string, object>)state;
        foreach (Saveable saveable in GetComponents<Saveable>())
        {
            string typeString = saveable.GetType().ToString();
            if (stateDict.ContainsKey(typeString))
            {
                saveable.RestoreState(stateDict[typeString]);
                //print("Restoring state for" + GetUniqueID());
            }
        }

    }
    /// <summary>
    /// used in order to make sure that unique keys are used for every game object in game.
    /// </summary>
    /// <param name="unique"></param>
    /// <returns></returns>

    private bool IsUnique(string unique)
    {

        if (!globalchecker.ContainsKey(unique)) return true;

        if (globalchecker[unique] == this) return true;

        //below statement is used due to the static nature of our dictionary.
        //It is to prevent unique id's from changing throughout the game.
        if (globalchecker[unique] == null)
        {
            globalchecker.Remove(unique);
            return true;
        }

        //this is just incase the dictionary becomes out of date,
        //it will set up the new object with a new key.
        if (globalchecker[unique].GetUniqueID() != unique)
        {
            globalchecker.Remove(unique);
            return true;
        }

        return false;
    }

#if UNITY_EDITOR
    public void Update()
    {
        if (Application.IsPlaying(gameObject)) return;
        if (string.IsNullOrEmpty(gameObject.scene.path)) return;

        SerializedObject serializedobject = new SerializedObject(this);
        SerializedProperty property = serializedobject.FindProperty("uniqueIdentifier");

        if (string.IsNullOrEmpty(property.stringValue) || !IsUnique(property.stringValue))
        {
            property.stringValue = System.Guid.NewGuid().ToString();
            serializedobject.ApplyModifiedProperties();
        }

        globalchecker[property.stringValue] = this;

    }
#endif
}
