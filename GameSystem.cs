﻿namespace LastLaugh
{
    internal abstract class GameSystem
    {
        internal abstract void Update(Arch.Core.World world);
        internal virtual void UpdateNoCamera(Arch.Core.World world) { }
    }
}
