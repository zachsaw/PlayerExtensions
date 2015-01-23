﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Mpdn.PlayerExtensions.Example
{
    public class HelloWorld : IPlayerExtension
    {
        public ExtensionDescriptor Descriptor
        {
            get
            {
                return new ExtensionDescriptor
                {
                    Guid = new Guid("9714174F-B64D-43D8-BB16-52C5FEE2417B"),
                    Name = "Hello World",
                    Description = "Player Extension Example",
                    Copyright = "Copyright Example © 2014-2015. All rights reserved."
                };
            }
        }

        public void Initialize()
        {
            PlayerControl.KeyDown += PlayerKeyDown;
        }

        public void Destroy()
        {
            PlayerControl.KeyDown -= PlayerKeyDown;
        }

        public IList<Verb> Verbs
        {
            get
            {
                return new[]
                {
                    new Verb(Category.Help, string.Empty, "Hello World", "Ctrl+Shift+H", "Say hello world", HelloWorldClick),
                    new Verb(Category.Help, "My subcategory", "Another Hello World", "Ctrl+Shift+Y", "Say hello world too", HelloWorld2Click)
                };
            }
        }

        public bool ShowConfigDialog(IWin32Window owner)
        {
            return false;
        }

        private void HelloWorldClick()
        {
            MessageBox.Show(PlayerControl.VideoPanel, "Hello World!");
        }

        private void HelloWorld2Click()
        {
            MessageBox.Show(PlayerControl.VideoPanel, "Hello World Too!");
        }

        private void PlayerKeyDown(object sender, PlayerControlEventArgs<KeyEventArgs> e)
        {
            switch (e.InputArgs.KeyData)
            {
                case Keys.Control | Keys.Shift | Keys.H:
                    HelloWorldClick();
                    break;
                case Keys.Control | Keys.Shift | Keys.Y:
                    HelloWorld2Click();
                    break;
            }
        }
    }
}
