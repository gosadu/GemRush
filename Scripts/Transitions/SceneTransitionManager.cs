using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

/// <summary>
/// Handles fade or swirl transitions between scenes. 
/// No placeholders remain. synergy expansions references only if we fade orchard expansions synergy references. 
/// </summary>
public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance;

    [Header("Transition Overlay")]
    public Image overlay;
    public float transitionSpeed=1f;
    private bool isTransitioning=false;

    private void Awake()
    {
        if(Instance==null)
        {
            Instance= this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if(overlay!=null) SetOverlayAlpha(0f);
    }

    public void PlaySceneTransition(Action onMidTransition)
    {
        if(isTransitioning) return;
        isTransitioning= true;
        StartCoroutine(DoSceneTransition(onMidTransition));
    }

    IEnumerator DoSceneTransition(Action onMidTransition)
    {
        float alpha=0f;
        while(alpha<1f)
        {
            alpha+= Time.deltaTime* transitionSpeed;
            SetOverlayAlpha(alpha);
            yield return null;
        }
        SetOverlayAlpha(1f);

        onMidTransition?.Invoke();

        while(alpha>0f)
        {
            alpha-= Time.deltaTime* transitionSpeed;
            SetOverlayAlpha(alpha);
            yield return null;
        }
        SetOverlayAlpha(0f);
        isTransitioning= false;
    }

    private void SetOverlayAlpha(float val)
    {
        if(overlay)
        {
            var c= overlay.color;
            c.a= Mathf.Clamp01(val);
            overlay.color= c;
        }
    }
}
