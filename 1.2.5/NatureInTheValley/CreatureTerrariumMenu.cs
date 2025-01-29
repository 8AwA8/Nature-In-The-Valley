using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.BellsAndWhistles;
using StardewValley.Menus;

namespace NatureInTheValley
{
	// Token: 0x02000016 RID: 22
	public class CreatureTerrariumMenu : InventoryMenu
	{
		// Token: 0x0600010B RID: 267 RVA: 0x0000E060 File Offset: 0x0000C260
		public CreatureTerrariumMenu() : base(Game1.viewport.Width / 2 - 384, Game1.viewport.Height / 2, false, null, new InventoryMenu.highlightThisItem(CreatureTerrariumMenu.CheckDonated), Game1.player.Items.Count, Math.Max(1, Game1.player.Items.Count / 12), 0, 0, true)
		{
			base.initializeUpperRightCloseButton();
			this.exitFunction = delegate()
			{
				CreatureTerrariumMenu.ExitFunction(this.DonationCompletionType);
			};
			this.title = NatureInTheValleyEntry.staticHelper.Translation.Get("InsectariumC5");
			this.showGrayedOutSlots = true;
			base.initializeUpperRightCloseButton();
		}

		// Token: 0x0600010C RID: 268 RVA: 0x000028B4 File Offset: 0x00000AB4
		public static void ExitFunction(int d)
		{
			if (d == 0)
			{
				Game1.DrawDialogue(new Dialogue(Game1.currentLocation.getCharacterFromName("IvyInsectarium"), "ENGL", NatureInTheValleyEntry.staticHelper.Translation.Get("IvyTerrariumMessage")));
				return;
			}
		}

		// Token: 0x0600010D RID: 269 RVA: 0x0000E110 File Offset: 0x0000C310
		public static bool CheckDonated(Item item)
		{
			string text;
			return item.Category == -81 && item.ItemId.Contains("NatInValley.Creature.") && Game1.currentLocation.modData.TryGetValue("NatureInTheValley/Donated/" + item.ItemId.Split("NatInValley.Creature.", StringSplitOptions.None)[1], out text);
		}

		// Token: 0x0600010E RID: 270 RVA: 0x0000E16C File Offset: 0x0000C36C
		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
			Item itemAt = base.getItemAt(x, y);
			if (itemAt == null)
			{
				base.exitThisMenu(true);
				return;
			}
			if (!CreatureTerrariumMenu.CheckDonated(itemAt) || !Game1.player.couldInventoryAcceptThisItem(new Terrarium("Tera." + itemAt.ItemId, Vector2.Zero)))
			{
				return;
			}
			Game1.playSound("newArtifact", null);
			this.DonationCompletionType = 1;
			Item item = itemAt;
			int stack = item.Stack;
			item.Stack = stack - 1;
			if (itemAt.Stack <= 0)
			{
				Game1.player.removeItemFromInventory(itemAt);
			}
			Game1.player.addItemToInventory(new Terrarium("Tera." + itemAt.ItemId, Vector2.Zero));
		}

		// Token: 0x0600010F RID: 271 RVA: 0x0000E220 File Offset: 0x0000C420
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
			SpriteText.drawStringWithScrollCenteredAt(b, this.title, this.xPositionOnScreen - 64 + (this.width + 128) / 2, this.yPositionOnScreen - 128 - 25, SpriteText.getWidthOfString(this.title, 999999) + 16, 1f, null, 0, 0.88f, false);
			base.draw(b);
			base.drawMouse(b, false, -1);
		}

		// Token: 0x04000097 RID: 151
		private int DonationCompletionType;

		// Token: 0x04000098 RID: 152
		private string title;
	}
}
