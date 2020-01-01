using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionController : MonoBehaviour
{
    public static TransitionController instance;

    public RawImage overlayImage;
    public Material transitionMaterialPrefab;

    void Awake()
    {
        instance = this;
        overlayImage.material = new Material(transitionMaterialPrefab); // use cloned material instead of original
    }

    /*
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            ShowScene(!sceneVisible);
        }
    }
    */

    static bool sceneVisible = true;
    public static void ShowScene(bool show, float speed = 1f, Texture2D transitionEffect = null)
    {
        if(transitioningOverlay != null)
        {
            instance.StopCoroutine(transitioningOverlay);
        }
        
        sceneVisible = show;

        if(transitionEffect != null)
        {
            instance.overlayImage.material.SetTexture("_AlphaTex", transitionEffect);
        }

        transitioningOverlay = instance.StartCoroutine(TransitioningOverlay(show, speed));
    }

    static Coroutine transitioningOverlay = null;
    static IEnumerator TransitioningOverlay(bool show, float speed)
    {
        float targetVal = show ? 1 : 0;
        float currVal = instance.overlayImage.material.GetFloat("_Cutoff");

        while(currVal != targetVal)
        {
            currVal = Mathf.MoveTowards(currVal, targetVal, speed * Time.deltaTime);
            instance.overlayImage.material.SetFloat("_Cutoff", currVal);
            yield return new WaitForEndOfFrame();
        }

        transitioningOverlay = null;
    }

    public static void TransitionLayer(LayerController.LAYER layer, Texture2D targetImage, Texture2D transitionEffect, float speed = .5f)
    {
        if(layer.transitionCoroutine != null)
            instance.StopCoroutine(layer.transitionCoroutine);
        
        if(targetImage != null)
        {
            layer.transitionCoroutine = instance.StartCoroutine(TransitioningLayer(layer, targetImage, transitionEffect, speed));
        }
        else
        {
            layer.transitionCoroutine = instance.StartCoroutine(TransitioningLayerToNull(layer, transitionEffect, speed));
        }

    }

    static IEnumerator TransitioningLayer(LayerController.LAYER layer, Texture2D targetImage, Texture2D transitionEffect, float speed)
    {
        GameObject ob = Instantiate(layer.layerTemplate, layer.root.transform) as GameObject;
        ob.SetActive(true);

        RawImage im = ob.GetComponent<RawImage>();
        im.texture = targetImage;

        layer.activeImage = im;
        layer.allImages.Add(im);

        im.material = new Material(instance.transitionMaterialPrefab);
        im.material.SetTexture("_AlphaTex", transitionEffect);
        im.material.SetFloat("_Cutoff", 1);
        float currVal = 1;

        while(currVal>0)
        {
            currVal = Mathf.MoveTowards(currVal, 0, speed * Time.deltaTime);
            im.material.SetFloat("_Cutoff", currVal);
            yield return new WaitForEndOfFrame();
        }

        // remove the material so we can use the regular alpha for transitions.
        // check for null so we rapidly progress through fading and transition overlaps.
        if(im != null)
        {
            im.material = null;
            // transition does not use alpha so we make sure alpha is up
            im.color = VN_Util.SetAlpha(im.color, 1);
        }

        // now remove all other images on layer.
        for(int i = layer.allImages.Count - 1; i>=0; i--)
        {
            if(layer.allImages[i] == layer.activeImage && layer.activeImage != null)
            {   
                continue;
            }    
            
            if(layer.allImages[i] != null)
            {
                Destroy(layer.allImages[i].gameObject, 0.01f);
            }

            layer.allImages.RemoveAt(i);
        }

        // clear special transition fields
        layer.transitionCoroutine = null;
    }

    static IEnumerator TransitioningLayerToNull(LayerController.LAYER layer, Texture2D transitionEffect, float speed)
    {
        List<RawImage> currentImagesOnLayer = new List<RawImage>();

        foreach(RawImage im in layer.allImages)
        {
            im.material = new Material(instance.transitionMaterialPrefab);
            im.material.SetTexture("_AlphaTex", transitionEffect);
            im.material.SetFloat("_Cutoff", 0);
            currentImagesOnLayer.Add(im);
        }

        float currVal = 0;
        while(currVal < 1)
        {
            currVal = Mathf.MoveTowards(currVal, 1, speed*Time.deltaTime);
            for(int i=0; i<layer.allImages.Count; i++)
            {
                layer.allImages[i].material.SetFloat("_Cutoff", currVal);
            }
            yield return new WaitForEndOfFrame();
        }

        foreach (RawImage im in currentImagesOnLayer)
        {
            layer.allImages.Remove(im);
            if(im.material != null)
            {
                Destroy(im.gameObject, 0.01f);
            }
        }

        layer.transitionCoroutine = null;

    }

}
