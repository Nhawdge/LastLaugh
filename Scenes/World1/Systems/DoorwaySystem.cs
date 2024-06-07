using Arch.Core;
using Arch.Core.Extensions;
using LastLaugh.Extensions;
using LastLaugh.Scenes.Components;

namespace LastLaugh.Scenes.World1.Systems
{
    internal class DoorwaySystem : GameSystem
    {
        internal override void Update(World world)
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

                if (Raylib.CheckCollisionRecs(playerSprite.CollisionDestination, doorSprite.CollisionDestination) && Raylib.IsKeyPressed(KeyboardKey.E))
                {
                    shouldTeleport = true;
                    teleTask = () => LastLaughEngine.Instance.ActiveScene = new World1Scene(doorway.LevelId, doorway.TargetEntityId);
                }
            });

            if (shouldTeleport)
            {
                teleTask();
            }
        }
    }
}
