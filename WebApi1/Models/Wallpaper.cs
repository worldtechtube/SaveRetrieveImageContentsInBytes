using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi1.Models
{
    public class Wallpaper
    {
        public int id { get; set; }
        public byte[] wallpaperContent { get; set; }
    }
}