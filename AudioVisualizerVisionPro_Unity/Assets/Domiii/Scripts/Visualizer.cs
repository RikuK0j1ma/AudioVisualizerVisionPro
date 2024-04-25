using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.Networking;

public class Visualizer : MonoBehaviour
{
    public SpectrumController prefab;
    public float maxHeight = 1024;
    public int radius = 32;
    [SerializeField] private Vector3 visualizerSize;

    List<Vector2> generationList = new List<Vector2>();

     async void Start()
    {
        var radiusSq = radius * radius;
        for (int y = -radius; y < radius; y++)
        {
            for (int x = -radius; x < radius; x++)
            {
                var p = new Vector2(x, y);
                if (p.sqrMagnitude < radiusSq)
                {
                    generationList.Add(p);
                }
            }
        }

        GenerateAsync().Forget();
        //await UniTask.Delay(1000,cancellationToken:this.GetCancellationTokenOnDestroy());
         //transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
    }

    async UniTask GenerateAsync()
    {
        while (generationList.Count != 0)
        {
            var currentVector = generationList[Random.Range(0, generationList.Count)];
            GenerateTile(currentVector);
            generationList.Remove(currentVector);
            Debug.Log($"カウント：{generationList.Count}");
        }
        
        await UniTask.WaitUntil(() =>generationList.Count == 0,cancellationToken:this.GetCancellationTokenOnDestroy());
        transform.localScale = visualizerSize;
    }

    void GenerateTile(Vector2 vc2)
    {
        Vector3 pos = new Vector3(vc2.x, 0, vc2.y);
        var dist = vc2.magnitude;

        var spawnedTile = Instantiate(prefab, pos, prefab.transform.rotation);
        spawnedTile.maxHeight = maxHeight;
        spawnedTile.spectrumIndex = (int)(Mathf.Round(dist));
        spawnedTile.transform.SetParent(transform);
    }

    void OnDrawGizmos()
    {
        var c = Color.yellow;
        c.a = 0.2f;
        Gizmos.color = c;

        Gizmos.DrawWireSphere(transform.position, radius);
    }
}