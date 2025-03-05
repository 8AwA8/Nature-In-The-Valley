using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley;

namespace NatureInTheValley
{
	// Token: 0x0200001A RID: 26
	[XmlType("Mods_NatInValeySaphNet")]
	[XmlRoot(ElementName = "NatInValeySaphNet", Namespace = "")]
	[KnownType(typeof(NatInValeySaphNet))]
	[XmlInclude(typeof(NatInValeySaphNet))]
	public class NatInValeySaphNet : Tool
	{
		// Token: 0x06000184 RID: 388 RVA: 0x00002D5E File Offset: 0x00000F5E
		protected override Item GetOneNew()
		{
			return new NatInValeySaphNet();
		}

		// Token: 0x06000185 RID: 389 RVA: 0x00002D65 File Offset: 0x00000F65
		protected override string loadDisplayName()
		{
			return this.helper.Translation.Get("SaphNetName");
		}

		// Token: 0x06000186 RID: 390 RVA: 0x00002D81 File Offset: 0x00000F81
		public override string getDescription()
		{
			return this.helper.Translation.Get("SaphNetDescript");
		}

		// Token: 0x06000187 RID: 391 RVA: 0x00014148 File Offset: 0x00012348
		public override void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, StackDrawType drawStackNumber, Color color, bool drawShadow)
		{
			spriteBatch.Draw(NatInValeySaphNet.texture, location + new Vector2(32f, 32f), new Rectangle?(new Rectangle(0, 0, 16, 16)), Color.White * transparency, 0f, new Vector2(8f, 8f), (float)(4.0 * (double)scaleSize), SpriteEffects.None, layerDepth);
		}

		// Token: 0x06000188 RID: 392 RVA: 0x000141B8 File Offset: 0x000123B8
		public override void actionWhenStopBeingHeld(Farmer who)
		{
			base.actionWhenStopBeingHeld(who);
			NatureInTheValleyEntry.AnimatedBump.Value = 0;
			who.canMove = true;
			this.isHeld = false;
			if (who.UsingTool)
			{
				who.UsingTool = false;
				if (who.FarmerSprite.PauseForSingleAnimation)
				{
					who.FarmerSprite.PauseForSingleAnimation = false;
				}
			}
			if (who.hasBuff("NatCSpeedN"))
			{
				who.buffs.Remove("NatCSpeedN");
			}
		}

		// Token: 0x06000189 RID: 393 RVA: 0x0001422C File Offset: 0x0001242C
		public override bool beginUsing(GameLocation location, int x, int y, Farmer who)
		{
			who.jitterStrength = 1f;
			who.canMove = true;
			this.save = 0;
			this.isHeld = true;
			who.forceCanMove();
			if (!who.hasBuff("NatCSpeedN"))
			{
				who.applyBuff(new Buff("NatCSpeedN", null, null, 20000, null, -1, null, null, null, null)
				{
					effects = 
					{
						Speed = 
						{
							Value = -((float)who.Speed + who.buffs.Speed - 2.5f)
						}
					},
					millisecondsDuration = 20000
				});
			}
			return false;
		}

		// Token: 0x0600018A RID: 394 RVA: 0x0000264E File Offset: 0x0000084E
		public virtual int salePrice()
		{
			return -1;
		}

		// Token: 0x0600018B RID: 395 RVA: 0x000142D0 File Offset: 0x000124D0
		public override bool onRelease(GameLocation location, int x, int y, Farmer who)
		{
			who.stopJittering();
			this.isHeld = false;
			who.canMove = false;
			who.jitterStrength = 0f;
			who.UsingTool = false;
			if (this.chargeSound != null)
			{
				this.chargeSound.Stop(AudioStopOptions.Immediate);
			}
			if (who.hasBuff("NatCSpeedN"))
			{
				ICue cue;
				Game1.playSound("NIVNetSwing", out cue);
				cue.Volume = cue.Volume * 2.5f / 3f;
			}
			switch (who.FacingDirection)
			{
			case 0:
				((FarmerSprite)who.Sprite).animateOnce(295, 27f, 8, delegate(Farmer f)
				{
					this.EndOfUse(f);
				});
				this.Update(0, 0, who);
				break;
			case 1:
				((FarmerSprite)who.Sprite).animateOnce(296, 27f, 8, delegate(Farmer f)
				{
					this.EndOfUse(f);
				});
				this.Update(1, 0, who);
				break;
			case 2:
				((FarmerSprite)who.Sprite).animateOnce(297, 27f, 8, delegate(Farmer f)
				{
					this.EndOfUse(f);
				});
				this.Update(2, 0, who);
				break;
			case 3:
				((FarmerSprite)who.Sprite).animateOnce(298, 27f, 8, delegate(Farmer f)
				{
					this.EndOfUse(f);
				});
				this.Update(3, 0, who);
				break;
			}
			who.canReleaseTool = false;
			return false;
		}

