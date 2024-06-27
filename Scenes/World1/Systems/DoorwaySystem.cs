using Arch.Core;
using Arch.Core.Extensions;
using LastLaugh.Extensions;
using LastLaugh.Scenes.Components;
using System.Numerics;

namespace LastLaugh.Scenes.World1.Systems
{
    internal class DoorwaySystem : GameSystem
    {
        internal override void Update(World world)
        {
            var query = new QueryDescription().WithAll<Doorway>();

            world.Query(in query, (entity) =>  
            {
                var doorway = entity.Get<Doorway>();
                var doorSprite = entity.Get<Render>();
            });
        }

        internal override void UpdateNoCamera(World world)
        {
            var player = world.QueryFirst<Player>();
            var playerSprite = player.Get<Sprite>();

            var query = new QueryDescription().WithAll<Doorway>();

            var shouldTeleport = false;
            var teleTask = () => { };

            world.Query(in query, (entity) =>
            {
                var doorway = entity.Get<Doorway>();
                var doorSprite = entity.Get<Render>();
                if (Raylib.CheckCollisionRecs(playerSprite.CollisionDestination, doorSprite.CollisionDestination))
                {
                    var text = "Press E to interact";
                    var textSize = Raylib.MeasureText(text, 20);

                    var center = new Vector2(Raylib.GetScreenWidth() / 2 - textSize / 2, Raylib.GetScreenHeight() / 4 * 3);

                    Raylib.DrawText(text, (int)center.X, (int)center.Y, 20, Raylib.Fade(Color.White, 0.75f));
                    if (Raylib.IsKeyPressed(KeyboardKey.E))
                    {
                        shouldTeleport = true;
                        teleTask = () => LastLaughEngine.Instance.ActiveScene = new World1Scene(doorway.LevelId, doorway.TargetEntityId);
                    }
                }
            });

            if (shouldTeleport)
            {
                teleTask();
            }
        }
    }
}
