﻿using System;
using System.Collections.Generic;
using Mpdn.RenderScript;
using YAXLib;

namespace Mpdn.PlayerExtensions.GitHub
{
    public class RenderScriptPreset
    {
        [YAXAttributeForClass]
        public string Name { get; set; }

        [YAXAttributeForClass]
        public Guid Guid { get; set; }

        public IRenderScriptUi Script { get; set; }

        public RenderScriptPreset()
        {
            Guid = Guid.NewGuid();
        }
    }

    public class PresetSettings
    {
        public List<RenderScriptPreset> PresetList { get; set; }

        public RenderScriptPreset ActivePreset { get; set; }

        public PresetSettings()
        {
            PresetList = new List<RenderScriptPreset>();
            ActivePreset = new RenderScriptPreset();
        }
    }

    public class PresetExtension : ConfigurablePlayerExtension<PresetSettings, PresetDialog>
    {
        protected new static PresetSettings Config = new PresetSettings();
        public static List<RenderScriptPreset> PresetList { get { return Config.PresetList; } }
        public static RenderScriptPreset ActivePreset 
        {
            get { return Config.ActivePreset; }
            set { Config.ActivePreset = value; }
        }

        public static Guid ScriptGuid;

        public static RenderScriptPreset LoadPreset(Guid guid)
        {
            return PresetList.Find(x => x.Guid == guid);
        }

        public override IList<Verb> Verbs
        {
            get { return new Verb[] {}; }
        }

        public override void Initialize()
        {
            base.Initialize();
            Config = ScriptConfig.Config;

            ScriptConfig.Config.ActivePreset = LoadPreset(ScriptConfig.Config.ActivePreset.Guid) ?? ScriptConfig.Config.ActivePreset;
        }

        public override bool ShowConfigDialog(System.Windows.Forms.IWin32Window owner)
        {
            var result = base.ShowConfigDialog(owner);
            if (result) OnPresetChanged();
            return result;
        }

        private void OnPresetChanged()
        {
            if (ScriptGuid != Guid.Empty)
                PlayerControl.SetRenderScript(ScriptGuid);
        }

        protected override string ConfigFileName
        {
            get { return "Mpdn.Presets"; }
        }

        protected override PlayerExtensionDescriptor ScriptDescriptor
        {
            get
            {
                return new PlayerExtensionDescriptor
                {
                    Guid = new Guid("26B49403-28D3-4C75-88C0-AB5372796CCC"),
                    Name = "RenderScript presets",
                    Description = "Extends renderscript funciontality with presets"
                };
            }
        }
    }
}