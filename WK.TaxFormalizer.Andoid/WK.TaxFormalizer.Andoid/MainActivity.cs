using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Net.Http;
using Android.Database;
using Android.Graphics;
using System.Collections.Generic;
using Android.Provider;
using Android.Content.PM;
using Java.IO;
using System.Threading.Tasks;
using Android.Content.Res;
using Java.Net;
using System.Xml.Serialization;
using WK.TaxFormalizer.Andoid.Models;
using System.Threading;


namespace WK.TaxFormalizer.Andoid
{
    [Activity(Label = "Tax Formalizer", MainLauncher = true, Icon = "@drawable/taxlogo")]
    public class MainActivity : Activity
    {
        HttpClient _client = null;
        public static readonly int PickImageId = 1000;
        private ImageView _imageView;
        string serviceURL = "http://ppanigrahy-lt.wk.pune/WK.TaxFormalizer.Service/";

        public static class App
        {
            public static Java.IO.File _file;
            public static Java.IO.File _dir;
            public static Bitmap bitmap;
            public static string filePath;
        }


        protected override void OnCreate(Bundle bundle)
        {

            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            //progress = new ProgressDialog(this);

            // Get our button from the layout resource,
            // and attach an event to it
            if (IsThereAnAppToTakePictures())
            {
                CreateDirectoryForPictures();

                Button mybutton = FindViewById<Button>(Resource.Id.myButton);
                _imageView = FindViewById<ImageView>(Resource.Id.imageView1);
                mybutton.Click += TakeAPicture;
            }

            _imageView = FindViewById<ImageView>(Resource.Id.imageView1);
            Button button = FindViewById<Button>(Resource.Id.button1);
            button.Click += ButtonOnClick;

            Button UploadButton = FindViewById<Button>(Resource.Id.UploadImage);
            UploadButton.Visibility = ViewStates.Gone;
            UploadButton.Click += UploadImage;
        }

        private void UploadImage(object sender, EventArgs eventArgs)
        {
            if (App.filePath == null)
            {//Image capture. Need to compress         
                App._file = new File(App._dir, String.Format("InvoiceCompressed{0}.jpg", Guid.NewGuid()));
                try
                {
                    using (var os = new System.IO.FileStream(App._file.AbsolutePath, System.IO.FileMode.Create))
                    {
                        App.bitmap.Compress(Bitmap.CompressFormat.Png, 100, os);
                    }
                }
                catch (Exception ex)
                {
                    System.Console.Write(ex.Message);
                }
                UploadNow(App._file.Path);
            }

            else
                UploadNow(App.filePath);
        }

        private void UploadNow(string path)
        {
            Button UploadButton = FindViewById<Button>(Resource.Id.UploadImage);
            string oldText = UploadButton.Text;
            UploadButton.Text = "Processing...";
            UploadButton.Enabled = false;
            UploadFileToServer(path);
            UploadButton.Text = oldText;
            UploadButton.Enabled = true;
        }

        private void ButtonOnClick(object sender, EventArgs eventArgs)
        {
            Intent = new Intent();
            Intent.SetType("image/*");
            Intent.SetAction(Intent.ActionGetContent);
            StartActivityForResult(Intent.CreateChooser(Intent, "Select Picture"), PickImageId);
        }

        private void TakeAPicture(object sender, EventArgs eventArgs)
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            Intent.SetType("image/*");
            App._file = new File(App._dir, String.Format("Invoice{0}.jpg", Guid.NewGuid()));
            intent.PutExtra(MediaStore.ExtraOutput, Android.Net.Uri.FromFile(App._file));

            StartActivityForResult(intent, PickImageId);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            Button UploadButton = FindViewById<Button>(Resource.Id.UploadImage);

