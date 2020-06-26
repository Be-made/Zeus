using UnityEngine;

public class EventService : IService
{
    InteractionService  interactionService;
    LightningService    lightningService;

    public void StartService(EngineScript instance)
    {
        interactionService  = instance.GetService<InteractionService>();
        lightningService    = instance.GetService<LightningService>();
        var uiService       = instance.GetService<UIService>();

#if DEBUG
        interactionService.KeyDownD += () => 
        {
            uiService.ActivatePauseMenu();
            Time.timeScale = 0f; 
        };
        
        interactionService.KeyDownF += () => 
        {
            uiService.DeactivatePauseMenu();
            Time.timeScale = 1f;  
        };
#endif

        interactionService.OneTouchBegin    += (pos)             => lightningService.ShowSphereLihgtning(pos);
        interactionService.OneTouchMove     += (pos)             => lightningService.ShowSphereLihgtning(pos);
        interactionService.OneTouchEnd      += ()                => lightningService.HideSphereLihgtning();
        interactionService.DoubleTouchBegin += (pos1, pos2)      => lightningService.ShowDoubleLihgtning(pos1, pos2);
        interactionService.DoubleTouchMoved += (pos1, pos2)      => lightningService.ShowDoubleLihgtning(pos1, pos2);
        interactionService.DoubleTouchEnd   += ()                => lightningService.HideDoubleLihgtning();
    }

}
