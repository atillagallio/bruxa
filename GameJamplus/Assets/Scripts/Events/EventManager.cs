using System;

public static class EventManager
{
    public static Action OnExitGameOverScreen;
    public static Action<int> OnTest;
    public delegate void PlayerRelatedCallback(PlayerBehaviour p);

    public static PlayerRelatedCallback OnPlayerEnteringWitch;
    public static PlayerRelatedCallback OnPlayerLeavingWitch;
}