using ItemAPI;
using UnityEngine;
using Steamworks;
namespace Items
{
    class LiteralTrash : PlayerItem
     {
        public static void Init()
        {
            string itemName = "Test Item";

            string resourceName =  "Items/Resources/test_icon.png";
            GameObject obj = new GameObject(itemName);
//⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⢀⣤⣄⠄⡀⠄⠄⠄⠄⠄⠄ ⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⣴⣿⣿⣿⣿⣷⡒⠄⠄⠄⠄⠄⠄
//⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⢀⡀⣹⣿⣿⣿⣿⣿⣯⠄⠄⠄⠄⠄⠄ ⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⢀⣀⣀⣴⣿⣿⣿⣿⣿⣿⠿⠋⠄⠄⠄⠄⠄⠄
//⠄⠄⠄⠄⢀⣀⣤⣶⣾⠿⠿⠿⠿⣿⣿⣿⣿⣿⣿⣿⡇⠄⠄⠄⠄⠄⠄⠄ ⠄⡶⣶⡿⠛⠛⠉⠉⠄⠄⠄⠄⢸⣿⣿⣿⣿⣿⣿⣿⠃⠄⠄⠄⠄⠄⠄⠄
//⠄⠄⠘⠃⠄⠄⠄⠄⠄⠄⠄⠄⢠⣿⣿⣿⣿⣿⡟⠁⠄⠄⠄⠄⠄⠄⠄⠄ ⠄⠄⠄⠄⠄⠄⠄⠄⠄⣤⣾⣷⣿⣿⣿⣿⡏⠁⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄
//⠄⠄⠄⠄⠄⢀⣠⣴⣾⣿⣿⣿⣿⣿⣿⣿⣿⠂⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄ ⠄⢀⣤⣴⣾⣿⣿⣿⣿⡿⠛⠻⣿⣿⣿⣿⡇⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄
//⠄⠸⣿⣿⣿⣿⠋⠉⠄⠄⠄⠄⣼⣿⣿⡿⠇⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄ ⠄⠄⠈⠻⣿⣿⣆⠄⠄⠄⠄⠄⣿⣿⣿⣷⡀⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄
//⠄⠄⠄⠄⠄⠻⣿⣿⣆⡀⠄⠄⠈⠻⣿⣿⣿⣦⡄⠄⠄⠄⠄⠄⠄⠄⠄⠄ ⠄⠄⠄⠄⠄⣀⣌⣿⣿⣿⣦⡄⠄⠄⠄⠙⠻⣿⣿⣦⣀⠄⠄⠄⠄⠄⠄⠄
//⠄⠄⠄⠄⠈⠉⠉⠉⠉⠉⠁⠄⠄⠄⠄⠄⠄⠄⠘⠻⣿⢿⢖⠄⠄⠄⠄⠄ ⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠉⠉⠁⠄⠄⠄⠄⠄
//⠄⠄⠄⠄⠄⠄⠄⠄⠄⢠⣴⣧⣤⣴⡖⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄ ⠄⠄⠄⠄⠄⠄⠄⠄⣰⣿⣿⣿⣿⣿⣷⣀⡀⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄
//⠄⠄⠄⠄⠄⠄⠄⠄⣿⣿⣿⣿⣿⣿⣿⣿⣷⣶⡄⠄⠄⠄⠄⠄⠄⠄⠄⠄ ⠄⠄⠄⠄⠄⠄⠄⠄⠈⠘⠻⢿⣿⣿⣿⣿⣿⣿⣿⣆⠄⠄⠄⠄⠄⠄⠄⠄
    //⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⣰⣿⣿⣿⣿⣿⣿⣿⣿⣿⡆⠄⠄⠄⠄⠄⠄⠄ ⠄⠄⠄⢤⣴⣦⣄⣀⣀⣴⣿⡟⢿⣿⡿⣿⣿⣿⣿⣿⣿⡄⠄⠄⠄⠄⠄⠄
//⠄⠄⠄⠉⠉⠙⠻⠿⣿⡿⠋⠄⠈⢀⣀⣠⣾⣿⣿⣿⣿⣿⡄⠄⠄⠄⠄⠄ ⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠾⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣇⠄⠄⠄⠄⠄
//⠄⠄⠄⠄⠄⠄⠄⠄⢀⣠⣴⣾⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡏⠄⠄⠄⠄⠄ ⠄⠄⠄⠄⠄⠄⠄⣶⣿⣿⣿⣿⣿⣿⣿⣿⣿⡟⠉⠋⠉⠉⠁⠄⠄⠄⠄⠄
//⠄⠄⠄⠄⠄⠄⠄⠈⠛⠛⣿⣿⣿⣿⣿⣿⣇⡀⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄ ⠄⠄⠄⠄⠄⠄⠄⢀⣠⣶⣿⣿⠿⢛⣿⣿⣿⣿⣷⣤⠄⠄⠄⠄⠄⠄⠄⠄
//⠄⠄⠄⠄⠄⠄⣶⣷⣿⣿⡉⠄⠄⠄⠄⠉⠉⠉⠉⠉⠄⠄⠄⠄⠄⠄⠄⠄ ⠄⠄⠄⠄⠄⠄⠄⠘⠛⠟⢿⣤⣤⡀⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄
//⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄ ⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⢀⠄⣠⣶⣶⣷⣿⣶⡊⠄⠄⣀⣤⠄⠄⠄⠄
//⠄⠄⠄⠄⠄⠄⠄⣀⣴⣶⣾⢿⣿⣿⣿⣿⣿⣿⣿⣿⣶⣿⣿⡏⠄⠄⠄⠄ ⠄⠄⠄⠄⠄⠄⢸⣿⡍⠁⠄⠈⢿⣿⣿⣿⣿⣿⣿⣿⣿⠿⠁⠄⠄⠄⠄⠄
//⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⣼⣿⣿⣿⣿⣿⣿⣿⠏⠄⠄⠄⠄⠄⠄⠄ ⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⣿⣿⣿⣿⣿⣿⣿⡿⠄⠄⠄⠄⠄⠄⠄⠄
//⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⢸⣿⣿⣿⣿⣿⡿⠋⠄⠄⠄⠄⠄⠄⠄⠄⠄ ⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠈⠻⣿⣿⣿⣿⣡⣶⣶⣄⠄⠄⠄⠄⠄⠄⠄
//⠄⠄⠄⠄⠄⠄⠄⣀⣀⣠⣴⣦⡤⣿⣿⣿⣿⡻⣿⣿⡯⠄⠄⠄⠄⠄⠄⠄ ⠄⠄⠄⠄⠄⠄⣿⣿⣿⣿⣿⣿⣷⣿⣿⣿⣿⣿⣿⡟⠄⠄⠄⠄⠄⠄⠄⠄
//⠄⠄⠄⠄⠄⠄⢻⣿⣿⡏⠉⠙⠛⢛⣿⣿⣿⣿⠟⠄⠄⠄⠄⠄⠄⠄⠄⠄ ⠄⠄⠄⠄⠄⠄⠄⢿⣿⡧⠄⠄⢠⣾⣿⣿⡿⠁⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄
//⠄⠄⠄⠄⠄⠄⠄⠈⣿⣿⣄⣼⣿⣿⣿⠏⠁⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄ ⠄⠄⠄⠄⠄⠄⠄⠄⠸⡿⣻⣿⣿⣿⣿⣆⡀⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄
//⠄⠄⠄⠄⠄⠄⠄⠄⠄⣿⣻⠟⠈⠻⢿⣿⣿⣆⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄ ⠄⠄⠄⠄⠄⠄⠄⠄⠄⠿⠍⠄⠄⠄⠄⠉⠻⣿⣷⡤⣀⠄⠄⠄⠄⠄⠄⠄
//⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠈⢻⣿⡿⠄⠄⠄⠄⠄⠄⠄ ⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⣿⡯⠄⠄⠄⠄⠄⠄⠄
//⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠸⠃⠄⠄⠄⠄⠄⠄⠄ ⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⢀⣠⣶⣶⣤⡀⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄
//⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⣾⣿⣿⣿⣿⣿⡞⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄ ⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⣿⣿⣿⣿⣿⣿⡿⢃⡀⠄⠄⠄⠄⠄⠄⠄⠄
//⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠘⢿⣿⣿⣿⣿⣿⣿⣿⣧⡀⠄⠄⠄⠄⠄⠄ ⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⢈⣽⣿⣿⣿⣿⣿⣿⣿⢿⣷⣦⣀⠄⠄⠄
//⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⣸⣿⣿⣿⣿⣿⣿⣿⣿⠄⢉⣻⣿⡇⠄⠄ ⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⢠⣿⣿⡉⣀⣿⣿⣿⣿⣋⣴⣿⠟⠋⠄⠄⠄
//⠄⠄⠄⠄⠄⠄⠄⠄⠄⣠⣴⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣏⠄⠄⠄⠄⠄⠄ ⠄⠄⠄⠄⠄⠄⢀⣀⣼⣿⣿⣿⣿⣿⣿⠿⢿⣿⣿⣿⣿⣿⣮⡠⠄⠄⠄⠄
//⠄⠄⠄⠄⢰⣾⣿⣿⡿⠿⠛⠛⠛⠉⠄⠄⠄⠄⠙⠻⢿⣿⣿⣿⣶⣆⡀⠄ ⠄⠄⠄⠄⠄⠹⣿⣿⣦⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⢉⣿⣿⣿⣿⣿⠂
//⠄⠄⠄⠄⠄⠄⠈⢿⣿⣇⠄⠄⠄⠄⠄⠄⠄⠄⠄⠄⣴⣾⣿⡿⠟⠉⠄⠄ ⠄⠄⠄⠄⠄⠄⠄⠂⢿⣿⣥⡄⠄⠄⠄⠄⢀⣠⣶⣿⣿⠟⠋⠁⠄⠄⠄⠄
//⠄⠄⠄⠄⠄⣀⣤⣾⣿⣿⣷⣿⣃⡀⢴⣿⣿⡿⣿⣍⠄⠄⠄⠄⠄⠄⠄⠄ ⠄⠄⠄⠄⠄⠈⠉⠉⠉⠉⠉⠉⠉⠄⠄⠄⠉⠙⠛⠛⠛⠛⠂⠄⠄⠄⠄⠄

            var item = obj.AddComponent<LiteralTrash>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Test Des " + "cription.";
            string longDesc = "Test Item. \n\n";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Timed, 10f);
            item.consumable = false;
            item.quality = ItemQuality.EXCLUDED;
            item.sprite.IsPerpendicular = true;
            
