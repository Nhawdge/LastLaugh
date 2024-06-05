using LastLaugh.Scenes.Components;

namespace LastLaugh.Scenes.MainMenu.Components
{
    internal class SpriteButton : UiTitle
    {
        internal Sprite ButtonSprite;
        internal Sprite TextSprite;
        internal Action Action { get; set; } = () => { };
    }
}
