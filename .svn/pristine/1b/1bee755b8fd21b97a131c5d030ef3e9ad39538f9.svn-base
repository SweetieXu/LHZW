using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.IO;

namespace Asiatek.Common
{
    /// <summary>
    /// 验证码
    /// 作者：戴天辰
    /// </summary>
    public class ValidateCode
    {
        #region 属性和字段
        private int width = 60;

        public int Width
        {
            get { return width; }
            set { width = value; }
        }
        private int height = 30;

        public int Height
        {
            get { return height; }
            set { height = value; }
        }
        private int size = 4;

        public int Size
        {
            get { return size; }
            set { size = value; }
        }

        private int fontSize = 12;

        public int FontSize
        {
            get { return fontSize; }
            set { fontSize = value; }
        }

        private int lineNum = 5;

        public int LineNum
        {
            get { return lineNum; }
            set { lineNum = value; }
        }

        private List<string> codes = new List<string>()
        {
            "a","b","c","d","e","f","g","h","j","k","m","n","p","r","s","t","u","v","w","x","y",
            "A","B","C","D","E","F","G","H","J","K","L","M","N","P","R","S","T","U","V","W","X","Y"
        };

        public List<string> Codes
        {
            get { return codes; }
            set { codes = value; }
        }

        private List<Color> colors = new List<Color>()
        {
            Color.DarkRed,Color.DarkBlue,Color.DarkGreen,Color.DarkGoldenrod,Color.DarkGray
        };

        public List<Color> Colors
        {
            get { return colors; }
            set { colors = value; }
        }

        private List<Font> fonts = new List<Font>()
        {
            new Font("宋体",16,FontStyle.Bold),new Font("黑体",16,FontStyle.Bold),new Font("微软雅黑",16,FontStyle.Bold),new Font("幼圆",16,FontStyle.Bold)
        };

        public List<Font> Fonts
        {
            get { return fonts; }
            set { fonts = value; }
        }
        #endregion

        #region 生成验证码
        public byte[] CreateValidateCode(out string code)
        {
            #region 绘制验证码
            Random rnd = new Random();
            Bitmap bmp = new Bitmap(width, height);
            Graphics gs = Graphics.FromImage(bmp);
            gs.FillRectangle(new SolidBrush(Color.White), 0, 0, width, height);
            code = "";
            for (int i = 0; i < size; i++)
            {
                code += codes[rnd.Next(0, codes.Count)];
            }
            for (int i = 0; i < code.Length; i++)
            {
                gs.DrawString(code[i].ToString(), fonts[rnd.Next(0, fonts.Count)], new SolidBrush(colors[rnd.Next(0, colors.Count)]), i * fontSize + 2, 5);
            }
            for (int i = 0; i < lineNum; i++)
            {
                gs.DrawLine(new Pen(new SolidBrush(colors[rnd.Next(0, colors.Count)])), new Point(rnd.Next(0, width), rnd.Next(0, height)), new Point(rnd.Next(0, width), rnd.Next(0, height)));
            }
            gs.Dispose();
            #endregion

            #region 将验证码保存到内存
            MemoryStream ms = new MemoryStream();
            bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            bmp.Dispose();
            #endregion

            return ms.ToArray();
        }
        #endregion


    }
}