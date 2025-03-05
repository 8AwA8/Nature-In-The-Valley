using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NatureInTheValley;
using StardewValley;
using StardewValley.Monsters;
using xTile.Dimensions;

namespace Creatures
{
	// Token: 0x02000007 RID: 7
	public class NatCreature
	{
		// Token: 0x0600008A RID: 138 RVA: 0x0000ED64 File Offset: 0x0000CF64
		public virtual void Draw(SpriteBatch b)
		{
			if (this.scale < 0.08f)
			{
				return;
			}
			Microsoft.Xna.Framework.Rectangle boundingBox = this.GetBoundingBox();
			if (this.rotaryAnims)
			{
				float rotation = (float)Math.Atan2((double)(this.desiredPos.Y - this.Position.Y), (double)(this.desiredPos.X - this.Position.X));
				Vector2 value = Game1.GlobalToLocal(Game1.viewport, this.Position + new Vector2((float)this.Sprite.SpriteWidth / 2f - (float)(this.Sprite.SpriteWidth - 32), (float)((!this.IsGrounded) ? -90 : 0) - (float)(this.Sprite.SpriteHeight - 32)));
				this.Sprite.draw(b, value - new Vector2((float)this.Sprite.SpriteWidth, (float)this.Sprite.SpriteHeight) * 1.2f * this.scale * (float)(Math.Sqrt((double)this.addedScale) - 1.0), (float)boundingBox.Center.Y / 10000f, 0, 0, Color.White, false, this.scale * 1.2f * this.addedScale, rotation, false);
				return;
			}
			Vector2 value2 = Game1.GlobalToLocal(Game1.viewport, this.Position + new Vector2((float)this.Sprite.SpriteWidth / 2f - (float)(this.Sprite.SpriteWidth - 32), (float)((!this.IsGrounded) ? -90 : 0) - (float)(this.Sprite.SpriteHeight - 32)));
			this.Sprite.draw(b, value2 - new Vector2((float)this.Sprite.SpriteWidth, (float)this.Sprite.SpriteHeight) * 1.2f * this.scale * (float)(Math.Sqrt((double)this.addedScale) - 1.0), (float)boundingBox.Center.Y / 10000f, 0, 0, Color.White, false, this.scale * 1.2f * this.addedScale, 0f, false);
		}

		// Token: 0x0600008B RID: 139 RVA: 0x0000EFA4 File Offset: 0x0000D1A4
		public Vector2 getLocalPosition(xTile.Dimensions.Rectangle viewport)
		{
			Vector2 position = this.Position;
			return new Vector2(position.X - (float)viewport.X, position.Y - (float)viewport.Y);
		}

		// Token: 0x0600008C RID: 140 RVA: 0x0000EFDC File Offset: 0x0000D1DC
		public virtual Microsoft.Xna.Framework.Rectangle GetBoundingBox()
		{
			if (this.Sprite == null)
			{
				return Microsoft.Xna.Framework.Rectangle.Empty;
			}
			Vector2 position = this.Position;
			return new Microsoft.Xna.Framework.Rectangle((int)position.X + 8, (int)position.Y + 16, this.Sprite.SpriteWidth, this.Sprite.SpriteHeight + ((this.scale > 3f && this.IsGrounded) ? 12 : 0));
		}

		// Token: 0x0600008D RID: 141 RVA: 0x0000F048 File Offset: 0x0000D248
		public virtual void DrawShadow(SpriteBatch b)
		{
			if (this.scale < 0.9f)
			{
				return;
			}
			b.Draw(Game1.shadowTexture, Game1.GlobalToLocal(Game1.viewport, this.Position) + new Vector2((float)this.Sprite.SpriteWidth * 1.4f + (float)this.shadowXOffset - (float)(this.Sprite.SpriteWidth - 32), (float)this.Sprite.SpriteHeight * 1.1f + (float)this.shadowYOffset - (float)(this.Sprite.SpriteHeight - 32)), new Microsoft.Xna.Framework.Rectangle?(Game1.shadowTexture.Bounds), Color.White, 0f, new Vector2((float)Game1.shadowTexture.Bounds.Center.X, (float)Game1.shadowTexture.Bounds.Center.Y), 2f, SpriteEffects.None, Math.Max(0f, (float)this.GetBoundingBox().Center.Y / 10000f) - 1E-06f);
		}

