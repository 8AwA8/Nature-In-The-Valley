using System;
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
		// Token: 0x06000116 RID: 278 RVA: 0x000130B4 File Offset: 0x000112B4
		public Terrarium(string itemId, Vector2 tile) : base(itemId, tile)
		{
			this.data = NatureInTheValleyEntry.staticCreatureData[itemId.Split("Tera.NatInValley.Creature.", StringSplitOptions.None)[1]];
			this.Sprite = new AnimatedSprite(this.data[15]);
			this.Sprite.SpriteHeight = ((this.data.Count > 27) ? int.Parse(this.data[27]) : 32);
			this.Sprite.SpriteWidth = ((this.data.Count > 26) ? int.Parse(this.data[26]) : 32);
			this.Sprite.framesPerAnimation = int.Parse(this.data[14]);
			this.defaultBoundingBox.Value = new Rectangle(this.defaultBoundingBox.X, this.defaultBoundingBox.Y, this.defaultBoundingBox.Width * 2, this.defaultBoundingBox.Height);
		}

		// Token: 0x06000117 RID: 279 RVA: 0x000131CC File Offset: 0x000113CC
		public override void actionOnPlayerEntryOrPlacement(GameLocation environment, bool dropDown)
		{
			this.data = NatureInTheValleyEntry.staticCreatureData[this.itemId.Value.Split("Tera.NatInValley.Creature.", StringSplitOptions.None)[1]];
			this.Sprite = new AnimatedSprite(this.data[15]);
			this.Sprite.SpriteHeight = ((this.data.Count > 27) ? int.Parse(this.data[27]) : 32);
			this.Sprite.SpriteWidth = ((this.data.Count > 26) ? int.Parse(this.data[26]) : 32);
			this.Sprite.framesPerAnimation = int.Parse(this.data[14]);
			base.actionOnPlayerEntryOrPlacement(environment, dropDown);
		}

		// Token: 0x06000118 RID: 280 RVA: 0x0000290D File Offset: 0x00000B0D
		protected override Item GetOneNew()
		{
			return new Terrarium(base.ItemId, this.tileLocation.Value);
		}

		// Token: 0x06000119 RID: 281 RVA: 0x000132A0 File Offset: 0x000114A0
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

		// Token: 0x0600011A RID: 282 RVA: 0x00002925 File Offset: 0x00000B25
		public override bool placementAction(GameLocation location, int x, int y, Farmer who = null)
		{
			return base.placementAction(location, x, y, who);
		}

		// Token: 0x0600011B RID: 283 RVA: 0x00002932 File Offset: 0x00000B32
		public float GetGlassDrawLayer()
		{
			return this.GetBaseDrawLayer() + 0.0001f;
		}

		// Token: 0x0600011C RID: 284 RVA: 0x000135FC File Offset: 0x000117FC
		public float GetBaseDrawLayer()
		{
			if (this.furniture_type.Value == 12)
			{
				return 2E-09f;
			}
			return (float)(this.boundingBox.Value.Bottom - ((this.furniture_type.Value == 6 || this.furniture_type.Value == 13) ? 48 : 8)) / 10000f;
		}

		// Token: 0x0600011D RID: 285 RVA: 0x0001365C File Offset: 0x0001185C
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

		// Token: 0x0600011E RID: 286 RVA: 0x00013A6C File Offset: 0x00011C6C
		public List<string> GetData()
		{
			if (this.data == null || this.data.Count < 1)
			{
				this.data = NatureInTheValleyEntry.staticCreatureData[this.itemId.Value.Split("Tera.NatInValley.Creature.", StringSplitOptions.None)[1]];
			}
			return this.data;
		}

		// Token: 0x0600011F RID: 287 RVA: 0x00002940 File Offset: 0x00000B40
		public Texture2D GetFrontTexture()
		{
			if (this.FrontTexture == null)
			{
				this.FrontTexture = NatureInTheValleyEntry.staticHelper.ModContent.Load<Texture2D>("PNGs\\Terrarium");
			}
			return this.FrontTexture;
		}

		// Token: 0x06000120 RID: 288 RVA: 0x0000296A File Offset: 0x00000B6A
		public Texture2D GetBackTexture()
		{
			if (this.bgTexture == null)
			{
				this.bgTexture = NatureInTheValleyEntry.staticHelper.ModContent.Load<Texture2D>("PNGs\\Base" + this.GetLocation());
			}
			return this.bgTexture;
		}

		// Token: 0x06000121 RID: 289 RVA: 0x0000299F File Offset: 0x00000B9F
		public Terrarium()
		{
		}

		// Token: 0x06000122 RID: 290 RVA: 0x000029B9 File Offset: 0x00000BB9
		public Texture2D GetAddedTexture()
		{
			if (this.AddedTexture == null)
			{
				this.AddedTexture = NatureInTheValleyEntry.staticHelper.ModContent.Load<Texture2D>("PNGs\\Added" + this.GetLocal());
			}
			return this.AddedTexture;
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00013AC0 File Offset: 0x00011CC0
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

		// Token: 0x06000124 RID: 292 RVA: 0x00013B30 File Offset: 0x00011D30
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

		// Token: 0x06000125 RID: 293 RVA: 0x000029EE File Offset: 0x00000BEE
		public Texture2D GetBackBackTexture()
		{
			if (this.BackBackdrop == null)
			{
				this.BackBackdrop = NatureInTheValleyEntry.staticHelper.ModContent.Load<Texture2D>("PNGs\\BackBack" + this.GetLocation());
			}
			return this.BackBackdrop;
		}

		// Token: 0x06000126 RID: 294 RVA: 0x00013C04 File Offset: 0x00011E04
		public override void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, StackDrawType drawStackNumber, Color color, bool drawShadow)
		{
			base.drawInMenu(spriteBatch, location, scaleSize, transparency, layerDepth, drawStackNumber, color, drawShadow);
			if (base.ItemId.Contains("NatInValley.Creature."))
			{
				spriteBatch.Draw(this.GetItemTexture().GetTexture(), location + new Vector2(56f, 52f), new Rectangle?(this.GetItemTexture().GetSourceRect(0, null)), color * transparency * 0.8f, 0f, new Vector2((float)(this.sourceRect.Width / 2), (float)(this.sourceRect.Height / 2)), 3f, SpriteEffects.None, layerDepth);
			}
		}

		// Token: 0x06000127 RID: 295 RVA: 0x00002A23 File Offset: 0x00000C23
		public ParsedItemData GetItemTexture()
		{
			if (this.itemTexture == null)
			{
				this.itemTexture = ItemRegistry.GetData(base.ItemId.Split("Tera.", StringSplitOptions.None)[1]);
			}
			return this.itemTexture;
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000128 RID: 296 RVA: 0x00002A51 File Offset: 0x00000C51
		public override string DisplayName
		{
			get
			{
				return NatureInTheValleyEntry.staticHelper.Translation.Get("Terrarium");
			}
		}

		// Token: 0x04000092 RID: 146
		[XmlIgnore]
		public bool fishDirty = true;

		// Token: 0x04000093 RID: 147
		[XmlIgnore]
		private Texture2D bgTexture;

		// Token: 0x04000094 RID: 148
		public List<string> data = new List<string>();

		// Token: 0x04000095 RID: 149
		protected AnimatedSprite Sprite;

		// Token: 0x04000096 RID: 150
		[XmlIgnore]
		private Texture2D FrontTexture;

		// Token: 0x04000097 RID: 151
		[XmlIgnore]
		private Texture2D AddedTexture;

		// Token: 0x04000098 RID: 152
		[XmlIgnore]
		private float xOffset;

		// Token: 0x04000099 RID: 153
		[XmlIgnore]
		private bool left;

		// Token: 0x0400009A RID: 154
		[XmlIgnore]
		private Texture2D BackBackdrop;

		// Token: 0x0400009B RID: 155
		[XmlIgnore]
		private ParsedItemData itemTexture;
	}
}
