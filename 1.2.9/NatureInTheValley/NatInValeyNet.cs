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
	// Token: 0x02000008 RID: 8
	[XmlType("Mods_NatInValeyNet")]
	[XmlRoot(ElementName = "NatInValeyNet", Namespace = "")]
	[KnownType(typeof(NatInValeyNet))]
	[XmlInclude(typeof(NatInValeyNet))]
	public class NatInValeyNet : Tool
	{
		// Token: 0x0600009A RID: 154 RVA: 0x0000260F File Offset: 0x0000080F
		protected override Item GetOneNew()
		{
			return new NatInValeyNet();
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00002616 File Offset: 0x00000816
		protected override string loadDisplayName()
		{
			return this.helper.Translation.Get("NetName");
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00002632 File Offset: 0x00000832
		public override string getDescription()
		{
			return this.helper.Translation.Get("NetDescript");
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00010370 File Offset: 0x0000E570
		public override void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, StackDrawType drawStackNumber, Color color, bool drawShadow)
		{
			spriteBatch.Draw(NatInValeyNet.texture, location + new Vector2(32f, 32f), new Rectangle?(new Rectangle(0, 0, 16, 16)), Color.White * transparency, 0f, new Vector2(8f, 8f), (float)(4.0 * (double)scaleSize), SpriteEffects.None, layerDepth);
		}

		// Token: 0x0600009E RID: 158 RVA: 0x000103E0 File Offset: 0x0000E5E0
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

		// Token: 0x0600009F RID: 159 RVA: 0x00010454 File Offset: 0x0000E654
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

		// Token: 0x060000A0 RID: 160 RVA: 0x0000264E File Offset: 0x0000084E
		public virtual int salePrice()
		{
			return -1;
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x000104F8 File Offset: 0x0000E6F8
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

		// Token: 0x060000A2 RID: 162 RVA: 0x00010674 File Offset: 0x0000E874
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

		// Token: 0x060000A3 RID: 163 RVA: 0x00002651 File Offset: 0x00000851
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

		// Token: 0x060000A4 RID: 164 RVA: 0x0000268F File Offset: 0x0000088F
		public override bool canBeTrashed()
		{
			return true;
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x0000268F File Offset: 0x0000088F
		public override bool canBeDropped()
		{
			return true;
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00010830 File Offset: 0x0000EA30
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

		// Token: 0x04000052 RID: 82
		public static Texture2D texture;

		// Token: 0x04000053 RID: 83
		private IModHelper helper;

		// Token: 0x04000054 RID: 84
		public bool isHeld;

		// Token: 0x04000055 RID: 85
		private int save;

		// Token: 0x04000056 RID: 86
		public int variant;

		// Token: 0x04000057 RID: 87
		[XmlIgnore]
		public ICue chargeSound;

		// Token: 0x04000058 RID: 88
		private float castingPower;

		// Token: 0x04000059 RID: 89
		private float castingTimerSpeed = 0.0006f;

		// Token: 0x0400005A RID: 90
		[XmlIgnore]
		private NatInValleyConfig config;
	}
}
