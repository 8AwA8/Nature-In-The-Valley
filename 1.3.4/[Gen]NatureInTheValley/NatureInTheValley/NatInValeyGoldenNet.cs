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
	// Token: 0x0200000F RID: 15
	[XmlType("Mods_NatInValeyGoldenNet")]
	[XmlRoot(ElementName = "natInValeyGoldenNet", Namespace = "")]
	[KnownType(typeof(NatInValeyGoldenNet))]
	[XmlInclude(typeof(NatInValeyGoldenNet))]
	public class NatInValeyGoldenNet : Tool
	{
		// Token: 0x060000DA RID: 218 RVA: 0x0000278B File Offset: 0x0000098B
		protected override Item GetOneNew()
		{
			return new NatInValeyGoldenNet();
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00002792 File Offset: 0x00000992
		protected override string loadDisplayName()
		{
			return this.helper.Translation.Get("NetName");
		}

		// Token: 0x060000DC RID: 220 RVA: 0x000027AE File Offset: 0x000009AE
		public override string getDescription()
		{
			return this.helper.Translation.Get("NetDescript");
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00012D64 File Offset: 0x00010F64
		public override void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, StackDrawType drawStackNumber, Color color, bool drawShadow)
		{
			spriteBatch.Draw(NatInValeyGoldenNet.texture, location + new Vector2(32f, 32f), new Rectangle?(new Rectangle(0, 0, 16, 16)), Color.White * transparency, 0f, new Vector2(8f, 8f), (float)(4.0 * (double)scaleSize), SpriteEffects.None, layerDepth);
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00012DD4 File Offset: 0x00010FD4
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

		// Token: 0x060000DF RID: 223 RVA: 0x00012E48 File Offset: 0x00011048
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

		// Token: 0x060000E0 RID: 224 RVA: 0x000026FF File Offset: 0x000008FF
		public virtual int salePrice()
		{
			return -1;
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00012EEC File Offset: 0x000110EC
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

		// Token: 0x060000E2 RID: 226 RVA: 0x00013068 File Offset: 0x00011268
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

		// Token: 0x060000E3 RID: 227 RVA: 0x000027CA File Offset: 0x000009CA
		private void EndOfUse(Farmer who)
		{
			who.canMove = true;
			this.isHeld = false;
			who.jitterStrength = 0f;
			if (who.hasBuff("NatCSpeedN"))
			{
				who.buffs.Remove("NatCSpeedN");
			}
			NatureInTheValleyEntry.TryCatch(who);
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00002740 File Offset: 0x00000940
		public override bool canBeTrashed()
		{
			return true;
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00002740 File Offset: 0x00000940
		public override bool canBeDropped()
		{
			return true;
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00013224 File Offset: 0x00011424
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

		// Token: 0x04000078 RID: 120
		public static Texture2D texture;

		// Token: 0x04000079 RID: 121
		private IModHelper helper;

		// Token: 0x0400007A RID: 122
		public bool isHeld;

		// Token: 0x0400007B RID: 123
		private int save;

		// Token: 0x0400007C RID: 124
		public int variant;

		// Token: 0x0400007D RID: 125
		[XmlIgnore]
		public ICue chargeSound;

		// Token: 0x0400007E RID: 126
		private float castingPower;

		// Token: 0x0400007F RID: 127
		private float castingTimerSpeed = 0.0006f;

		// Token: 0x04000080 RID: 128
		[XmlIgnore]
		private NatInValleyConfig config;
	}
}
