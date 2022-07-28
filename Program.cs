using System;
using System.Threading.Tasks;
using System.Linq;
using WIC;

namespace heic2jpg
{
    class Program
    {
        static async Task Main(string[] args)
        {
            if (!OperatingSystem.IsWindows())
            {
                Console.WriteLine("Sorry this tool is not supported on this operating system.");
                return;
            }

            if (OperatingSystem.IsWindows())
                AddRightClickMenu();

            if (args == null || args.Length == 0)
            {
                Console.WriteLine("Usage: heic2jpg [filename]");
                return;
            }

            foreach (var file in args)
            {
                try
                {
                    await ConvertFile(file);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing file or path '{file}': {ex.Message}");
                    System.Diagnostics.Trace.WriteLine($"Error processing file or path '{file}': {ex.ToString()}");
                }
            }
        }

        static async Task ConvertFile(string pathOrFile)
        {

            await Task.CompletedTask;

            var uncPath = System.IO.Path.GetFullPath(pathOrFile);
            var filenames = System.IO.Directory.GetFiles(
                System.IO.Path.GetDirectoryName(uncPath) ?? throw new InvalidOperationException(),
                System.IO.Path.GetFileName(uncPath));
            
            foreach (var filename in filenames)
            {
                try
                {
                    var imagingFactory = new WIC.WICImagingFactory();
                    var decoder = imagingFactory.CreateDecoderFromFilename(filename, Guid.Empty,
                        WIC.StreamAccessMode.GENERIC_READ,
                        WIC.WICDecodeOptions.WICDecodeMetadataCacheOnLoad);

                    //ShowMetadata(decoder.GetFrame(0));
                    if (decoder.GetDecoderInfo().GetCLSID() == WIC.Decoder.Jpeg)
                    {
                        Console.WriteLine($"'{filename}' is already a JPEG file.");
                        return;
                    }

                    var output = imagingFactory.CreateStream();
                    output.InitializeFromFilename(System.IO.Path.ChangeExtension(filename, ".jpg"),
                        WIC.StreamAccessMode.GENERIC_WRITE);
                    var encoder = imagingFactory.CreateEncoder(ContainerFormat.Jpeg);
                    encoder.Initialize(output, WICBitmapEncoderCacheOption.WICBitmapEncoderNoCache);

                    for (int i = 0; i < decoder.GetFrameCount(); i++)
                    {
                        var frame = decoder.GetFrame(i);
                        encoder.CreateNewFrame(out var frameJpg, null);
                        frameJpg.Initialize(null);
                        frameJpg.SetSize(frame.GetSize());
                        frameJpg.SetResolution(frame.GetResolution());
                        frameJpg.SetPixelFormat(frame.GetPixelFormat());


                        var reader = frame.AsMetadataBlockReader();
                        var count = reader.GetCount();

                        //Get the EXIF data from the original photo.
                        var metadataReader = frame.GetMetadataQueryReader();
                        var metadataWriter = frameJpg.GetMetadataQueryWriter();
                        foreach (var name in metadataReader.GetNamesRecursive())
                        {
                            try
                            {
                                var val = metadataReader.GetMetadataByName(name);
                                if (name.StartsWith("/ifd/"))
                                    metadataWriter.SetMetadataByName(
                                        "/app1" + name.Replace("/ifd/{ushort=34665}/", "/ifd/exif/")
                                            .Replace("/ifd/{ushort=34853}/", "/ifd/gps/"), val);
                                else if (name.StartsWith("/xmp/"))
                                    metadataWriter.SetMetadataByName(name, val);
                            }
                            catch
                            {
                                System.Diagnostics.Trace.WriteLine($"Error setting '{name}'");
                            }
                        }

                        var photoProperties =
                            SystemProperties.Concat(SystemPhotoProperties.Concat(SystemGpsProperties));
                        foreach (var photoProp in photoProperties)
                        {
                            var action = "getting";
                            try
                            {
                                var val = metadataReader.GetMetadataByName(photoProp);
                                //System.Diagnostics.Trace.WriteLine($"{photoProp} = {val}");
                                action = "setting";
                                metadataWriter.SetMetadataByName(photoProp, val);
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Trace.WriteLine($"Error {action} '{photoProp}': " + ex.Message);
                            }
                        }

                        frameJpg.WriteSource(frame);

                        frameJpg.Commit();

                        frame = null;
                        frameJpg = null;
                    }

                    encoder.Commit();
                    output.Commit(WIC.STGC.STGC_DEFAULT);
                    encoder = null;
                    output = null;

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error converting file '{filename}': {ex.Message}");
                    System.Diagnostics.Trace.WriteLine($"Error converting file '{filename}': {ex.ToString()}");
                }
            }
        }

