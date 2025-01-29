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
	// Token: 0x02000007 RID: 7
	[XmlType("Mods_NatInValeyNet")]
	[XmlRoot(ElementName = "natInValeyNet", Namespace = "")]
	[KnownType(typeof(NatInValeyNet))]
	[XmlInclude(typeof(NatInValeyNet))]
	public class NatInValeyNet : Tool
	{
		// Token: 0x06000077 RID: 119 RVA: 0x0000248C File Offset: 0x0000068C
		protected override Item GetOneNew()
		{
			return new NatInValeyNet();
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00002493 File Offset: 0x00000693
		protected override string loadDisplayName()
		{
			return this.helper.Translation.Get("NetName");
		}

		// Token: 0x06000079 RID: 121 RVA: 0x000024AF File Offset: 0x000006AF
		public override string getDescription()
		{
			return this.helper.Translation.Get("NetDescript");
		}

		// Token: 0x0600007A RID: 122 RVA: 0x0000A7C8 File Offset: 0x000089C8
		public override void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, StackDrawType drawStackNumber, Color color, bool drawShadow)
		{
			spriteBatch.Draw(NatInValeyNet.texture, location + new Vector2(32f, 32f), new Rectangle?(new Rectangle(0, 0, 16, 16)), Color.White * transparency, 0f, new Vector2(8f, 8f), (float)(4.0 * (double)scaleSize), SpriteEffects.None, layerDepth);
		}

		// Token: 0x0600007B RID: 123 RVA: 0x0000A838 File Offset: 0x00008A38
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

		// Token: 0x0600007C RID: 124 RVA: 0x0000A8AC File Offset: 0x00008AAC
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

		// Token: 0x0600007D RID: 125 RVA: 0x000024CB File Offset: 0x000006CB
		public virtual int salePrice()
		{
			return -1;
		}

		// Token: 0x0600007E RID: 126 RVA: 0x0000A950 File Offset: 0x00008B50
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

		// Token: 0x0600007F RID: 127 RVA: 0x0000AADC File Offset: 0x00008CDC
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

		// Token: 0x06000080 RID: 128 RVA: 0x000024CE File Offset: 0x000006CE
		private void EndOfUse(Farmer who)
		{
			who.canMove = true;
			this.isHeld = false;
			who.jitterStrength = 0f;
			NatureInTheValleyEntry.TryCatch(who);
		}

		// Token: 0x06000081 RID: 129 RVA: 0x000024EF File Offset: 0x000006EF
		public override bool canBeTrashed()
		{
			return true;
		}

		// Token: 0x06000082 RID: 130 RVA: 0x000024EF File Offset: 0x000006EF
		public override bool canBeDropped()
		{
			return true;
		}

		// Token: 0x06000083 RID: 131 RVA: 0x0000AC98 File Offset: 0x00008E98
		public NatInValeyNet()
		{
			base.ItemId = "NIVNet";
			this.helper = NatureInTheValleyEntry.staticHelper;
			this.config = this.helper.ReadConfig<NatInValleyConfig>();
			NatInValeyNet.texture = this.helper.ModContent.Load<Texture2D>("PNGs\\NormalNet");
			base.Category = -99;
			this.Name = "NatValleyNet";
			this.Stack = 1;
		}

		// Token: 0x0400004B RID: 75
		public static Texture2D texture;

		// Token: 0x0400004C RID: 76
		private IModHelper helper;

		// Token: 0x0400004D RID: 77
		public bool isHeld;

		// Token: 0x0400004E RID: 78
		private int save;

		// Token: 0x0400004F RID: 79
		public int variant;

		// Token: 0x04000050 RID: 80
		[XmlIgnore]
		public ICue chargeSound;

		// Token: 0x04000051 RID: 81
		private float castingPower;

		// Token: 0x04000052 RID: 82
		private float castingTimerSpeed = 0.0006f;

		// Token: 0x04000053 RID: 83
		[XmlIgnore]
		private NatInValleyConfig config;
	}
}
