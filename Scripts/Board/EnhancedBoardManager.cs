// FILE: EnhancedBoardManager.cs
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// EnhancedBoardManager: A comprehensive Match-3 board system with:
///  - Arc tween swaps (ArcSwapEffect)
///  - Horizontal/vertical match detection
///  - Bomb & rainbow (wild) gem logic
///  - No illegal move blocking (any gem can swap with any other)
///  - Aggregator synergy (aggregatorPoints, AddAggregatorPoints)
///  - Player HP & HealPlayer
///  - RemoveGem & SwapGems methods for backward compatibility
///  - Optional chain reaction & shuffle
///  - Optional vanish/shatter (AnimationSystem)
/// 
/// This merges all changes, ensuring references like HealPlayer, SwapGems, 
/// RemoveGem, AddAggregatorPoints are included so external scripts won't break.
/// </summary>
public class EnhancedBoardManager : MonoBehaviour
{
    [Header("Board Dimensions")]
    public int rows = 8;
    public int cols = 8;
    public float cellSize = 100f;

    [Header("Gem Settings")]
    public GameObject gemPrefab;     // Prefab for each gem (must have GemView)
    public Sprite[] gemSprites;      // Array of gem sprites (0..5 normal, 6..7 bomb, 8..9 rainbow, etc.)

    [Header("Aggregator Synergy")]
    public bool useAggregator = true;   // if true, aggregator synergy accumulates and can damage boss
    private int aggregatorPoints;       // aggregator synergy total

    [Header("Player HP")]
    public int playerMaxHP = 100;
    private int playerHP;

    [Header("References")]
    public AnimationSystem animationSystem;  // optional vanish/shatter
    public ArcSwapEffect arcSwapEffect;      // optional arc-based swap tween
    public SoundManager soundManager;        // if you want swap / match sounds
    public BossManager bossManager;          // if aggregator synergy deals boss damage
    public UIManager uiManager;              // if you track moves or UI updates

    [Header("Shuffle Handling")]
    public bool shuffleIfNoMoves = true;     // auto-shuffle if no matches remain

    [Header("Debug Options")]
    public bool logDebug = false;            // if set, logs debug details

    private GemData[,] board;                // 2D array of gem logic
    private bool isBoardBusy;                // lock while swapping/cascading
    private bool hasBoardInited;             // for one-time init
    private bool isSwapping;                 // extra lock if needed

    // ---------------------------------------------------------
    // GemData: The logical info for each gem
    // ---------------------------------------------------------
    [System.Serializable]
    public class GemData
    {
        public int row;
        public int col;
        public int spriteIndex;  // index into gemSprites
        public bool isBomb;      // if true, remove neighbors
        public bool isRainbow;   // if true, can match any color
    }

    // ---------------------------------------------------------
    // GemView: The visual component (UI image) for each gem
    // ---------------------------------------------------------
    [RequireComponent(typeof(RectTransform))]
    public class GemView : MonoBehaviour
    {
        public GemData data;
        public Image gemImage;

        [HideInInspector] public EnhancedBoardManager boardRef;

        // Initialize gem with data and sprite
        public void InitGem(GemData gemData, Sprite sprite, EnhancedBoardManager board)
        {
            data = gemData;
            boardRef = board;
            if (gemImage && sprite)
            {
                gemImage.sprite = sprite;
            }
        }

        // Called externally to swap these two gems
        public void SwapWith(GemView other)
        {
            if (boardRef == null) return;
            boardRef.DoSwapGems(this, other);
        }
    }

    // ---------------------------------------------------------
    // ArcSwapEffect: optional smooth arc tween for swapping
    // ---------------------------------------------------------
    [System.Serializable]
    public class ArcSwapEffect
    {
        [Range(0f,1f)] public float arcHeightFactor = 0.4f;
        public float swapDuration = 0.3f;

        // actually do the tween
        public IEnumerator DoArcSwap(RectTransform rt1, RectTransform rt2)
        {
            Vector2 startPos1 = rt1.anchoredPosition;
            Vector2 startPos2 = rt2.anchoredPosition;
            float distance = Vector2.Distance(startPos1, startPos2);
            float arcHeight = distance * arcHeightFactor;

            float time = 0f;
            while (time < swapDuration)
            {
                time += Time.deltaTime;
                float t = Mathf.Clamp01(time / swapDuration);
                float eased = EaseInOutCubic(t);

                // Lerp base
                Vector2 newPos1 = Vector2.Lerp(startPos1, startPos2, eased);
                Vector2 newPos2 = Vector2.Lerp(startPos2, startPos1, eased);

                // Arc offsets
                float offset1 = Mathf.Sin(Mathf.PI * eased) * arcHeight;
                float offset2 = Mathf.Sin(Mathf.PI * eased) * arcHeight;

                newPos1.y += offset1;
                newPos2.y += offset2;

                rt1.anchoredPosition = newPos1;
                rt2.anchoredPosition = newPos2;

                yield return null;
            }
        }

