using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// EnhancedBoardManager manages an 8x8 (or any rows/cols) gem board.
/// Auto-fits the board to a designated gemBoardContainer RectTransform,
/// recalculating cell size whenever that container changes dimension.
///
/// Also includes an enum BoardScalingMode to specify:
///  - Square (keep each gem cell equally wide/tall),
///  - FillWidth (stretch horizontally; gem height can differ),
///  - FillHeight (stretch vertically; gem width can differ).
/// 
/// Attach this script to a GameObject named "EnhancedBoardManager" in your scene.
/// Then, in the Inspector:
///  1) Set autoFitGems=true.
///  2) Assign gemBoardContainer= your "GemBoardContainer" RectTransform.
///  3) (Optional) Select your BoardScalingMode preference.
///  4) Provide gemViewPrefab, gemSprites, references to animationSystem, etc.
///  5) Call InitBoard() after you set everything, e.g., from GameManager.Start().
/// 
/// Make sure your "GemBoardContainer" is anchored & sized to fill the center space!
/// This script then uses container.rect.width/height to compute cellSize.
/// </summary>
[RequireComponent(typeof(ArcSwapEffect))]
[RequireComponent(typeof(InvalidMoveFX))]
[RequireComponent(typeof(BoardSettleFX))]
public class EnhancedBoardManager : MonoBehaviour
{
    // --------------------------
    // Board Size
    // --------------------------
    [Header("Board Size")]
    public int rows = 8;
    public int cols = 8;
    public float cellSize = 100f; // used if autoFitGems=false

    // --------------------------
    // Gem Settings
    // --------------------------
    [Header("Gem Settings")]
    public GameObject gemViewPrefab;
    public Sprite[] gemSprites;

    // --------------------------
    // Aggregator / HP
    // --------------------------
    [Header("Aggregator Synergy")]
    public bool useAggregator = true;
    private int aggregatorPoints;
    private bool aggregatorVisible;

    [Header("Player HP")]
    public int playerMaxHP = 100;
    private int playerHP;

    // --------------------------
    // External References
    // --------------------------
    [Header("References")]
    public AnimationSystem animationSystem;
    public SoundManager soundManager;
    public UIManager uiManager;
    public BossManager bossManager;

    // --------------------------
    // Auto-Fit
    // --------------------------
    public enum BoardScalingMode
    {
        Square,    // each gem cell is square, use Mathf.Min(...)
        FillWidth, // fill horizontal, cell height is (containerHeight/rows) can differ
        FillHeight // fill vertical, cell width is (containerWidth/cols) can differ
    }

    [Header("Optional Auto-Fit")]
    [Tooltip("If true, we compute cellSize from gemBoardContainer's width/height.")]
    public bool autoFitGems = true;

    [Tooltip("RectTransform that defines the area for the gem board (child of CenterPanel).")]
    public RectTransform gemBoardContainer;

    [Tooltip("Choose how you want to scale the board if the container is not square.")]
    public BoardScalingMode scalingMode = BoardScalingMode.Square;

    // --------------------------
    // Internal
    // --------------------------
    private ArcSwapEffect arcSwap;
    private InvalidMoveFX invalidMoveFX;
    private BoardSettleFX boardSettleFX;
    private GemData[,] board;
    private bool isBoardReady;
    private int movesLeft;
    private bool isSwapping = false;

    // ----------------------------------------------------------
    // Unity Lifecycle
    // ----------------------------------------------------------
    void Awake()
    {
        arcSwap = GetComponent<ArcSwapEffect>();
        invalidMoveFX = GetComponent<InvalidMoveFX>();
        boardSettleFX = GetComponent<BoardSettleFX>();

        // Attempt to auto-size this manager's own RectTransform if it was never set,
        // but typically we rely on gemBoardContainer for actual layout.
        RectTransform rt = GetComponent<RectTransform>();
        if (rt && rt.sizeDelta.magnitude < 0.1f)
        {
            rt.anchorMin = new Vector2(0.5f, 0.5f);
            rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.pivot = new Vector2(0.5f, 0.5f);
            rt.sizeDelta = new Vector2(800f, 800f);
        }
    }

