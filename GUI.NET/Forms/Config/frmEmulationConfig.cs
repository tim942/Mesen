﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mesen.GUI.Config;
using Mesen.GUI.GoogleDriveIntegration;

namespace Mesen.GUI.Forms.Config
{
	public partial class frmEmulationConfig : BaseConfigForm
	{
		public frmEmulationConfig()
		{
			InitializeComponent();

			Entity = ConfigManager.Config.EmulationInfo;

			AddBinding("EmulationSpeed", nudEmulationSpeed);

			AddBinding("UseAlternativeMmc3Irq", chkUseAlternativeMmc3Irq);
			AddBinding("AllowInvalidInput", chkAllowInvalidInput);
			AddBinding("RemoveSpriteLimit", chkRemoveSpriteLimit);

			AddBinding("OverclockRate", nudOverclockRate);
			AddBinding("OverclockAdjustApu", chkOverclockAdjustApu);

			AddBinding("PpuExtraScanlinesBeforeNmi", nudExtraScanlinesBeforeNmi);
			AddBinding("PpuExtraScanlinesAfterNmi", nudExtraScanlinesAfterNmi);
		}

		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			base.OnFormClosed(e);
			EmulationInfo.ApplyConfig();
		}

		private void tmrUpdateClockRate_Tick(object sender, EventArgs e)
		{
			decimal clockRateMultiplierNtsc = (nudOverclockRate.Value * (1 + (nudExtraScanlinesAfterNmi.Value + nudExtraScanlinesBeforeNmi.Value) / 262));
			decimal clockRateMultiplierPal = (nudOverclockRate.Value * (1 + (nudExtraScanlinesAfterNmi.Value + nudExtraScanlinesBeforeNmi.Value) / 312));
			lblEffectiveClockRateValue.Text = (1789773 * clockRateMultiplierNtsc / 100000000).ToString("#.####") + " mhz (" + ((int)clockRateMultiplierNtsc).ToString() + "%)";
			lblEffectiveClockRateValuePal.Text = (1662607 * clockRateMultiplierPal / 100000000).ToString("#.####") + " mhz (" + ((int)clockRateMultiplierPal).ToString() + "%)";
		}

		private void OverclockConfig_Validated(object sender, EventArgs e)
		{
			if(string.IsNullOrWhiteSpace(nudExtraScanlinesAfterNmi.Text)) {
				nudExtraScanlinesAfterNmi.Value = 0;
			}
			if(string.IsNullOrWhiteSpace(nudExtraScanlinesBeforeNmi.Text)) {
				nudExtraScanlinesBeforeNmi.Value = 0;
			}
			if(string.IsNullOrWhiteSpace(nudOverclockRate.Text)) {
				nudOverclockRate.Value = 0;
			}
		}
	}
}