		// Token: 0x0600018C RID: 396 RVA: 0x0001444C File Offset: 0x0001264C
		public override void tickUpdate(GameTime time, Farmer who)
		{
			if (!this.isHeld || !who.hasBuff("NatCSpeedN"))
			{
				if (this.chargeSound != null && this.chargeSound.IsPlaying)
				{
					this.chargeSound.Stop(AudioStopOptions.Immediate);
				}
				who.stopJittering();
				return;
			}
			double num = (double)this.castingPower;
			double num2 = (double)this.castingTimerSpeed;
			double num3 = (double)time.ElapsedGameTime.Milliseconds;
			double num4 = num2 * num3;
			this.castingPower = Math.Max(0f, Math.Min(1f, (float)(num + num4)));
			if (this.PlayUseSounds && who.IsLocalPlayer && this.config.includeChargeSound)
			{
				if (this.chargeSound == null || !this.chargeSound.IsPlaying)
				{
					this.castingPower = 0.1f;
					Game1.playSound("SinWave", out this.chargeSound);
					this.chargeSound.Volume = this.chargeSound.Volume * 3f / 4f;
				}
				Game1.sounds.SetPitch(this.chargeSound, 2400f * this.castingPower, true);
			}
			if (this.castingPower == 1f || this.castingPower == 0f)
			{
				this.castingTimerSpeed = -this.castingTimerSpeed;
			}
			who.jitterStrength = 0.5f;
			switch (who.FacingDirection)
			{
			case 0:
				who.FarmerSprite.setCurrentSingleFrame(36, 32000, false, false);
				return;
			case 1:
				who.FarmerSprite.setCurrentSingleFrame(48, 100, false, false);
				return;
			case 2:
				who.FarmerSprite.setCurrentSingleFrame(66, 32000, false, false);
				return;
			case 3:
				who.FarmerSprite.setCurrentSingleFrame(48, 100, false, true);
				return;
			default:
				return;
			}
		}

		// Token: 0x0600018D RID: 397 RVA: 0x00002D9D File Offset: 0x00000F9D
		private void EndOfUse(Farmer who)
		{
			who.canMove = true;
			this.isHeld = false;
			who.jitterStrength = 0f;
			NatureInTheValleyEntry.TryCatch(who);
			if (who.hasBuff("NatCSpeedN"))
			{
				who.buffs.Remove("NatCSpeedN");
			}
		}

		// Token: 0x0600018E RID: 398 RVA: 0x0000268F File Offset: 0x0000088F
		public override bool canBeTrashed()
		{
			return true;
		}

		// Token: 0x0600018F RID: 399 RVA: 0x0000268F File Offset: 0x0000088F
		public override bool canBeDropped()
		{
			return true;
		}

		// Token: 0x06000190 RID: 400 RVA: 0x00014608 File Offset: 0x00012808
		public NatInValeySaphNet()
		{
			base.ItemId = "NIVSaphNet";
			this.helper = NatureInTheValleyEntry.staticHelper;
			this.config = this.helper.ReadConfig<NatInValleyConfig>();
			NatInValeySaphNet.texture = this.helper.ModContent.Load<Texture2D>("PNGs\\SaphireNet");
			base.Category = -99;
			this.Name = "NatValleyNet";
			this.Stack = 1;
		}

		// Token: 0x040000C6 RID: 198
		public static Texture2D texture;

		// Token: 0x040000C7 RID: 199
		private IModHelper helper;

		// Token: 0x040000C8 RID: 200
		public bool isHeld;

		// Token: 0x040000C9 RID: 201
		private int save;

		// Token: 0x040000CA RID: 202
		public int variant;

		// Token: 0x040000CB RID: 203
		[XmlIgnore]
		public ICue chargeSound;

		// Token: 0x040000CC RID: 204
		private float castingPower;

		// Token: 0x040000CD RID: 205
		private float castingTimerSpeed = 0.0006f;

		// Token: 0x040000CE RID: 206
		[XmlIgnore]
		private NatInValleyConfig config;
	}
}
