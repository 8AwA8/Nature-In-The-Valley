using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley;
using StardewValley.BellsAndWhistles;
using StardewValley.Menus;

namespace NatureInTheValley
{
	// Token: 0x0200000C RID: 12
	internal class CreatureHighlightPage : IClickableMenu
	{
		// Token: 0x060000A6 RID: 166 RVA: 0x0000BCC0 File Offset: 0x00009EC0
		public CreatureHighlightPage(Item item)
		{
			this.Item = item;
			string text;
			this.donated = Game1.getLocationFromName("NIVInnerInsec").modData.TryGetValue("NatureInTheValley/Donated/" + item.ItemId.Split("NatInValley.Creature.", StringSplitOptions.None)[1], out text);
			this._title = (this.donated ? item.DisplayName : "????????");
			this.width = 600 + IClickableMenu.borderWidth * 2;
			this.height = 800 + IClickableMenu.borderWidth * 2;
			this.translation = NatureInTheValleyEntry.staticHelper.Translation;
			base.initializeUpperRightCloseButton();
			this.exitFunction = delegate()
			{
				CreatureHighlightPage.ExitFunction();
			};
			this.xPositionOnScreen = Game1.viewport.Width / 2 - (600 + IClickableMenu.borderWidth * 2) / 2;
			this.yPositionOnScreen = Game1.viewport.Height / 2 - (800 + IClickableMenu.borderWidth * 2) / 2;
			this.xPositionOnScreen = (int)((double)((float)this.xPositionOnScreen) * Math.Pow((double)Game1.options.zoomLevel, 1.5) * Math.Pow((double)(1f / Game1.options.uiScale), 1.5));
			this.yPositionOnScreen = (int)((double)((float)this.yPositionOnScreen) * Math.Pow((double)Game1.options.zoomLevel, 1.5) * Math.Pow((double)(1f / Game1.options.uiScale), 1.5));
			int xPositionOnScreen = this.xPositionOnScreen;
			int borderWidth = IClickableMenu.borderWidth;
			int spaceToClearSideBorder = IClickableMenu.spaceToClearSideBorder;
			int yPositionOnScreen = this.yPositionOnScreen;
			int borderWidth2 = IClickableMenu.borderWidth;
			int spaceToClearTopBorder = IClickableMenu.spaceToClearTopBorder;
			this.name = item.DisplayName;
			this.data = NatureInTheValleyEntry.staticCreatureData[item.ItemId.Split("NatInValley.Creature.", StringSplitOptions.None)[1]];
			this.backGround = NatureInTheValleyEntry.staticHelper.ModContent.Load<Texture2D>(this.data[11].Contains("3") ? "PNGs\\CaveBG" : (this.data[11].Contains("4") ? "PNGs\\DesertBG" : (this.data[11].Contains("2") ? "PNGs\\BeachBG" : "PNGs\\ForestBG")));
			base.initializeUpperRightCloseButton();
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x0000BF50 File Offset: 0x0000A150
		public override void draw(SpriteBatch b)
		{
			if (!Game1.options.showMenuBackground)
			{
				b.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.4f);
			}
			else
			{
				base.drawBackground(b);
			}
			base.draw(b);
			Game1.drawDialogueBox(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height, false, true, null, false, true, -1, -1, -1);
			SpriteText.drawStringWithScrollCenteredAt(b, this._title, this.xPositionOnScreen + this.width / 2, (int)(((double)Game1.viewport.Height / 6.0 + 20.0) * Math.Pow((double)Game1.options.zoomLevel, 1.5) * Math.Pow((double)(1f / Game1.options.uiScale), 1.5)), SpriteText.getWidthOfString(this._title, 999999) + 16, 1f, null, 0, 0.88f, false);
			b.End();
			b.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, null);
			if (this.donated)
			{
				new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width / 4, this.yPositionOnScreen + this.height / 8, this.width / 2, (int)((double)this.height / 2.5)), this.backGround, new Rectangle(20, 90, 280, 230), 1f, false).draw(b, Color.White, 0.5f, 0, 28, 90);
				Game1.drawDialogueBox(this.xPositionOnScreen + this.width / 4, this.yPositionOnScreen + this.height / 8, this.width / 2, (int)((double)this.height / 2.5), false, true, null, false, true, -1, -1, -1);
				this.Item.drawInMenu(b, new Vector2((float)((double)this.xPositionOnScreen + (double)this.width / 2.2), (float)((double)this.yPositionOnScreen + (double)this.height / 3.1)), 2.6f, 1f, 5f, StackDrawType.Hide, this.donated ? Color.White : (Color.Black * 0.2f), true);
			}
			else
			{
				Game1.drawDialogueBox(this.xPositionOnScreen + this.width / 4, this.yPositionOnScreen + this.height / 8, this.width / 2, (int)((double)this.height / 2.5), false, true, null, false, true, -1, -1, -1);
				SpriteText.drawStringWithScrollCenteredAt(b, this.translation.Get("Unknown"), this.xPositionOnScreen + this.width / 4 + this.width / 4, this.yPositionOnScreen + this.height / 8 + (int)((double)this.height / 2.5) / 2, SpriteText.getWidthOfString(this.translation.Get("Unknown"), 999999) + 16, 1f, null, 0, 0.88f, false);
			}
			b.DrawString(Game1.dialogueFont, this.translation.Get("Rarity." + this.data[0]), new Vector2((float)(this.xPositionOnScreen + 50), (float)(this.yPositionOnScreen + this.height / 2 + 10)), (this.data[0] == "0") ? Color.Black : ((this.data[0] == "1") ? Color.Green : ((this.data[0] == "2") ? Color.Blue : ((this.data[0] == "3") ? Color.Purple : Color.DarkGoldenrod))));
			b.DrawString(Game1.dialogueFont, this.donated ? ((this.data[8] == this.translation.Get("true")) ? this.translation.Get("Dangerous") : this.translation.Get("Safe")) : "?????", new Vector2((float)(this.xPositionOnScreen + 50), (float)(this.yPositionOnScreen + this.height / 2 + 65)), this.donated ? ((this.data[8] == this.translation.Get("true")) ? Color.DarkRed : Color.Green) : Color.Black);
			string[] array = new string[0];
			if (this.data[9].Contains(","))
			{
				array = this.data[9].Split(",", StringSplitOptions.None);
			}
			else
			{
				array = new string[]
				{
					this.data[9]
				};
			}
			string text = "";
			int num = 0;
			if (array.Length == 4)
			{
				text = this.translation.Get("AllYear");
			}
			else
			{
				foreach (string text2 in array)
				{
					if (text != "")
					{
						if (!text.Contains(text2))
						{
							if (array.Length - 1 > num)
							{
								text = text + ", " + text2;
							}
							else
							{
								text = text + this.translation.Get("and") + text2;
							}
						}
					}
					else
					{
						text = text + this.translation.Get("Found") + text2;
					}
					num++;
				}
			}
			b.DrawString(Game1.dialogueFont, text, new Vector2((float)(this.xPositionOnScreen + 50), (float)(this.yPositionOnScreen + this.height / 2 + 120)), Color.Black);
			b.DrawString(Game1.dialogueFont, this.translation.Get("Found") + ((this.data[10] == "0") ? this.translation.Get("AWeather") : ((this.data[10] == "1") ? this.translation.Get("SWeather") : ((this.data[10] == "2") ? this.translation.Get("WWeather") : ((this.data[10] == "3") ? this.translation.Get("StWeather") : this.translation.Get("SnWeather"))))), new Vector2((float)(this.xPositionOnScreen + 50), (float)(this.yPositionOnScreen + this.height / 2 + 175)), (this.data[10] == "0") ? Color.Black : ((this.data[10] == "1") ? Color.DarkOrange : ((this.data[10] == "2") ? Color.Blue : ((this.data[10] == "3") ? Color.Yellow : Color.LightSkyBlue))));
			if (this.data[11].Contains(","))
			{
				array = this.data[11].Split(",", StringSplitOptions.None);
			}
			else
			{
				array = new string[]
				{
					this.data[11]
				};
			}
			text = "";
			num = 0;
			foreach (string c in array)
			{
				if (text != "")
				{
					if (!text.ToUpper().Contains(this.CodeToLoc(c).ToUpper()))
					{
						if (array.Length - 1 > num)
						{
							text = text + ", " + this.CodeToLoc(c);
						}
						else
						{
							text = text + this.translation.Get("and") + this.CodeToLoc(c);
						}
					}
				}
				else
				{
					char[] array3 = this.CodeToLoc(c).ToCharArray();
					array3[0] = char.ToUpper(array3[0]);
					text += new string(array3);
				}
				num++;
			}
			b.DrawString(Game1.dialogueFont, text, new Vector2((float)(this.xPositionOnScreen + 50), (float)(this.yPositionOnScreen + this.height / 2 + 230)), Color.Black);
			b.DrawString(Game1.dialogueFont, this.translation.Get("FoundBet") + this.GetTimeFromTime(this.data[12]) + this.translation.Get("and") + this.GetTimeFromTime(this.data[13]), new Vector2((float)(this.xPositionOnScreen + 50), (float)(this.yPositionOnScreen + this.height / 2 + 285)), Color.Black);
			b.DrawString(Game1.dialogueFont, this.translation.Get("FoundO") + ((this.data[17] == "0") ? this.translation.Get("Wander") : ((this.data[17] == "1") ? this.translation.Get("OTrees") : ((this.data[17] == "2") ? this.translation.Get("OBush") : ((this.data[17] == "3") ? this.translation.Get("OWater") : this.translation.Get("OLStump"))))), new Vector2((float)(this.xPositionOnScreen + 50), (float)(this.yPositionOnScreen + this.height / 2 + 340)), Color.Black);
			b.End();
			b.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, null);
			base.drawMouse(b, false, -1);
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00002583 File Offset: 0x00000783
		public static void ExitFunction()
		{
			Game1.activeClickableMenu = new ClickIntoCreatureInfoMenu();
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x0000CA28 File Offset: 0x0000AC28
		private string CodeToLoc(string c)
		{
			if (c == "0")
			{
				return this.translation.Get("Outside");
			}
			if (c == "1")
			{
				return this.translation.Get("IWoods");
			}
			if (c == "2")
			{
				return this.translation.Get("OBeaches");
			}
			if (c == "3")
			{
				return this.translation.Get("ICaves");
			}
			if (c == "4")
			{
				return this.translation.Get("IDeserts");
			}
			if (Game1.getLocationFromName(c) != null)
			{
				return Game1.getLocationFromName(c).DisplayName;
			}
			return c;
		}

		// Token: 0x060000AA RID: 170 RVA: 0x0000CAFC File Offset: 0x0000ACFC
		private string GetTimeFromTime(string time)
		{
			int num = int.Parse(time);
			if (num <= 1200)
			{
				if (num == 1200)
				{
					return "12pm";
				}
				return time.Replace("00", "") + "am";
			}
			else
			{
				if (num < 2400)
				{
					return (num - 1200).ToString().Replace("00", "") + "pm";
				}
				if (num == 2400)
				{
					return "12am";
				}
				return (num - 2400).ToString().Replace("00", "") + "am";
			}
		}

		// Token: 0x0400005C RID: 92
		private List<string> data = new List<string>();

		// Token: 0x0400005D RID: 93
		private string name;

		// Token: 0x0400005E RID: 94
		private Item Item;

		// Token: 0x0400005F RID: 95
		private bool donated;

		// Token: 0x04000060 RID: 96
		private string hoverText = "";

		// Token: 0x04000061 RID: 97
		private int currentPage;

		// Token: 0x04000062 RID: 98
		private string _title;

		// Token: 0x04000063 RID: 99
		private ITranslationHelper translation;

		// Token: 0x04000064 RID: 100
		private Texture2D backGround;
	}
}
