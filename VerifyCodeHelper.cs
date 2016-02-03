
/// <summary>
/// 自定义生成校验码图片
/// </summary>
public class VerifyCodeHelper
{

    #region 构造函数
    public VerifyCodeHelper()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
    }
    /// <summary>
    /// 指定图片高、宽的构造
    /// 高和宽同时大于零时才生效
    /// </summary>
    /// <param name="h"></param>
    /// <param name="w"></param>
    public VerifyCodeHelper(int h, int w)
    {
        if (h > 0 && w > 0)
        {
            _height = h;
            _width = w;
        }
    }
    /// <summary>
    /// 指定图片高、宽,字体大小的构造
    /// 高和宽同时大于零时才生效
    /// </summary>
    /// <param name="h"></param>
    /// <param name="w"></param>
    /// <param name="fontSize"></param>
    public VerifyCodeHelper(int h, int w, int fontSize)
    {
        if (h > 0 && w > 0)
        {
            _height = h;
            _width = w;
        }
        if (fontSize > 0)
        {
            _fontSize = fontSize;
        }
    }
    #endregion

    private const double Pi = Math.PI;
    private const double Pi2 = Math.PI * 2;

    #region 字段

    private string _code;

    private Bitmap _imgmap;

    private bool _isWave = true;

    int _length = 4;
    int _fontSize = 30;
    private int _height = 60;
    private int _width = 150;
    int _padding = 1;
    int _noise = 30;
    private int _noiseLine = 20;
    private int _noiseLeve = 2;

    Color _chaosColor = Color.LightGray;
    Color _backgroundColor = Color.White;
    Color[] _colors = { Color.Black, Color.Red, Color.DarkBlue, Color.Green, Color.DarkMagenta, Color.Brown, Color.DarkCyan, Color.Purple };

    string[] _fonts = { "Gautami", "Tahoma", "宋体" };
    string _codeSerial = "a,b,c,d,e,f,g,h,i,j,k,m,n,p,q,2,3,4,5,s,t,u,v,w,x,y,z,6,7,8,A,B,D,E,F,G,H,K,M,N,Q";

    #endregion

    #region 属性
    /// <summary>
    /// 获取生成的验证码字符串
    /// </summary>
    public string Code
    {
        get { return _code; }
    }
    /// <summary>
    /// 获取生成的验证码图片
    /// </summary>
    Bitmap VerifyCodeImg
    {
        get
        {
            if (_imgmap == null)
            {
                _code = CreateVerifyCode(this.Length); ;
                _imgmap = CreateImage(_code);
            }
            return _imgmap;
        }
    }
    /// <summary>
    /// 验证码长度(默认4个验证码的长度)
    /// </summary>
    public int Length
    {
        get { return _length; }
        set { _length = value; }
    }
    /// <summary>
    /// 生成的图片高度
    /// </summary>
    public int Heitht
    {
        get { return _height; }
        set { _height = value; }
    }
    /// <summary>
    /// 是否对图片进行扭曲
    /// </summary>
    public bool IsWave
    {
        get { return _isWave; }
        set { _isWave = value; }
    }
    /// <summary>
    /// 生成的图片宽度
    /// </summary>
    public int Width
    {
        get { return _width; }
        set { _width = value; }
    }
    /// <summary>
    /// 验证码字体大小默认30像素
    /// </summary>
    public int FontSize
    {
        get { return _fontSize; }
        set { _fontSize = value; }
    }
    /// <summary>
    /// 字符间距
    /// </summary>
    public int Padding
    {
        get { return _padding; }
        set { _padding = value; }
    }

    /// <summary>
    /// 燥音线数量
    /// </summary>
    public int NoiseLine
    {
        get { return _noiseLine; }
        set { _noiseLine = value; }
    }

    /// <summary>
    /// 燥音线干扰程度，数值越大，干扰越严重
    /// </summary>
    public int NoiseLeve
    {
        get { return _noiseLeve; }
        set
        {
            if (value <= 0) value = 1;
            _noiseLeve = value;
        }
    }

    /// <summary>
    /// 燥点数量、图片宽度的倍数
    /// </summary>
    public int Noise
    {
        get { return _noise; }
        set { _noise = value; }
    }
    /// <summary>
    /// 输出燥点的颜色(默认灰色)
    /// </summary>
    public Color ChaosColor
    {
        get { return _chaosColor; }
        set { _chaosColor = value; }
    }
    /// <summary>
    /// 自定义背景色(默认白色)
    /// </summary>
    public Color BackgroundColor
    {
        get { return _backgroundColor; }
        set { _backgroundColor = value; }
    }
    /// <summary>
    /// 自定义随机颜色数组
    /// </summary>
    public Color[] Colors
    {
        get { return _colors; }
        set { _colors = value; }
    }
    /// <summary>
    /// 自定义字体数组
    /// </summary>
    public string[] Fonts
    {
        get { return _fonts; }
        set { _fonts = value; }
    }
    /// <summary>
    /// 自定义随机码字符串序列(使用逗号分隔)
    /// </summary>
    public string CodeSerial
    {
        get { return _codeSerial; }
        set { _codeSerial = value; }
    }
    #endregion


    #region 生成随机字符码
    public string CreateVerifyCode(int codeLen)
    {
        if (codeLen == 0)
        {
            codeLen = Length;
        }
        string[] arr = CodeSerial.Split(',');
        string code = "";
        int temp = -1;
        Random rand = new Random();
        for (int i = 0; i < codeLen; i++)
        {
            if (temp != -1)
            {
                rand = new Random(temp * i * ((int)DateTime.Now.Ticks));
            }

            int t = rand.Next(arr.Length - 1);

            while (temp == t)
            {
                t = rand.Next(arr.Length - 1);
            }

            temp = t;
            code += arr[t];
        }

        return code;
    }

    #endregion

    #region 获得 BitmapData 指定坐标的颜色信息
    /// <summary>
    /// 获得 BitmapData 指定坐标的颜色信息
    /// </summary>
    /// <param name="srcData">从图像数据获得颜色 必须为 PixelFormat.Format24bppRgb 格式图像数据</param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns>x,y 坐标的颜色数据</returns>
    /// <remarks>
    /// Format24BppRgb 已知X，Y坐标，像素第一个元素的位置为Scan0+(Y*Stride)+(X*3)。
    /// 这是blue字节的位置，接下来的2个字节分别含有green、red数据。
    /// </remarks>
    private Color BitmapDataColorAt(BitmapData srcData, int x, int y)
    {
        byte[] rgbValues = new byte[3];
        Marshal.Copy((IntPtr)((long)srcData.Scan0 + ((y * srcData.Stride) + (x * 3))), rgbValues, 0, 3);
        return Color.FromArgb(rgbValues[2], rgbValues[1], rgbValues[0]);
    }
    #endregion

    #region 设置 BitmapData 指定坐标的颜色信息
    /// <summary>
    /// 设置 BitmapData 指定坐标的颜色信息
    /// </summary>
    /// <param name="destData">设置图像数据的颜色 必须为 PixelFormat.Format24bppRgb 格式图像数据</param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="color">待设置颜色</param>
    /// <remarks>
    /// Format24BppRgb 已知X，Y坐标，像素第一个元素的位置为Scan0+(Y*Stride)+(X*3)。
    /// 这是blue字节的位置，接下来的2个字节分别含有green、red数据。
    /// </remarks>
    private void BitmapDataColorSet(BitmapData destData, int x, int y, Color color)
    {
        byte[] rgbValues = new byte[3] { color.B, color.G, color.R };
        Marshal.Copy(rgbValues, 0, (IntPtr)((long)destData.Scan0 + ((y * destData.Stride) + (x * 3))), 3);
    }
    #endregion

    #region 波纹扭曲
    /// <summary>
    /// 波纹扭曲
    /// </summary>
    private Bitmap WaveDistortion(Bitmap bitmap)
    {
        Random rnd = new Random();

        var width = bitmap.Width;
        var height = bitmap.Height;

        Bitmap destBmp = new Bitmap(width, height, PixelFormat.Format24bppRgb);
        {
            Color foreColor = Color.FromArgb(rnd.Next(10, 100), rnd.Next(10, 100), rnd.Next(10, 100));
            Color backColor = Color.FromArgb(rnd.Next(200, 250), rnd.Next(200, 250), rnd.Next(200, 250));

            using (Graphics g = Graphics.FromImage(destBmp))
            {
                g.Clear(backColor);

                // periods 时间
                double rand1 = rnd.Next(710000, 1200000) / 10000000.0;
                double rand2 = rnd.Next(710000, 1200000) / 10000000.0;
                double rand3 = rnd.Next(710000, 1200000) / 10000000.0;
                double rand4 = rnd.Next(710000, 1200000) / 10000000.0;

                // phases  相位
                double rand5 = rnd.Next(0, 31415926) / 10000000.0;
                double rand6 = rnd.Next(0, 31415926) / 10000000.0;
                double rand7 = rnd.Next(0, 31415926) / 10000000.0;
                double rand8 = rnd.Next(0, 31415926) / 10000000.0;

                // amplitudes 振幅
                double rand9 = rnd.Next(330, 420) / 110.0;
                double rand10 = rnd.Next(330, 450) / 110.0;
                double amplitudesFactor = rnd.Next(5, 6) / 12.0;//振幅小点防止出界
                double center = width / 2.0;

                //wave distortion 波纹扭曲
                BitmapData destData = destBmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, destBmp.PixelFormat);
                BitmapData srcData = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, bitmap.PixelFormat);
                for (var x = 0; x < width; x++)
                {
                    for (var y = 0; y < height; y++)
                    {
                        var sx = x + (Math.Sin(x * rand1 + rand5)
                                    + Math.Sin(y * rand2 + rand6)) * rand9 - width / 2 + center + 1;
                        var sy = y + (Math.Sin(x * rand3 + rand7)
                                    + Math.Sin(y * rand4 + rand8)) * rand10 * amplitudesFactor;

                        int color, color_x, color_y, color_xy;
                        Color overColor = Color.Empty;

                        if (sx < 0 || sy < 0 || sx >= width - 1 || sy >= height - 1)
                        {
                            continue;
                        }
                        else
                        {
                            color = BitmapDataColorAt(srcData, (int)sx, (int)sy).B;
                            color_x = BitmapDataColorAt(srcData, (int)(sx + 1), (int)sy).B;
                            color_y = BitmapDataColorAt(srcData, (int)sx, (int)(sy + 1)).B;
                            color_xy = BitmapDataColorAt(srcData, (int)(sx + 1), (int)(sy + 1)).B;
                        }

                        if (color == 255 && color_x == 255 && color_y == 255 && color_xy == 255)
                        {
                            continue;
                        }
                        else if (color == 0 && color_x == 0 && color_y == 0 && color_xy == 0)
                        {
                            overColor = Color.FromArgb(foreColor.R, foreColor.G, foreColor.B);
                        }

                        else
                        {
                            double frsx = sx - Math.Floor(sx);
                            double frsy = sy - Math.Floor(sy);
                            double frsx1 = 1 - frsx;
                            double frsy1 = 1 - frsy;

                            double newColor =
                                 color * frsx1 * frsy1 +
                                 color_x * frsx * frsy1 +
                                 color_y * frsx1 * frsy +
                                 color_xy * frsx * frsy;

                            if (newColor > 255) newColor = 255;
                            newColor = newColor / 255;
                            double newcolor0 = 1 - newColor;

                            int newred = Math.Min((int)(newcolor0 * foreColor.R + newColor * backColor.R), 255);
                            int newgreen = Math.Min((int)(newcolor0 * foreColor.G + newColor * backColor.G), 255);
                            int newblue = Math.Min((int)(newcolor0 * foreColor.B + newColor * backColor.B), 255);
                            overColor = Color.FromArgb(newred, newgreen, newblue);
                        }
                        BitmapDataColorSet(destData, x, y, overColor);
                    }
                }
                destBmp.UnlockBits(destData);
                bitmap.UnlockBits(srcData);
            }
            if (bitmap != null)
                bitmap.Dispose();
        }
        return destBmp;
    }
    #endregion

    #region 产生波形滤镜效果
    /// <summary>
    /// 正弦曲线Wave扭曲图片（Edit By 51aspx.com）
    /// </summary>
    /// <param name="srcBmp">图片路径</param>
    /// <param name="bXDir">如果扭曲则选择为True</param>
    /// <param name="dMultValue">波形的幅度倍数，越大扭曲的程度越高，一般为3</param>
    /// <param name="dPhase">波形的起始相位，取值区间[0-2*PI)</param>
    /// <returns></returns>
    public System.Drawing.Bitmap TwistImage(Bitmap srcBmp, bool bXDir, double dMultValue, double dPhase)
    {
        var destBmp = new Bitmap(srcBmp.Width, srcBmp.Height);

        // 将位图背景填充为白色(防止扭曲后边角缺色)
        var graph = System.Drawing.Graphics.FromImage(destBmp);
        graph.Clear(BackgroundColor);
        //graph.FillRectangle(new SolidBrush(System.Drawing.Color.White), 0, 0, destBmp.Width, destBmp.Height);
        graph.Dispose();

        double dBaseAxisLen = bXDir ? (double)destBmp.Height : (double)destBmp.Width;

        for (int i = 0; i < destBmp.Width; i++)
        {
            for (int j = 0; j < destBmp.Height; j++)
            {
                double dx = 0;
                dx = bXDir ? (Pi2 * (double)j) / dBaseAxisLen : (Pi2 * (double)i) / dBaseAxisLen;
                dx += dPhase;
                double dy = Math.Sin(dx);

                // 取得当前点的颜色
                int nOldX = 0, nOldY = 0;
                nOldX = bXDir ? i + (int)(dy * dMultValue) : i;
                nOldY = bXDir ? j : j + (int)(dy * dMultValue);

                System.Drawing.Color color = srcBmp.GetPixel(i, j);
                if (nOldX >= 0 && nOldX < destBmp.Width && nOldY >= 0 && nOldY < destBmp.Height)
                {
                    destBmp.SetPixel(nOldX, nOldY, color);
                }
            }
        }
        return destBmp;
    }

    #endregion

    //#region 创建GIF动画
    ///// <summary>
    ///// 创建GIF动画
    ///// 不会覆盖VerifyCodeImg属性，创建的图像只能通过返回的 fullname 访问，请在访问后及时删除
    ///// </summary>
    ///// <param name="filename">扩展名为".gif"的文件名，可包含目录名，为空时按日期自动生成</param>
    ///// <param name="fullname">返回文件的全名，包含物理路径</param>
    //public void CreateImageGif(string filename, out string fullname)
    //{
    //    AnimatedGifEncoder GifPic = new AnimatedGifEncoder();

    //    Random rand = new Random();

    //    filename = string.IsNullOrEmpty(filename) ? DateTime.Now.ToString("yyyyMMdd_HHmmssfff") + ".GIF" : filename;
    //    fullname = pathHelper.getLocalPath(filename);

    //    GifPic.Start(fullname);

    //    //确保视觉残留
    //    GifPic.SetDelay(3);
    //    //-1:no repeat,0:always repeat
    //    GifPic.SetRepeat(0);
    //    for (int i = 0; i < 3; i++)
    //    {
    //        int int1 = rand.Next(4, 12);
    //        int int2 = rand.Next(1, 6);
    //        bool _bool = int2 > 3;
    //        var image = TwistImage(VerifyCodeImg, _bool, int1, int2);
    //        GifPic.AddFrame(image);
    //    }
    //    GifPic.Finish();
    //}
    //#endregion


    #region 图片生成方法
    /// <summary>
    /// 图片生成方法，内部使用
    /// </summary>
    /// <param name="code">用于识别的字符串</param>
    /// <returns></returns>
    private Bitmap CreateImage(string code)
    {
        int fSize = FontSize;
        int fWidth = fSize + Padding;

        var image = new System.Drawing.Bitmap(_width, _height, PixelFormat.Format24bppRgb);

        Graphics g = Graphics.FromImage(image);
        g.Clear(BackgroundColor);

        Random rand = new Random();

        #region 画图片的背景噪音线
        if (NoiseLine > 0)
        {
            for (int i = 0; i < NoiseLine; i++)
            {
                int x1 = rand.Next(image.Width);
                int x2 = rand.Next(image.Width);
                int y1 = rand.Next(image.Height);
                int y2 = rand.Next(image.Height);
                g.DrawLine(new Pen(Colors[rand.Next(Colors.Length - 1)], rand.Next(NoiseLeve)), x1, y1, x2, y2);
            }
        }
        #endregion

        #region 生成随机字体和颜色的验证码字符

        int left, top, cindex, findex;
        Font f;
        Brush b;

        for (int i = 0; i < code.Length; i++)
        {
            cindex = rand.Next(Colors.Length - 1);
            findex = rand.Next(Fonts.Length - 1);

            top = i % 2 == 0 ? 1 : 2;
            left = i * fWidth;

            f = new System.Drawing.Font(Fonts[findex], fSize, System.Drawing.FontStyle.Bold);
            b = new System.Drawing.SolidBrush(Colors[cindex]);

            g.DrawString(code.Substring(i, 1), f, b, left, top);
        }
        #endregion

        #region 添加随机生成的燥点

        if (Noise > 0)
        {
            //Pen pen = new Pen(ChaosColor, 0);
            int c = Length * _noise;
            for (int i = 0; i < c; i++)
            {
                int x = rand.Next(image.Width);
                int y = rand.Next(image.Height);
                g.DrawRectangle(new Pen(ChaosColor, rand.Next(NoiseLeve)), x, y, 1, 1);
            }
        }
        #endregion

        //渐变效果
        //g.FillRectangle(new LinearGradientBrush(new Point(0, 0), new Point(image.Width, image.Height), Color.FromArgb(0, 0, 0, 0), Color.FromArgb(255, 255, 255, 255)), 0, 0, image.Width, image.Height);

        //画一个边框边框颜色为Color.Gainsboro
        //g.DrawRectangle(new Pen(Color.Gainsboro, 0), 0, 0, image.Width - 1, image.Height - 1);

        g.Dispose();

        //波纹扭曲
        if (IsWave)
        {
            //随机生成后三个参数(true or false,5~8，(0~2PI),)
            int int1 = rand.Next(4, 12);
            int int2 = rand.Next(1, 6);
            bool _bool = int2 > 3;
            image = TwistImage(image, _bool, int1, int2);
        }

        return image;
    }
    #endregion

    #region 生成验证码图片
    /// <summary>
    /// 生成验证码图片
    /// </summary>
    /// <param name="code">验证字符，为空则随机生成</param>
    /// <param name="covered">是否覆盖已生成的验证码图片，默认为覆盖</param>
    /// <returns></returns>
    public Bitmap CreateImg(string code, bool covered = true)
    {
        if (string.IsNullOrEmpty(code))
        {
            code = this.CreateVerifyCode(this.Length); ;
        }
        if (covered)
        {
            _code = code;
            _imgmap = CreateImage(_code);
            return _imgmap;
        }
        return CreateImage(code);
    }
    #endregion

    #region 获得图片数据的字节数组
    /// <summary>
    /// 获得图片数据的字节数组
    /// </summary>
    /// <returns></returns>
    public byte[] GetImageData()
    {
        if (VerifyCodeImg != null)
        {
            var ms = new System.IO.MemoryStream();
            VerifyCodeImg.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            return ms.ToArray();
        }
        return null;
    }
    #endregion

    #region 将创建好的图片输出到页面
    /// <summary>
    /// 将创建好的图片输出到页面
    /// </summary>
    /// <param name="context">用于输出的 HttpContext 实例 </param>
    /// <param name="sessionKey">session 名称 默认为 CheckCode </param>
    public void CreateImageOnPage(HttpContext context, string sessionKey = "CheckCode")
    {
        context.Response.ClearContent();
        context.Response.ContentType = "image/Jpeg";

        context.Response.BinaryWrite(GetImageData());
        if (!string.IsNullOrEmpty(sessionKey))
        {
            //如果没有实现IRequiresSessionState，则这里会出错，也无法生成图片
            context.Session[sessionKey] = Code;
        }
    }
    #endregion

}