        static void ShowMetadata(IWICBitmapFrameDecode frame)
        {
            var metadataReader = frame.GetMetadataQueryReader();
            foreach (var name in metadataReader.GetNamesRecursive())
            {
                var val = metadataReader.GetMetadataByName(name);
                System.Diagnostics.Trace.WriteLine($"{name}: {GetValue(val)}");
            }
        }

        static object GetValue(object val)
        {
            if (val is WICBlob)
                return BitConverter.ToString(((WICBlob) val).Bytes);
            if (val is Array)
                return "[" + string.Join("][", ((Array) val).Cast<object>()) + "]";
            return val;
        }

        [System.Runtime.Versioning.SupportedOSPlatform("windows")]
        static void AddRightClickMenu()
        {
            try
            {
                var path = Environment.ProcessPath; // System.Reflection.Assembly.GetEntryAssembly()?.Location;
                var key = Microsoft.Win32.Registry.ClassesRoot.CreateSubKey(
                    "SystemFileAssociations\\.heic\\Shell\\Convert to JPG");
                key.SetValue("NeverDefault", "", Microsoft.Win32.RegistryValueKind.String);
                key = Microsoft.Win32.Registry.ClassesRoot.CreateSubKey(
                    "SystemFileAssociations\\.heic\\Shell\\Convert to JPG\\command");
                key.SetValue(null, $"\"{path}\" \"%1\"", Microsoft.Win32.RegistryValueKind.String);

                //var key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("Software\\Classes\\.heic", true);
                //var heicClass = (string?)key.GetValue(null, string.Empty);
                //if (string.IsNullOrEmpty(heicClass))
                //{
                //    heicClass = "heicfile";
                //    key.SetValue(null, heicClass, Microsoft.Win32.RegistryValueKind.String);
                //}
                //key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("Software\\Classes\\.heic\\OpenWithProgids", true);
                //key.SetValue(heicClass, "", Microsoft.Win32.RegistryValueKind.String);

                //key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey($"Software\\Classes\\{heicClass}\\Shell\\Convert to jpg\\command", true);
                //key.SetValue(null, "\"heic2jpg.exe\" \"%1\"", Microsoft.Win32.RegistryValueKind.String);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine($"Error adding registry entries: {ex.Message}");
            }
        }


        /// <summary>
        /// A list of all System photo metadata properties
        /// https://docs.microsoft.com/en-us/windows/win32/wic/system
        /// </summary>
        static string[] SystemProperties =
        {
            "System.ApplicationName",
            "System.Author",
            "System.Comment",
            "System.Copyright",
            "System.DateAcquired",
            "System.Keywords",
            "System.Rating",
            "System.SimpleRating",
            "System.Subject",
            "System.Title"
        };