            string TheProphecy = "'Did you ever hear the Tragedy of Darth Plagueis the wise? I thought not. It's not a sto";
            string Meme =    "ry the Jedi would tell you. It's a Sith legend. Darth Plagueis was a Dark Lord of the Sith, so powerful and so wise he could use the Force to influence the midichlorians to create life... He had such a knowledge of the dark side that he could even keep the ones he cared about from dying. The dark side of the Force is a pathway to many abilities some consider to be unnatural. He became so powerful... the only thing he was afraid of was losing his power, which eventually, of course, he did. Unfortunately, he taught his apprentice everything he knew, then his apprentice killed him in his sleep. It's ironic he could save others from death, but not himself.' ";
        }
        





        































//pp































        public class epic
        {

        }

        protected override void DoEffect(PlayerController user)
        {
            if(this is PlayerItem)
            {
                    if(this.gameObject.GetComponent<LiteralTrash>() == this.gameObject.GetComponent<LiteralTrash>() && !this is Gun && !this is epic)
                {
                    if(this is Gun)
                    {
                        //die
                        
                    }
                }
            }
            string username = SteamFriends.GetPersonaName();
            if (username == "TheTurtleMelon")
            {

                Application.OpenURL("https://www.youtube.com/watch?v=RMMlWvLJBtQ");
            }
            else
            {
                if(username == "blazeykat")
                {
                    Application.OpenURL("https://www.youtube.com/watch?v=M5V_IXMewl4");
                }
                //
                else
                {
                    Application.OpenURL("https://www.youtube.com/watch?v=dQw4w9WgXcQ");
                }
            }
            //

            
                        int decease = 0 ;
            int PlaceHolder = 0 ;
            do
            {
                PlaceHolder++ ;
                PlaceHolder++ ;
            PlaceHolder++;
                PlaceHolder++ ;
                }
            while(decease > -4d   ) ;
        }
    }
}