		// Token: 0x0600008E RID: 142 RVA: 0x0000F15C File Offset: 0x0000D35C
		public virtual void Update(GameTime time)
		{
			this.jumpTime += time.ElapsedGameTime.Milliseconds;
			if (this.isRunning)
			{
				this.desiredPos = this.Position + new Vector2(-150f, 0f);
				this.Position = Vector2.SmoothStep(this.Position, this.desiredPos, 10f * this.speed);
				return;
			}
			if (this.cueName != "")
			{
				if (this.ticks >= 1150)
				{
					if (Vector2.Distance(this.Position, Game1.player.Position) < 500f)
					{
						Game1.playSound(this.cueName, null);
						this.saoz = Game1.random.Next(1, 4);
					}
					this.ticks = 0;
				}
				else
				{
					this.ticks += this.saoz;
				}
			}
			if (!this.isStatic)
			{
				Farmer farmer = (Vector2.Distance(this.Position / 64f, Game1.player.Position / 64f) < (float)((int)((float)this.PlayerRange * (this.Dangerous ? 1f : NatureInTheValleyEntry.staticConfig.creatureRangeMultiplier)))) ? Game1.player : null;
				if (farmer != null)
				{
					if (!this.Dangerous && !this.friendly && this.DoesRun && farmer.isMoving() && (!farmer.hasBuff("NatCSpeedN") || Vector2.Distance(farmer.Position, this.GetEffectivePosition()) < 28f))
					{
						this.isRunning = true;
						return;
					}
					if (this.Dangerous)
					{
						this.PlayerRange = this.savedRange * 3;
						if (Vector2.Distance(farmer.Position, (this.Position + this.GetEffectivePosition()) / 2f) < 50f)
						{
							Bug bug = new Bug();
							farmer.takeDamage(this.dangerDamage, true, bug);
							bug.Removed();
						}
						if (this.isMover && this.DoesRun)
						{
							this.isMoving = true;
							this.desiredPos = this.GetComplexDesiredForFarmer(farmer);
							if (this.desiredPos != Vector2.Zero)
							{
								this.Position = Vector2.Lerp(this.Position, this.desiredPos, this.speed * 4.25f * Math.Max(1f, Math.Min(2.75f, 170f / Vector2.Distance(this.desiredPos, this.Position))));
							}
							return;
						}
					}
					if (this.friendly && Vector2.Distance(farmer.Position, this.Position) >= 128f && this.isMover && this.DoesRun)
					{
						this.isMoving = true;
						this.desiredPos = this.GetComplexDesiredForFarmer(farmer);
						if (this.desiredPos != Vector2.Zero)
						{
							this.Position = Vector2.Lerp(this.Position, this.desiredPos, this.speed * 2.5f * Math.Max(0.8f, Math.Min(3.15f, 170f / Vector2.Distance(this.desiredPos, this.Position))));
						}
						return;
					}
				}
			}
			if (!this.isMover)
			{
				return;
			}
			if (this.desiredPos == Vector2.Zero)
			{
				Vector2 vector = new Vector2((float)Game1.random.Next(-180, 180), (float)Game1.random.Next(-180, 180));
				if (this.ValidPosition(this.GetEffectivePosition() + vector * 2f / 3f) && this.ValidPosition(this.GetEffectivePosition() + vector) && this.ValidPosition(this.GetEffectivePosition() + vector / 3f))
				{
					this.desiredPos = this.Position + vector;
				}
				else
				{
					this.desiredPos = Vector2.Zero;
				}
				if (this.oscillations > 200 && !this.isRunning && !this.isStatic)
				{
					this.isRunning = true;
					this.scale = 0f;
				}
				this.oscillations++;
				return;
			}
			if (this.stopTime > this.jumpTime)
			{
				this.isMoving = false;
				return;
			}
			this.isMoving = true;
			this.oscillations = 0;
			this.Position = Vector2.SmoothStep(this.Position, this.desiredPos, 10f * this.speed);
			if (Vector2.Distance(this.Position, this.desiredPos) < 10f)
			{
				this.desiredPos = Vector2.Zero;
				this.jumpTime = 0;
				return;
			}
		}

