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
	// Token: 0x0200000E RID: 14
	[XmlType("Mods_NatInValeyGoldenNet")]
	[XmlRoot(ElementName = "natInValeyGoldenNet", Namespace = "")]
	[KnownType(typeof(NatInValeyGoldenNet))]
	[XmlInclude(typeof(NatInValeyGoldenNet))]
	public class NatInValeyGoldenNet : Tool
	{
		// Token: 0x060000AE RID: 174 RVA: 0x000025A2 File Offset: 0x000007A2
		protected override Item GetOneNew()
		{
			return new NatInValeyGoldenNet();
		}

		// Token: 0x060000AF RID: 175 RVA: 0x000025A9 File Offset: 0x000007A9
		protected override string loadDisplayName()
		{
			return this.helper.Translation.Get("NetName");
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x000025C5 File Offset: 0x000007C5
		public override string getDescription()
		{
			return this.helper.Translation.Get("NetDescript");
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x0000CBA8 File Offset: 0x0000ADA8
		public override void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, StackDrawType drawStackNumber, Color color, bool drawShadow)
		{
			spriteBatch.Draw(NatInValeyGoldenNet.texture, location + new Vector2(32f, 32f), new Rectangle?(new Rectangle(0, 0, 16, 16)), Color.White * transparency, 0f, new Vector2(8f, 8f), (float)(4.0 * (double)scaleSize), SpriteEffects.None, layerDepth);
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x0000CC18 File Offset: 0x0000AE18
		public override void actionWhenStopBeingHeld(Farmer who)
		{
			base.actionWhenStopBeingHeld(who);
			who.canMove = true;
			NatureInTheValleyEntry.AnimatedBump.Value = 0;
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

		// Token: 0x060000B3 RID: 179 RVA: 0x0000CC8C File Offset: 0x0000AE8C
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
							Value = -((float)who.Speed + who.buffs.Speed - 1.5f)
						}
					},
					millisecondsDuration = 20000
				});
			}
			return false;
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00002515 File Offset: 0x00000715
		public virtual int salePrice()
		{
			return -1;
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x0000CD30 File Offset: 0x0000AF30
		public override bool onRelease(GameLocation location, int x, int y, Farmer who)
		{
			who.stopJittering();
			this.isHeld = false;
			who.canMove = false;
			who.jitterStrength = 0f;
			who.UsingTool = false;
			if (who.hasBuff("NatCSpeedN"))
			{
				ICue cue;
				Game1.playSound("NIVNetSwing", out cue);
				cue.Volume = cue.Volume * 2.5f / 3f;
				who.buffs.Remove("NatCSpeedN");
			}
			if (this.chargeSound != null)
			{
				this.chargeSound.Stop(AudioStopOptions.Immediate);
			}
			who.canReleaseTool = false;
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
			return false;
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x0000CEBC File Offset: 0x0000B0BC
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

		// Token: 0x060000B7 RID: 183 RVA: 0x000025E1 File Offset: 0x000007E1
		private void EndOfUse(Farmer who)
		{
			who.canMove = true;
			this.isHeld = false;
			who.jitterStrength = 0f;
			NatureInTheValleyEntry.TryCatch(who);
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00002539 File Offset: 0x00000739
		public override bool canBeTrashed()
		{
			return true;
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00002539 File Offset: 0x00000739
		public override bool canBeDropped()
		{
			return true;
		}

		// Token: 0x060000BA RID: 186 RVA: 0x0000D078 File Offset: 0x0000B278
		public NatInValeyGoldenNet()
		{
			base.ItemId = "NIVGoldNet";
			this.helper = NatureInTheValleyEntry.staticHelper;
			this.config = this.helper.ReadConfig<NatInValleyConfig>();
			NatInValeyGoldenNet.texture = this.helper.ModContent.Load<Texture2D>("PNGs\\GoldenNet");
			base.Category = -99;
			this.Name = "NatValleyGoldNet";
			this.Stack = 1;
		}

		// Token: 0x04000067 RID: 103
		public static Texture2D texture;

		// Token: 0x04000068 RID: 104
		private IModHelper helper;

		// Token: 0x04000069 RID: 105
		public bool isHeld;

		// Token: 0x0400006A RID: 106
		private int save;

		// Token: 0x0400006B RID: 107
		public int variant;

		// Token: 0x0400006C RID: 108
		[XmlIgnore]
		public ICue chargeSound;

		// Token: 0x0400006D RID: 109
		private float castingPower;

		// Token: 0x0400006E RID: 110
		private float castingTimerSpeed = 0.0006f;

		// Token: 0x0400006F RID: 111
		[XmlIgnore]
		private NatInValleyConfig config;
	}
}
