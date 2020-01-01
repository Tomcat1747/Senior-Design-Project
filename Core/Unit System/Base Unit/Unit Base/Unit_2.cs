using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit_2 : Unit {
    protected Animator animator;
    public RuntimeAnimatorController animatorController {
        get { return animator.runtimeAnimatorController; }
        set { animator.runtimeAnimatorController = value; }
    }

    #region Animations & Color Markers
    #region Movement Animation
    /// <summary>
    /// Implements Unit Movement Animation in coordination with the Unit's animator. Takes a List path as a parameter.
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    protected override IEnumerator MovementAnimation (List<Cell> path) {
        isMoving = true;
        AnimateUnit (isMoving);
        path.Reverse ();
        foreach (var cell in path) {
            Vector3 destination_pos = new Vector3 (cell.Coord.x, cell.Coord.y, -1);
            AnimateDirection (transform.localPosition, destination_pos);
            while (transform.localPosition != destination_pos) {
                transform.localPosition = Vector3.MoveTowards (transform.localPosition, destination_pos, Time.deltaTime * MovementSpeed);
                yield return 0;
            }
        }

        isMoving = false;
        AnimateUnit (isMoving);
    }

    /// <summary>
    /// Toggle via the isStarting bool to turn on and off the Movement Animation for the given unit.
    /// </summary>
    /// <param name="isStarting"></param>
    protected virtual void AnimateUnit (bool isStarting) {
        animator.SetBool ("isMoving", isStarting);
        if (!isStarting) {
            animator.SetFloat ("XInput", 0f);
            animator.SetFloat ("YInput", 0f);
        }
    }

    /// <summary>
    /// Based on the startingPoint & endPoint, set the current float values accordingly in the animator.
    /// </summary>
    /// <param name="startPoint"></param>
    /// <param name="endPoint"></param>
    protected virtual void AnimateDirection (Vector3 startPoint, Vector3 endPoint) {
        float XInput = endPoint.x - startPoint.x;
        float YInput = endPoint.y - startPoint.y;
        animator.SetFloat ("XInput", XInput);
        animator.SetFloat ("YInput", YInput);
    }
    protected virtual void AnimateDirection (Vector3 result) {
        animator.SetFloat ("XInput", result.x);
        animator.SetFloat ("YInput", result.y);
    }
    #endregion

    private Coroutine PulseCoroutine;

    public override void OnUnitDeselected () {
        base.OnUnitDeselected ();
        StopCoroutine (PulseCoroutine);
        transform.localScale = new Vector3 (1, 1, 1);
    }

    public override void MarkAsAttacking (Unit other) { // Modify this for the unit attack animations
        StartCoroutine (Jerk (other));
    }

    private IEnumerator Jerk (Unit other) {
        AnimateUnit (true);
        GetComponent<SpriteRenderer> ().sortingOrder = 6;
        var heading = other.transform.position - transform.position;
        var direction = heading / heading.magnitude;
        AnimateDirection (heading);

        float startTime = Time.time;

        while (startTime + 0.25f > Time.time) {
            transform.position = Vector3.Lerp (transform.position, transform.position + (direction / 5f), ((startTime + 0.25f) - Time.time));
            yield return 0;
        }
        startTime = Time.time;
        while (startTime + 0.25f > Time.time) {
            transform.position = Vector3.Lerp (transform.position, transform.position - (direction / 5f), ((startTime + 0.25f) - Time.time));
            yield return 0;
        }
        transform.position = Cell.transform.position + new Vector3 (0, 0, -1f);
        GetComponent<SpriteRenderer> ().sortingOrder = 4;
        AnimateUnit (false);
    }

    public override void MarkAsDefending (Unit other) {
        StartCoroutine (Glow (new Color (1, 0, 0, 0.8f), 1));
    }

    public override void MarkAsAvoiding (Unit other) {
        StartCoroutine (Glow (new Color (1, 1, 0, 0.8f), 1));
    }

    public override void MarkAsCritical (Unit other) {
        StartCoroutine (Glow (new Color (0, 1, 1, 0.8f), 1));
    }

    private IEnumerator Glow (Color color, float cooloutTime) {
        var _renderer = transform.Find ("Marker").GetComponent<SpriteRenderer> ();
        float startTime = Time.time;

        while (startTime + cooloutTime > Time.time) {
            _renderer.color = Color.Lerp (new Color (1, 1, 1, 0), color, (startTime + cooloutTime) - Time.time);
            yield return 0;
        }

        _renderer.color = Color.clear;
    }

    public override void MarkAsDestroyed () { }

    public override void MarkAsFriendly () {
        SetColor (Color.white);
    }

    public override void MarkAsReachableEnemy () {
        SetColor (new Color (1, 0, 0));
    }

    public override void MarkAsSelected () { // Modify this for the unit selected animation
        PulseCoroutine = StartCoroutine (Pulse (1.0f, 0.5f, 1.25f));
        SetColor (new Color (0.8f, 0.8f, 1));
    }
    private IEnumerator Pulse (float breakTime, float delay, float scaleFactor) {
        var baseScale = transform.localScale;
        while (true) {
            float growingTime = Time.time;
            while (growingTime + delay > Time.time) {
                transform.localScale = Vector3.Lerp (baseScale * scaleFactor, baseScale, (growingTime + delay) - Time.time);
                yield return 0;
            }

            float shrinkingTime = Time.time;
            while (shrinkingTime + delay > Time.time) {
                transform.localScale = Vector3.Lerp (baseScale, baseScale * scaleFactor, (shrinkingTime + delay) - Time.time);
                yield return 0;
            }

            yield return new WaitForSeconds (breakTime);
        }
    }

    public override void MarkAsFinished () {
        SetColor (Color.gray);
    }

    public override void UnMark () {
        SetColor (Color.white);
    }

    private void SetColor (Color color) {
        var _renderer = GetComponent<SpriteRenderer> ();
        if (_renderer != null) {
            _renderer.color = color;
        }
    }
    #endregion
}