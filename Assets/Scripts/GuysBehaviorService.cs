using System;
using System.Collections.Generic;
using UnityEngine;

public class GuysBehaviorService : IService, IUpdater
{
    List<GameObject> behavioredGuys                      = new List<GameObject>();
    Dictionary<GameObject, Information>  guysInformation = new Dictionary<GameObject, Information>();
    UIService uiService;

    float xMin, xMax, yMin, yMax;

    public void StartService(EngineScript instance)
    {
        uiService = instance.GetService<UIService>();

        xMin = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
        xMax = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;
        yMin = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).y;
        yMax = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y;
    }

    public void Update()
    {
        if (Time.timeScale == 0)
            return;

        foreach (var guy in behavioredGuys)
        {
            MoveGuy(guy);
        }
    }

    private void MoveGuy(GameObject guy)
    {
        var guyInformation = guysInformation[guy];

        if (guyInformation.Health == 0)
            return;

        if (guy.transform.position.x < xMin || guy.transform.position.x > xMax)
        {
            guyInformation.directionX *= -1;
            guy.GetComponent<SpriteRenderer>().flipX = (guyInformation.directionX < 0);
        }

        if (guy.transform.position.y < yMin || guy.transform.position.y > yMax)
        {
            guyInformation.directionY *= -1;
        }

        guy.transform.position = guy.transform.position + new Vector3(guyInformation.MovementSpeed * Time.deltaTime * guyInformation.directionX, guyInformation.MovementSpeed * Time.deltaTime * guyInformation.directionY, 0);
    }

    public void TouchGuy(GameObject guy)
    {
        var guyInformation = guysInformation[guy];

        guyInformation.Health -= 2;

        if (guyInformation.Health <= 0)
            killGuy(guy);
    }

    public void TouchGuyDoubleLightning(GameObject guy)
    {
        var guyInformation = guysInformation[guy];

        guyInformation.Health -= 1;

        if (guyInformation.Health <= 0)
            killGuy(guy);
    }

    private void killGuy(GameObject guy)
    {
        guy.GetComponent<Animator>().SetBool("Die", true);

        uiService.SetScore(2);
    }

    public void AddBehavior(GameObject gameObject)
    {
        behavioredGuys.Add(gameObject);

        var guyInfo = new Information();

        //guyInfo.directionX = UnityEngine.Random.Range(0, 1) == 0 ? -1 : 1;
        //guyInfo.directionY = UnityEngine.Random.Range(0, 1) == 0 ? -1 : 1;

        guyInfo.ScaleChanging += () => gameObject.transform.localScale = gameObject.transform.localScale - new Vector3(0.25f, 0.25f, 0);

        guysInformation.Add(gameObject, guyInfo);
    }

    public void RemoveBehavior(GameObject gameObject)
    {
        behavioredGuys.Remove(gameObject);
        guysInformation.Remove(gameObject);
    }
}

public class Information
{
    public int directionX = 1;
    public int directionY = 1;

    public event Action ScaleChanging = () => { };

    private int health = 6;
    public int Health
    {
        get { return health; }
        set
        {
            MovementSpeed += 0.5f;
            health = value;

            ScaleChanging?.Invoke();
        }
    }
    public float MovementSpeed = 1.5f;
}