        private float EaseInOutCubic(float x)
        {
            if (x < 0.5f)
                return 4f * x * x * x;
            else
                return 1f - Mathf.Pow(-2f*x+2f,3f)/2f;
        }
    }

    // ---------------------------------------------------------
    // AnimationSystem: optional vanish/shatter
    // ---------------------------------------------------------
    [System.Serializable]
    public class AnimationSystem
    {
        public float vanishDuration = 0.3f;
        public GameObject shatterPrefab;
        public GameObject shockwavePrefab;

        public IEnumerator AnimateRemoval(List<GemView> gemViews)
        {
            float time = 0f;
            while (time < vanishDuration)
            {
                time += Time.deltaTime;
                float alpha = 1f - (time/vanishDuration);
                foreach (var gv in gemViews)
                {
                    if (gv && gv.gemImage)
                    {
                        Color c = gv.gemImage.color;
                        c.a = alpha;
                        gv.gemImage.color = c;
                    }
                }
                yield return null;
            }

            // spawn effects
            foreach (var gv in gemViews)
            {
                if (gv)
                {
                    if (shatterPrefab)
                        GameObject.Instantiate(shatterPrefab, gv.transform.position, Quaternion.identity);
                    if (shockwavePrefab)
                        GameObject.Instantiate(shockwavePrefab, gv.transform.position, Quaternion.identity);
                }
            }
        }
    }

    // ---------------------------------------------------------
    // Unity Lifecycle
    // ---------------------------------------------------------
    void Start()
    {
        if (!hasBoardInited)
        {
            InitBoard();
            hasBoardInited = true;
        }
    }