    // If the container changes (due to safe area or resolution change),
    // we recalc cellSize if autoFit is true.
    protected void OnRectTransformDimensionsChange()
    {
        if (!autoFitGems || gemBoardContainer == null) return;
        RecalculateCellSizeAndRedraw();
    }

    // ----------------------------------------------------------
    // Board Init
    // ----------------------------------------------------------
    public void InitBoard()
    {
        // Clear existing gems
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        aggregatorPoints = 0;
        aggregatorVisible = false;
        isBoardReady = false;
        movesLeft = 30;
        playerHP = playerMaxHP;

        if (!gemViewPrefab)
        {
            Debug.LogError("[EnhancedBoardManager] gemViewPrefab is missing!");
            return;
        }
        if (gemSprites == null || gemSprites.Length == 0)
        {
            Debug.LogError("[EnhancedBoardManager] gemSprites is empty!");
            return;
        }

        // Compute cellSize from container if needed
        if (autoFitGems && gemBoardContainer != null)
        {
            RecalculateCellSize();
        }

        // Create data array
        board = new GemData[rows, cols];

        // Create all gems
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                CreateGem(r, c);
            }
        }

        isBoardReady = true;
        if (uiManager) uiManager.UpdateMoves(movesLeft);
    }

    private void RecalculateCellSizeAndRedraw()
    {
        RecalculateCellSize();
        RedrawBoard();
    }

    private void RecalculateCellSize()
    {
        float w = gemBoardContainer.rect.width;
        float h = gemBoardContainer.rect.height;

        switch (scalingMode)
        {
            case BoardScalingMode.Square:
                {
                    // Use the smaller dimension for a perfect square cell
                    float cw = w / cols;
                    float ch = h / rows;
                    cellSize = Mathf.Min(cw, ch);
                    break;
                }
            case BoardScalingMode.FillWidth:
                {
                    // Fill horizontally, cell height might be smaller/larger
                    cellSize = w / cols;
                    break;
                }
            case BoardScalingMode.FillHeight:
                {
                    // Fill vertically, cell width might differ
                    cellSize = h / rows;
                    break;
                }
        }

        // Debug to see final result
        Debug.Log($"[EnhancedBoardManager] autoFit cellSize={cellSize}, container=({w}x{h}), mode={scalingMode}");
    }

    // ----------------------------------------------------------
    // Gem Creation
    // ----------------------------------------------------------
    private void CreateGem(int r, int c)
    {
        // pick random color index from gemSprites
        int colorIndex = Random.Range(0, gemSprites.Length);
        bool isWildcard = (colorIndex >= 9); // example logic if 9.. are wild
        GemData data = new GemData(r, c, colorIndex, isWildcard);
        board[r, c] = data;

        Vector2 pos = CalculatePosition(r, c);

        GameObject gemObj = Instantiate(gemViewPrefab, this.transform);
        RectTransform rt = gemObj.GetComponent<RectTransform>();
        if (rt) rt.anchoredPosition = pos;
        else Debug.LogWarning("[CreateGem] No RectTransform on gem prefab!");

        GemView gv = gemObj.GetComponent<GemView>();
        if (gv)
        {
            Sprite assignedSprite = gemSprites[colorIndex];
            gv.InitGem(data, assignedSprite, this);
        }
    }

    // E.g., center the board around (0,0). If you prefer top-left,
    // just remove the offsets so x= c*cellSize, y= -r*cellSize.
    private Vector2 CalculatePosition(int r, int c)
    {
        float startX = -(cols * cellSize) / 2f + (cellSize / 2f);
        float startY = (rows * cellSize) / 2f - (cellSize / 2f);
        float x = startX + (c * cellSize);
        float y = startY - (r * cellSize);
        return new Vector2(x, y);
    }

    // ----------------------------------------------------------
    // Board Swapping & Matching
    // ----------------------------------------------------------
    public void SwapGems(GemData g1, GemData g2)
    {
        if (!isBoardReady || isSwapping) return;
        isSwapping = true;
        StartCoroutine(DoArcSwapGems(g1, g2));
    }

    private IEnumerator DoArcSwapGems(GemData g1, GemData g2)
    {
        GemView gv1 = FindGemView(g1);
        GemView gv2 = FindGemView(g2);
        if (!gv1 || !gv2)
        {
            Debug.LogWarning("[EnhancedBoardManager] Missing GemView for swap.");
            isSwapping = false;
            yield break;
        }

        RectTransform r1 = gv1.GetComponent<RectTransform>();
        RectTransform r2 = gv2.GetComponent<RectTransform>();

        float swapDuration = 0.3f;
        yield return StartCoroutine(arcSwap.DoArcSwap(r1, r2, swapDuration, null));

        bool moveIsValid = CheckIfValidMove(g1, g2);
        if (!moveIsValid)
        {
            // revert
            yield return StartCoroutine(invalidMoveFX.DoInvalidMove(r1, 0.25f));
            yield return StartCoroutine(arcSwap.DoArcSwap(r1, r2, 0.2f, null));
            isSwapping = false;
            yield break;
        }

        // finalize data
        board[g1.row, g1.col] = g2;
        board[g2.row, g2.col] = g1;
        int oldR = g1.row, oldC = g1.col;
        g1.row = g2.row; g1.col = g2.col;
        g2.row = oldR;   g2.col = oldC;

        movesLeft--;
        if (uiManager) uiManager.UpdateMoves(movesLeft);

        RedrawBoard();
        if (soundManager) soundManager.PlaySwapSound();

        StartCoroutine(CheckMatches());
        isSwapping = false;
    }

    private bool CheckIfValidMove(GemData g1, GemData g2)
    {
        if (!AreNeighbors(g1, g2)) return false;
        board[g1.row, g1.col] = g2;
        board[g2.row, g2.col] = g1;

        List<GemData> matched = FindMatches();

        // revert
        board[g1.row, g1.col] = g1;
        board[g2.row, g2.col] = g2;

        return (matched.Count > 0);
    }

    private bool AreNeighbors(GemData a, GemData b)
    {
        int rowDist = Mathf.Abs(a.row - b.row);
        int colDist = Mathf.Abs(a.col - b.col);
        return (rowDist + colDist == 1);
    }

    private IEnumerator CheckMatches()
    {
        yield return new WaitForSeconds(0.2f);

        List<GemData> matched = FindMatches();
        if (matched.Count > 0)
        {
            if (animationSystem) 
                animationSystem.AnimateGemRemoval(matched, board, this);
            else 
                Debug.LogWarning("[EnhancedBoardManager] No AnimationSystem assigned!");

            if (soundManager) soundManager.PlayMatchSound();
            if (useAggregator) aggregatorPoints += matched.Count * 10;

            float waitTime = (animationSystem) ? (animationSystem.vanishDuration + 0.3f) : 0.5f;
            yield return new WaitForSeconds(waitTime);

            CascadeGems();
            yield return new WaitForSeconds(0.3f);
            StartCoroutine(CheckMatches());
        }
        else
        {
            if (useAggregator && aggregatorPoints > 0 && !aggregatorVisible)
            {
                aggregatorVisible = true;
                yield return new WaitForSeconds(1f);
                if (bossManager) bossManager.TakeDamage(aggregatorPoints);
                aggregatorPoints = 0;
                aggregatorVisible = false;
            }

            yield return StartCoroutine(DoBoardSettleEffect());
        }
    }

    private IEnumerator DoBoardSettleEffect()
    {
        GemView[] allGems = FindObjectsOfType<GemView>();
        yield return StartCoroutine(boardSettleFX.DoBoardExhale(allGems, 0.5f));
    }

    private List<GemData> FindMatches()
    {
        List<GemData> matched = new List<GemData>();

        // Horizontal
        for (int r = 0; r < rows; r++)
        {
            int matchCount = 1;
            for (int c = 1; c < cols; c++)
            {
                if (board[r,c] != null && board[r,c-1] != null &&
                    board[r,c].colorIndex == board[r,c-1].colorIndex &&
                    !board[r,c].isSpecial && !board[r,c-1].isSpecial)
                {
                    matchCount++;
                }
                else
                {
                    if (matchCount >= 3)
                    {
                        int startC = (c - 1) - (matchCount - 1);
                        for (int cc = startC; cc <= (c - 1); cc++)
                        {
                            if (!matched.Contains(board[r, cc]))
                                matched.Add(board[r, cc]);
                        }
                    }
                    matchCount = 1;
                }
            }
            // edge case
            if (matchCount >= 3)
            {
                int startC = (cols - 1) - (matchCount - 1);
                for (int cc = startC; cc <= (cols - 1); cc++)
                {
                    if (!matched.Contains(board[r, cc]))
                        matched.Add(board[r, cc]);
                }
            }
        }

        // Vertical
        for (int c = 0; c < cols; c++)
        {
            int matchCount = 1;
            for (int r = 1; r < rows; r++)
            {
                if (board[r,c] != null && board[r-1,c] != null &&
                    board[r,c].colorIndex == board[r-1,c].colorIndex &&
                    !board[r,c].isSpecial && !board[r-1,c].isSpecial)
                {
                    matchCount++;
                }
                else
                {
                    if (matchCount >= 3)
                    {
                        int startR = (r - 1) - (matchCount - 1);
                        for (int rr = startR; rr <= (r - 1); rr++)
                        {
                            if (!matched.Contains(board[rr, c]))
                                matched.Add(board[rr, c]);
                        }
                    }
                    matchCount = 1;
                }
            }
            // edge case
            if (matchCount >= 3)
            {
                int startR = (rows - 1) - (matchCount - 1);
                for (int rr = startR; rr <= (rows - 1); rr++)
                {
                    if (!matched.Contains(board[rr, c]))
                        matched.Add(board[rr, c]);
                }
            }
        }

        return matched;
    }

    private void CascadeGems()
    {
        for (int c = 0; c < cols; c++)
        {
            List<GemData> stack = new List<GemData>();
            for (int r = rows - 1; r >= 0; r--)
            {
                if (board[r,c] != null)
                    stack.Add(board[r,c]);
            }
            for (int r = rows - 1; r >= 0; r--)
            {
                if (stack.Count > 0)
                {
                    GemData gem = stack[0];
                    stack.RemoveAt(0);
                    gem.row = r;
                    gem.col = c;
                    board[r,c] = gem;
                }
                else
                {
                    // spawn new gem
                    CreateGem(r, c);
                }
            }
        }
        RedrawBoard();
    }

    /// <summary>
    /// Repositions existing gemViews after changes in row/col or cellSize.
    /// Called after CascadeGems, or if dimension changes (OnRectTransformDimensionsChange).
    /// </summary>
    public void RedrawBoard()
    {
        foreach (Transform child in transform)
        {
            GemView gv = child.GetComponent<GemView>();
            if (gv)
            {
                Vector2 newPos = CalculatePosition(gv.gemData.row, gv.gemData.col);
                RectTransform rt = child.GetComponent<RectTransform>();
                if (rt)
                    rt.anchoredPosition = newPos;
            }
        }
    }

    // ----------------------------------------------------------
    // Helper Functions
    // ----------------------------------------------------------
    public void RemoveGem(GemData data)
    {
        if (board[data.row, data.col] == data)
            board[data.row, data.col] = null;
    }

    public void AddAggregatorPoints(int amt)
    {
        aggregatorPoints += amt;
    }

    public void HealPlayer(int amt)
    {
        playerHP += amt;
        if (playerHP > playerMaxHP) playerHP = playerMaxHP;
    }

    private GemView FindGemView(GemData data)
    {
        GemView[] all = FindObjectsOfType<GemView>();
        foreach (var gv in all)
        {
            if (gv.gemData == data)
                return gv;
        }
        return null;
    }

    // optional skill / item usage events
    public event System.Action<int> OnSkillReady;
    public event System.Action<int> OnItemUsed;

    private void SkillBecameReady(int skillID)
    {
        OnSkillReady?.Invoke(skillID);
    }

    private void ItemWasUsed(int itemSlotIndex)
    {
        OnItemUsed?.Invoke(itemSlotIndex);
    }
}
