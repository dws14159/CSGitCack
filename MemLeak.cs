using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace CSGitCack
{
    public class MemLeak
    {
        public BitmapImage ImageSourceFromFile(string myImageFile)
        {
            try
            {
                if (string.IsNullOrEmpty(myImageFile))
                    return null;

                var image = new BitmapImage();
                var ms = new MemoryStream();
                BitmapDecoder bd;
                using (FileStream fs = File.OpenRead(myImageFile))
                {
                    string ext = Path.GetExtension(myImageFile).ToUpper();
                    switch (ext)
                    {
                        case ".BMP":
                            bd = new BmpBitmapDecoder(fs, BitmapCreateOptions.PreservePixelFormat,
                                BitmapCacheOption.OnLoad);
                            break;
                        case ".GIF":
                            bd = new GifBitmapDecoder(fs, BitmapCreateOptions.PreservePixelFormat,
                                BitmapCacheOption.OnLoad);
                            break;
                        case ".JPG":
                        case ".JPEG":
                            bd = new JpegBitmapDecoder(fs, BitmapCreateOptions.PreservePixelFormat,
                                BitmapCacheOption.OnLoad);
                            break;
                        case ".PNG":
                            bd = new PngBitmapDecoder(fs, BitmapCreateOptions.PreservePixelFormat,
                                BitmapCacheOption.OnLoad);
                            break;
                        default:
                            throw new Exception($"Unknown file extension '{ext}'");
                    }

                    var png = new PngBitmapEncoder();
                    png.Frames.Add(bd.Frames[0]);
                    png.Save(ms);

                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.StreamSource = ms;
                    image.EndInit();
                }

                return image;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"File Open error: '{ex.Message}'");
                return null;
            }
        }
    }
}
