using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

/// <summary>
/// 4x4 forging puzzle awarding puzzlePerformance bonus (0-30).
/// synergy expansions or cameo illusions usage hooking references appear in
/// higher-level forging code, no placeholders remain here.
/// </summary>
public class MiniForgePuzzleManager : MonoBehaviour
{
    public int width=4;
    public int height=4;
    public GameObject gemPrefab;
    public Transform puzzleRoot;

    public int performanceScore=0; 
    public float matchDelay=0.2f;
    private bool puzzleActive=true;

    private ForgeGemSlot[,] slots;

    void Start()
    {
        InitPuzzle();
    }

    void InitPuzzle()
    {
        slots= new ForgeGemSlot[width,height];
        for(int x=0;x<width;x++)
        {
            for(int y=0;y<height;y++)
            {
                slots[x,y]= new ForgeGemSlot();
                slots[x,y].position= new Vector2Int(x,y);
                SpawnForgeGem(x,y);
            }
        }
    }

    void SpawnForgeGem(int x,int y)
    {
        GameObject obj= Instantiate(gemPrefab, puzzleRoot);
        obj.transform.localPosition= new Vector3(x,y,0f);
        ForgeGem gem= obj.GetComponent<ForgeGem>();
        gem.InitForgeGem(this);
        slots[x,y].gem= gem;
    }

    public void TrySwapForgeGems(Vector2Int posA, Vector2Int posB)
    {
        if(!puzzleActive) return;
        int dist= Mathf.Abs(posA.x-posB.x)+Mathf.Abs(posA.y-posB.y);
        if(dist==1)
        {
            StartCoroutine(DoSwapCheck(posA,posB));
        }
    }

    IEnumerator DoSwapCheck(Vector2Int posA, Vector2Int posB)
    {
        puzzleActive=false;
        // swap
        var temp= slots[posA.x,posA.y].gem;
        slots[posA.x,posA.y].gem= slots[posB.x,posB.y].gem;
        slots[posB.x,posB.y].gem= temp;
        UpdateGemPos(posA);
        UpdateGemPos(posB);

        yield return new WaitForSeconds(matchDelay);
        yield return CheckForgeMatches();
        puzzleActive=true;
    }

    void UpdateGemPos(Vector2Int pos)
    {
        if(slots[pos.x,pos.y].gem)
        {
            slots[pos.x,pos.y].gem.transform.DOLocalMove(new Vector3(pos.x,pos.y,0), 0.2f)
                                       .SetEase(Ease.OutQuad);
        }
    }

    IEnumerator CheckForgeMatches()
    {
        var groups= FindForgeMatches();
        if(groups.Count>0)
        {
            foreach(var grp in groups)
            {
                float groupSize= grp.Count;
                performanceScore+= Mathf.RoundToInt(groupSize);
                foreach(var cell in grp)
                {
                    if(slots[cell.x,cell.y].gem)
                    {
                        slots[cell.x,cell.y].gem.transform
                            .DOScale(Vector3.zero, 0.15f).SetEase(Ease.InBack)
                            .OnComplete(()=> 
                            {
                                Destroy(slots[cell.x,cell.y].gem.gameObject);
                                slots[cell.x,cell.y].gem= null;
                            });
                    }
                }
            }
            yield return new WaitForSeconds(matchDelay);
            yield return RefillForgeBoard();
            yield return new WaitForSeconds(matchDelay);
            yield return CheckForgeMatches();
        }
    }

    List<List<Vector2Int>> FindForgeMatches()
    {
        List<List<Vector2Int>> results= new List<List<Vector2Int>>();
        // Horizontal
        for(int y=0;y<height;y++)
        {
            for(int x=0;x<width-2;x++)
            {
                ForgeGemColor c= GetForgeGemColor(x,y);
                if(c!=ForgeGemColor.None && c== GetForgeGemColor(x+1,y) && c== GetForgeGemColor(x+2,y))
                {
                    var match= new List<Vector2Int>();
                    match.Add(new Vector2Int(x,y));
                    match.Add(new Vector2Int(x+1,y));
                    match.Add(new Vector2Int(x+2,y));
                    int ext= x+3;
                    while(ext<width && GetForgeGemColor(ext,y)== c)
                    {
                        match.Add(new Vector2Int(ext,y));
                        ext++;
                    }
                    x= ext-1;
                    results.Add(match);
                }
            }
        }
        // Vertical
        for(int x=0;x<width;x++)
        {
            for(int y=0;y<height-2;y++)
            {
                ForgeGemColor c= GetForgeGemColor(x,y);
                if(c!=ForgeGemColor.None && c== GetForgeGemColor(x,y+1) && c== GetForgeGemColor(x,y+2))
                {
                    var match= new List<Vector2Int>();
                    match.Add(new Vector2Int(x,y));
                    match.Add(new Vector2Int(x,y+1));
                    match.Add(new Vector2Int(x,y+2));
                    int ext= y+3;
                    while(ext<height && GetForgeGemColor(x,ext)== c)
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

    ForgeGemColor GetForgeGemColor(int x,int y)
    {
        if(x<0||x>=width||y<0||y>=height) return ForgeGemColor.None;
        if(slots[x,y].gem==null) return ForgeGemColor.None;
        return slots[x,y].gem.forgeColor;
    }

    IEnumerator RefillForgeBoard()
    {
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
                    UpdateGemPos(new Vector2Int(x,y-empty));
                }
            }
        }
        yield return new WaitForSeconds(matchDelay);

        // spawn new
        for(int x=0;x<width;x++)
        {
            for(int y=height-1;y>=0;y--)
            {
                if(slots[x,y].gem==null)
                {
                    SpawnForgeGem(x,y);
                    UpdateGemPos(new Vector2Int(x,y));
                }
            }
        }
    }

    public int GetFinalPerformanceScore()
    {
        return Mathf.Clamp(performanceScore,0,30);
    }
}

public class ForgeGemSlot
{
    public Vector2Int position;
    public ForgeGem gem;
}
