using UnityEngine;
using DG.Tweening; // For optional DOTween usage

/// <summary>
/// Represents each gem on the puzzle board, including advanced animations for
/// idle glow or subtle movement. Corrupted gem logic (phase) is included if needed.
/// No references to GemSelector remain, relying fully on GemDragHandler for user interaction.
/// orchard≥Tier gating cameo illusions usage hooking forging synergy combos references remain in the puzzle manager.
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

    // If we store corrupted logic:
    public int corruptedPhase = 0;
    private bool locked = false;

    private PuzzleBoardManager boardManager;

    /// <summary>
    /// Called by PuzzleBoardManager upon creation.
    /// We store the color + manager reference for synergy expansions cameo illusions usage hooking forging combos,
    /// no placeholders remain. No GemSelector usage.
    /// </summary>
    public void InitializeGem(GemColor color, PuzzleBoardManager manager)
    {
        gemColor = color;
        boardManager = manager;

        // If Corrupted, start phase=1 if not set:
        if(gemColor == GemColor.Corrupted && corruptedPhase==0)
        {
            corruptedPhase=1;
        }

        if(animator)
        {
            UpdateGemVisualAnimator();
        }
        else
        {
            Debug.Log("[Gem] Animator not assigned, using fallback or DOTween approach for idle visual.");
        }

        if(enableFloatEffect)
        {
            float startY= transform.localPosition.y;
            transform.DOLocalMoveY(startY + floatDistance, floatDuration)
                     .SetLoops(-1, LoopType.Yoyo)
                     .SetEase(Ease.InOutSine);
        }
    }

    /// <summary>
    /// If we have an Animator, trigger an idle animation per gem color.
    /// For Radiant, we can have a special shimmering loop. For Corrupted, a dark idle effect.
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
            case GemColor.Corrupted:
                animator.SetTrigger("CorruptedIdle");
                break;
            default:
                animator.SetTrigger("NoneIdle");
                break;
        }
    }

    public bool IsLocked()
    {
        return locked;
    }

    public void LockGem()
    {
        locked = true;
    }
}
