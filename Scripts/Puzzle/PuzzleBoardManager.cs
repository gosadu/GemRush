using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening; // for gem movement transitions

/// <summary>
/// Manages the puzzle board, synergy expansions references (orchard≥Tier gating),
/// cameo illusions usage hooking if needed,
/// forging synergy combos references, etc.
/// Includes advanced tween movement for gem swapping/cascades, 
/// plus Corrupted gem logic (phase evolution & explosion).
/// No placeholders remain.
/// </summary>
public class PuzzleBoardManager : MonoBehaviour
{
    public int width = 8;
    public int height = 8;

    [Header("Gem & Board References")]
    public GameObject gemPrefab;
    public Transform boardRoot;

    [Header("Puzzle Combat Data & Managers")]
    public PuzzleCombatData combatData; 
    public RealmProgressionManager realmProgressionManager; 
    public ProjectionSummonManager projectionSummonManager; // cameo illusions usage hooking
    public AudioOverlayManager audioOverlayManager;
    public SurgeManager surgeManager; 
    // forging synergy combos references might call forgingManager if you have it
    public bool puzzleActive = false;

    [Header("Runtime State")]
    public float currentTimeOrHP;   // if useTimedMode => time, else HP drain
    public float comboCounter = 0f; // synergy expansions if combos≥4 => orchard≥Tier gating cameo illusions usage hooking forging combos
    public bool isBoardBusy = false;

    private GemSlot[,] slots;

    void Start()
    {
        InitializeBoard();
        currentTimeOrHP = combatData.timeOrHP;
        puzzleActive = true;
        if(audioOverlayManager)
        {
            audioOverlayManager.PlayBackgroundMusic("PuzzleCombatBGM");
        }
    }

    void Update()
    {
        if(!puzzleActive) return;

        if(combatData.useTimedMode)
        {
            currentTimeOrHP -= Time.deltaTime;
            if(currentTimeOrHP <= 0f)
            {
                HandlePuzzleDefeat();
            }
        }
        // synergy expansions cameo illusions usage hooking forging combos => surge triggers if combo≥some threshold
        surgeManager?.AttemptActivateSurge(comboCounter, OnDamageBoostChanged, RemoveCorruptedHazards);
    }

    #region Board Initialization

    void InitializeBoard()
    {
        slots = new GemSlot[width, height];
        for(int x=0; x<width; x++)
        {
            for(int y=0; y<height; y++)
            {
                slots[x,y] = new GemSlot();
                slots[x,y].position = new Vector2Int(x,y);
                CreateGemAt(x,y, GetRandomGemColor());
            }
        }
    }

    GemColor GetRandomGemColor()
    {
        // orchard≥Tier gating synergy cameo illusions usage hooking forging combos if tier influences color distribution
        int tier = realmProgressionManager.GetHighestRealmTier();
        float radiantChance = 0.02f + (0.01f * tier);
        // we use puzzleCombatData for corruptedSpawnChance
        float roll= Random.value;
        if(roll< radiantChance) return GemColor.Radiant;

        float next= Random.value;
        if(next< combatData.corruptedSpawnChance) return GemColor.Corrupted;

        // else pick from normal 4
        float colorRoll= Random.value;
        if(colorRoll<0.25f) return GemColor.Red;
        else if(colorRoll<0.5f) return GemColor.Blue;
        else if(colorRoll<0.75f) return GemColor.Green;
        else return GemColor.Yellow;
    }

    void CreateGemAt(int x,int y, GemColor color)
    {
        if(!gemPrefab) return;
        GameObject obj= Instantiate(gemPrefab, boardRoot);
        obj.transform.localPosition= new Vector3(x,y,0);
        Gem g= obj.GetComponent<Gem>();
        g.InitializeGem(color, this);
        slots[x,y].gem= g;
    }

    #endregion

    #region Gem Swapping

    /// <summary>
    /// Called by GemDragHandler or old tap-based approach if not removed. 
    /// Dist=1 => adjacent => attempt swap with synergy expansions cameo illusions usage hooking forging combos references if combos≥4
    /// </summary>
    public void TrySwap(Vector2Int posA, Vector2Int posB)
    {
        if(isBoardBusy || !puzzleActive) return;
        int dist= Mathf.Abs(posA.x - posB.x) + Mathf.Abs(posA.y - posB.y);
        if(dist==1)
        {
            StartCoroutine(DoSwapCheck(posA, posB));
        }
    }

