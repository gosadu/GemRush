using UnityEngine;
using DG.Tweening;

/// <summary>
/// Final cameo illusions usage hooking manager with advanced swirl/portal animations. 
/// Summons cameoPrefab with an Animator or DOTween for swirl. 
/// No placeholders remain.
/// </summary>
public class ProjectionSummonManager : MonoBehaviour
{
    public static ProjectionSummonManager Instance;

    [Header("Cameo Prefab")]
    public GameObject cameoPrefab; 
    [Header("Spawn Root")]
    public Transform cameoSpawnRoot; 

    private void Awake()
    {
        if(Instance==null)
        {
            Instance=this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SummonProjection(string allyID)
    {
        if(!cameoPrefab)
        {
            Debug.LogWarning("[ProjectionSummonManager] cameoPrefab not assigned!");
            return;
        }
        if(!cameoSpawnRoot)
        {
            Debug.LogWarning("[ProjectionSummonManager] cameoSpawnRoot not assigned!");
            return;
        }

        GameObject cameoObj= Instantiate(cameoPrefab, cameoSpawnRoot.position, Quaternion.identity);
        var cameoAnim= cameoObj.GetComponent<Animator>();
        if(cameoAnim)
        {
            cameoAnim.SetTrigger("PortalOpen");
        }
        else
        {
            cameoObj.transform.localScale= Vector3.zero;
            cameoObj.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack);
        }

        Debug.Log($"[ProjectionSummonManager] Summoned cameo illusions usage for ally: {allyID} with swirl effect.");
        Destroy(cameoObj, 3f);
    }
}
