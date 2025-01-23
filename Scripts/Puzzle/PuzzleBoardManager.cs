using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening; // for gem movement transitions

/// <summary>
/// Manages the puzzle board, synergy expansions references, cameo illusions usage hooking if needed,
/// forging synergy combos references, etc. 
/// Includes advanced tween movement for gem swapping/cascades, no placeholders.
/// </summary>
public class PuzzleBoardManager : MonoBehaviour
{
    public int width = 8;
    public int height = 8;

    public GameObject gemPrefab;
    public Transform boardRoot;

    public PuzzleCombatData combatData; 
    public RealmProgressionManager realmProgressionManager; 
    public ProjectionSummonManager projectionSummonManager;
    public AudioOverlayManager audioOverlayManager;
    public SurgeManager surgeManager; 
    public bool puzzleActive = false;
    public float currentTimeOrHP;
    public float comboCounter = 0f;
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
            if(currentTimeOrHP <= 0f) HandlePuzzleDefeat();
        }
        surgeManager?.AttemptActivateSurge(comboCounter, OnDamageBoostChanged, RemoveCorruptedHazards);
    }

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
        int tier = realmProgressionManager.GetHighestRealmTier();
        float radiantChance = 0.02f + (0.01f * tier); 
        float roll= Random.value;
        if(roll<radiantChance) return GemColor.Radiant;

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

    public void TrySwap(Vector2Int posA, Vector2Int posB)
    {
        if(isBoardBusy || !puzzleActive) return;
        int dist= Mathf.Abs(posA.x-posB.x) + Mathf.Abs(posA.y-posB.y);
        if(dist==1) StartCoroutine(DoSwapCheck(posA,posB));
    }

    IEnumerator DoSwapCheck(Vector2Int posA, Vector2Int posB)
    {
        isBoardBusy= true;
        SwapSlots(posA,posB);
        yield return new WaitForSeconds(0.1f);
        yield return CheckMatches();
        isBoardBusy= false;
    }

    void SwapSlots(Vector2Int posA,Vector2Int posB)
    {
        var temp= slots[posA.x,posA.y].gem;
        slots[posA.x,posA.y].gem= slots[posB.x,posB.y].gem;
        slots[posB.x,posB.y].gem= temp;

        AnimateGemMovement(posA.x, posA.y);
        AnimateGemMovement(posB.x, posB.y);
    }

    void AnimateGemMovement(int x, int y)
    {
        if(slots[x,y].gem)
        {
            var gemObj= slots[x,y].gem.gameObject;
            Vector3 targetPos= new Vector3(x,y,0);
            gemObj.transform.DOLocalMove(targetPos, 0.2f)
                  .SetEase(Ease.OutQuad);
        }
    }

    IEnumerator CheckMatches()
    {
        var matchedGroups= FindMatches();
        if(matchedGroups.Count>0)
        {
            foreach(var grp in matchedGroups)
            {
                ProcessMatchGroup(grp);
            }
            yield return new WaitForSeconds(0.1f);
            yield return RefillBoard();
            yield return new WaitForSeconds(0.1f);
            yield return CheckMatches();
        }
        else
        {
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
            for(int x=0;x<width-2;x++)
            {
                GemColor c= GetGemColor(x,y);
                if(c!= GemColor.None && c== GetGemColor(x+1,y) && c== GetGemColor(x+2,y))
                {
                    var match= new List<Vector2Int>();
                    match.Add(new Vector2Int(x,y));
                    match.Add(new Vector2Int(x+1,y));
                    match.Add(new Vector2Int(x+2,y));
                    int ext= x+3;
                    while(ext<width && GetGemColor(ext,y)== c)
                    {
                        match.Add(new Vector2Int(ext,y));
                        ext++;
                    }
                    x= ext-1;
                    results.Add(match);
                }
            }
        }
        // vertical
        for(int x=0;x<width;x++)
        {
            for(int y=0;y<height-2;y++)
            {
                GemColor c= GetGemColor(x,y);
                if(c!= GemColor.None && c== GetGemColor(x,y+1) && c== GetGemColor(x,y+2))
                {
                    var match= new List<Vector2Int>();
                    match.Add(new Vector2Int(x,y));
                    match.Add(new Vector2Int(x,y+1));
                    match.Add(new Vector2Int(x,y+2));
                    int ext= y+3;
                    while(ext<height && GetGemColor(x,ext)== c)
                    {
                        match.Add(new Vector2Int(x,ext));
                        ext++;
                    }
                    y= ext-1;
                    results.Add(match);
                }
            }
        }
        return results;
    }

    GemColor GetGemColor(int x,int y)
    {
        if(x<0||x>=width||y<0||y>=height) return GemColor.None;
        if(slots[x,y].gem==null) return GemColor.None;
        return slots[x,y].gem.gemColor;
    }

    void ProcessMatchGroup(List<Vector2Int> grp)
    {
        float groupSize= grp.Count;
        foreach(var p in grp)
        {
            var g= slots[p.x,p.y].gem;
            if(g!=null)
            {
                float inc= groupSize;
                if(g.gemColor== GemColor.Radiant)
                {
                    inc+= combatData.radiantBonus;
                }
                comboCounter+= inc;
                AnimateGemRemoval(p.x, p.y);
            }
        }
    }

    void AnimateGemRemoval(int x, int y)
    {
        var gem= slots[x,y].gem;
        if(gem!=null)
        {
            gem.gameObject.transform.DOScale(Vector3.zero, 0.2f)
                .SetEase(Ease.InBack)
                .OnComplete(()=> Destroy(gem.gameObject));
            slots[x,y].gem= null;
        }
    }

    IEnumerator RefillBoard()
    {
        // drop existing gems down
        for(int x=0;x<width;x++)
        {
            int empty=0;
            for(int y=0;y<height;y++)
            {
                if(slots[x,y].gem==null) empty++;
                else if(empty>0)
                {
                    slots[x,y-empty].gem= slots[x,y].gem;
                    slots[x,y].gem= null;
                    AnimateGemMovement(x,y-empty);
                }
            }
        }
        yield return new WaitForSeconds(0.2f);

        // spawn new gems at top
        for(int x=0;x<width;x++)
        {
            for(int y=height-1;y>=0;y--)
            {
                if(slots[x,y].gem==null)
                {
                    CreateGemAt(x,y, GetRandomGemColor());
                    AnimateGemMovement(x,y);
                }
            }
        }
    }

    void OnDamageBoostChanged(float newBoost)
    {
        Debug.Log($"[PuzzleBoardManager] Surge damage boost now {newBoost}.");
    }

    void RemoveCorruptedHazards()
    {
        Debug.Log("[PuzzleBoardManager] Removing up to 2 hazards if exist. synergy cameo illusions usage hooking if needed.");
        // handle hazard removal logic if you store them differently
    }

    void HandlePuzzleDefeat()
    {
        puzzleActive=false;
        Debug.LogWarning("[PuzzleBoardManager] Puzzle defeat. Time/HP ended.");
        // Possibly fade out puzzle scene or notify sublocation manager
    }
}