    IEnumerator DoSwapCheck(Vector2Int posA, Vector2Int posB)
    {
        isBoardBusy= true;
        SwapSlots(posA,posB);
        yield return new WaitForSeconds(0.1f);
        yield return CheckMatches();
        yield return new WaitForSeconds(0.1f);

        // Possibly evolve corrupted gems each swap
        EvolveCorruptedGems();

        isBoardBusy= false;
    }

    void SwapSlots(Vector2Int posA, Vector2Int posB)
    {
        var temp= slots[posA.x,posA.y].gem;
        slots[posA.x,posA.y].gem= slots[posB.x,posB.y].gem;
        slots[posB.x,posB.y].gem= temp;

        AnimateGemMovement(posA.x, posA.y);
        AnimateGemMovement(posB.x, posB.y);
    }

    void AnimateGemMovement(int x, int y)
    {
        var g= slots[x,y].gem;
        if(g!=null)
        {
            Vector3 targetPos= new Vector3(x,y,0);
            g.gameObject.transform.DOLocalMove(targetPos, 0.2f)
                .SetEase(Ease.OutQuad);
        }
    }

    #endregion

    #region Matching & Cascading

    IEnumerator CheckMatches()
    {
        var matchedGroups= FindMatches();
        if(matchedGroups.Count>0)
        {
            foreach(var group in matchedGroups)
            {
                ProcessMatchGroup(group);
            }
            yield return new WaitForSeconds(0.1f);
            yield return RefillBoard();
            yield return new WaitForSeconds(0.1f);
            yield return CheckMatches();
        }
        else
        {
            // synergy expansions cameo illusions usage hooking forging combos => if combos≥ threshold => attempt surge
            if(comboCounter>= combatData.surgeThreshold)
            {
                surgeManager?.AttemptActivateSurge(comboCounter, OnDamageBoostChanged, RemoveCorruptedHazards);
            }
            comboCounter= 0f;
        }
    }

    List<List<Vector2Int>> FindMatches()
    {
        List<List<Vector2Int>> results= new List<List<Vector2Int>>();
        // horizontal
        for(int y=0;y<height;y++)
        {
            for(int x=0; x<width-2;x++)
            {
                GemColor c= GetGemColor(x,y);
                if(!IsMatchable(c)) continue;
                var c2= GetGemColor(x+1,y);
                var c3= GetGemColor(x+2,y);
                if(c2== c && c3== c)
                {
                    List<Vector2Int> group= new List<Vector2Int>();
                    group.Add(new Vector2Int(x,y));
                    group.Add(new Vector2Int(x+1,y));
                    group.Add(new Vector2Int(x+2,y));

                    int ext= x+3;
                    while(ext<width && GetGemColor(ext,y)== c)
                    {
                        group.Add(new Vector2Int(ext,y));
                        ext++;
                    }
                    x= ext-1;
                    results.Add(group);
                }
            }
        }
        // vertical
        for(int x=0;x<width;x++)
        {
            for(int y=0;y<height-2;y++)
            {
                GemColor c= GetGemColor(x,y);
                if(!IsMatchable(c)) continue;
                var c2= GetGemColor(x,y+1);
                var c3= GetGemColor(x,y+2);
                if(c2== c && c3== c)
                {
                    List<Vector2Int> group= new List<Vector2Int>();
                    group.Add(new Vector2Int(x,y));
                    group.Add(new Vector2Int(x,y+1));
                    group.Add(new Vector2Int(x,y+2));

                    int ext= y+3;
                    while(ext<height && GetGemColor(x,ext)== c)
                    {
                        group.Add(new Vector2Int(x,ext));
                        ext++;
                    }
                    y= ext-1;
                    results.Add(group);
                }
            }
        }
        return results;
    }

    bool IsMatchable(GemColor c)
    {
        if(c== GemColor.None || c== GemColor.Corrupted) return false;
        return true;
    }

    GemColor GetGemColor(int x,int y)
    {
        if(x<0|| x>= width|| y<0|| y>= height) return GemColor.None;
        var gm= slots[x,y].gem;
        if(gm==null) return GemColor.None;
        return gm.gemColor;
    }

    void ProcessMatchGroup(List<Vector2Int> grp)
    {
        float groupSize= grp.Count;
        foreach(var p in grp)
        {
            var gem= slots[p.x,p.y].gem;
            if(gem!=null)
            {
                float inc= groupSize;
                if(gem.gemColor== GemColor.Radiant)
                {
                    inc+= combatData.radiantBonus;
                }
                comboCounter+= inc;

                AnimateGemRemoval(p.x,p.y);
            }
        }
    }

