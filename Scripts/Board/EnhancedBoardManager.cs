using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(ArcSwapEffect))]
[RequireComponent(typeof(InvalidMoveFX))]
[RequireComponent(typeof(BoardSettleFX))]
public class EnhancedBoardManager : MonoBehaviour
{
    [Header("Board Size")]
    public int rows = 8;
    public int cols = 8;
    public float cellSize = 100f;

    [Header("Gem Settings")]
    public GameObject gemViewPrefab;
    public Sprite[] gemSprites;

    [Header("Aggregator Synergy")]
    public bool useAggregator = true;
    private int aggregatorPoints;
    private bool aggregatorVisible;

    [Header("Player HP")]
    public int playerMaxHP = 100;
    private int playerHP;

    [Header("References")]
    public AnimationSystem animationSystem;
    public SoundManager soundManager;
    public UIManager uiManager;
    public BossManager bossManager;

    private ArcSwapEffect arcSwap;
    private InvalidMoveFX invalidMoveFX;
    private BoardSettleFX boardSettleFX;

    private GemData[,] board;
    private bool isBoardReady;
    private int movesLeft;
    private bool isSwapping = false;

    void Awake()
    {
        arcSwap = GetComponent<ArcSwapEffect>();
        invalidMoveFX = GetComponent<InvalidMoveFX>();
        boardSettleFX = GetComponent<BoardSettleFX>();

        // [NEW] Attempt to auto-size the parent Rect so negative offsets are visible
        RectTransform rt = GetComponent<RectTransform>();
        if (rt)
        {
            // If the user never sized it in the Editor, forcibly set an 800x800 center pivot.
            if (rt.sizeDelta.magnitude < 0.1f) // means basically zero
            {
                rt.anchorMin = new Vector2(0.5f, 0.5f);
                rt.anchorMax = new Vector2(0.5f, 0.5f);
                rt.pivot = new Vector2(0.5f, 0.5f);
                rt.sizeDelta = new Vector2(800f, 800f);
            }
        }
    }

    public void InitBoard()
    {
        // Clear old
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

        board = new GemData[rows, cols];

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

    private void CreateGem(int r, int c)
    {
        // pick random color index from 0..gemSprites.Length-1
        int colorIndex = Random.Range(0, gemSprites.Length);
        bool isWildcard = (colorIndex >= 9); // e.g., if 9..13 are wildcard
        GemData data = new GemData(r, c, colorIndex, isWildcard);
        board[r, c] = data;

        Vector2 pos = CalculatePosition(r, c);
        Debug.Log($"[CreateGem] row={r}, col={c}, pos={pos}");

        GameObject gemObj = Instantiate(gemViewPrefab, this.transform);
        RectTransform rt = gemObj.GetComponent<RectTransform>();
        if (rt)
        {
            rt.anchoredPosition = pos;
        }
        else
        {
            Debug.LogWarning("[CreateGem] No RectTransform found on gem prefab!");
        }

        GemView gv = gemObj.GetComponent<GemView>();
        if (gv)
        {
            Sprite assignedSprite = gemSprites[colorIndex];
            gv.InitGem(data, assignedSprite, this);
        }
    }

    private Vector2 CalculatePosition(int r, int c)
    {
        // We'll center the board in the parent's rect:
        float startX = -(cols * cellSize) / 2f + (cellSize / 2f);
        float startY = (rows * cellSize) / 2f - (cellSize / 2f);
        float x = startX + (c * cellSize);
        float y = startY - (r * cellSize);
        return new Vector2(x, y);
    }

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

        int oldR = g1.row;
        int oldC = g1.col;
        g1.row = g2.row;
        g1.col = g2.col;
        g2.row = oldR;
        g2.col = oldC;

        movesLeft--;
        if (uiManager) uiManager.UpdateMoves(movesLeft);

        RedrawBoard();
        if (soundManager) soundManager.PlaySwapSound();

        StartCoroutine(CheckMatches());
        isSwapping = false;
    }

    private bool CheckIfValidMove(GemData g1, GemData g2)
    {
        if (!AreNeighbors(g1, g2))
        {
            return false;
        }

        // simulate swap
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
            {
                animationSystem.AnimateGemRemoval(matched, board, this);
            }
            else
            {
                Debug.LogWarning("[EnhancedBoardManager] No AnimationSystem assigned!");
            }

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
            // aggregator logic
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
                            {
                                matched.Add(board[r, cc]);
                            }
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
                    {
                        matched.Add(board[r, cc]);
                    }
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
                            {
                                matched.Add(board[rr, c]);
                            }
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
                    {
                        matched.Add(board[rr, c]);
                    }
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
                {
                    stack.Add(board[r,c]);
                }
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
                {
                    rt.anchoredPosition = newPos;
                }
            }
        }
    }

    public void RemoveGem(GemData data)
    {
        if (board[data.row, data.col] == data)
        {
            board[data.row, data.col] = null;
        }
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
}