    /// <summary>
    /// Clear old objects, spawn new board
    /// </summary>
    public void InitBoard()
    {
        // Clear old
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        board = new GemData[rows, cols];
        aggregatorPoints = 0;
        playerHP = playerMaxHP;
        isBoardBusy = false;
        isSwapping = false;

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                SpawnNewGem(r, c);
            }
        }
    }

    private void SpawnNewGem(int r, int c)
    {
        int idx = Random.Range(0, gemSprites.Length);
        bool bomb = (idx == 6 || idx == 7);    // example bomb
        bool rainbow = (idx == 8 || idx == 9); // example rainbow

        GemData data = new GemData
        {
            row = r,
            col = c,
            spriteIndex = idx,
            isBomb = bomb,
            isRainbow = rainbow
        };
        board[r, c] = data;

        // create gem object
        GameObject gemObj = Instantiate(gemPrefab, transform);
        RectTransform rt = gemObj.GetComponent<RectTransform>();
        rt.anchoredPosition = CalculatePos(r, c);

        GemView gv = gemObj.GetComponent<GemView>();
        gv.InitGem(data, gemSprites[idx], this);
        gemObj.name = $"Gem({r},{c})";
    }

    private Vector2 CalculatePos(int r, int c)
    {
        float startX = -(cols * cellSize)/2f + cellSize/2f;
        float startY = (rows * cellSize)/2f - cellSize/2f;
        float x = startX + (c * cellSize);
        float y = startY - (r * cellSize);
        return new Vector2(x,y);
    }

    // ---------------------------------------------------------
    // The *NEW* swap logic (no adjacency restriction)
    // ---------------------------------------------------------
    public void DoSwapGems(GemView gv1, GemView gv2)
    {
        if (isBoardBusy || isSwapping) return;
        StartCoroutine(DoSwapCoroutine(gv1, gv2));
    }

    private IEnumerator DoSwapCoroutine(GemView gv1, GemView gv2)
    {
        isBoardBusy = true;
        isSwapping = true;

        // Arc tween
        if (arcSwapEffect != null)
        {
            RectTransform r1 = gv1.GetComponent<RectTransform>();
            RectTransform r2 = gv2.GetComponent<RectTransform>();
            yield return StartCoroutine(arcSwapEffect.DoArcSwap(r1, r2));
        }
        else
        {
            // direct swap
            Vector2 p1 = gv1.GetComponent<RectTransform>().anchoredPosition;
            Vector2 p2 = gv2.GetComponent<RectTransform>().anchoredPosition;
            gv1.GetComponent<RectTransform>().anchoredPosition = p2;
            gv2.GetComponent<RectTransform>().anchoredPosition = p1;
        }

        // Swap in board
        GemData d1 = gv1.data;
        GemData d2 = gv2.data;
        board[d1.row, d1.col] = d2;
        board[d2.row, d2.col] = d1;
        int oldR = d1.row; int oldC = d1.col;
        d1.row = d2.row; d1.col = d2.col;
        d2.row = oldR;   d2.col = oldC;

        // optional sound
        if (soundManager) soundManager.PlaySwapSound();

        // match/cascade
        yield return StartCoroutine(CheckAndResolveMatches());

        isSwapping = false;
        isBoardBusy = false;
    }

    /// <summary>
    /// Finds matches, removes them, cascades, spawns new, repeat. 
    /// Also handle aggregator synergy & optional boss damage after chain reaction.
    /// </summary>
    private IEnumerator CheckAndResolveMatches()
    {
        bool anyMatchHappened = false;

        while (true)
        {
            var matches = FindAllMatches();
            if (matches.Count == 0)
            {
                if (logDebug) Debug.Log("[Board] No matches => done chain reaction.");
                break;
            }

            // aggregator synergy
            if (useAggregator)
            {
                // e.g. add points for each matched gem
                aggregatorPoints += matches.Count * 5;
                Debug.Log($"[Board] aggregatorPoints => {aggregatorPoints}");
            }

            // remove matches
            yield return StartCoroutine(RemoveMatches(matches));

            // cascade
            yield return StartCoroutine(CascadeGemsDown());

            anyMatchHappened = true;
        }

        // aggregator synergy => boss damage if any
        if (useAggregator && aggregatorPoints > 0 && anyMatchHappened)
        {
            if (bossManager)
            {
                bossManager.TakeDamage(aggregatorPoints);
                Debug.Log($"[Board] Dealt {aggregatorPoints} aggregator damage to boss!");
            }
            aggregatorPoints = 0;
        }

        // optional shuffle
        if (shuffleIfNoMoves && !HasAnyMatch())
        {
            ShuffleBoard();
        }
    }

    // ---------------------------------------------------------
    // MATCH DETECTION
    // ---------------------------------------------------------
    private List<GemData> FindAllMatches()
    {
        HashSet<GemData> matched = new HashSet<GemData>();

        // horizontal
        for (int r = 0; r < rows; r++)
        {
            int matchCount = 1;
            for (int c = 1; c < cols; c++)
            {
                if (IsSameType(board[r,c], board[r,c-1]))
                {
                    matchCount++;
                }
                else
                {
                    if (matchCount >= 3)
                    {
                        int startC = c - matchCount;
                        for (int cc = startC; cc < c; cc++)
                            matched.Add(board[r, cc]);
                    }
                    matchCount = 1;
                }
            }
            if (matchCount >= 3)
            {
                int startC = cols - matchCount;
                for (int cc = startC; cc < cols; cc++)
                    matched.Add(board[r, cc]);
            }
        }

        // vertical
        for (int c = 0; c < cols; c++)
        {
            int matchCount = 1;
            for (int r = 1; r < rows; r++)
            {
                if (IsSameType(board[r,c], board[r-1,c]))
                {
                    matchCount++;
                }
                else
                {
                    if (matchCount >= 3)
                    {
                        int startR = r - matchCount;
                        for (int rr = startR; rr < r; rr++)
                            matched.Add(board[rr, c]);
                    }
                    matchCount = 1;
                }
            }
            if (matchCount >= 3)
            {
                int startR = rows - matchCount;
                for (int rr = startR; rr < rows; rr++)
                    matched.Add(board[rr, c]);
            }
        }

        // bombs => remove neighbors
        List<GemData> bombArea = new List<GemData>();
        foreach (var g in matched)
        {
            if (g.isBomb)
            {
                for (int dr = -1; dr <= 1; dr++)
                {
                    for (int dc = -1; dc <= 1; dc++)
                    {
                        int rr = g.row + dr;
                        int cc = g.col + dc;
                        if (InBounds(rr, cc))
                            bombArea.Add(board[rr, cc]);
                    }
                }
            }
        }
        foreach (var b in bombArea)
        {
            matched.Add(b);
        }

        return new List<GemData>(matched);
    }

    private bool IsSameType(GemData a, GemData b)
    {
        if (a == null || b == null) return false;
        if (a.isRainbow || b.isRainbow) return true; // wildcard
        return (a.spriteIndex == b.spriteIndex);
    }

    private bool InBounds(int r, int c)
    {
        return (r >= 0 && r < rows && c >= 0 && c < cols);
    }

    // ---------------------------------------------------------
    // REMOVAL & CASCADING
    // ---------------------------------------------------------
    private IEnumerator RemoveMatches(List<GemData> matched)
    {
        List<GemView> gemViews = new List<GemView>();
        GemView[] allGems = Object.FindObjectsByType<GemView>(
            FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        foreach (var v in allGems)
        {
            if (matched.Contains(v.data))
            {
                gemViews.Add(v);
            }
        }

        if (animationSystem != null)
        {
            yield return StartCoroutine(animationSystem.AnimateRemoval(gemViews));
        }

        // destroy
        foreach (var gv in gemViews)
        {
            if (gv) Destroy(gv.gameObject);
        }

        // remove from board array
        foreach (var m in matched)
        {
            board[m.row, m.col] = null;
        }
        yield return null;
    }

    private IEnumerator CascadeGemsDown()
    {
        for (int c = 0; c < cols; c++)
        {
            List<GemData> stack = new List<GemData>();
            for (int r = rows - 1; r >= 0; r--)
            {
                if (board[r, c] != null)
                {
                    stack.Add(board[r, c]);
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
                    board[r, c] = gem;
                }
                else
                {
                    SpawnNewGem(r, c);
                }
            }
        }
        // visually reposition
        RedrawBoard();
        yield return new WaitForSeconds(0.2f);
    }

    private bool HasAnyMatch()
    {
        var m = FindAllMatches();
        return (m.Count > 0);
    }

    private void ShuffleBoard()
    {
        if (logDebug) Debug.Log("[Board] Shuffling due to no matches.");

        // gather existing
        List<GemData> all = new List<GemData>();
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                if (board[r, c] != null)
                    all.Add(board[r, c]);
            }
        }

        // shuffle
        for (int i = 0; i < all.Count; i++)
        {
            int rnd = Random.Range(i, all.Count);
            var temp = all[i];
            all[i] = all[rnd];
            all[rnd] = temp;
        }

        // reassign
        int index = 0;
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                board[r, c] = (index < all.Count) ? all[index] : null;
                if (board[r, c] != null)
                {
                    board[r, c].row = r;
                    board[r, c].col = c;
                }
                index++;
            }
        }

        // destroy old gemViews
        GemView[] allViews = Object.FindObjectsByType<GemView>(
            FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        foreach (var v in allViews)
        {
            Destroy(v.gameObject);
        }

        // re-spawn
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                if (board[r, c] != null)
                {
                    SpawnVisualOnly(board[r, c]);
                }
                else
                {
                    SpawnNewGem(r, c);
                }
            }
        }
    }

    private void SpawnVisualOnly(GemData d)
    {
        GameObject gemObj = Instantiate(gemPrefab, transform);
        RectTransform rt = gemObj.GetComponent<RectTransform>();
        rt.anchoredPosition = CalculatePos(d.row, d.col);

        GemView gv = gemObj.GetComponent<GemView>();
        gv.InitGem(d, gemSprites[d.spriteIndex], this);
        gemObj.name = $"Gem({d.row},{d.col})";
    }

    /// <summary>
    /// Repositions all gemViews based on row/col
    /// </summary>
    public void RedrawBoard()
    {
        GemView[] allViews = Object.FindObjectsByType<GemView>(
            FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        foreach (var v in allViews)
        {
            if (v && v.data != null)
            {
                RectTransform rt = v.GetComponent<RectTransform>();
                if (rt)
                    rt.anchoredPosition = CalculatePos(v.data.row, v.data.col);
            }
        }
    }

    // ---------------------------------------------------------
    // Additional Methods to Satisfy External References:
    // HealPlayer, AddAggregatorPoints, RemoveGem, SwapGems
    // ---------------------------------------------------------

    /// <summary>
    /// Heals the player by amt (clamped by playerMaxHP).
    /// </summary>
    public void HealPlayer(int amt)
    {
        playerHP += amt;
        if (playerHP > playerMaxHP) playerHP = playerMaxHP;
        Debug.Log($"[EnhancedBoardManager] Player HP now {playerHP}");
    }

    /// <summary>
    /// Manually add aggregator synergy points.
    /// </summary>
    public void AddAggregatorPoints(int amt)
    {
        if (!useAggregator) return;
        aggregatorPoints += amt;
        Debug.Log($"[EnhancedBoardManager] aggregatorPoints = {aggregatorPoints}");
    }

    /// <summary>
    /// Remove a gem from the board data so it's no longer recognized.
    /// (Does not handle destroying the GemView object.)
    /// </summary>
    public void RemoveGem(GemData data)
    {
        if (data == null) return;
        if (board[data.row, data.col] == data)
        {
            board[data.row, data.col] = null;
        }
    }

    /// <summary>
    /// A direct "SwapGems" method to match older references.
    /// Just calls our new "DoSwapGems" but requires two GemData.
    /// </summary>
    public void SwapGems(GemData g1, GemData g2)
    {
        if (g1 == null || g2 == null) return;
        // find their GemViews
        GemView[] all = Object.FindObjectsByType<GemView>(
            FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        GemView gv1 = null;
        GemView gv2 = null;
        foreach (var v in all)
        {
            if (v.data == g1) gv1 = v;
            else if (v.data == g2) gv2 = v;
        }
        if (gv1 && gv2)
        {
            DoSwapGems(gv1, gv2);
        }
    }
}