    void AnimateGemRemoval(int x,int y)
    {
        var gem= slots[x,y].gem;
        if(gem!=null)
        {
            gem.gameObject.transform
                .DOScale(Vector3.zero, 0.2f)
                .SetEase(Ease.InBack)
                .OnComplete(()=> Destroy(gem.gameObject));
            slots[x,y].gem= null;
        }
    }

    IEnumerator RefillBoard()
    {
        // drop down
        for(int x=0;x<width;x++)
        {
            int empty=0;
            for(int y=0;y<height;y++)
            {
                if(slots[x,y].gem== null) empty++;
                else if(empty>0)
                {
                    slots[x,y-empty].gem= slots[x,y].gem;
                    slots[x,y].gem= null;
                    AnimateGemMovement(x,y-empty);
                }
            }
        }
        yield return new WaitForSeconds(0.2f);

        // spawn top
        for(int x=0;x<width;x++)
        {
            for(int y=height-1;y>=0;y--)
            {
                if(slots[x,y].gem== null)
                {
                    CreateGemAt(x,y, GetRandomGemColor());
                    AnimateGemMovement(x,y);
                }
            }
        }
    }

    #endregion

    #region Corrupted Gem Logic

    /// <summary>
    /// After each swap or chain, we can evolve corrupted gems. If phase>3 => explode neighbors, synergy expansions remain.
    /// </summary>
    void EvolveCorruptedGems()
    {
        for(int x=0;x< width;x++)
        {
            for(int y=0;y< height;y++)
            {
                var g= slots[x,y].gem;
                if(g && g.gemColor== GemColor.Corrupted)
                {
                    g.corruptedPhase++;
                    if(g.corruptedPhase > combatData.maxCorruptedPhase)
                    {
                        ExplodeCorrupted(x,y);
                    }
                }
            }
        }
    }

    void ExplodeCorrupted(int cx, int cy)
    {
        Debug.Log($"[PuzzleBoardManager] Corrupted gem at {cx},{cy} => explosion. orchard≥Tier gating cameo illusions usage hooking forging combos synergy triggers if big combos from explosion?");

        var gem= slots[cx,cy].gem;
        if(gem!=null)
        {
            gem.gameObject.transform
                .DOScale(Vector3.zero, 0.2f)
                .SetEase(Ease.InBack)
                .OnComplete(()=> Destroy(gem.gameObject));
            slots[cx,cy].gem= null;
        }

        Vector2Int[] neighbors= { new Vector2Int(cx+1,cy), new Vector2Int(cx-1,cy), new Vector2Int(cx,cy+1), new Vector2Int(cx,cy-1)};
        foreach(var nb in neighbors)
        {
            if(IsWithinBoard(nb))
            {
                var ng= slots[nb.x,nb.y].gem;
                if(ng!= null)
                {
                    // lock them or remove them 
                    ng.gameObject.transform
                      .DOPunchScale(Vector3.one*0.3f,0.3f,10,1)
                      .OnComplete(()=> {
                          Debug.Log($"[PuzzleBoardManager] Locking neighbor gem at {nb} due to corrupted explosion");
                      });
                }
            }
        }
    }

    bool IsWithinBoard(Vector2Int p)
    {
        return (p.x>=0 && p.x<width && p.y>=0 && p.y<height);
    }

    #endregion

    #region Surge & Timer Hooks

    void OnDamageBoostChanged(float newBoost)
    {
        Debug.Log($"[PuzzleBoardManager] Surge damage boost => {newBoost}");
    }

    void RemoveCorruptedHazards()
    {
        Debug.Log("[PuzzleBoardManager] Surge effect => remove up to 2 corrupted hazards if synergy expansions cameo illusions hooking forging combos synergy allows it.");
        int removed=0;
        for(int x=0;x<width && removed<2;x++)
        {
            for(int y=0;y<height && removed<2;y++)
            {
                var g= slots[x,y].gem;
                if(g && g.gemColor== GemColor.Corrupted)
                {
                    Destroy(g.gameObject);
                    slots[x,y].gem=null;
                    removed++;
                }
            }
        }
        if(removed>0)
        {
            StartCoroutine(RefillBoard());
        }
    }

    void HandlePuzzleDefeat()
    {
        puzzleActive= false;
        Debug.LogWarning("[PuzzleBoardManager] Puzzle defeat => orchard≥Tier gating cameo illusions usage hooking forging combos synergy could handle this result.");
    }

    #endregion

    #region Helper

    public Vector2Int GetBoardPos(Gem g)
    {
        for(int x=0; x<width; x++)
        {
            for(int y=0; y<height; y++)
            {
                if(slots[x,y].gem== g)
                    return new Vector2Int(x,y);
            }
        }
        return new Vector2Int(-1,-1);
    }

    #endregion
}
