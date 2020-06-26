using System.Collections.Generic;
using UnityEngine;

public class SpawnService : IService, IUpdater
{
    List<GameObject> spawnedObject = new List<GameObject>();

    int maxSpawnedObject  = 10;

    GuysBehaviorService guysBehaviorService;

    public void StartService(EngineScript instance)
    {
        guysBehaviorService = instance.GetService<GuysBehaviorService>();
    }

    public void CreateBadGuy(Vector3 position)
    {
        var badGuy = GameObject.Instantiate(Resources.Load("Prefabs\\BadGuy") as GameObject, position, Quaternion.identity);

        spawnedObject.Add(badGuy);

        guysBehaviorService.AddBehavior(badGuy);
    }

    public void CreateGoodGuy(Vector3 position)
    {
        var goodGuy = UnityEngine.Object.Instantiate(Resources.Load("Prefabs\\GoodGuy") as GameObject, position, Quaternion.identity);

        spawnedObject.Add(goodGuy);

        guysBehaviorService.AddBehavior(goodGuy);
    }

    public void DestroyObject(GameObject gameObject)
    {
        spawnedObject.Remove(gameObject);

        guysBehaviorService.RemoveBehavior(gameObject);

        UnityEngine.Object.Destroy(gameObject);
    }

    UnityEngine.Random random = new UnityEngine.Random();
    public void Update()
    {
        if (spawnedObject.Count == maxSpawnedObject)
            return;

        UnityEngine.Random.InitState(System.DateTime.Now.Millisecond);

        Vector3 p0 = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
        Vector3 p1 = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.nearClipPlane));

        float height = 2f * Camera.main.orthographicSize;
        float width = height * Camera.main.aspect;

        var _xAxis = UnityEngine.Random.Range(p0.x, p1.x);
        var _yAxis = UnityEngine.Random.Range(p0.y, p1.y);

        if (UnityEngine.Random.Range(0, 10) > 5)
        {
            CreateGoodGuy(new Vector3(_xAxis,_yAxis,2f));
        }
        else
        {
            CreateBadGuy(new Vector3(_xAxis, _yAxis, 2f));
        }
    }
}
