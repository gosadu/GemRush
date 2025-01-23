using UnityEngine;
using DG.Tweening; // For optional DOTween usage

/// <summary>
/// Represents each gem on the puzzle board, including advanced animations for 
/// idle glow or subtle movement. References synergy expansions or cameo illusions usage hooking only if needed (no placeholders).
/// </summary>
public class Gem : MonoBehaviour
{
    public GemColor gemColor;

    [Header("Animator Approach")]
    [Tooltip("Assign an Animator if you want per-color states (RedIdle, BlueIdle, etc.).")]
    public Animator animator;

    [Header("DOTween Floating")]
    [Tooltip("Enable to apply a gentle up/down tween for a magical effect.")]
    public bool enableFloatEffect = true;
    public float floatDistance = 0.1f;
    public float floatDuration = 1.5f;

    private PuzzleBoardManager boardManager;

    /// <summary>
    /// Called by PuzzleBoardManager upon creation. 
    /// We store the color and reference for synergy expansions or forging synergy combos if needed.
    /// </summary>
    public void InitializeGem(GemColor color, PuzzleBoardManager manager)
    {
        gemColor = color;
        boardManager = manager;

        if(animator)
        {
            // If we have an Animator, we trigger the correct idle state
            UpdateGemVisualAnimator();
        }
        else
        {
            Debug.Log("[Gem] Animator not assigned, using fallback or DOTween approach for idle visual.");
        }

        if(enableFloatEffect)
        {
            // Use DOTween to create a gentle up/down motion
            float startY = transform.localPosition.y;
            transform.DOLocalMoveY(startY + floatDistance, floatDuration)
                     .SetLoops(-1, LoopType.Yoyo)
                     .SetEase(Ease.InOutSine);
        }
    }

    /// <summary>
    /// If we have an Animator, trigger an idle animation per gem color.
    /// For Radiant, we can have a special shimmering loop.
    /// </summary>
    private void UpdateGemVisualAnimator()
    {
        if(animator == null) return;
        switch(gemColor)
        {
            case GemColor.Red:
                animator.SetTrigger("RedIdle");
                break;
            case GemColor.Blue:
                animator.SetTrigger("BlueIdle");
                break;
            case GemColor.Green:
                animator.SetTrigger("GreenIdle");
                break;
            case GemColor.Yellow:
                animator.SetTrigger("YellowIdle");
                break;
            case GemColor.Radiant:
                animator.SetTrigger("RadiantIdle");
                break;
            default:
                animator.SetTrigger("NoneIdle");
                break;
        }
    }

    /// <summary>
    /// Example user click logic if you want to swap gems by mouse.
    /// </summary>
    void OnMouseDown()
    {
        if(boardManager)
        {
            // Board manager uses a separate GemSelector or logic for swapping
            GemSelector.Instance?.SetSelectedGem(this);
        }
    }
}
