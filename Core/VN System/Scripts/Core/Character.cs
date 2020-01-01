using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Character
{

    /// <summary>
    /// Determines whether "characterName returns ??? or the actual private name
    /// </summary>
    public bool hasMet = true;
    string _characterName;
    public string characterName {
        get { return hasMet ? _characterName : "???"; }
        set { _characterName = value; }
    }
    [HideInInspector]
    public RectTransform root;
    public Renderers renderers = new Renderers();

    public bool enabled { get { return root.gameObject.activeInHierarchy;} set { root.gameObject.SetActive(value);} }
    public Vector2 anchorPadding { get { return root.anchorMax - root.anchorMin;} }
    DialogueSystem dialogue;

    /// <summary>
    /// Create a new character.
    /// </summary>
    public Character(string _name, bool enabledOnStart = true)
    {
        CharacterManager cm = CharacterManager.instance;
        // locate the character prefab
        GameObject prefab = Resources.Load("Prefabs/Character["+_name+"]") as GameObject;
        GameObject ob = Object.Instantiate(prefab, cm.characterPanel);

        root = ob.GetComponent<RectTransform>();
        characterName = _name;

        //get the renderer(s)
        renderers.bodyRenderer = ob.transform.Find("Body").GetComponentInChildren<Image>();
        renderers.faceRenderer = ob.transform.Find("Face").GetComponentInChildren<Image>();
        renderers.allBodyRenderers.Add(renderers.bodyRenderer);
        renderers.allFaceRenderers.Add(renderers.faceRenderer);

        dialogue = DialogueSystem.instance;

        enabled = enabledOnStart;
    }

    /// <summary>
    /// Make this character say something.
    /// </summary>
    public void Say(string speech, bool add = false)
    {
        dialogue.Say(speech, characterName, add);
    }

    #region Movement System
    Vector2 targetPosition;
    Coroutine moving;
    bool isMoving{ get{ return moving != null;}}
    /// <summary>
    /// Move to a specific point relative to the current canvas space: (1,1) = far top right; (0,0) = far bottom left; (0.5,0.5) = middle;
    /// </summary>
    public void MoveTo(Vector2 target, float speed)
    {
        // if we are moving, then stop moving
        StopMoving();
        // start new moving coroutine
        moving = CharacterManager.instance.StartCoroutine(Moving(target, speed));
    }

    public void MoveTo(Vector2 start, Vector2 end, float speed)
    {
        SetPosition(start);
        MoveTo(end, speed);
    }

    /// <summary>
    /// Stops the character in its tracks, either setting it immediately at the target position or exactly where it currently is.
    /// </summary>
    public void StopMoving(bool arriveAtTargetPositionImmediately = false)
    {
        if(isMoving)
            CharacterManager.instance.StopCoroutine(moving);
            if(arriveAtTargetPositionImmediately)
                SetPosition(targetPosition);
        moving = null;
    }

    /// <summary>
    /// Immediately set the position of this character to the intended target.
    /// </summary>
    public void SetPosition(Vector2 target)
    {
        Vector2 padding = anchorPadding;
        float maxX = 1f - padding.x;
        float maxY = 1f - padding.y;
        Vector2 minAnchorTarget = new Vector2(maxX * targetPosition.x, maxY * targetPosition.y);
        root.anchorMin = minAnchorTarget;
        root.anchorMax = root.anchorMin + padding;

    }

    /// <summary>
    /// The coroutine that runs to gradually move the character towards a position.
    /// </summary>
    IEnumerator Moving(Vector2 target, float speed)
    {
        targetPosition = target;
        // now we want to get the padding between the anchors of this character so that we know what their min and max positions are.
        Vector2 padding = anchorPadding;
        // now get the limitations for 0 to 100% movement. The farthest a character can move to the right before reaching 100% should be the 1 value - the padding.
        float maxX = 1f - padding.x;
        float maxY = 1f - padding.y;

        // now get the actual position target for the minimum anchors (left/bottom bounds) of the character. because maxX and maxY is just a percentage reference.
        Vector2 minAnchorTarget = new Vector2(maxX * targetPosition.x, maxY * targetPosition.y);
        speed *= Time.deltaTime;

        // move until we reach the target position.
        while(root.anchorMin != minAnchorTarget)
        {
            root.anchorMin = Vector2.MoveTowards(root.anchorMin, minAnchorTarget, speed);
            root.anchorMax = root.anchorMin + padding;
            yield return new WaitForEndOfFrame();
        }

        StopMoving();
    }
    #endregion

    #region Facial Transition System
    public Sprite GetSprite(int index=0)
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>("Images/Characters/" + characterName);
        return sprites[index];
    }

    public Sprite GetSprite(string spriteName = "")
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>("Images/Characters/" + characterName);
        for(int i=0; i<sprites.Length; i++)
        {
            if (sprites[i].name == spriteName)
                return sprites[i];
        }
        return sprites.Length>0 ? sprites[0] : null;
    }

    public void SetBody(int index)
    {
        renderers.bodyRenderer.sprite = GetSprite(index);
    }

    public void SetBody(Sprite sprite)
    {
        renderers.bodyRenderer.sprite = sprite;
    }
    public void SetBody(string spriteName)
    {
        renderers.bodyRenderer.sprite = GetSprite(spriteName);
    }

    public void SetFace(int index)
    {
        renderers.faceRenderer.sprite = GetSprite(index);
    }

    public void SetFace(Sprite sprite)
    {
        renderers.faceRenderer.sprite = sprite;
    }
    public void SetFace(string spriteName)
    {
        renderers.faceRenderer.sprite = GetSprite(spriteName);
    }

    bool isTransitioningBody{ get { return transitioningBody != null;} }
    Coroutine transitioningBody = null;

    public void TransitionBody(Sprite sprite, float speed)
    {
        StopTransitioningBody();
        transitioningBody = CharacterManager.instance.StartCoroutine(TransitioningBody(sprite, speed));
    }

    void StopTransitioningBody()
    {
        if(isTransitioningBody)
            CharacterManager.instance.StopCoroutine(transitioningBody);
        transitioningBody = null;
    }

    protected IEnumerator TransitioningBody(Sprite sprite, float speed)
    {
        for(int i=0; i<renderers.allBodyRenderers.Count; i++)
        {
            Image image = renderers.allBodyRenderers[i];
            if(image.sprite == sprite)
            {
                renderers.bodyRenderer = image;
                break;
            }
        }

        if(renderers.bodyRenderer.sprite != sprite)
        {
            Image image = Object.Instantiate(renderers.bodyRenderer.gameObject, renderers.bodyRenderer.transform.parent).GetComponent<Image>();
            renderers.allBodyRenderers.Add(image);
            renderers.bodyRenderer = image;
            image.color = VN_Util.SetAlpha(image.color, 0f);
            image.sprite = sprite;
        }
        
        while(VN_Util.TransitionImages(ref renderers.bodyRenderer, ref renderers.allBodyRenderers, speed, true))
            yield return new WaitForEndOfFrame();
        StopTransitioningBody();
    }


    bool isTransitioningFace { get { return transitioningFace != null; } }
    Coroutine transitioningFace = null;

    public void TransitionFace(Sprite sprite, float speed)
    {
        StopTransitioningFace();
        transitioningFace = CharacterManager.instance.StartCoroutine(TransitioningFace(sprite, speed));
    }

    void StopTransitioningFace()
    {
        if (isTransitioningFace)
            CharacterManager.instance.StopCoroutine(transitioningFace);
        transitioningFace = null;
    }

    protected IEnumerator TransitioningFace(Sprite sprite, float speed)
    {
        for (int i = 0; i < renderers.allFaceRenderers.Count; i++)
        {
            Image image = renderers.allFaceRenderers[i];
            if (image.sprite == sprite)
            {
                renderers.faceRenderer = image;
                break;
            }
        }

        if (renderers.faceRenderer.sprite != sprite)
        {
            Image image = Object.Instantiate(renderers.faceRenderer.gameObject, renderers.faceRenderer.transform.parent).GetComponent<Image>();
            renderers.allFaceRenderers.Add(image);
            renderers.faceRenderer = image;
            image.color = VN_Util.SetAlpha(image.color, 0f);
            image.sprite = sprite;
        }

        while (VN_Util.TransitionImages(ref renderers.faceRenderer, ref renderers.allFaceRenderers, speed, true))
            yield return new WaitForEndOfFrame();
        StopTransitioningFace();
    }

    #endregion

    /// <summary>
    /// Horizontally reverses the orientation of the given character(s) on its root Vector3 gameObject.
    /// </summary>
    public void Flip()
    {
        root.localScale = new Vector3(root.localScale.x * -1, 1, 1);
    }

    /// <summary>
    /// Flips character horizontally to the left on its root Vector3 gameObject (x->1)
    /// </summary>
    public void FaceLeft()
    {
        root.localScale = Vector3.one;
    }

    /// <summary>
    /// Flips character horizontally to the left on its root Vector3 gameObject (x->-1)
    /// </summary>
    public void FaceRight()
    {
        root.localScale = new Vector3(-1, 1, 1);
    }

    public void FadeOut(float speed = 3f)
    {
        Sprite alphaSprite = Resources.Load<Sprite>("Images/AlphaOnly/empty");

        lastBodySprite = renderers.bodyRenderer.sprite;
        lastFaceSprite = renderers.faceRenderer.sprite;

        TransitionBody(alphaSprite, speed);
        TransitionFace(alphaSprite, speed);
    }

    Sprite lastBodySprite, lastFaceSprite = null;
    public void FadeIn(float speed = 3f)
    {
        if(lastBodySprite != null && lastFaceSprite != null)
        {
            TransitionBody(lastBodySprite, speed);
            TransitionFace(lastFaceSprite, speed);
        }
    }

    [System.Serializable]
    public class Renderers
    {
        /// <summary>
        /// The body renderer for a multi-layer character.
        /// </summary>
        public Image bodyRenderer;
        /// <summary>
        /// The facial expression renderer for a multi-layer character.
        /// </summary>
        public Image faceRenderer;

        public List<Image> allBodyRenderers = new List<Image>();
        public List<Image> allFaceRenderers = new List<Image>();
    }
    
}
