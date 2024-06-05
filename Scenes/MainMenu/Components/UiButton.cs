using LastLaugh.Utilities;

namespace LastLaugh.Scenes.MainMenu.Components
{
    internal class UiButton : UiTitle
    {
        internal TextureKey Background;

        internal Action Action { get; set; } = () => { };
    }
}
