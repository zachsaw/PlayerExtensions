﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Mpdn.PlayerExtensions.GitHub
{
    public class MouseControl : ConfigurablePlayerExtension<MouseControlSettings, MouseControlConfigDialog>
    {
        protected override PlayerExtensionDescriptor ScriptDescriptor
        {
            get
            {
                return new PlayerExtensionDescriptor
                {
                    Guid = new Guid("DCF7797B-9D36-41F3-B28C-0A92793B94F5"),
                    Name = "Mouse Control",
                    Description =
                        string.Format("Use mouse {0}forward/back buttons to navigate chapters/playlist/folder",
                            Settings.EnableMouseWheelSeek ? "wheel to seek and " : string.Empty)
                };
            }
        }

        protected override string ConfigFileName
        {
            get { return "Example.MouseControl"; }
        }

        public override void Initialize()
        {
            base.Initialize();

            PlayerControl.MouseClick += PlayerMouseClick;
            PlayerControl.MouseWheel += PlayerMouseWheel;
        }

        public override void Destroy()
        {
            base.Destroy();

            PlayerControl.MouseClick -= PlayerMouseClick;
            PlayerControl.MouseWheel -= PlayerMouseWheel;
        }

        public override IList<Verb> Verbs
        {
            get { return new Verb[0]; }
        }

        private void PlayerMouseWheel(object sender, PlayerControlEventArgs<MouseEventArgs> e)
        {
            if (!Settings.EnableMouseWheelSeek)
                return;

            var pos = PlayerControl.MediaPosition;
            pos += e.InputArgs.Delta*1000000/40;
            pos = Math.Max(pos, 0);
            PlayerControl.SeekMedia(pos);
            e.Handled = true;
        }

        private void PlayerMouseClick(object sender, PlayerControlEventArgs<MouseEventArgs> e)
        {
            if (e.InputArgs.Button == MouseButtons.Middle)
            {
                ToggleMode();
                return;
            }

            if (PlayerControl.PlayerState == PlayerState.Closed)
                return;

            var chapters = PlayerControl.Chapters.OrderBy(chapter => chapter.Position);

            var pos = PlayerControl.MediaPosition;

            bool next;
            switch (e.InputArgs.Button)
            {
                case MouseButtons.XButton2:
                    next = true;
                    break;
                case MouseButtons.XButton1:
                    next = false;
                    break;
                default:
                    return;
            }

            var nextChapter = next
                ? chapters.SkipWhile(chapter => chapter.Position < pos).FirstOrDefault()
                : chapters.TakeWhile(chapter => chapter.Position < Math.Max(pos - 1000000, 0)).LastOrDefault();
            if (nextChapter != null)
            {
                PlayerControl.SeekMedia(nextChapter.Position);
                return;
            }

            if (PlaylistForm.PlaylistCount <= 1)
            {
                switch (e.InputArgs.Button)
                {
                    case MouseButtons.XButton2:
                        SendKeys.Send("^{PGDN}");
                        break;
                    case MouseButtons.XButton1:
                        SendKeys.Send("^{PGUP}");
                        break;
                }
            }
            else
            {
                switch (e.InputArgs.Button)
                {
                    case MouseButtons.XButton2:
                        SendKeys.Send("^%n");
                        break;
                    case MouseButtons.XButton1:
                        SendKeys.Send("^%b");
                        break;
                }
            }
        }

        private void ToggleMode()
        {
            if (!Settings.EnableMiddleClickFsToggle)
                return;

            if (PlayerControl.InFullScreenMode)
            {
                PlayerControl.GoWindowed();
            }
            else
            {
                PlayerControl.GoFullScreen();
            }
        }
    }

    public class MouseControlSettings
    {
        public MouseControlSettings()
        {
            EnableMouseWheelSeek = false;
            EnableMiddleClickFsToggle = false;
        }

        public bool EnableMouseWheelSeek { get; set; }
        public bool EnableMiddleClickFsToggle { get; set; }
    }
}
