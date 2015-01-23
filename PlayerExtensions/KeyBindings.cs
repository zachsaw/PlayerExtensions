﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Mpdn.PlayerExtensions.GitHub
{
    public class KeyBindings : PlayerExtension
    {
        public override ExtensionDescriptor Descriptor
        {
            get
            {
                return new ExtensionDescriptor
                {
                    Guid = new Guid("E3E54699-0B2B-4B1B-8F6B-4739273670CD"),
                    Name = "Key Bindings",
                    Description = "Extra shortcut key bindings"
                };
            }
        }

        public override void Initialize()
        {
            base.Initialize();
            PlayerControl.KeyDown += PlayerKeyDown;
        }

        public override void Destroy()
        {
            PlayerControl.KeyDown -= PlayerKeyDown;
            base.Destroy();
        }

        public override IList<Verb> Verbs
        {
            get { return new Verb[0]; }
        }

        public override bool ShowConfigDialog(IWin32Window owner)
        {
            return false;
        }

        private void PlayerKeyDown(object sender, PlayerControlEventArgs<KeyEventArgs> e)
        {
            switch (e.InputArgs.KeyData)
            {
                case Keys.Control | Keys.Enter:
                    e.OutputArgs = new KeyEventArgs(Keys.Alt | Keys.Enter);
                    break;
                case Keys.Escape:
                    if (PlayerControl.InFullScreenMode)
                    {
                        PlayerControl.GoWindowed();
                    }
                    break;
                case Keys.F11:
                    ToggleMode();
                    break;
                case Keys.Shift | Keys.PageDown:
                    SelectAudioTrack(true);
                    break;
                case Keys.Shift | Keys.PageUp:
                    SelectAudioTrack(false);
                    break;
                case Keys.Alt | Keys.Shift | Keys.PageDown:
                    SelectSubtitleTrack(true);
                    break;
                case Keys.Alt | Keys.Shift | Keys.PageUp:
                    SelectSubtitleTrack(false);
                    break;
            }
        }

        private void SelectAudioTrack(bool next)
        {
            if (PlayerControl.PlayerState == PlayerState.Closed)
                return;

            var activeTrack = PlayerControl.ActiveAudioTrack;
            if (activeTrack == null)
                return;

            var tracks = PlayerControl.AudioTracks;
            var audioTrack = next
                ? tracks.SkipWhile(track => !track.Equals(activeTrack)).Skip(1).FirstOrDefault()
                : tracks.TakeWhile(track => !track.Equals(activeTrack)).LastOrDefault();
            if (audioTrack != null)
            {
                PlayerControl.SelectAudioTrack(audioTrack);
            }
        }

        private void SelectSubtitleTrack(bool next)
        {
            if (PlayerControl.PlayerState == PlayerState.Closed)
                return;

            var activeTrack = PlayerControl.ActiveSubtitleTrack;
            if (activeTrack == null)
                return;

            var tracks = PlayerControl.SubtitleTracks;
            var subtitleTrack = next
                ? tracks.SkipWhile(track => !track.Equals(activeTrack)).Skip(1).FirstOrDefault()
                : tracks.TakeWhile(track => !track.Equals(activeTrack)).LastOrDefault();
            if (subtitleTrack != null)
            {
                PlayerControl.SelectSubtitleTrack(subtitleTrack);
            }
        }

        private void ToggleMode()
        {
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
}