		// Token: 0x0600008F RID: 143 RVA: 0x0000F614 File Offset: 0x0000D814
		public virtual void UpdateAnim(GameTime time)
		{
			if (!this.has5FrameLines)
			{
				if (this.isRunning)
				{
					this.Sprite.AnimateUp(time, 0, "");
					this.scale = this.scale * 0.975f - 0.0052f;
					return;
				}
				if (!this.isMover)
				{
					if (this.name == "Chameleon")
					{
						if (Game1.random.NextDouble() < 0.15)
						{
							this.Sprite.AnimateDown(time, 0, "");
						}
						return;
					}
					this.Sprite.AnimateDown(time, 0, "");
					return;
				}
				else if (this.desiredPos.X > this.Position.X)
				{
					if (this.isMoving)
					{
						this.Sprite.AnimateRight(time, 0, "");
						return;
					}
					this.Sprite.Animate(time, this.Sprite.framesPerAnimation, 1, 10f);
					return;
				}
				else
				{
					if (this.isMoving)
					{
						this.Sprite.AnimateDown(time, 0, "");
						return;
					}
					this.Sprite.Animate(time, 0, 1, 10f);
					return;
				}
			}
			else
			{
				if (this.isRunning)
				{
					this.Sprite.Animate(time, this.Sprite.framesPerAnimation * ((this.has5FrameLines && this.complexIdling) ? 8 : 5) + this.idleFrame % this.Sprite.framesPerAnimation, 1, 40f);
					this.idleFrame++;
					this.scale = this.scale * 0.96f - 0.0052f;
					return;
				}
				if (!this.isMover)
				{
					if (this.name == "Chameleon")
					{
						if (Game1.random.NextDouble() < 0.15)
						{
							this.Sprite.AnimateLeft(time, 0, "");
						}
						return;
					}
					this.Sprite.AnimateLeft(time, 0, "");
					return;
				}
				else
				{
					float num = Math.Abs(this.desiredPos.X - this.Position.X);
					float num2 = Math.Abs(this.desiredPos.Y - this.Position.Y);
					if (num > num2)
					{
						if (this.desiredPos.X > this.Position.X)
						{
							if (this.isMoving)
							{
								this.Sprite.AnimateRight(time, 0, "");
								this.idleFrame = 0;
								return;
							}
							if (this.complexIdling)
							{
								this.Sprite.Animate(time, this.Sprite.framesPerAnimation * 5 + this.idleFrame % this.Sprite.framesPerAnimation, 1, 10f);
								if (time.TotalGameTime.Ticks % 10L == 0L)
								{
									this.idleFrame++;
								}
								return;
							}
							this.Sprite.Animate(time, this.Sprite.framesPerAnimation * 4 + 1, 1, 10f);
							return;
						}
						else
						{
							if (this.isMoving)
							{
								this.Sprite.AnimateLeft(time, 0, "");
								this.idleFrame = 0;
								return;
							}
							if (this.complexIdling)
							{
								this.Sprite.Animate(time, this.Sprite.framesPerAnimation * 7 + this.idleFrame % this.Sprite.framesPerAnimation, 1, 10f);
								if (time.TotalGameTime.Ticks % 10L == 0L)
								{
									this.idleFrame++;
								}
								return;
							}
							this.Sprite.Animate(time, this.Sprite.framesPerAnimation * 4 + 3, 1, 10f);
							return;
						}
					}
					else if (this.desiredPos.Y > this.Position.Y)
					{
						if (this.isMoving)
						{
							this.Sprite.AnimateDown(time, 0, "");
							this.idleFrame = 0;
							return;
						}
						if (this.complexIdling)
						{
							this.Sprite.Animate(time, this.Sprite.framesPerAnimation * 4 + this.idleFrame % this.Sprite.framesPerAnimation, 1, 10f);
							if (time.TotalGameTime.Ticks % 10L == 0L)
							{
								this.idleFrame++;
							}
							return;
						}
						this.Sprite.Animate(time, this.Sprite.framesPerAnimation * 4, 1, 10f);
						return;
					}
					else
					{
						if (this.isMoving)
						{
							this.Sprite.AnimateUp(time, 0, "");
							this.idleFrame = 0;
							return;
						}
						if (this.complexIdling)
						{
							this.Sprite.Animate(time, this.Sprite.framesPerAnimation * 6 + this.idleFrame % this.Sprite.framesPerAnimation, 1, 10f);
							if (time.TotalGameTime.Ticks % 10L == 0L)
							{
								this.idleFrame++;
							}
							return;
						}
						this.Sprite.Animate(time, this.Sprite.framesPerAnimation * 4 + 2, 1, 10f);
						return;
					}
				}
			}
		}

