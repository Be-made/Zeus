using UnityEngine;

public class TimerGameSession : IService, IUpdater
{
    private float gameSessionDuration = 30f;

    UIService uiService;
    public void StartService(EngineScript instance)
    {
        uiService = instance.GetService<UIService>();
    }

    public void Update()
    {
        gameSessionDuration -= Time.deltaTime;

        uiService.SetTimer((int)gameSessionDuration);
    }
}
