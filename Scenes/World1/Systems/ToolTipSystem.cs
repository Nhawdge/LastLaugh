using Arch.Core;
using LastLaugh;
using LastLaugh.Scenes.World1.Helpers;
using LastLaugh.Utilities;

namespace LastLaugh.Scenes.World1.Systems
{
    internal class ToolTipSystem : GameSystem
    {
        internal override void Update(World world) { }

        internal override void UpdateNoCamera(World world)
        {
            UiHelpers.DrawToolTipOnMouse();
            InteractionHelper.ClickProcessed = false;
            InteractionHelper.ScrollProcessed = false;
        }
    }
}