		// Token: 0x06000090 RID: 144 RVA: 0x0000FB00 File Offset: 0x0000DD00
		public virtual bool TimeChange()
		{
			if (this.isStatic)
			{
				return false;
			}
			this.LifeTime += 10;
			if (this.LifeTime >= 1000 * (this.Dangerous ? 2 : 1))
			{
				this.isRunning = true;
				if (this.LifeTime > 1000 * (this.Dangerous ? 2 : 1) + 10)
				{
					return true;
				}
			}
			return this.scale < 0.09f;
		}

		// Token: 0x06000091 RID: 145 RVA: 0x0000FB74 File Offset: 0x0000DD74
		public bool ValidPosition(Vector2 position)
		{
			Vector2 tileLocation = new Vector2(position.X / 64f, position.Y / 64f);
			return this.currentLocation.map != null && (this.LocalLocationCode == 3 ^ !this.currentLocation.isWaterTile((int)(position / 64f).X, (int)(position / 64f).Y)) && (!this.currentLocation.IsTileOccupiedBy(new Vector2((float)((int)(position / 64f).X), (float)((int)(position / 64f).Y)), ~(CollisionMask.Characters | CollisionMask.Farmers | CollisionMask.Flooring), CollisionMask.None, false) || this.currentLocation.CanSpawnCharacterHere(tileLocation)) && this.currentLocation.isTilePassable(new Vector2((float)((int)(position / 64f).X), (float)((int)(position / 64f).Y))) && this.currentLocation.doesTileHaveProperty((int)(position / 64f).X, (int)(position / 64f).Y, "Passable", "Back", false) == null;
		}

		// Token: 0x06000092 RID: 146 RVA: 0x000025D8 File Offset: 0x000007D8
		public GameLocation GetLocation()
		{
			return this.currentLocation;
		}

		// Token: 0x06000093 RID: 147 RVA: 0x000025E0 File Offset: 0x000007E0
		public void SetLocation(GameLocation l)
		{
			this.currentLocation = l;
		}

		// Token: 0x06000094 RID: 148 RVA: 0x000025E9 File Offset: 0x000007E9
		public int GetFrame()
		{
			return this.Sprite.currentFrame;
		}

		// Token: 0x06000095 RID: 149 RVA: 0x000025F6 File Offset: 0x000007F6
		public void SetSprite(int i)
		{
			this.Sprite.currentFrame = i;
			this.Sprite.UpdateSourceRect();
		}

		// Token: 0x06000096 RID: 150 RVA: 0x0000FCB0 File Offset: 0x0000DEB0
		public Vector2 GetEffectivePosition()
		{
			return this.Position + new Vector2(22f * (float)this.Sprite.SpriteWidth / 32f, 22f * (float)this.Sprite.SpriteHeight / 32f) * this.scale;
		}

