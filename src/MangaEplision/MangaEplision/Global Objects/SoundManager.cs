using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Media;

namespace MangaEplision.GlobalObjs
{
    public static class SoundManager
    {
        public static void Initialize()
        {
            string windir = Environment.GetFolderPath(Environment.SpecialFolder.Windows);

            WindowsNotify = new SoundPlayer(windir + "\\Media\\Windows Notify.wav");
            WindowsNotify.Load();

            WindowsBalloon = new SoundPlayer(windir + "\\Media\\Windows Balloon.wav");
            WindowsBalloon.Load();


        }
        public static dynamic WindowsNotify { get; private set; }
        public static dynamic WindowsBalloon { get; private set; }
    }
}
