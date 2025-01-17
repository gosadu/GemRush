using UnityEngine;

public class ConfettiEffect : MonoBehaviour
{
    [SerializeField] private GameObject singleConfettiPrefab;
    public int confettiCount = 10;

    void Start()
    {
        for (int i = 0; i < confettiCount; i++)
        {
            var conf = Instantiate(singleConfettiPrefab, transform.position, Quaternion.identity, transform);
        }
    }
}
