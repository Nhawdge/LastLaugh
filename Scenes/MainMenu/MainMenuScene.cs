using LastLaugh.Scenes.Components;
using LastLaugh.Scenes.MainMenu.Components;
using LastLaugh.Scenes.MainMenu.Systems;
using LastLaugh.Scenes.World1;
using LastLaugh.Utilities;
using System.Numerics;

namespace LastLaugh.Scenes.MainMenu
{
    internal class MainMenuScene : BaseScene
    {
        internal Render Logo;

        internal Sprite RaylibLogo;

        internal float LoadingTime = 0;
        public MainMenuScene()
        {
            Systems.Add(new MenuSystem());

            Logo = new Render(TextureKey.Empty);
            Logo.Position = new Vector2(Raylib.GetScreenWidth() / 2, 100);

            RaylibLogo = new Sprite(TextureKey.RaylibLogo);
            RaylibLogo.Position = new Vector2(Raylib.GetScreenWidth() / 2, Raylib.GetScreenHeight() / 2);

            World.Create(new UiButton
            {
                Order = 2,
                Action = () =>
                {
                    Console.WriteLine("Start Game");
                    LastLaughEngine.Instance.ActiveScene = new World1Scene();
                },
                Text = "Start Game",
                //Background = TextureKey.BlueBox,
            });

            //var logoRender = new Render(TextureKey.MainLogo);
            //logoRender.OriginPos = Render.OriginAlignment.Center;
            //logoRender.Position = new Vector2(10, 30);
            //World.Create(logoRender, new SkyLayer());

        }
    }
}
