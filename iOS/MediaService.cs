using System;
using System.Collections.Generic;
using System.Text;
using UIKit;
using CoreGraphics;
using System.Drawing;
using Xamarin.Forms;
using Lebenslauf.iOS;

[assembly: Dependency(typeof(MediaService))]
namespace Lebenslauf.iOS
{
    public class MediaService : IMediaService
    //Ändert die Bildgröße 
    //TODO: Höhe und Breite des Bildes sind bei der Ausgabe vertauscht, Anmerkung: Alle Versuche, den Fehler zu beheben, führten zu einem verzerrten Bild.
    //      Höhe und Breite müssen beim Einfügen in das Word-Dokumente vertauscht werden, solange dieser Fehler nicht behoben wurde.
    {
        public byte[] ResizeImage(byte[] imageData, float width, float height)
        {
            UIImage originalImage = ImageFromByteArray(imageData);
            UIImageOrientation orientImg = originalImage.Orientation;

            float oldWidth = (float)originalImage.Size.Width;
            float oldHeight = (float)originalImage.Size.Height;
            float scaleFactor = 0f;

            if (oldWidth > oldHeight)
            {
                scaleFactor = width / oldWidth;
            }
            else
            {
                scaleFactor = height / oldHeight;
            }

            float newHeight = oldHeight * scaleFactor;
            float newWidth = oldWidth * scaleFactor;

            //create a 24bit RGB image
            using (CGBitmapContext context = new CGBitmapContext(IntPtr.Zero,
                (int)newWidth, (int)newHeight, 8,
                (int)(4 * newWidth), CGColorSpace.CreateDeviceRGB(),
                CGImageAlphaInfo.PremultipliedFirst))
            {

                RectangleF imageRect = new RectangleF(0, 0, newWidth, newHeight);

                // draw the image

                context.DrawImage(imageRect, originalImage.CGImage);

                //UIKit.UIImage resizedImage = UIKit.UIImage.FromImage(context.ToImage());

                //Das skalierte Bild um 90 Grad drehen

                //UIKit.UIImage resizedImage = UIKit.UIImage.FromImage(context.ToImage(), 1, UIImageOrientation.Right);
                UIKit.UIImage resizedImage = UIKit.UIImage.FromImage(context.ToImage(), 1, orientImg);
                //Das gedrehte Bild neu rendern, höhe und Breite vertauscht um das Seitenverhältniss beizubehalten
                UIGraphics.BeginImageContextWithOptions(new CGSize((float)resizedImage.CGImage.Height, (float)resizedImage.CGImage.Width), true, 1.0f); //TODO: Height und Width sind evtl. vertauscht
                resizedImage.Draw(new CGRect(0, 0, (float)resizedImage.CGImage.Height, (float)resizedImage.CGImage.Width)); //TODO: Height und Width sind evtl. vertauscht

                var resultImage = UIGraphics.GetImageFromCurrentImageContext();
                UIGraphics.EndImageContext();

                resizedImage = resultImage;
                           
                return resizedImage.AsJPEG(100).ToArray();

                // save the image as a jpeg
                //return resizedImage.AsJPEG(100).ToArray();
            }
        }

        //Hier funktioniert das drehen des Bildes
        //public byte[] ResizeImage(byte[] imageData, float width, float height)
        //{
        //    UIImage originalImage = ImageFromByteArray(imageData);

        //    float oldWidth = (float)originalImage.Size.Width;
        //    float oldHeight = (float)originalImage.Size.Height;
        //    float scaleFactor = 0f;

        //    if (oldWidth > oldHeight)
        //    {
        //        scaleFactor = width / oldWidth;
        //    }
        //    else
        //    {
        //        scaleFactor = height / oldHeight;
        //    }

        //    float newHeight = oldHeight * scaleFactor;
        //    float newWidth = oldWidth * scaleFactor;

        //    //create a 24bit RGB image
        //    using (CGBitmapContext context = new CGBitmapContext(IntPtr.Zero,
        //        (int)newWidth, (int)newHeight, 8,
        //        (int)(4 * newWidth), CGColorSpace.CreateDeviceRGB(),
        //        CGImageAlphaInfo.PremultipliedFirst))
        //    {

        //        RectangleF imageRect = new RectangleF(0, 0, newWidth, newHeight);

        //        // draw the image

        //        context.DrawImage(imageRect, originalImage.CGImage);

        //        // context.TranslateCTM(context.Width / 2, context.Height / 2);
        //        //context.RotateCTM(DegreesToRadians(90));

        //        //UIKit.UIImage resizedImage = UIKit.UIImage.FromImage(context.ToImage());
        //        UIKit.UIImage resizedImage = UIKit.UIImage.FromImage(context.ToImage(),1,UIImageOrientation.Right);

        //        UIGraphics.BeginImageContextWithOptions(new CGSize((float)resizedImage.CGImage.Height, (float)resizedImage.CGImage.Width), true, 1.0f);
        //        resizedImage.Draw(new CGRect(0, 0, (float)resizedImage.CGImage.Height, (float)resizedImage.CGImage.Width));

        //        var resultImage = UIGraphics.GetImageFromCurrentImageContext();
        //        UIGraphics.EndImageContext();
        //        resizedImage = resultImage;





        //        return resizedImage.AsJPEG(100).ToArray();

        //        // save the image as a jpeg
        //        //return resizedImage.AsJPEG(100).ToArray();
        //    }
        //}

        public static UIKit.UIImage ImageFromByteArray(byte[] data)
        {
            if (data == null)
            {
                return null;
            }

            UIKit.UIImage image;
            try
            {
                image = new UIKit.UIImage(Foundation.NSData.FromArray(data));
            }
            catch (Exception e)
            {
                Console.WriteLine("Image load failed: " + e.Message);
                return null;
            }
            return image;
        }

       float DegreesToRadians(float degrees)
        {
            return degrees * (float)Math.PI / 180;
        }

        //UIImage RotateImage(UIImage imageIn)
        //{
        //    //int kMaxResolution = 2048;

        //    CGImage imgRef = imageIn.CGImage;
        //    float width = imgRef.Width;
        //    float height = imgRef.Height;
        //    CGAffineTransform transform = CGAffineTransform.MakeIdentity();
        //    RectangleF bounds = new RectangleF(0, 0, width, height);

        //    //if (width > kMaxResolution || height > kMaxResolution)
        //    //{
        //    //    float ratio = width / height;

        //    //    if (ratio > 1)
        //    //    {
        //    //        bounds.Width = kMaxResolution;
        //    //        bounds.Height = bounds.Width / ratio;
        //    //    }
        //    //    else
        //    //    {
        //    //        bounds.Height = kMaxResolution;
        //    //        bounds.Width = bounds.Height * ratio;
        //    //    }
        //    //}

        //    float scaleRatio = bounds.Width / width;
        //    SizeF imageSize = new SizeF(width, height);
        //    UIImageOrientation orient = imageIn.Orientation;
        //    float boundHeight;

        //    switch (orient)
        //    {
        //        case UIImageOrientation.Up:                                        //EXIF = 1
        //            transform = CGAffineTransform.MakeIdentity();
        //            break;

        //        case UIImageOrientation.UpMirrored:                                //EXIF = 2
        //            transform = CGAffineTransform.MakeTranslation(imageSize.Width, 0f);
        //            transform = CGAffineTransform.MakeScale(-1.0f, 1.0f);
        //            break;

        //        case UIImageOrientation.Down:                                      //EXIF = 3
        //            transform = CGAffineTransform.MakeTranslation(imageSize.Width, imageSize.Height);
        //            transform = CGAffineTransform.Rotate(transform, (float)Math.PI);
        //            break;

        //        case UIImageOrientation.DownMirrored:                              //EXIF = 4
        //            transform = CGAffineTransform.MakeTranslation(0f, imageSize.Height);
        //            transform = CGAffineTransform.MakeScale(1.0f, -1.0f);
        //            break;

        //        case UIImageOrientation.LeftMirrored:                              //EXIF = 5
        //            boundHeight = bounds.Height;
        //            bounds.Height = bounds.Width;
        //            bounds.Width = boundHeight;
        //            transform = CGAffineTransform.MakeTranslation(imageSize.Height, imageSize.Width);
        //            transform = CGAffineTransform.MakeScale(-1.0f, 1.0f);
        //            transform = CGAffineTransform.Rotate(transform, 3.0f * (float)Math.PI / 2.0f);
        //            break;

        //        case UIImageOrientation.Left:                                      //EXIF = 6
        //            boundHeight = bounds.Height;
        //            bounds.Height = bounds.Width;
        //            bounds.Width = boundHeight;
        //            transform = CGAffineTransform.MakeTranslation(0.0f, imageSize.Width);
        //            transform = CGAffineTransform.Rotate(transform, 3.0f * (float)Math.PI / 2.0f);
        //            break;

        //        case UIImageOrientation.RightMirrored:                             //EXIF = 7
        //            boundHeight = bounds.Height;
        //            bounds.Height = bounds.Width;
        //            bounds.Width = boundHeight;
        //            transform = CGAffineTransform.MakeScale(-1.0f, 1.0f);
        //            transform = CGAffineTransform.Rotate(transform, (float)Math.PI / 2.0f);
        //            break;

        //        case UIImageOrientation.Right:                                     //EXIF = 8
        //            boundHeight = bounds.Height;
        //            bounds.Height = bounds.Width;
        //            bounds.Width = boundHeight;
        //            transform = CGAffineTransform.MakeTranslation(imageSize.Height, 0.0f);
        //            transform = CGAffineTransform.Rotate(transform, (float)Math.PI / 2.0f);
        //            break;

        //        default:
        //            throw new Exception("Invalid image orientation");
        //            break;
        //    }

        //    UIGraphics.BeginImageContext(bounds.Size);

        //    CGContext context = UIGraphics.GetCurrentContext();



        //    if (orient == UIImageOrientation.Right || orient == UIImageOrientation.Left)
        //    {
        //        context.ScaleCTM(-scaleRatio, scaleRatio);
        //        context.TranslateCTM(-height, 0);
        //    }
        //    else
        //    {
        //        context.ScaleCTM(scaleRatio, -scaleRatio);
        //        context.TranslateCTM(0, -height);
        //    }

        //    context.ConcatCTM(transform);
        //    context.DrawImage(new RectangleF(0, 0, width, height), imgRef);

        //    UIImage imageCopy = UIGraphics.GetImageFromCurrentImageContext();
        //    UIGraphics.EndImageContext();

        //    return imageCopy;
        //}



    }
}
