﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.ItemTypeDefinitions;
using StardewValley.Objects;

namespace NatureInTheValley
{
	// Token: 0x02000015 RID: 21
	[XmlType("Mods_Terrarium")]
	[XmlRoot(ElementName = "Terrarium", Namespace = "")]
	[KnownType(typeof(Terrarium))]
	[XmlInclude(typeof(Terrarium))]
	public class Terrarium : Furniture
	{
		// Token: 0x060000F8 RID: 248 RVA: 0x0000D458 File Offset: 0x0000B658
		public Terrarium(string itemId, Vector2 tile) : base(itemId, tile)
		{
			this.data = NatureInTheValleyEntry.staticCreatureData[itemId.Split("Tera.NatInValley.Creature.", StringSplitOptions.None)[1]];
			this.Sprite = new AnimatedSprite(this.data[15]);
			this.Sprite.SpriteHeight = ((this.data.Count > 27) ? int.Parse(this.data[27]) : 32);
			this.Sprite.SpriteWidth = ((this.data.Count > 26) ? int.Parse(this.data[26]) : 32);
			this.Sprite.framesPerAnimation = int.Parse(this.data[14]);
			this.defaultBoundingBox.Value = new Rectangle(this.defaultBoundingBox.X, this.defaultBoundingBox.Y, this.defaultBoundingBox.Width * 2, this.defaultBoundingBox.Height);
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x0000D570 File Offset: 0x0000B770
		public override void actionOnPlayerEntryOrPlacement(GameLocation environment, bool dropDown)
		{
			this.data = NatureInTheValleyEntry.staticCreatureData[this.itemId.Value.Split("Tera.NatInValley.Creature.", StringSplitOptions.None)[1]];
			this.Sprite = new AnimatedSprite(this.data[15]);
			this.Sprite.SpriteHeight = ((this.data.Count > 27) ? int.Parse(this.data[27]) : 32);
			this.Sprite.SpriteWidth = ((this.data.Count > 26) ? int.Parse(this.data[26]) : 32);
			this.Sprite.framesPerAnimation = int.Parse(this.data[14]);
			base.actionOnPlayerEntryOrPlacement(environment, dropDown);
		}

		// Token: 0x060000FA RID: 250 RVA: 0x00002755 File Offset: 0x00000955
		protected override Item GetOneNew()
		{
			return new Terrarium(base.ItemId, this.tileLocation.Value);
		}

		// Token: 0x060000FB RID: 251 RVA: 0x0000D644 File Offset: 0x0000B844
		public override void updateWhenCurrentLocation(GameTime time)
		{
			GameLocation currentLocation = Game1.currentLocation;
			GameLocation location = this.Location;
			if (this.Sprite != null)
			{
				if ((float)this.Sprite.Texture.Height / (float)this.Sprite.SpriteHeight <= 3.1f)
				{
					if (this.GetData()[6] != "true")
					{
						if (base.name.Contains("Chameleon"))
						{
							if (Game1.random.NextDouble() >= 0.15)
							{
								return;
							}
							this.Sprite.AnimateDown(time, 0, "");
						}
						else
						{
							this.Sprite.AnimateDown(time, 0, "");
						}
					}
					else if (!this.left)
					{
						this.xOffset -= 5f * float.Parse(this.GetData()[2]);
						this.Sprite.AnimateDown(time, 0, "");
						if (this.xOffset < -18f)
						{
							this.left = true;
						}
					}
					else
					{
						this.xOffset += 5f * float.Parse(this.GetData()[2]);
						this.Sprite.AnimateRight(time, 0, "");
						if (this.xOffset > 18f)
						{
							this.left = false;
						}
					}
				}
				else if (this.GetData()[6] != "true")
				{
					if (base.name.Contains("Chameleon"))
					{
						if (Game1.random.NextDouble() >= 0.15)
						{
							return;
						}
						this.Sprite.AnimateRight(time, 0, "");
					}
					else
					{
						this.Sprite.AnimateRight(time, 0, "");
					}
				}
				else if (!this.left)
				{
					this.xOffset -= 5f * float.Parse(this.GetData()[2]);
					this.Sprite.AnimateLeft(time, 0, "");
					if (this.xOffset < -18f)
					{
						this.left = true;
					}
				}
				else
				{
					this.xOffset += 5f * float.Parse(this.GetData()[2]);
					this.Sprite.AnimateRight(time, 0, "");
					if (this.xOffset > 18f)
					{
						this.left = false;
					}
				}
			}
			else
			{
				this.Sprite = new AnimatedSprite(this.GetData()[15]);
				this.Sprite.SpriteHeight = ((this.data.Count > 27) ? int.Parse(this.data[27]) : 32);
				this.Sprite.SpriteWidth = ((this.data.Count > 26) ? int.Parse(this.data[26]) : 32);
				this.Sprite.framesPerAnimation = int.Parse(this.GetData()[14]);
				this.defaultBoundingBox.Value = new Rectangle(this.defaultBoundingBox.X, this.defaultBoundingBox.Y, this.defaultBoundingBox.Width * 2, this.defaultBoundingBox.Height);
			}
			base.updateWhenCurrentLocation(time);
		}

		// Token: 0x060000FC RID: 252 RVA: 0x0000276D File Offset: 0x0000096D
		public override bool placementAction(GameLocation location, int x, int y, Farmer who = null)
		{
			return base.placementAction(location, x, y, who);
		}

		// Token: 0x060000FD RID: 253 RVA: 0x0000277A File Offset: 0x0000097A
		public float GetGlassDrawLayer()
		{
			return this.GetBaseDrawLayer() + 0.0001f;
		}

		// Token: 0x060000FE RID: 254 RVA: 0x0000D9A0 File Offset: 0x0000BBA0
		public float GetBaseDrawLayer()
		{
			if (this.furniture_type.Value == 12)
			{
				return 2E-09f;
			}
			return (float)(this.boundingBox.Value.Bottom - ((this.furniture_type.Value == 6 || this.furniture_type.Value == 13) ? 48 : 8)) / 10000f;
		}

		// Token: 0x060000FF RID: 255 RVA: 0x0000DA00 File Offset: 0x0000BC00
		public override void draw(SpriteBatch spriteBatch, int x, int y, float alpha = 1f)
		{
			Vector2 zero = Vector2.Zero;
			if (this.isTemporarilyInvisible)
			{
				return;
			}
			Vector2 vector = this.drawPosition.Value;
			if (!Furniture.isDrawingLocationFurniture)
			{
				vector = new Vector2((float)x, (float)y) * 64f;
				vector.Y -= (float)(this.sourceRect.Height * 4 - this.boundingBox.Height);
			}
			if (NatureInTheValleyEntry.staticConfig.useTerrariumWallpapers)
			{
				spriteBatch.Draw(this.GetBackBackTexture(), Game1.GlobalToLocal(Game1.viewport, vector + zero), new Rectangle?(new Rectangle(0, 0, 128, 128)), Color.White * alpha, 0f, Vector2.Zero, 1f, this.flipped.Value ? SpriteEffects.FlipHorizontally : SpriteEffects.None, this.GetGlassDrawLayer() - 0.0002f);
			}
			spriteBatch.Draw(this.GetBackTexture(), Game1.GlobalToLocal(Game1.viewport, vector + zero), new Rectangle?(new Rectangle(0, 0, 128, 128)), Color.White * alpha, 0f, Vector2.Zero, 1f, this.flipped.Value ? SpriteEffects.FlipHorizontally : SpriteEffects.None, this.GetGlassDrawLayer() - 0.0001f);
			spriteBatch.Draw(this.GetAddedTexture(), Game1.GlobalToLocal(Game1.viewport, vector + zero), new Rectangle?(new Rectangle(0, 0, 128, 128)), Color.White * alpha, 0f, Vector2.Zero, 1f, this.flipped.Value ? SpriteEffects.FlipHorizontally : SpriteEffects.None, this.GetGlassDrawLayer() - 1E-06f);
			if (this.Sprite != null)
			{
				this.Sprite.draw(spriteBatch, Game1.GlobalToLocal(Game1.viewport, vector) + new Vector2(this.xOffset, (this.GetData()[1] == "true") ? 30f : 25f) + new Vector2(32f, 32f) * 1f / Math.Min(float.Parse(this.GetData()[4]), 2.7f) + new Vector2((float)this.Sprite.SpriteWidth / 2f, 0f), this.GetGlassDrawLayer(), 0, 0, Color.White * alpha, false, Math.Min(float.Parse(this.GetData()[4]), 2.7f) * Math.Min(32f / (float)this.Sprite.SpriteHeight, 32f / (float)this.Sprite.SpriteWidth), 0f, false);
			}
			else
			{
				this.Sprite = new AnimatedSprite(this.GetData()[15]);
				this.Sprite.SpriteHeight = ((this.data.Count > 27) ? int.Parse(this.data[27]) : 32);
				this.Sprite.SpriteWidth = ((this.data.Count > 26) ? int.Parse(this.data[26]) : 32);
				this.Sprite.framesPerAnimation = int.Parse(this.GetData()[14]);
				this.defaultBoundingBox.Value = new Rectangle(this.defaultBoundingBox.X, this.defaultBoundingBox.Y, this.defaultBoundingBox.Width * 2, this.defaultBoundingBox.Height);
			}
			spriteBatch.Draw(this.GetFrontTexture(), Game1.GlobalToLocal(Game1.viewport, vector + zero), new Rectangle?(new Rectangle(0, 0, 128, 128)), Color.White * alpha, 0f, Vector2.Zero, 1f, this.flipped.Value ? SpriteEffects.FlipHorizontally : SpriteEffects.None, this.GetGlassDrawLayer() + 0.0001f);
		}

		// Token: 0x06000100 RID: 256 RVA: 0x0000DE10 File Offset: 0x0000C010
		public List<string> GetData()
		{
			if (this.data == null || this.data.Count < 1)
			{
				this.data = NatureInTheValleyEntry.staticCreatureData[this.itemId.Value.Split("Tera.NatInValley.Creature.", StringSplitOptions.None)[1]];
			}
			return this.data;
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00002788 File Offset: 0x00000988
		public Texture2D GetFrontTexture()
		{
			if (this.FrontTexture == null)
			{
				this.FrontTexture = NatureInTheValleyEntry.staticHelper.ModContent.Load<Texture2D>("PNGs\\Terrarium");
			}
			return this.FrontTexture;
		}

		// Token: 0x06000102 RID: 258 RVA: 0x000027B2 File Offset: 0x000009B2
		public Texture2D GetBackTexture()
		{
			if (this.bgTexture == null)
			{
				this.bgTexture = NatureInTheValleyEntry.staticHelper.ModContent.Load<Texture2D>("PNGs\\Base" + this.GetLocation());
			}
			return this.bgTexture;
		}

		// Token: 0x06000103 RID: 259 RVA: 0x000027E7 File Offset: 0x000009E7
		public Terrarium()
		{
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00002801 File Offset: 0x00000A01
		public Texture2D GetAddedTexture()
		{
			if (this.AddedTexture == null)
			{
				this.AddedTexture = NatureInTheValleyEntry.staticHelper.ModContent.Load<Texture2D>("PNGs\\Added" + this.GetLocal());
			}
			return this.AddedTexture;
		}

		// Token: 0x06000105 RID: 261 RVA: 0x0000DE64 File Offset: 0x0000C064
		private string GetLocation()
		{
			if (this.GetData()[11].Contains("3"))
			{
				return "3";
			}
			if (this.GetData()[11].Contains("4"))
			{
				return "4";
			}
			if (this.GetData()[11].Contains("2"))
			{
				return "2";
			}
			return "1";
		}

		// Token: 0x06000106 RID: 262 RVA: 0x0000DED4 File Offset: 0x0000C0D4
		private string GetLocal()
		{
			if (this.GetData()[17] == "1" && (this.GetLocation() == "2" || this.GetLocation() == "4"))
			{
				return "5";
			}
			if (this.GetData()[17].Contains("0"))
			{
				return "0";
			}
			if (this.GetData()[17].Contains("3"))
			{
				return "3";
			}
			if (this.GetData()[17].Contains("2"))
			{
				return "2";
			}
			if (this.GetData()[17].Contains("4"))
			{
				return "4";
			}
			return "1";
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00002836 File Offset: 0x00000A36
		public Texture2D GetBackBackTexture()
		{
			if (this.BackBackdrop == null)
			{
				this.BackBackdrop = NatureInTheValleyEntry.staticHelper.ModContent.Load<Texture2D>("PNGs\\BackBack" + this.GetLocation());
			}
			return this.BackBackdrop;
		}

		// Token: 0x06000108 RID: 264 RVA: 0x0000DFA8 File Offset: 0x0000C1A8
		public override void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, StackDrawType drawStackNumber, Color color, bool drawShadow)
		{
			base.drawInMenu(spriteBatch, location, scaleSize, transparency, layerDepth, drawStackNumber, color, drawShadow);
			if (base.ItemId.Contains("NatInValley.Creature."))
			{
				spriteBatch.Draw(this.GetItemTexture().GetTexture(), location + new Vector2(56f, 52f), new Rectangle?(this.GetItemTexture().GetSourceRect(0, null)), color * transparency * 0.8f, 0f, new Vector2((float)(this.sourceRect.Width / 2), (float)(this.sourceRect.Height / 2)), 3f, SpriteEffects.None, layerDepth);
			}
		}

		// Token: 0x06000109 RID: 265 RVA: 0x0000286B File Offset: 0x00000A6B
		public ParsedItemData GetItemTexture()
		{
			if (this.itemTexture == null)
			{
				this.itemTexture = ItemRegistry.GetData(base.ItemId.Split("Tera.", StringSplitOptions.None)[1]);
			}
			return this.itemTexture;
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600010A RID: 266 RVA: 0x00002899 File Offset: 0x00000A99
		public override string DisplayName
		{
			get
			{
				return NatureInTheValleyEntry.staticHelper.Translation.Get("Terrarium");
			}
		}

		// Token: 0x0400008D RID: 141
		[XmlIgnore]
		public bool fishDirty = true;

		// Token: 0x0400008E RID: 142
		[XmlIgnore]
		private Texture2D bgTexture;

		// Token: 0x0400008F RID: 143
		public List<string> data = new List<string>();

		// Token: 0x04000090 RID: 144
		protected AnimatedSprite Sprite;

		// Token: 0x04000091 RID: 145
		[XmlIgnore]
		private Texture2D FrontTexture;

		// Token: 0x04000092 RID: 146
		[XmlIgnore]
		private Texture2D AddedTexture;

		// Token: 0x04000093 RID: 147
		[XmlIgnore]
		private float xOffset;

		// Token: 0x04000094 RID: 148
		[XmlIgnore]
		private bool left;

		// Token: 0x04000095 RID: 149
		[XmlIgnore]
		private Texture2D BackBackdrop;

		// Token: 0x04000096 RID: 150
		[XmlIgnore]
		private ParsedItemData itemTexture;
	}
}
