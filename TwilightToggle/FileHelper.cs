﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace TwilightToggle
{
    public class FileHelper
    {
        public String readFileReturnString(String fileLocation)
        {
            String readText = File.ReadAllText(fileLocation);
            Console.WriteLine(readText);
            return readText;
        }
    }
}