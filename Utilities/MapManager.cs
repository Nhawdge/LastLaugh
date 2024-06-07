using Arch.Core;
using Arch.Core.Utils;
using LastLaugh.Extensions;
using LastLaugh.Scenes.Components;
using LastLaugh.Scenes.World1.Data;
using LastLaugh.Utilities.Entities;
using QuickType.Map;
using System.Numerics;

namespace LastLaugh.Utilities
{
    internal class MapManager
    {

        internal static MapManager Instance = new MapManager();

        internal Dictionary<Guid, string> LevelIdMap = new();

        private MapManager() { }

        public void LoadMap(string key, World world, Guid spawnEntity = default)
        {
            var mapName = "MainMap";
            var mapData = new LdtkData();
            var mapFile = File.ReadAllText($"Assets/LDTK/{mapName}.ldtk");
            var data = LdtkData.FromJson(mapFile);

            LevelIdMap = data.Levels.ToDictionary(x => x.Iid, x => x.Identifier);

            var level = data.Levels.FirstOrDefault(x => x.Identifier == key);

            if (level == null)
            {
                throw new ArgumentException($"key: '{key}' not found in {mapName}.ldtk");
            }

            Singleton.Instance.CameraConstraints.Width = level.PxWid;
            Singleton.Instance.CameraConstraints.Height = level.PxHei;
            Console.WriteLine($"Level: {level.Identifier}, Width: {level.PxWid}, Height: {level.PxHei}");

            var mapTileArchetype = new ComponentType[] { typeof(MapTile), typeof(Render) };

            foreach (var layer in level.LayerInstances)
            {
                Console.WriteLine($"Layer: {layer.Identifier}");
                if (layer.Identifier == "Ground")
                {
                    foreach (var tile in layer.GridTiles)
                    {
                        var sprite = new Render(TextureKey.Tiles);
                        sprite.OriginPos = Render.OriginAlignment.LeftTop;
                        //sprite.SourceX = (int)tile.Src[0];
                        //sprite.SourceY = (int)tile.Src[1];
                        sprite.SetSource(new Rectangle((int)tile.Src[0], (int)tile.Src[1], (int)layer.GridSize, (int)layer.GridSize));
                        sprite.Position = tile.Px.ToVector2();
                        world.Create(new MapTile(tile.Px.ToVector2() / layer.GridSize, layer.GridSize), sprite, new GroundLayer());
                    }
                }
                if (layer.Identifier == "Structures")
                {
                    foreach (var tile in layer.GridTiles)
                    {
                        var sprite = new Render(TextureKey.Structures);
                        sprite.OriginPos = Render.OriginAlignment.LeftTop;
                        //sprite.SourceX = (int)tile.Src[0];
                        //sprite.SourceY = (int)tile.Src[1];
                        sprite.SetSource(new Rectangle((int)tile.Src[0], (int)tile.Src[1], (int)layer.GridSize, (int)layer.GridSize));
                        sprite.Position = tile.Px.ToVector2();
                        world.Create(new MapTile(tile.Px.ToVector2() / layer.GridSize, layer.GridSize), sprite, new StructureLayer());
                    }
                }

                if (layer.Identifier == "Walls")
                {
                    foreach (var tile in layer.GridTiles)
                    {
                        var sprite = new Render(TextureKey.Walls);
                        sprite.OriginPos = Render.OriginAlignment.LeftTop;
                        //sprite.SourceX = (int)tile.Src[0];
                        //sprite.SourceY = (int)tile.Src[1];
                        sprite.SetSource(new Rectangle((int)tile.Src[0], (int)tile.Src[1], (int)layer.GridSize, (int)layer.GridSize));
                        sprite.Position = tile.Px.ToVector2();
                        world.Create(new MapTile(tile.Px.ToVector2() / layer.GridSize, layer.GridSize), sprite, new StructureLayer());
                    }
                }
                //if (layer.Identifier == "AutoTiles")
                //{
                //    foreach (var tile in layer.AutoLayerTiles)
                //    {
                //        var sprite = new Render(TextureKey.ScifiTiles);
                //        sprite.OriginPos = Render.OriginAlignment.LeftTop;
                //        var sourcePos = tile.Src.ToVector2();
                //        sprite.SetSource(new Rectangle((int)sourcePos.X, (int)sourcePos.Y, (int)layer.GridSize, (int)layer.GridSize));
                //        sprite.Position = tile.Px.ToVector2();
                //        world.Create(new MapTile(tile.Px.ToVector2() / layer.GridSize, layer.GridSize), sprite, new GroundLayer());
                //    }
                //}
                if (layer.Identifier == "Collisions")
                {
                    Singleton.Instance.CollisionGrid = new Dictionary<Rectangle, CollisionType>();

                    foreach (var (tileInt, index) in layer.IntGridCsv.Select((x, i) => (x, i)))
                    {
                        var position = new Rectangle(
                            index % layer.CWid * layer.GridSize,
                            index / layer.CWid * layer.GridSize,
                            layer.GridSize,
                            layer.GridSize);
                        //Console.WriteLine($"Index: {index}, tileInt: {tileInt}, dest: {position}");
                        var collision = int.Parse(tileInt.ToString()) switch
                        {
                            1 => CollisionType.Solid,
                            _ => CollisionType.None,
                        };
                        if (collision == CollisionType.None)
                        {
                            continue;
                        }
                        Singleton.Instance.CollisionGrid.Add(position, collision);

                    }
                }

                var playerLoadAt = new Vector2(0, 0);

                foreach (var entity in layer.EntityInstances)
                {
                    if (entity.Iid == spawnEntity)
                    {
                        playerLoadAt = entity.Px.ToVector2();
                        PlayerUtilities.BuildPlayer(world, playerLoadAt);
                        Console.WriteLine($"Found Player Spawn at {playerLoadAt}");
                    }
                    if (entity.Identifier == "Player_Spawn" && spawnEntity == Guid.Empty)
                    {
                        playerLoadAt = entity.Px.ToVector2();
                        PlayerUtilities.BuildPlayer(world, playerLoadAt);
                        Console.WriteLine($"Found Player Spawn at {playerLoadAt}");
                    }
                    if (entity.Identifier == "Doorway")
                    {
                        var doorAt = entity.Px.ToVector2();
                        var door = new Doorway(doorAt);
                        var levelConnection = entity.FieldInstances.First(x => x.Identifier == "Destination");


                        if (LevelIdMap.TryGetValue(levelConnection.Value.LevelIid, out var id))
                            door.LevelId = id;

                        door.TargetEntityId = levelConnection.Value.EntityIid;

                        var doorSprite = new Render(TextureKey.Empty) { Position = doorAt };
                        //doorSprite.SetSource(new Rectangle(8 * 5, 8 * 31, 8, 8));
                        doorSprite.OriginPos = Render.OriginAlignment.LeftTop;

                        world.Create(door, doorSprite, new StructureLayer());
                    }
                }

            }
        }
    }

    public class MapDetails
    {
        public Vector2 MapEdge;
        internal int TileSize;
    }
}