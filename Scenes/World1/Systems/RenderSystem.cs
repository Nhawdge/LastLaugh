using Arch.Core;
using Arch.Core.Extensions;
using LastLaugh;
using LastLaugh.Extensions;
using LastLaugh.Scenes.Components;

namespace LastLaugh.Scenes.World1.Systems
{
    internal class RenderSystem : GameSystem
    {
        internal override void Update(World world)
        {

            var query1 = new QueryDescription().WithAll<UnderLayer>();
            world.Query(in query1, (entity) =>
            {
                if (entity.Has<Render>())
                {
                    var render = entity.Get<Render>();
                    render.Draw();
                }
                else if (entity.Has<Sprite>())
                {
                    var render = entity.Get<Sprite>();
                    render.Draw();
                }
            });
            var query2 = new QueryDescription().WithAll<GroundLayer>();
            world.Query(in query2, (entity) =>
            {
                if (entity.Has<Render>())
                {
                    var render = entity.Get<Render>();
                    render.Draw();
                }
                else if (entity.Has<Sprite>())
                {
                    var render = entity.Get<Sprite>();
                    render.Draw();
                }
            });
            var query3 = new QueryDescription().WithAll<StructureLayer>();
            world.Query(in query3, (entity) =>
            {
                if (entity.Has<Render>())
                {
                    var render = entity.Get<Render>();
                    render.Draw();
                }
                else if (entity.Has<Sprite>())
                {
                    var render = entity.Get<Sprite>();
                    render.Draw();
                }
            });
            var query4 = new QueryDescription().WithAll<UnitLayer>();
            world.Query(in query4, (entity) =>
            {
                if (entity.Has<Render>())
                {
                    var render = entity.Get<Render>();
                    render.Draw();
                }
                else if (entity.Has<Sprite>())
                {
                    var render = entity.Get<Sprite>();
                    //Raylib.DrawRectangleRec(render.CollisionDestination, Color.Purple);
                    render.Draw();
                    //Raylib.DrawCircleV(render.GetCollider(CollisionDetectors.Center), 5, Color.Red);
                }
            });
            var query5 = new QueryDescription().WithAll<SkyLayer>();
            world.Query(in query5, (entity) =>
            {
                if (entity.Has<Render>())
                {
                    var render = entity.Get<Render>();
                    render.Draw();
                }
                else if (entity.Has<Sprite>())
                {
                    var render = entity.Get<Sprite>();
                    render.Draw();
                }
            });

            //foreach (var tile in Singleton.Instance.CollisionGrid)
            //{
            //    var color = new Color(Random.Shared.Next(128, 255), Random.Shared.Next(128, 255), Random.Shared.Next(128, 255), 128);
            //    Raylib.DrawRectangleRec(tile.Key, color);
            //}

        }
    }
}
