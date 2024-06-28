using Arch.Core;
using Arch.Core.Extensions;
using LastLaugh.Extensions;
using LastLaugh.Scenes.Components;
using LastLaugh.Scenes.Ending;
using LastLaugh.Scenes.World1.Data;
using LastLaugh.Utilities;
using System.Numerics;

namespace LastLaugh.Scenes.World1.Systems
{
    internal class DialogueSystem : GameSystem
    {
        internal override void Update(World world) { }
        internal override void UpdateNoCamera(World world)
        {
            var player = world.QueryFirst<Player>();
            var playerSprite = player.Get<Sprite>();
            var chatActivatedThisFrame = false;

            var text = "Press E to interact";
            var fontSize = 20;
            var textSize = Raylib.MeasureText(text, fontSize);

            var query = new QueryDescription().WithAll<Npc>();

            if (Singleton.Instance.ActiveDialogue == null)
            {
                world.Query(in query, (entity) =>
                {
                    var npc = entity.Get<Npc>();
                    var sprite = entity.Get<Sprite>();

                    var center = new Vector2(Raylib.GetScreenWidth() / 2 - textSize / 2, Raylib.GetScreenHeight() / 4 * 3);
                    if (Raylib.CheckCollisionRecs(playerSprite.CollisionDestination, sprite.CollisionDestination) && !string.IsNullOrEmpty(npc.DialogueKey))
                    {
                        Raylib.DrawText(text, (int)center.X, (int)center.Y, fontSize, Raylib.Fade(Color.White, 0.75f));

                        if (Raylib.IsKeyPressed(KeyboardKey.E))
                        {
                            var selectedDialogue = DialogueStore.Instance.Dialogues.FirstOrDefault(x => x.Key == npc.DialogueKey);
                            if (selectedDialogue.Value == null)
                            {
                                return;
                            }
                            Singleton.Instance.ActiveDialogue = selectedDialogue.Value;

                            Singleton.Instance.ActiveDialogueIndex = 0;
                            chatActivatedThisFrame = true;
                        }
                    }
                });
            }
            if (Singleton.Instance.ActiveDialogue != null)
            {
                var currentDialogue = Singleton.Instance.ActiveDialogue.Lines[Singleton.Instance.ActiveDialogueIndex];
                var texture = DialogueStore.GetPortrait(currentDialogue.Speaker);
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

                    var validOptions = Singleton.Instance.ActiveDialogue.Options
                        .Where(x => DialogueStore.Instance.HasRequiredKeys(x.RequiredKeys));

                    foreach (var (option, index) in validOptions.Select((x, i) => (x, i)))
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
                        if (Singleton.Instance.DialogueSelection >= validOptions.Count())
                        {
                            Singleton.Instance.DialogueSelection = validOptions.Count() - 1;
                        }
                    }
                    else if (Raylib.IsKeyPressed(KeyboardKey.E) && chatActivatedThisFrame == false)
                    {
                        var currentOption = validOptions.ToList()[Singleton.Instance.DialogueSelection];
                        var key = currentOption.NextDialogueId;

                        DialogueStore.Instance.AddUnlockedKey(currentOption.CreatedKeys);

                        if (key == "exit")
                        {
                            Singleton.Instance.ActiveDialogue = null;
                        }
                        else if (key == "end-game")
                        {
                            Singleton.Instance.ActiveDialogue = null;
                            Singleton.Instance.ActiveDialogueIndex = 0;
                            LastLaughEngine.Instance.ActiveScene = new EndScene();
                        }
                        else
                        {
                            Singleton.Instance.ActiveDialogue = DialogueStore.GetDialogue(key);
                        }
                        Singleton.Instance.ActiveDialogueIndex = 0;
                    }
                }
            }
        }
    }
}