        /// <summary>
        /// A list of all the writable System.Photo.* properties.
        /// Commented out properties are calculated or read-only.
        /// </summary>
        static string[] SystemPhotoProperties =
        {
            //"System.Photo.Aperture",
            "System.Photo.ApertureDenominator",
            "System.Photo.ApertureNumerator",
            //"System.Photo.Brightness",
            "System.Photo.BrightnessDenominator",
            "System.Photo.BrightnessNumerator",
            "System.Photo.CameraManufacturer",
            "System.Photo.CameraModel",
            "System.Photo.CameraSerialNumber",
            "System.Photo.Contrast",
            //"System.Photo.ContrastText",
            "System.Photo.DateTaken",
            //"System.Photo.DigitalZoom",
            "System.Photo.DigitalZoomDenominator",
            "System.Photo.DigitalZoomNumerator",
            "System.Photo.Event",
            "System.Photo.EXIFVersion",
            //"System.Photo.ExposureBias",
            "System.Photo.ExposureBiasDenominator",
            "System.Photo.ExposureBiasNumerator",
            //"System.Photo.ExposureIndex",
            "System.Photo.ExposureIndexDenominator",
            "System.Photo.ExposureIndexNumerator",
            "System.Photo.ExposureProgram",
            //"System.Photo.ExposureProgramText",
            //"System.Photo.ExposureTime",
            "System.Photo.ExposureTimeDenominator",
            "System.Photo.ExposureTimeNumerator",
            "System.Photo.Flash",
            //"System.Photo.FlashEnergy",
            "System.Photo.FlashEnergyDenominator",
            "System.Photo.FlashEnergyNumerator",
            "System.Photo.FlashManufacturer",
            "System.Photo.FlashModel",
            //"System.Photo.FlashText",
            //"System.Photo.FNumber",
            "System.Photo.FNumberDenominator",
            "System.Photo.FNumberNumerator",
            //"System.Photo.FocalLength",
            "System.Photo.FocalLengthDenominator",
            "System.Photo.FocalLengthInFilm",
            "System.Photo.FocalLengthNumerator",
            //"System.Photo.FocalPlaneXResolution",
            "System.Photo.FocalPlaneXResolutionDenominator",
            "System.Photo.FocalPlaneXResolutionNumerator",
            //"System.Photo.FocalPlaneYResolution",
            "System.Photo.FocalPlaneYResolutionDenominator",
            "System.Photo.FocalPlaneYResolutionNumerator",
            //"System.Photo.GainControl",
            "System.Photo.GainControlDenominator",
            "System.Photo.GainControlNumerator",
            //"System.Photo.GainControlText",
            "System.Photo.ISOSpeed",
            "System.Photo.LensManufacturer",
            "System.Photo.LensModel",
            "System.Photo.LightSource",
            "System.Photo.MakerNote",
            "System.Photo.MakerNoteOffset",
            //"System.Photo.MaxAperture",
            "System.Photo.MaxApertureDenominator",
            "System.Photo.MaxApertureNumerator",
            "System.Photo.MeteringMode",
            //"System.Photo.MeteringModeText",
            "System.Photo.Orientation",
            //"System.Photo.OrientationText",
            //"System.Photo.PeopleNames",
            //"System.Photo.PhotometricInterpretation",
            //"System.Photo.PhotometricInterpretationText",
            "System.Photo.ProgramMode",
            //"System.Photo.ProgramModeText",
            //"System.Photo.RelatedSoundFile",
            "System.Photo.Saturation",
            //"System.Photo.SaturationText",
            "System.Photo.Sharpness",
            //"System.Photo.SharpnessText",
            //"System.Photo.ShutterSpeed",
            "System.Photo.ShutterSpeedDenominator",
            "System.Photo.ShutterSpeedNumerator",
            //"System.Photo.SubjectDistance",
            "System.Photo.SubjectDistanceDenominator",
            "System.Photo.SubjectDistanceNumerator",
            //"System.Photo.TagViewAggregate",
            "System.Photo.TranscodedForSync",
            "System.Photo.WhiteBalance",
            //"System.Photo.WhiteBalanceText"
        };

        /// <summary>
        /// A list of all the writable System.Gps.* properties.
        /// Commented out properties are calculated or read-only.
        /// </summary>
        static string[] SystemGpsProperties =
        {
            //"System.GPS.Altitude",
            "System.GPS.AltitudeDenominator",
            "System.GPS.AltitudeNumerator",
            "System.GPS.AltitudeRef",
            "System.GPS.AreaInformation",
            "System.GPS.Date",
            //"System.GPS.DestBearing",
            "System.GPS.DestBearingDenominator",
            "System.GPS.DestBearingNumerator",
            "System.GPS.DestBearingRef",
            //"System.GPS.DestDistance",
            "System.GPS.DestDistanceDenominator",
            "System.GPS.DestDistanceNumerator",
            "System.GPS.DestDistanceRef",
            //"System.GPS.DestLatitude",
            "System.GPS.DestLatitudeDenominator",
            "System.GPS.DestLatitudeNumerator",
            "System.GPS.DestLatitudeRef",
            //"System.GPS.DestLongitude",
            "System.GPS.DestLongitudeDenominator",
            "System.GPS.DestLongitudeNumerator",
            "System.GPS.DestLongitudeRef",
            "System.GPS.Differential",
            //"System.GPS.DOP",
            "System.GPS.DOPDenominator",
            "System.GPS.DOPNumerator",
            //"System.GPS.ImgDirection",
            "System.GPS.ImgDirectionDenominator",
            "System.GPS.ImgDirectionNumerator",
            "System.GPS.ImgDirectionRef",
            //"System.GPS.Latitude",
            //"System.GPS.LatitudeDecimal",
            "System.GPS.LatitudeDenominator",
            "System.GPS.LatitudeNumerator",
            "System.GPS.LatitudeRef",
            //"System.GPS.Longitude",
            //"System.GPS.LongitudeDecimal",
            "System.GPS.LongitudeDenominator",
            "System.GPS.LongitudeNumerator",
            "System.GPS.LongitudeRef",
            "System.GPS.MapDatum",
            "System.GPS.MeasureMode",
            "System.GPS.ProcessingMethod",
            "System.GPS.Satellites",
            //"System.GPS.Speed",
            "System.GPS.SpeedDenominator",
            "System.GPS.SpeedNumerator",
            "System.GPS.SpeedRef",
            "System.GPS.Status",
            //"System.GPS.Track",
            "System.GPS.TrackDenominator",
            "System.GPS.TrackNumerator",
            "System.GPS.TrackRef",
            "System.GPS.VersionID"
        };
    }

}
