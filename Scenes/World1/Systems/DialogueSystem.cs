using Arch.Core;
using Arch.Core.Extensions;
using LastLaugh.Extensions;
using LastLaugh.Scenes.Components;
using LastLaugh.Scenes.World1.Data;
using LastLaugh.Utilities;
using System.Numerics;
using System.Security;

namespace LastLaugh.Scenes.World1.Systems
{
    internal class DialogueSystem : GameSystem
    {
        internal override void Update(World world)
        {
            var player = world.QueryFirst<Player>();
            var playerSprite = player.Get<Sprite>();

            var query = new QueryDescription().WithAll<Npc>();

            if (Singleton.Instance.ActiveDialogue == null)
            {
                world.Query(in query, (entity) =>
                {
                    var npc = entity.Get<Npc>();
                    var sprite = entity.Get<Sprite>();

                    if (Raylib.CheckCollisionRecs(playerSprite.CollisionDestination, sprite.CollisionDestination) && Raylib.IsKeyPressed(KeyboardKey.E))
                    {
                        Singleton.Instance.ActiveDialogue = DialogueStore.Instance.Dialogues.First(x => x.Key == npc.DialogueKey).Value;
                        Singleton.Instance.ActiveDialogueIndex = 0;
                    }
                });
            }
        }
        internal override void UpdateNoCamera(World world)
        {

            if (Singleton.Instance.ActiveDialogue != null)
            {
                var currentDialogue = Singleton.Instance.ActiveDialogue.Lines[Singleton.Instance.ActiveDialogueIndex];
                var texture = currentDialogue.Speaker.ToLower() == "player" ? TextureKey.PortraitPlayer : TextureKey.PortraitNpc1;
                var portrait = TextureManager.Instance.TextureStore[texture];

                Raylib.DrawRectangle(0, Raylib.GetScreenHeight() - 200, Raylib.GetScreenWidth(), 200, Raylib.Fade(Color.Black, 0.5f));

                Raylib.DrawTexturePro(portrait,
                    new Rectangle(0, 0, portrait.Width, portrait.Height),
                    new Rectangle(10, Raylib.GetScreenHeight() - 190, 180, 180), new Vector2(0, 0), 0, Color.White);

                Raylib.DrawText(currentDialogue.Text, 200, Raylib.GetScreenHeight() - 190, 20, Color.White);

                if (Raylib.IsKeyPressed(KeyboardKey.Space))
                {
                    var max = Singleton.Instance.ActiveDialogue.Lines.Count();
                    Singleton.Instance.ActiveDialogueIndex += 1;
                    if (Singleton.Instance.ActiveDialogueIndex >= max)
                    {
                        Singleton.Instance.ActiveDialogueIndex = max - 1;
                    }
                }

                if (Singleton.Instance.ActiveDialogueIndex < Singleton.Instance.ActiveDialogue.Lines.Count() - 1)
                {
                    Raylib.DrawText("Press Space to continue", Raylib.GetScreenWidth() - 300, Raylib.GetScreenHeight() - 50, 20, Color.White);
                }
                else
                {
                    Raylib.DrawText("Press E to select", Raylib.GetScreenWidth() - 300, Raylib.GetScreenHeight() - 50, 20, Color.White);

                    var selection = Singleton.Instance.DialogueSelection;
                    foreach (var (option, index) in Singleton.Instance.ActiveDialogue.Options
                        .Where(x => DialogueStore.Instance.HasRequiredKeys(x.RequiredKeys))
                        .Select((x, i) => (x, i)))
                    {
                        var color = selection == index ? Color.Red : Color.White;
                        Raylib.DrawText(option.Text, 200, Raylib.GetScreenHeight() - 150 + index * 20, 20, color);
                    };
                    if (Raylib.IsKeyPressed(KeyboardKey.W))
                    {
                        Singleton.Instance.DialogueSelection -= 1;
                        if (Singleton.Instance.DialogueSelection < 0)
                        {
                            Singleton.Instance.DialogueSelection = 0;
                        }
                    }
                    else if (Raylib.IsKeyPressed(KeyboardKey.S))
                    {
                        Singleton.Instance.DialogueSelection += 1;
                        if (Singleton.Instance.DialogueSelection >= Singleton.Instance.ActiveDialogue.Options.Count())
                        {
                            Singleton.Instance.DialogueSelection = Singleton.Instance.ActiveDialogue.Options.Count() - 1;
                        }
                    }
                    else if (Raylib.IsKeyPressed(KeyboardKey.E))
                    {
                        var currentOption = Singleton.Instance.ActiveDialogue.Options[Singleton.Instance.DialogueSelection];
                        var key = currentOption.NextDialogueId;

                        DialogueStore.Instance.AddUnlockedKey(currentOption.CreatedKeys);

                        DialogueStore.Instance.AddUnlockedKey(currentOption.CreatedKeys);
                        if (key == "exit")
                        {
                            Singleton.Instance.ActiveDialogue = null;
                        }
                        else
                        {
                            Singleton.Instance.ActiveDialogue = DialogueStore.Instance.GetDialogue(key);
                        }
                        Singleton.Instance.ActiveDialogueIndex = 0;
                    }
                }
            }
        }
    }
}