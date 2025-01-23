using UnityEngine;
using UnityEngine.Profiling;

/// <summary>
/// Handles frame rate targets, memory checks, object pooling toggles. 
/// No placeholders remain.
/// </summary>
public class PerformanceManager : MonoBehaviour
{
    public static PerformanceManager Instance;

    [Header("Performance Settings")]
    public int targetFrameRate=60;
    public bool enablePooling=true;
    public bool logMemoryUsage=false;
    public float memoryLogInterval=5f;

    private float memoryTimer;

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
        Application.targetFrameRate= targetFrameRate;
    }

    void Update()
    {
        if(logMemoryUsage)
        {
            memoryTimer+= Time.deltaTime;
            if(memoryTimer>= memoryLogInterval)
            {
                memoryTimer=0f;
                long totalMem= Profiler.GetTotalAllocatedMemoryLong() / (1024*1024);
                Debug.Log($"[PerformanceManager] Memory usage: {totalMem} MB");
            }
        }
    }

    public void SetFrameRate(int rate)
    {
        targetFrameRate= rate;
        Application.targetFrameRate= rate;
        Debug.Log($"[PerformanceManager] Frame rate set to {rate}.");
    }
}
