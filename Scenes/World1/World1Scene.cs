using LastLaugh.Scenes.World1.Systems;
using LastLaugh.Utilities;

namespace LastLaugh.Scenes.World1
{
    internal class World1Scene : BaseScene
    {
        public World1Scene(string mapId = "Level_2", Guid spawnEntity = default)
        {
            LoadingTasks.Add("Loading World", () =>
            {
                MapManager.Instance.LoadMap(mapId, World, spawnEntity);
            });

            LoadingTasks.Add("Loading", () =>
            {
                Systems.Add(new RenderSystem());
                Systems.Add(new CameraSystem());
                Systems.Add(new PlayerControlSystem());
                //Systems.Add(new MovementForceSystem());
                Systems.Add(new DialogueSystem());
                Systems.Add(new ToolTipSystem()); // Always last-ish plz
                Systems.Add(new DoorwaySystem());
            });

            LoadingTasks.Add("Ready", () =>
            {
                //SNEEngine.Instance.Camera.Target.X = 50 * 128;
                //SNEEngine.Instance.Camera.Target.Y = 50 * 128;
            });
        }
    }
}
