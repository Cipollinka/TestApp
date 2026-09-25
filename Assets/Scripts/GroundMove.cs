using UnityEngine;
using System.Collections.Generic;


public class GroundMove : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    public int chunkCount = 5;
    public float removeZ = -75f;

    private List<GameObject> chunks = new ();
    private int chunkLeader;

    private void Start()
    {
        for (int i = 0; i < chunkCount; i++)
        {
            GameObject chunk = Instantiate(prefab, new Vector3(0, 0, i * removeZ * -1), Quaternion.identity, parent: transform);
            chunks.Add(chunk);
        }
        chunkLeader = chunkCount - 1;
    }

    private void Update()
    {
        if (!GameManager.Instance.isFirstTap || GameManager.Instance.isFinishGame)
        {
            return;
        }
        for (int i = 0; i < chunks.Count; i++)
        {
            if (chunks[i].transform.position.z < removeZ)
            {
                chunks[i].transform.position = new Vector3(0, 0, chunks[chunkLeader].transform.position.z + removeZ * -1);
                chunkLeader = i;
            }
            chunks[i].transform.Translate(GameManager.Instance.velocity * Time.deltaTime * Vector3.back);
        }
    }
}
