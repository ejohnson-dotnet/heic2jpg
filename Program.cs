using System;
using System.Threading.Tasks;
using System.Linq;

namespace heic2jpg
{
    class Program
    {
        static async Task Main(string[] args)
        {
            if (args == null || args.Length == 0)
            {
                Console.WriteLine("Usage: heic2jpg [filename]");
                return;
            }

            foreach (var file in args)
            {
                await ConvertFile(file);
            }
        }

        static async Task ConvertFile(string filename)
        {
            try
            {
                if (!System.IO.Path.IsPathRooted(filename))
                    filename = System.IO.Path.GetFullPath(filename);
                filename = System.IO.Path.GetFullPath(filename);
                var inputFile = await Windows.Storage.StorageFile.GetFileFromPathAsync(filename);
                using (var stream = await inputFile.OpenReadAsync())
                {
                    var decoder = await Windows.Graphics.Imaging.BitmapDecoder.CreateAsync(stream);

                    if (decoder.DecoderInformation.CodecId == Windows.Graphics.Imaging.BitmapDecoder.JpegDecoderId)
                    {
                        // Already JPEG file
                        if (!decoder.DecoderInformation.FileExtensions.Contains(inputFile.FileType, StringComparer.OrdinalIgnoreCase))
                        {
                            //Rename to .JPG file
                            var filename2 = System.IO.Path.ChangeExtension(filename, ".jpg");
                            await inputFile.RenameAsync(System.IO.Path.ChangeExtension(filename, ".jpg"));
                            Console.WriteLine($"Renamed '{filename}' to '{filename2}'");
                        }
                        else
                        {
                            Console.WriteLine($"'{filename}' is already a JPEG file.");
                        }
                        return;
                    }

                    var bitmap = await decoder.GetSoftwareBitmapAsync();
                    var outputFilename = System.IO.Path.GetFileName(System.IO.Path.ChangeExtension(filename, ".jpg"));
                    var outputFile = await (await inputFile.GetParentAsync())
                        .CreateFileAsync(outputFilename, Windows.Storage.CreationCollisionOption.ReplaceExisting);
                    using (var outputStream = await outputFile.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite))
                    {
                        var encoder = await Windows.Graphics.Imaging.BitmapEncoder.CreateAsync(Windows.Graphics.Imaging.BitmapEncoder.JpegEncoderId, outputStream);
                        encoder.SetSoftwareBitmap(bitmap);
                        encoder.IsThumbnailGenerated = true;
                        
                        await encoder.FlushAsync();
                    }

                    // Copy EXIF data.

                    //Get the EXIF data from the original photo.
                    var photoProperties = await inputFile.Properties.RetrievePropertiesAsync(SystemPhotoProperties.Union(SystemGpsProperties));

                    foreach (var p in photoProperties.OrderBy(k => k.Key))
                    {
                        //Console.WriteLine($"{p.Key}: {p.Value}");
                        System.Diagnostics.Trace.WriteLine($"{p.Key}: {(p.Value is Array ? string.Join(",", ((Array)p.Value).Cast<object>()) : p.Value)}");
                    }

                    
                    await outputFile.Properties.SavePropertiesAsync(photoProperties);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error converting file '{filename}': {ex.Message}");
                System.Diagnostics.Trace.WriteLine($"Error converting file '{filename}': {ex.ToString()}");
            }
        }

        /// <summary>
        /// A list of all the writable System.Photo.* properties.  
        /// Commented out properties are calculated or read-only.
        /// </summary>
        static string[] SystemPhotoProperties = {
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
            //"System.Photo.Orientation",
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
        static string[] SystemGpsProperties = {
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
