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
	// Token: 0x0200000D RID: 13
	internal class CreatureHighlightPage : IClickableMenu
	{
		// Token: 0x060000CC RID: 204 RVA: 0x000133A4 File Offset: 0x000115A4
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
			this.backGround = NatureInTheValleyEntry.staticHelper.ModContent.Load<Texture2D>((this.data[11].Contains("3") || this.data[11].Contains("222")) ? "PNGs\\CaveBG" : (this.data[11].Contains("4") ? "PNGs\\DesertBG" : (this.data[11].Contains("2") ? "PNGs\\BeachBG" : "PNGs\\ForestBG")));
			base.initializeUpperRightCloseButton();
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00013650 File Offset: 0x00011850
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
			Game1.drawDialogueBox(this.xPositionOnScreen, this.yPositionOnScreen, this.width, (int)((float)this.height * 0.82f), false, true, null, false, true, -1, -1, -1);
			SpriteText.drawStringWithScrollCenteredAt(b, this._title, this.xPositionOnScreen + this.width / 2, this.yPositionOnScreen, SpriteText.getWidthOfString(this._title, 999999) + 16, 1f, null, 0, 0.88f, false);
			b.End();
			b.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, null);
			if (this.donated)
			{
				new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width / 10, this.yPositionOnScreen + (int)((float)this.height / 12.4f), this.width / 3, (int)((double)this.height / 3.2000000476837158)), this.backGround, new Rectangle(40, 125, 86, 78), 2f, false).draw(b, Color.White, 0.5f, 0, 28, 90);
				Game1.drawDialogueBox(this.xPositionOnScreen + this.width / 10, this.yPositionOnScreen + (int)((float)this.height / 12.4f), (int)((float)this.width / 3f), (int)((double)this.height / 3.2000000476837158), false, true, null, false, true, -1, -1, -1);
				this.Item.drawInMenu(b, new Vector2((float)((double)this.xPositionOnScreen + (double)this.width / 4.6), (float)((double)this.yPositionOnScreen + (double)this.height / 4.3)), 1.3f, 1f, 5f, StackDrawType.Hide, this.donated ? Color.White : (Color.Black * 0.2f), false);
			}
			else
			{
				Game1.drawDialogueBox(this.xPositionOnScreen + this.width / 10, this.yPositionOnScreen + (int)((float)this.height / 12.4f), (int)((float)this.width / 3f), (int)((double)this.height / 3.2), false, true, null, false, true, -1, -1, -1);
				SpriteText.drawStringWithScrollCenteredAt(b, "???", this.xPositionOnScreen + this.width / 8 + this.width / 7 - 3, this.yPositionOnScreen + this.height / 8 + (int)((double)this.height / 10.0), SpriteText.getWidthOfString("???", 999999) + 16, 1f, null, 0, 0.88f, false);
			}
			new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width / 10 + 190, this.yPositionOnScreen + (int)((float)this.height / 12.4f) + 25, this.width / 3, (int)((double)this.height / 3.2000000476837158)), Game1.mouseCursors, new Rectangle(406, 441, 12, 8), 3f, false).draw(b, this.data[9].Contains("spring") ? Color.White : Color.Black, 0.5f, 0, 28, 90);
			new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width / 10 + 190, this.yPositionOnScreen + (int)((float)this.height / 12.4f) + 55, this.width / 3, (int)((double)this.height / 3.2000000476837158)), Game1.mouseCursors, new Rectangle(406, 449, 12, 8), 3f, false).draw(b, this.data[9].Contains("summer") ? Color.White : Color.Black, 0.5f, 0, 28, 90);
			new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width / 10 + 190, this.yPositionOnScreen + (int)((float)this.height / 12.4f) + 85, this.width / 3, (int)((double)this.height / 3.2000000476837158)), Game1.mouseCursors, new Rectangle(406, 457, 12, 8), 3f, false).draw(b, this.data[9].Contains("fall") ? Color.White : Color.Black, 0.5f, 0, 28, 90);
			new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width / 10 + 190, this.yPositionOnScreen + (int)((float)this.height / 12.4f) + 115, this.width / 3, (int)((double)this.height / 3.2000000476837158)), Game1.mouseCursors, new Rectangle(406, 465, 12, 8), 3f, false).draw(b, this.data[9].Contains("winter") ? Color.White : Color.Black, 0.5f, 0, 28, 90);
			b.DrawString(Game1.dialogueFont, this.translation.Get("Rarity." + this.data[0]), new Vector2((float)(this.xPositionOnScreen + 385), (float)(this.yPositionOnScreen + this.height / 8 + 70 + 10)), (this.data[0] == "0") ? Color.Black : ((this.data[0] == "1") ? Color.Green : ((this.data[0] == "2") ? Color.Blue : ((this.data[0] == "3") ? Color.Purple : Color.DarkGoldenrod))));
			b.DrawString(Game1.dialogueFont, this.donated ? ((this.data[8] == this.translation.Get("true")) ? this.translation.Get("Dangerous") : this.translation.Get("Safe")) : "?????", new Vector2((float)(this.xPositionOnScreen + 385), (float)(this.yPositionOnScreen + this.height / 8 + 70 + 65)), this.donated ? ((this.data[8] == this.translation.Get("true")) ? Color.DarkRed : Color.Green) : Color.Black);
			b.DrawString(Game1.dialogueFont, (this.data[10] == "0") ? this.translation.Get("AWeather") : ((this.data[10] == "1") ? this.translation.Get("SWeather") : ((this.data[10] == "2") ? this.translation.Get("WWeather") : ((this.data[10] == "3") ? this.translation.Get("StWeather") : this.translation.Get("SnWeather")))), new Vector2((float)(this.xPositionOnScreen + 95), (float)(this.yPositionOnScreen + this.height / 3 + 40 + 10)), (this.data[10] == "0") ? Color.Black : ((this.data[10] == "1") ? Color.DarkOrange : ((this.data[10] == "2") ? Color.Blue : ((this.data[10] == "3") ? Color.Yellow : Color.LightSkyBlue))));
			if (this.data.Count <= 52 || this.data[50] == "0" || this.data[51] == "0")
			{
				b.DrawString(Game1.dialogueFont, this.GetTimeFromTime(this.data[12]) + "-" + this.GetTimeFromTime(this.data[13]), new Vector2((float)(this.xPositionOnScreen + 95), (float)(this.yPositionOnScreen + this.height / 3 + 40 + 65)), Color.Black);
			}
			else
			{
				b.DrawString(Game1.dialogueFont, string.Concat(new string[]
				{
					this.GetTimeFromTime(this.data[12]),
					"-",
					this.GetTimeFromTime(this.data[13]),
					this.translation.Get("and"),
					this.GetTimeFromTime(this.data[50]),
					"-",
					this.GetTimeFromTime(this.data[51])
				}), new Vector2((float)(this.xPositionOnScreen + 95), (float)(this.yPositionOnScreen + this.height / 3 + 40 + 65)), Color.Black);
			}
			if (this.data.Count <= 50 || this.data[49] == "")
			{
				b.DrawString(Game1.dialogueFont, this.translation.Get("FoundO") + ((this.data[17] == "0") ? this.translation.Get("Wander") : ((this.data[17] == "1") ? this.translation.Get("OTrees") : ((this.data[17] == "2") ? this.translation.Get("OBush") : ((this.data[17] == "3") ? this.translation.Get("OWater") : this.translation.Get("OLStump"))))) + ",", new Vector2((float)(this.xPositionOnScreen + 95), (float)(this.yPositionOnScreen + this.height / 3 + 40 + 120)), Color.Black);
			}
			else
			{
				b.DrawString(Game1.dialogueFont, this.data[49] + ",", new Vector2((float)(this.xPositionOnScreen + 95), (float)(this.yPositionOnScreen + this.height / 3 + 40 + 120)), Color.Black);
			}
			if (this.data.Count <= 49 || this.data[48] == "")
			{
				string[] array = new string[0];
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
				string text = "";
				int num = 0;
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
						char[] value = this.CodeToLoc(c).ToCharArray();
						text += new string(value);
					}
					num++;
				}
				b.DrawString(Game1.dialogueFont, text, new Vector2((float)(this.xPositionOnScreen + 95), (float)(this.yPositionOnScreen + this.height / 3 + 40 + 175)), Color.Black);
			}
			else
			{
				b.DrawString(Game1.dialogueFont, this.data[48], new Vector2((float)(this.xPositionOnScreen + 95), (float)(this.yPositionOnScreen + this.height / 3 + 40 + 175)), Color.Black);
			}
			if (this.data.Count > 54 && this.data[53] != "")
			{
				b.DrawString(Game1.dialogueFont, this.data[53], new Vector2((float)(this.xPositionOnScreen + 95), (float)(this.yPositionOnScreen + this.height / 3 + 40 + 230)), Color.Black);
			}
			b.End();
			b.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, null);
			base.drawMouse(b, false, -1);
		}

		// Token: 0x060000CE RID: 206 RVA: 0x0000276C File Offset: 0x0000096C
		public static void ExitFunction()
		{
			Game1.activeClickableMenu = new ClickIntoCreatureInfoMenu();
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00014460 File Offset: 0x00012660
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

		// Token: 0x060000D0 RID: 208 RVA: 0x00014534 File Offset: 0x00012734
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

		// Token: 0x0400006B RID: 107
		private List<string> data = new List<string>();

		// Token: 0x0400006C RID: 108
		private string name;

		// Token: 0x0400006D RID: 109
		private Item Item;

		// Token: 0x0400006E RID: 110
		private bool donated;

		// Token: 0x0400006F RID: 111
		private string hoverText = "";

		// Token: 0x04000070 RID: 112
		private int currentPage;

		// Token: 0x04000071 RID: 113
		private string _title;

		// Token: 0x04000072 RID: 114
		private ITranslationHelper translation;

		// Token: 0x04000073 RID: 115
		private Texture2D backGround;
	}
}