            if ((requestCode == PickImageId) && (resultCode == Result.Ok) && (data != null))//Pick image from library
            {
                Android.Net.Uri uri = data.Data;
                var pathToImage = GetPathToImage(uri);

                //_imageView.SetImageURI(uri);
                _imageView.SetImageBitmap(BitmapHelpers.LoadAndResizeBitmap(pathToImage, 800, 600));

                App.filePath = pathToImage;
                //Bitmap image = MediaStore.Images.Media.GetBitmap(this.getContentResolver(), uri);
            }
            else if ((requestCode == PickImageId) && (resultCode == Result.Ok))//Capture Image
            {

                //BitmapFactory.Options options = await GetBitmapOptionsOfImageAsync();
                //Bitmap bitmapToDisplay = await LoadScaledDownBitmapForDisplayAsync(Resources, options, 150, 150);
                //_imageView.SetImageBitmap(bitmapToDisplay);                
                App.bitmap = BitmapHelpers.LoadAndResizeBitmap(App._file.AbsolutePath, 800, 600);
                _imageView.SetImageBitmap(App.bitmap);
                // Bitmap bitMap=   BitmapFactory.DecodeFile(App._file.AbsolutePath );
                //_imageView.SetImageBitmap(bitMap);
                //var uri = Android.Net.Uri.Parse(App._file.ToString());
                //_imageView.SetImageURI(uri);
                //UploadFileToServer(App._file.Path);

            }
            UploadButton.Visibility = ViewStates.Visible;
        }

        private string UploadFileToServer(string path)
        {
            System.Net.WebClient Client = new System.Net.WebClient();
            Client.Headers.Add("Content-Type", "binary/octet-stream");

            var fileName = System.IO.Path.GetFileName(path);
            Client.Headers.Add("imageName", fileName);
            try
            {
                byte[] result = Client.UploadFile(serviceURL + "/api/TaxFormalizer/FormalizeTax", "POST", path);

                string s = System.Text.Encoding.UTF8.GetString(result, 0, result.Length);
                TaxFormalizeResponse response = Newtonsoft.Json.JsonConvert.DeserializeObject<TaxFormalizeResponse>(s);
                TextView resultArea = FindViewById<TextView>(Resource.Id.result);

                resultArea.Text = "Collected Sales Tax: " + response.AppliedSalesTax.ToString("##.## $") + "\n" + "Applicable Sales Tax: " + response.ToBeAppliedSalesTax.ToString("##.## $");
                {
                    TextView airportResult = FindViewById<TextView>(Resource.Id.airportResult);
                    airportResult.Visibility = ViewStates.Visible;
                    airportResult.Text = " Refund: " + (response.AppliedSalesTax - response.ToBeAppliedSalesTax).ToString("##.## $");
                }
                return s;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        private string GetPathToImage(Android.Net.Uri uri)
        {
            string doc_id = "";

            using (var c1 = ContentResolver.Query(uri, null, null, null, null))
            {
                c1.MoveToFirst();
                String document_id = c1.GetString(0);
                doc_id = document_id.Substring(document_id.LastIndexOf(":") + 1);
            }

            string path = null;

            // The projection contains the columns we want to return in our query.
            string selection = Android.Provider.MediaStore.Images.Media.InterfaceConsts.Id + " =? ";
            using (var cursor = ManagedQuery(Android.Provider.MediaStore.Images.Media.ExternalContentUri, null, selection, new string[] { doc_id }, null))
            {
                if (cursor == null) return path;
                var columnIndex = cursor.GetColumnIndexOrThrow(Android.Provider.MediaStore.Images.Media.InterfaceConsts.Data);
                cursor.MoveToFirst();
                path = cursor.GetString(columnIndex);
            }
            return path;
        }

        //public String getPath(Uri uri)
        //{

        //    String[] projection = { MediaStore.Images.Media.DATA };
        //    Cursor cursor = managedQuery(uri, projection, null, null, null);
        //    int column_index = cursor.getColumnIndexOrThrow(MediaStore.Images.Media.DATA);
        //    cursor.moveToFirst();

        //    return cursor.getString(column_index);
        //}

        private void CreateDirectoryForPictures()
        {
            App._dir = new Java.IO.File(
                    Android.OS.Environment.GetExternalStoragePublicDirectory(
                    Android.OS.Environment.DirectoryPictures), "CameraAppDemo");

            //App._dir = new Java.IO.File("/storage/emulated/0/Download/");
            //Android.OS.Environment.GetExternalStoragePublicDirectory(
            //Android.OS.Environment.DirectoryPictures), "CameraAppDemo");
            if (!App._dir.Exists())
            {
                App._dir.Mkdirs();
            }
        }

        private bool IsThereAnAppToTakePictures()
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            IList<ResolveInfo> availableActivities =
                PackageManager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);
            return availableActivities != null && availableActivities.Count > 0;
        }