		// Token: 0x06000097 RID: 151 RVA: 0x0000FD08 File Offset: 0x0000DF08
		private Vector2 GetComplexDesiredForFarmer(Farmer farmer)
		{
			if (Vector2.Distance(farmer.Position, this.Position) < 128f)
			{
				return farmer.Position;
			}
			Vector2 zero = Vector2.Zero;
			Vector2 zero2 = Vector2.Zero;
			if (farmer.Position.X > this.GetEffectivePosition().X + 40f)
			{
				zero = new Vector2(64f, 0f);
			}
			else if (farmer.Position.X < this.GetEffectivePosition().X - 40f)
			{
				zero = new Vector2(-64f, 0f);
			}
			if (farmer.Position.Y > this.GetEffectivePosition().Y + 40f)
			{
				zero2 = new Vector2(0f, 64f);
			}
			else if (farmer.Position.Y < this.GetEffectivePosition().Y - 40f)
			{
				zero2 = new Vector2(0f, -64f);
			}
			if (this.ValidPosition(this.GetEffectivePosition() + zero / 2f + zero2 / 2f))
			{
				if (this.ValidPosition(this.GetEffectivePosition() + zero + zero2))
				{
					return this.Position + zero + zero2;
				}
				return this.Position + zero / 2f + zero2 / 2f;
			}
			else if (this.ValidPosition(this.GetEffectivePosition() + zero2 / 2f))
			{
				if (this.ValidPosition(this.GetEffectivePosition() + zero2))
				{
					return this.Position + zero2;
				}
				return this.Position + zero2 / 2f;
			}
			else
			{
				if (!this.ValidPosition(this.GetEffectivePosition() + zero / 2f))
				{
					return farmer.Position;
				}
				if (this.ValidPosition(this.GetEffectivePosition() + zero))
				{
					return this.Position + zero;
				}
				return this.Position + zero / 2f;
			}
		}

