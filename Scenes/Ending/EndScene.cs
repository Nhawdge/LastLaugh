using LastLaugh.Scenes.MainMenu.Components;
using LastLaugh.Utilities;
using System.ComponentModel.Design;

namespace LastLaugh.Scenes.Ending
{
    internal class EndScene : BaseScene
    {
        public EndScene()
        {
            Systems.Add(new EndingMenuSystem());

            var williamInvited = DialogueStore.Instance.HasRequiredKeys(new() { "william-invited" });
            var cecilyInvited = DialogueStore.Instance.HasRequiredKeys(new() { "cecily-invited" });
            var gertrudeInvited = DialogueStore.Instance.HasRequiredKeys(new() { "gertrude-invited" });
            var childrenInvited = DialogueStore.Instance.HasRequiredKeys(new() { "children-invited" });
            var text = williamInvited && cecilyInvited && gertrudeInvited && childrenInvited ? "You win!" : "You lose!";
            World.Create(new UiTitle
            {
                Text = text,
                Order = 2
            });

            var summaryText = "There was a big party.\nAt the end the jesters Sneako and Jane Foole battled their funniest laughs. \nThe King obviously voted for his favorite, Sneako\n";

            if (williamInvited && cecilyInvited)
                summaryText += " William and Cecily attended together happily, they both gave you their vote.\n";
            else if (williamInvited && !cecilyInvited)
                summaryText += " Cecily could not make it, so William didn't even bother to go\n";
            else
                summaryText += "Neither William nor Cecily attended, they never got their invitations\n";

            if (gertrudeInvited && !childrenInvited)
                summaryText += " Gertrude was there, but she was not happy\n Her children were throwing up stew everywhere,\n she was too busy and did not vote for you";
            else if (gertrudeInvited && childrenInvited)
                summaryText += " Gertrude was there, she was happy and voted for you,\n the children voted as well.";
            else
                summaryText += " Some say Gertrude is still looking for her stew ingredients to this day...";

            summaryText += "\n\nThanks for playing - Nhawdge, Sneeky09, Anchorlight";


            var lines = summaryText.Split('\n');
            var lastIndex = 0;

            foreach (var (line, index) in lines.Select((x, i) => (x, i)))
            {
                World.Create(new UiTitle
                {
                    Text = line,
                    Order = index + 3
                });
                lastIndex = index + 3;
            }

            World.Create(new UiButton
            {
                Order = lastIndex + 3,
                Action = () =>
                {
                    Environment.Exit(0);
                },
                Text = "Finish",
                //Background = TextureKey.BlueBox,
            });
        }
    }
}
