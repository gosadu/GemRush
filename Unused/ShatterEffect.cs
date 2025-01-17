using UnityEngine;

public class ShatterEffect : MonoBehaviour
{
    [SerializeField] private GameObject shatterPiecePrefab;
    public int count = 6;
    public Color shardColor = Color.white;

    void Start()
    {
        for (int i = 0; i < count; i++)
        {
            var piece = Instantiate(shatterPiecePrefab, transform.position, Quaternion.identity, transform);
            var sp = piece.GetComponent<ShatterPiece>();
            if (sp != null) sp.SetColor(shardColor);
        }
    }
}