		// Token: 0x06000098 RID: 152 RVA: 0x0000FF34 File Offset: 0x0000E134
		public NatCreature(Vector2 position, GameLocation location, string Name, int rarity, bool isGrounded, float speed, int stopTime, float scale, bool DoesRun, bool isMover, int PlayerRange, bool dangerous, int MinTime, int MaxTime, int FramesPerAnim, string textureName, int offsetShad, int localLocationCode, int offsetShadY, bool isStatic, int width, int height, bool hasFiveFrameLines, bool rotaryAnims, bool complexIdling, bool friendly, int dangerDamage, int health, string cueName, float addedScale, List<string> variants, float variantChance)
		{
			this.Sprite = new AnimatedSprite();
			this.currentLocation = new GameLocation();
			this.speed = 0.0032f;
			this.desiredPos = default(Vector2);
			this.scale = 1f;
			this.Position = position;
			this.currentLocation = location;
			this.has5FrameLines = hasFiveFrameLines;
			this.Sprite = new AnimatedSprite(textureName);
			foreach (string text in variants)
			{
				if (Game1.random.NextDouble() < (double)variantChance && text.Length > 5)
				{
					this.Sprite = new AnimatedSprite(text);
					break;
				}
			}
			this.width = width;
			this.height = height;
			this.Sprite.SpriteHeight = height;
			this.Sprite.SpriteWidth = width;
			this.Sprite.framesPerAnimation = FramesPerAnim;
			this.IsGrounded = isGrounded;
			this.speed = speed;
			this.scale = scale;
			this.stopTime = stopTime;
			this.Rarity = rarity;
			this.name = Name;
			this.DoesRun = DoesRun;
			this.isMover = isMover;
			this.PlayerRange = PlayerRange;
			this.savedRange = PlayerRange;
			this.Dangerous = dangerous;
			this.MinTime = MinTime;
			this.MaxTime = MaxTime;
			this.shadowXOffset = offsetShad;
			this.LocalLocationCode = localLocationCode;
			this.shadowYOffset = offsetShadY;
			this.isStatic = isStatic;
			this.rotaryAnims = rotaryAnims;
			this.complexIdling = complexIdling;
			this.friendly = friendly;
			this.dangerDamage = dangerDamage;
			this.maxHealth = health;
			this.health = health;
			this.cueName = cueName;
			this.addedScale = addedScale;
			this.Sprite.UpdateSourceRect();
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00010114 File Offset: 0x0000E314
		public NatCreature(Vector2 position, GameLocation location, string Name, CreatureModel m, float addedScale)
		{
			this.Sprite = new AnimatedSprite();
			this.currentLocation = new GameLocation();
			this.speed = 0.0032f;
			this.desiredPos = default(Vector2);
			this.scale = 1f;
			this.Position = position;
			this.currentLocation = location;
			this.has5FrameLines = m.compelxAnims;
			this.Sprite = new AnimatedSprite(m.spritePath);
			foreach (string text in m.variantList)
			{
				if (Game1.random.NextDouble() < (double)m.variantChance && text.Length > 5)
				{
					this.Sprite = new AnimatedSprite(text);
					break;
				}
			}
			this.Sprite.SpriteHeight = m.ySpriteSize;
			this.Sprite.SpriteWidth = m.xSpriteSize;
			this.Sprite.framesPerAnimation = m.frames;
			this.IsGrounded = m.grounded;
			this.speed = m.speed;
			this.scale = m.scale;
			this.stopTime = m.pauseTime;
			this.Rarity = m.rarity;
			this.name = Name;
			this.DoesRun = m.doesRun;
			this.isMover = m.isMover;
			this.PlayerRange = m.range;
			this.savedRange = this.PlayerRange;
			this.Dangerous = m.dangerous;
			this.MinTime = m.minTime;
			this.MaxTime = m.maxTime;
			this.shadowXOffset = m.xShadow;
			this.LocalLocationCode = int.Parse(m.localSpawnCode);
			this.shadowYOffset = m.yShadow;
			this.rotaryAnims = m.rotaryAnims;
			this.complexIdling = m.complexIdling;
			this.friendly = m.friendlyFollower;
			this.dangerDamage = m.dangerDamage;
			this.maxHealth = m.health;
			this.health = m.health;
			this.cueName = m.cueName;
			this.addedScale = addedScale;
			this.Sprite.UpdateSourceRect();
		}

		// Token: 0x04000029 RID: 41
		public float speed;

		// Token: 0x0400002A RID: 42
		public int stopTime;

		// Token: 0x0400002B RID: 43
		public int jumpTime;

		// Token: 0x0400002C RID: 44
		public float scale;

		// Token: 0x0400002D RID: 45
		private int LifeTime;

		// Token: 0x0400002E RID: 46
		public int shadowXOffset;

		// Token: 0x0400002F RID: 47
		public string name;

		// Token: 0x04000030 RID: 48
		public int LocalLocationCode;

		// Token: 0x04000031 RID: 49
		public int savedRange;

		// Token: 0x04000032 RID: 50
		public int shadowYOffset;

		// Token: 0x04000033 RID: 51
		private int oscillations;

		// Token: 0x04000034 RID: 52
		public bool isStatic;

		// Token: 0x04000035 RID: 53
		protected AnimatedSprite Sprite;

		// Token: 0x04000036 RID: 54
		public bool isMover;

		// Token: 0x04000037 RID: 55
		public bool DoesRun;

		// Token: 0x04000038 RID: 56
		public bool IsGrounded;

		// Token: 0x04000039 RID: 57
		protected GameLocation currentLocation;

		// Token: 0x0400003A RID: 58
		public Vector2 desiredPos;

		// Token: 0x0400003B RID: 59
		public bool isMoving;

		// Token: 0x0400003C RID: 60
		public int Rarity;

		// Token: 0x0400003D RID: 61
		public int PlayerRange;

		// Token: 0x0400003E RID: 62
		public bool Dangerous;

		// Token: 0x0400003F RID: 63
		public int MinTime;

		// Token: 0x04000040 RID: 64
		public int MaxTime;

		// Token: 0x04000041 RID: 65
		public bool isRunning;

		// Token: 0x04000042 RID: 66
		public Vector2 Position;

		// Token: 0x04000043 RID: 67
		public bool has5FrameLines;

		// Token: 0x04000044 RID: 68
		public int width;

		// Token: 0x04000045 RID: 69
		public int height;

		// Token: 0x04000046 RID: 70
		private bool rotaryAnims;

		// Token: 0x04000047 RID: 71
		private int idleFrame;

		// Token: 0x04000048 RID: 72
		public bool complexIdling;

		// Token: 0x04000049 RID: 73
		private bool friendly;

		// Token: 0x0400004A RID: 74
		private int dangerDamage;

		// Token: 0x0400004B RID: 75
		public int health;

		// Token: 0x0400004C RID: 76
		public int maxHealth;

		// Token: 0x0400004D RID: 77
		public bool released;

		// Token: 0x0400004E RID: 78
		public string cueName;

		// Token: 0x0400004F RID: 79
		private int ticks;

		// Token: 0x04000050 RID: 80
		private int saoz = 1;

		// Token: 0x04000051 RID: 81
		private float addedScale;
	}
}
