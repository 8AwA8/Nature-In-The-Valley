using System;
using System.Collections.Generic;
using StardewValley;
using StardewValley.GameData.Shops;
using StardewValley.Menus;

namespace NatureInTheValley
{
	// Token: 0x0200001B RID: 27
	public class MobileCreatureDonationMenu : ShopMenu
	{
		// Token: 0x060001C0 RID: 448 RVA: 0x00016748 File Offset: 0x00014948
		public MobileCreatureDonationMenu() : base("-202", new Dictionary<ISalable, ItemStockInformation>(), 0, null, null, null, true)
		{
			this.exitFunction = delegate()
			{
				MobileCreatureDonationMenu.ExitFunction(MobileCreatureDonationMenu.DonationCompletionType);
			};
			this.onPurchase = (ShopMenu.OnPurchaseDelegate)Delegate.Combine(this.onPurchase, new ShopMenu.OnPurchaseDelegate(MobileCreatureDonationMenu.Donated));
			new List<string>();
			foreach (Item item in Game1.player.Items)
			{
				if (item != null && MobileCreatureDonationMenu.CheckDonated(item))
				{
					StardewValley.Object key = new StardewValley.Object(item.ItemId, 0, false, -1, 0);
					if (!this.Donatable.ContainsKey(key))
					{
						this.Donatable.Add(key, new ItemStockInformation(0, 1, item.ItemId, new int?(1), LimitedStockMode.Global, null, null, new StackDrawType?(StackDrawType.Hide), null));
					}
				}
			}
			base.setItemPriceAndStock(this.Donatable);
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x00016860 File Offset: 0x00014A60
		public static bool Donated(ISalable salable, Farmer who, int countTaken, ItemStockInformation stock)
		{
			if (!MobileCreatureDonationMenu.CheckDonated(ItemRegistry.Create(salable.QualifiedItemId, 1, 0, false)))
			{
				return false;
			}
			Game1.playSound("newArtifact", null);
			MobileCreatureDonationMenu.DonationCompletionType = Math.Max(MobileCreatureDonationMenu.DonationCompletionType, MobileCreatureDonationMenu.GetRarityCode(salable.getDescription()));
			Game1.Multiplayer.globalChatInfoMessageEvenInSinglePlayer("NITVDonate", new string[]
			{
				Game1.player.Name,
				salable.DisplayName
			});
			Game1.currentLocation.modData["NatureInTheValley/Donated/" + salable.QualifiedItemId.Split("NatInValley.Creature.", StringSplitOptions.None)[1]] = "true";
			if ((Game1.activeClickableMenu as ShopMenu).heldItem.QualifiedItemId == salable.QualifiedItemId)
			{
				(Game1.activeClickableMenu as ShopMenu).heldItem = null;
			}
			else
			{
				foreach (Item item in Game1.player.Items)
				{
					if (item != null && salable.QualifiedItemId == item.QualifiedItemId)
					{
						int stack = item.Stack;
						item.Stack = stack - 1;
						Game1.currentLocation.modData["NatureInTheValley/Donated/" + item.ItemId.Split("NatInValley.Creature.", StringSplitOptions.None)[1]] = "true";
						if (item.Stack <= 0)
						{
							Game1.player.removeItemFromInventory(item);
						}
					}
				}
			}
			return false;
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x000125BC File Offset: 0x000107BC
		public static void ExitFunction(int d)
		{
			switch (d)
			{
			case 0:
				Game1.DrawDialogue(new Dialogue(Game1.currentLocation.getCharacterFromName("IvyInsectarium"), "ENGL", NatureInTheValleyEntry.staticHelper.Translation.Get("NoDonationResponse")));
				return;
			case 1:
				Game1.DrawDialogue(new Dialogue(Game1.currentLocation.getCharacterFromName("IvyInsectarium"), "ENGL", NatureInTheValleyEntry.staticHelper.Translation.Get("CommonDonationResponse")));
				return;
			case 2:
				Game1.DrawDialogue(new Dialogue(Game1.currentLocation.getCharacterFromName("IvyInsectarium"), "ENGL", NatureInTheValleyEntry.staticHelper.Translation.Get("UncommonDonationResponse")));
				return;
			case 3:
				Game1.DrawDialogue(new Dialogue(Game1.currentLocation.getCharacterFromName("IvyInsectarium"), "ENGL", NatureInTheValleyEntry.staticHelper.Translation.Get("RareDonationResponse")));
				return;
			case 4:
				Game1.DrawDialogue(new Dialogue(Game1.currentLocation.getCharacterFromName("IvyInsectarium"), "ENGL", NatureInTheValleyEntry.staticHelper.Translation.Get("VeryRareDonationResponse")));
				return;
			case 5:
				Game1.DrawDialogue(new Dialogue(Game1.currentLocation.getCharacterFromName("IvyInsectarium"), "ENGL", NatureInTheValleyEntry.staticHelper.Translation.Get("MythicDonationResponse")));
				return;
			default:
				return;
			}
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x00012738 File Offset: 0x00010938
		public static bool CheckDonated(Item item)
		{
			string text;
			return item.Category == -81 && item.ItemId.Contains("NatInValley.Creature.") && (NatureInTheValleyEntry.staticCreatureData[item.ItemId.Split("NatInValley.Creature.", StringSplitOptions.None)[1]].Count <= 44 || NatureInTheValleyEntry.staticCreatureData[item.ItemId.Split("NatInValley.Creature.", StringSplitOptions.None)[1]][43] == "true") && !Game1.currentLocation.modData.TryGetValue("NatureInTheValley/Donated/" + item.ItemId.Split("NatInValley.Creature.", StringSplitOptions.None)[1], out text);
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x000169F0 File Offset: 0x00014BF0
		public static int GetRarityCode(string Desc)
		{
			if (Desc.Contains(NatureInTheValleyEntry.staticHelper.Translation.Get("Rarity.4")))
			{
				return 5;
			}
			if (Desc.Contains(NatureInTheValleyEntry.staticHelper.Translation.Get("Rarity.3")))
			{
				return 4;
			}
			if (Desc.Contains(NatureInTheValleyEntry.staticHelper.Translation.Get("Rarity.2")))
			{
				return 3;
			}
			if (Desc.Contains(NatureInTheValleyEntry.staticHelper.Translation.Get("Rarity.1")))
			{
				return 2;
			}
			return 1;
		}

		// Token: 0x040000E5 RID: 229
		private readonly Dictionary<ISalable, ItemStockInformation> Donatable = new Dictionary<ISalable, ItemStockInformation>();

		// Token: 0x040000E6 RID: 230
		private static int DonationCompletionType;
	}
}
