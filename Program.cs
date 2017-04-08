using System.Threading;
using EloBuddy.SDK.Menu.Values;

namespace AutoSpellUp
{
    using System;
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK.Events;
    using EloBuddy.SDK.Menu;

    internal class Program
    {

        public static Menu Menu;

        public static AIHeroClient Player
        {
            get { return EloBuddy.Player.Instance; }
        }

        public static Slider Delay, DelayMin;
        public static CheckBox Enable;
		public static ComboBox L2, L3,L4, MAX, MAX2;
		

		
		
		public enum SPELL {
			NONE,Q,W,E
		}
		
		

        private static void Main()
        {
            Loading.OnLoadingComplete += Game_OnStart;
        }

        private static void Game_OnStart(EventArgs args)
        {
            Menu = MainMenu.AddMenu("CustomLevelUp", "CustomLevelUp");
			
			

            Menu.AddGroupLabel("Antho's custom Level Up");
			Enable = new CheckBox("Enable");
			Menu.Add("enable", Enable);
			Menu.AddSeparator();
			Menu.AddLabel("Level 1 : By yourself");
	
			L2 = new ComboBox("Level 2 : ", 
			Enum.GetValues(typeof (SPELL)).Cast<SPELL>().Select(o => o.ToString()));
            Menu.Add("L2", L2);
				
			L3 = new ComboBox("Level 3 : ", 
			Enum.GetValues(typeof (SPELL)).Cast<SPELL>().Select(o => o.ToString()));
            Menu.Add("L3", L3);
				
			L4 = new ComboBox("Level 4 : ", 
			Enum.GetValues(typeof (SPELL)).Cast<SPELL>().Select(o => o.ToString()));
            Menu.Add("L4", L4);
			
			
			Menu.AddSeparator();
			MAX = new ComboBox("Maximize : ", 
			Enum.GetValues(typeof (SPELL)).Cast<SPELL>().Select(o => o.ToString()));
            Menu.Add("MAX", MAX);
			
			Menu.AddSeparator();
			MAX2 = new ComboBox("Maximize2 : ", 
			Enum.GetValues(typeof (SPELL)).Cast<SPELL>().Select(o => o.ToString()));
            Menu.Add("MAX2", MAX2);
			
			Menu.AddSeparator();

			//Delay
			Menu.AddLabel("It's not working if Minimum >= Maximum");
			DelayMin = new Slider("Minimum Delay Value", 1000, 0, 15000);
			Menu.Add("DelayMin", DelayMin);
			Delay = new Slider("Maximum Delay Value", 1000, 0, 20000);
            Menu.Add("Delay", Delay);
			
			
            Game.OnTick += Game_OnTick;
        }

        private static void Game_OnTick(EventArgs args)
        {
            try
            {
                var qL = Player.Spellbook.GetSpell(SpellSlot.Q).Level ;
                var wL = Player.Spellbook.GetSpell(SpellSlot.W).Level ;
                var eL = Player.Spellbook.GetSpell(SpellSlot.E).Level ;
                var rL = Player.Spellbook.GetSpell(SpellSlot.R).Level ;
				
				var sum = qL + wL + eL + rL;
				bool maximize = false;
				
				if(ObjectManager.Player.Level == 1 || sum == ObjectManager.Player.Level) {
					return;
				}
				//On a pas encore activé le 2eme niveau
				else if(ObjectManager.Player.Level == 2 && sum ==1) {
				
					if(L2.CurrentValue == 1) {
						LevelUp(SpellSlot.Q);
					}
					else if(L2.CurrentValue == 2) {
					    LevelUp(SpellSlot.W);
					}
					else if(L2.CurrentValue == 3) {
						LevelUp(SpellSlot.E);
					}
					else {
					//Pas de 2eme niveau on maximise
						maximize = true;
					}
				}
				else if(ObjectManager.Player.Level == 3 && sum ==2) {
					if(L3.CurrentValue == 1) {
						LevelUp(SpellSlot.Q);
					}
					else if(L3.CurrentValue == 2) {
					    LevelUp(SpellSlot.W);
					}
					else if(L3.CurrentValue == 3) {
						LevelUp(SpellSlot.E);
					}
					else {
					//Pas de 2eme niveau on maximise
						maximize = true;
					}
				}
				else if(ObjectManager.Player.Level == 4 && sum ==3) {
					if(L4.CurrentValue == 1) {
						LevelUp(SpellSlot.Q);
					}
					else if(L4.CurrentValue == 2) {
					    LevelUp(SpellSlot.W);
					}
					else if(L4.CurrentValue == 3) {
						LevelUp(SpellSlot.E);
					}
					else {
					//Pas de 2eme niveau on maximise
						maximize = true;
					}
				}
				else if(ObjectManager.Player.Level > sum ) {
					maximize = true;
				}
				
				if(maximize) {
					LevelUp(SpellSlot.R);
					//first
					if(MAX.CurrentValue == 1) {
						LevelUp(SpellSlot.Q);
					}
					else if(MAX.CurrentValue == 2){
					    LevelUp(SpellSlot.W);
					}
					else if(MAX.CurrentValue == 3)  {
						LevelUp(SpellSlot.E);
					}
					
					//Second
					if(MAX2.CurrentValue == 1) {
						LevelUp(SpellSlot.Q);
					}
					else if(MAX2.CurrentValue == 2){
					    LevelUp(SpellSlot.W);
					}
					else if(MAX2.CurrentValue == 3)  {
						LevelUp(SpellSlot.E);
					}
					
					//3eme
					LevelUp(SpellSlot.Q);
					LevelUp(SpellSlot.W);
					LevelUp(SpellSlot.E);
				}

               
            }
            catch (Exception e)
            {
				Chat.Print(e);
                Console.WriteLine(e);
            }
        }

        public static void LevelUp(SpellSlot slot)
        {
			if(Enable.CurrentValue && DelayMin.CurrentValue < Delay.CurrentValue ) {
			
				EloBuddy.SDK.Core.DelayAction(() =>
				{
					Player.Spellbook.LevelSpell(slot);
				}, new Random().Next(DelayMin.CurrentValue, Delay.CurrentValue));
			}
        }
    }
}
