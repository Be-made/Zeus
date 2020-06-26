using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class EngineScript : MonoBehaviour
{
    private Dictionary<string, IService> serviceStorage = new Dictionary<string, IService>();

    InteractionService interactionSerice    = new InteractionService();
    SpawnService spawnService               = new SpawnService();
    EventService eventService               = new EventService();
    GuysBehaviorService guysBehaviorService = new GuysBehaviorService();
    LightningService lightningService       = new LightningService();
    UIService uiService                     = new UIService();
    TimerGameSession timerGameSession       = new TimerGameSession();



    void Start()
    {
        AddService(interactionSerice);
        AddService(eventService);
        AddService(spawnService);
        AddService(guysBehaviorService);
        AddService(lightningService);
        AddService(uiService);
        AddService(timerGameSession);

        startServices();
    }

    #region Methods for Updater logic
    private List<IFixedUpdater> needFixedUpdateServices = new List<IFixedUpdater>();
    private List<IUpdater> needUpdateServices           = new List<IUpdater>();
    private List<ILateUpdater> needLateUpdateServices   = new List<ILateUpdater>();

    private void FixedUpdate()
    {
        needFixedUpdateServices.ForEach(service => service.Update());
    }

    void Update()
    {
        needUpdateServices.ForEach(service => service.Update());
    }

    private void LateUpdate()
    {
        needLateUpdateServices.ForEach(service => service.Update());
    }

    private void startServices()
    {
        foreach (var service in serviceStorage)
        {
            service.Value.StartService(this);
        }
    }
    #endregion

    #region Methods for IService interface
    public T GetService<T>() where T : IService
    {
        string serviceName = typeof(T).Name;

        if (serviceStorage.ContainsKey(serviceName))
            return (T)serviceStorage[serviceName];

        throw new System.Exception("Unwnown service");
    }

    public void AddService<T>(T service) where T : IService
    {
        string serviceName = service.GetType().Name;

        if (serviceStorage.ContainsKey(serviceName))
            return;

        serviceStorage.Add(serviceName, service);

        IUpdater updatebleService = service as IUpdater;
        if (updatebleService != null)
            needUpdateServices.Add(service as IUpdater);

        IFixedUpdater fixUpdatebleService = service as IFixedUpdater;
        if (fixUpdatebleService != null)
            needFixedUpdateServices.Add(service as IFixedUpdater);

        ILateUpdater lateUpdatebleService = service as ILateUpdater;
        if (lateUpdatebleService != null)
            needLateUpdateServices.Add(service as ILateUpdater);
    }
    #endregion
}