        public HttpClient Client
        {
            get
            {
                return _client = _client ?? new HttpClient();
            }
            set
            {
                //for unit testing purpose
                _client = value;
            }
        }
    }



    public static class BitmapHelpers
    {
        public static Bitmap LoadAndResizeBitmap(this string fileName, int width, int height)
        {
            // First we get the the dimensions of the file on disk
            BitmapFactory.Options options = new BitmapFactory.Options { InJustDecodeBounds = true };
            BitmapFactory.DecodeFile(fileName, options);

            // Next we calculate the ratio that we need to resize the image by
            // in order to fit the requested dimensions.
            int outHeight = options.OutHeight;
            int outWidth = options.OutWidth;
            int inSampleSize = 1;

            if (outHeight > height || outWidth > width)
            {
                inSampleSize = outWidth > outHeight
                                   ? outHeight / height
                                   : outWidth / width;
            }

            // Now we will load the image and have BitmapFactory resize it for us.
            options.InSampleSize = inSampleSize;
            options.InJustDecodeBounds = false;
            Bitmap resizedBitmap = BitmapFactory.DecodeFile(fileName, options);

            return resizedBitmap;
        }
    }


}

//public static int CalculateInSampleSize(BitmapFactory.Options options, int reqWidth, int reqHeight)
//{
//    // Raw height and width of image
//    float height = options.OutHeight;
//    float width = options.OutWidth;
//    double inSampleSize = 1D;

//    if (height > reqHeight || width > reqWidth)
//    {
//        int halfHeight = (int)(height / 2);
//        int halfWidth = (int)(width / 2);

//        // Calculate a inSampleSize that is a power of 2 - the decoder will use a value that is a power of two anyway.
//        while ((halfHeight / inSampleSize) > reqHeight && (halfWidth / inSampleSize) > reqWidth)
//        {
//            inSampleSize *= 2;
//        }
//    }

//    return (int)inSampleSize;
//}

//async Task<BitmapFactory.Options> GetBitmapOptionsOfImageAsync()
//{
//    BitmapFactory.Options options = new BitmapFactory.Options
//    {
//        InJustDecodeBounds = true
//    };

//    // The result will be null because InJustDecodeBounds == true.
//    Bitmap result = await BitmapFactory.DecodeResourceAsync(Resources, Resource.Drawable.samoyed, options);

//    int imageHeight = options.OutHeight;
//    int imageWidth = options.OutWidth;

//    //_originalDimensions.Text = string.Format("Original Size= {0}x{1}", imageWidth, imageHeight);

//    return options;
//}

//public async Task<Bitmap> LoadScaledDownBitmapForDisplayAsync(Resources res, BitmapFactory.Options options, int reqWidth, int reqHeight)
//{
//    // Calculate inSampleSize
//    options.InSampleSize = CalculateInSampleSize(options, reqWidth, reqHeight);

//    // Decode bitmap with inSampleSize set
//    options.InJustDecodeBounds = false;

//    return await BitmapFactory.DecodeResourceAsync(res, Resource.Drawable.samoyed, options);
//}

//private string GetPathToImage(Android.Net.Uri uri)
//{
//    string path = null;
//    // The projection contains the columns we want to return in our query.
//    string[] projection = new[] { Android.Provider.MediaStore.Images.Media.InterfaceConsts.Data };
//    using (ICursor cursor = ManagedQuery(uri, projection, null, null, null))
//    {
//        if (cursor != null)
//        {
//            int columnIndex = cursor.GetColumnIndexOrThrow(Android.Provider.MediaStore.Images.Media.InterfaceConsts.Data);
//            cursor.MoveToFirst();
//            path = cursor.GetString(columnIndex);
//        }
//    }
//    return path;
//}