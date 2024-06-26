using Raylib_cs;

namespace LastLaugh.Utilities
{
    internal class TextureManager
    {
        internal static TextureManager Instance { get; } = new();

        internal Dictionary<TextureKey, Texture2D> TextureStore { get; set; } = new();
        internal Dictionary<TextureKey, string> FilePathMapping { get; set; } = new();
        internal Dictionary<TextureKey, NPatchInfo> NPatchInfos { get; set; } = new();


        internal Dictionary<string, Texture2D> TextureCache { get; set; } = new();

        private TextureManager()
        {
            MapKeys();
            LoadTextures();
        }

        private void MapKeys()
        {
            FilePathMapping.Add(TextureKey.Empty, "");
            FilePathMapping.Add(TextureKey.RaylibLogo, "Assets/raylib_logo_animation");
            FilePathMapping.Add(TextureKey.Player, "Assets/Art/Player");
            FilePathMapping.Add(TextureKey.Cecily, "Assets/Art/cecily");
            FilePathMapping.Add(TextureKey.Childf, "Assets/Art/childf");
            FilePathMapping.Add(TextureKey.Gertrude, "Assets/Art/gertrude");
            FilePathMapping.Add(TextureKey.King, "Assets/Art/king");



            FilePathMapping.Add(TextureKey.PortraitPlayer, "Assets/Art/Portraits/player1");
            FilePathMapping.Add(TextureKey.PortraitKing, "Assets/Art/Portraits/king1");
            FilePathMapping.Add(TextureKey.PortraitNpc1, "Assets/Art/Portraits/npc1");
            FilePathMapping.Add(TextureKey.PortraitMerchant1, "Assets/Art/Portraits/merchant1");
            FilePathMapping.Add(TextureKey.PortraitGuard1, "Assets/Art/Portraits/guard1");
            FilePathMapping.Add(TextureKey.PortraitCecily, "Assets/Art/Portraits/cecily1");
            FilePathMapping.Add(TextureKey.PortraitGertrude, "Assets/Art/Portraits/gertrude1");
            FilePathMapping.Add(TextureKey.PortraitChildren, "Assets/Art/Portraits/children1");

            FilePathMapping.Add(TextureKey.Fortress, "Assets/LDTK/Art/SI_Fortress");
            FilePathMapping.Add(TextureKey.Grasslands, "Assets/LDTK/Art/SI_Grasslands_Summer");
            FilePathMapping.Add(TextureKey.Interior, "Assets/LDTK/Art/SI_Interior");
            FilePathMapping.Add(TextureKey.Village, "Assets/LDTK/Art/SI_Village");
        }

        private void LoadTextures()
        {
            foreach (var mapping in FilePathMapping)
            {
                LoadKey(mapping.Key);
            }

            var mostPatchInfos = new NPatchInfo
            {
                Left = 10,
                Top = 10,
                Right = 10,
                Bottom = 10,
                Layout = NPatchLayout.NinePatch
            };

            // NPatchInfos.Add(TextureKey.BlueBox, mostPatchInfos);

        }

        private void LoadKey(TextureKey key)
        {
            TextureStore.Add(key, Raylib.LoadTexture($"{FilePathMapping[key]}.png"));
        }

        internal Texture2D GetTexture(TextureKey key)
        {
            if (TextureStore.Count <= 0)
            {
                LoadTextures();
            }
            return TextureStore[key];
        }
        internal Texture2D? GetCachedTexture(string key)
        {
            if (TextureCache.TryGetValue(key, out var texture))
                return texture;
            return null;
        }
    }

    internal enum TextureKey
    {
        Empty,
        RaylibLogo,
        PortraitPlayer,
        PortraitNpc1,
        PortraitKing,
        PortraitMerchant1,
        PortraitGuard1,
        Fortress,
        Grasslands,
        Interior,
        Village,
        Player,
        Cecily,
        Childf,
        Gertrude,
        King,
        PortraitCecily,
        PortraitGertrude,
        PortraitChildren,
    }
}
