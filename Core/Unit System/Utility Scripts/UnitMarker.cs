using System.Collections.Generic;
using System.Collections;
using UnitDataLibrary;
using UnityEngine;

public class UnitMarker : MonoBehaviour
{

    public static List<UnitMarker> Markers;
    public ClassType classType;
    public ClassLibrarySO ClassLibrary;
    public int PlayerNumber;
    public int level;
    protected ClassData classData { get { return ClassLibrary.getClass(classType); } }
    protected SpriteRenderer spriteRenderer;
    protected Animator animator;

    void Awake()
    {
        if (Markers == null)
        {
            Markers = new List<UnitMarker>();
        }
        Markers.Add(this);
    }

    public void updateSprite()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        spriteRenderer.sprite = classData.classSprites[PlayerNumber];
        if (PlayerNumber <= classData.AnimatorControllerList.Count)
            animator.runtimeAnimatorController = classData.AnimatorControllerList[PlayerNumber];
        else
            Debug.LogWarning("PlayerNumber: " + PlayerNumber + " exceeds AnimatorControllerList's length: " + classData.AnimatorControllerList.Count);
    }
}