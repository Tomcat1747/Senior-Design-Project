using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitDataLibrary;

[CreateAssetMenu(menuName = "Unit/Class Library")]
public class ClassLibrarySO : ScriptableObject
{
    [SerializeField]
    List<ClassData> ClassList = new List<ClassData>();
    private Dictionary<ClassType,ClassData> ClassLibrary = new Dictionary<ClassType, ClassData>();

    public ClassData getClass(ClassType type){
        if(ClassLibrary.Count == 0) initClassLibrary();
        return ClassLibrary[type];
    }

    public void initClassLibrary(){
        foreach(ClassData x in ClassList){
            ClassLibrary[x.classType] = x;
        }
    }

}
