using System;

namespace Game.Scripts.Runtime.Input
{
    [Flags]
    public enum InputEvent
    {
        None = 0,
        Move = 1,
        Jump = 1 << 1,
        Interact=1<<2,
        UIReturn=1<<3
    }

}