using QuickType.Dialogue;

namespace LastLaugh.Utilities
{
    internal class DialogueStore
    {
        internal static DialogueStore Instance { get; } = new();
        internal Dictionary<string, TextureKey> PortraitKeyMap = new();

        internal Dictionary<string, DialogueData> Dialogues;

        private DialogueStore()
        {
            LoadDialogue();
        }
        private void LoadDialogue()
        {
            Dialogues = new();
            var allFiles = Directory.GetFiles("Assets/Content/Dialogues", "*.json");
            foreach (var file in allFiles)
            {
                var dataText = File.ReadAllText(file);
                var dialogues = DialogueData.FromJson(dataText).ToDictionary(x => x.Key, x => x);
                foreach (var dialogue in dialogues)
                {
                    Dialogues.Add(dialogue.Key, dialogue.Value);
                }
            }

            // These tie to speaker name in dialogue json files
            PortraitKeyMap.Add("jane foole", TextureKey.PortraitPlayer);
            PortraitKeyMap.Add("king otto", TextureKey.PortraitKing);
            PortraitKeyMap.Add("random guard", TextureKey.PortraitGuard1);
            PortraitKeyMap.Add("cecily", TextureKey.PortraitCecily);
            PortraitKeyMap.Add("gertrude", TextureKey.PortraitGertrude);
            PortraitKeyMap.Add("children", TextureKey.PortraitChildren);
        }

        internal static TextureKey GetPortrait(string key)
        {
            if (key is not null && Instance.PortraitKeyMap.TryGetValue(key.ToLower(), out var texture))
            {
                return texture;
            }
            return TextureKey.Empty;
        }

        internal static DialogueData GetDialogue(string id)
        {
            return Instance.Dialogues[id];
        }

        private List<string> UnlockedKeys = new();

        internal void AddUnlockedKey(List<string> keys)
        {
            foreach (var key in keys)
            {
                var trimmedKey = key.Trim('-');
                if (key.StartsWith('-') && UnlockedKeys.Contains(trimmedKey))
                {
                    UnlockedKeys.Remove(trimmedKey);
                }
                else if (!UnlockedKeys.Contains(key))
                {
                    UnlockedKeys.Add(key);
                }
            }
        }

        internal bool HasRequiredKeys(List<string> keys)
        {
            return keys
                .Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x))
                .All(x =>
                {
                    if (x.StartsWith('-'))
                    {
                        var trimmedKey = x.Trim('-');
                        return !UnlockedKeys.Contains(trimmedKey);
                    }
                    else
                    {

                        return UnlockedKeys.Contains(x);
                    }
                });
        }
    }
}
