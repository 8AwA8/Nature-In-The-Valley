using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.BellsAndWhistles;
using StardewValley.Menus;

namespace NatureInTheValley
{
	// Token: 0x0200000A RID: 10
	public class CreatureDonationMenu : InventoryMenu
	{
		// Token: 0x060000C2 RID: 194 RVA: 0x00010E58 File Offset: 0x0000F058
		public CreatureDonationMenu() : base((int)(((double)Game1.viewport.Width / 2.0 - 384.0) * Math.Pow((double)Game1.options.zoomLevel, 1.5) * Math.Pow((double)(1f / Game1.options.uiScale), 1.5)), (int)((double)Game1.viewport.Height / 2.0 * Math.Pow((double)Game1.options.zoomLevel, 1.5) * Math.Pow((double)(1f / Game1.options.uiScale), 1.5)), false, null, new InventoryMenu.highlightThisItem(CreatureDonationMenu.CheckDonated), Game1.player.Items.Count, Math.Max(1, Game1.player.Items.Count / 12), 0, 0, true)
		{
			base.initializeUpperRightCloseButton();
			this.exitFunction = delegate()
			{
				CreatureDonationMenu.ExitFunction(this.DonationCompletionType);
			};
			this.showGrayedOutSlots = true;
			base.initializeUpperRightCloseButton();
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x0000D734 File Offset: 0x0000B934
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

		// Token: 0x060000C4 RID: 196 RVA: 0x0000D5E0 File Offset: 0x0000B7E0
		public static bool CheckDonated(Item item)
		{
			string text;
			return item.Category == -81 && item.ItemId.Contains("NatInValley.Creature.") && (NatureInTheValleyEntry.staticCreatureData[item.ItemId.Split("NatInValley.Creature.", StringSplitOptions.None)[1]].Count <= 44 || NatureInTheValleyEntry.staticCreatureData[item.ItemId.Split("NatInValley.Creature.", StringSplitOptions.None)[1]][43] == "true") && !Game1.currentLocation.modData.TryGetValue("NatureInTheValley/Donated/" + item.ItemId.Split("NatInValley.Creature.", StringSplitOptions.None)[1], out text);
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00010F74 File Offset: 0x0000F174
		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
			Item itemAt = base.getItemAt(x, y);
			if (itemAt == null)
			{
				base.exitThisMenu(true);
				return;
			}
			string text;
			if (!itemAt.ItemId.Contains("NatInValley.Creature.") || Game1.currentLocation.modData.TryGetValue("NatureInTheValley/Donated/" + itemAt.ItemId.Split("NatInValley.Creature.", StringSplitOptions.None)[1], out text))
			{
				return;
			}
			Game1.playSound("newArtifact", null);
			this.DonationCompletionType = Math.Max(this.DonationCompletionType, this.GetRarityCode(itemAt.getDescription()));
			Game1.Multiplayer.globalChatInfoMessageEvenInSinglePlayer("NITVDonate", new string[]
			{
				Game1.player.Name,
				itemAt.DisplayName
			});
			Item item = itemAt;
			int stack = item.Stack;
			item.Stack = stack - 1;
			Game1.currentLocation.modData["NatureInTheValley/Donated/" + itemAt.ItemId.Split("NatInValley.Creature.", StringSplitOptions.None)[1]] = "true";
			if (itemAt.Stack <= 0)
			{
				Game1.player.removeItemFromInventory(itemAt);
			}
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x0001108C File Offset: 0x0000F28C
		public override void draw(SpriteBatch b)
		{
			if (Game1.options.showMenuBackground)
			{
				base.drawBackground(b);
			}
			else
			{
				b.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.4f);
			}
			Game1.drawDialogueBox(this.xPositionOnScreen - 64, this.yPositionOnScreen - 128, this.width + 128, this.height + 176, false, true, null, false, true, -1, -1, -1);
			SpriteText.drawStringWithScrollCenteredAt(b, NatureInTheValleyEntry.staticHelper.Translation.Get("DonationTitle"), this.xPositionOnScreen - 64 + (this.width + 128) / 2, this.yPositionOnScreen - 128 - 25, SpriteText.getWidthOfString(NatureInTheValleyEntry.staticHelper.Translation.Get("DonationTitle"), 999999) + 16, 1f, null, 0, 0.88f, false);
			base.draw(b);
			base.drawMouse(b, false, -1);
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x000111AC File Offset: 0x0000F3AC
		public int GetRarityCode(string Desc)
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

		// Token: 0x04000062 RID: 98
		private int DonationCompletionType;
	}
}
