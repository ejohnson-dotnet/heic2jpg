# HEIF/HEIC

The [High Efficiency Image File \(HEIF) or High Efficiency Image Coding\(HEIC\)](https://en.wikipedia.org/wiki/High_Efficiency_Image_File_Format) image format that Apple uses by default in its iOS devices is good at storing images in a smaller file with better image quality. The problem is sometimes you need to use the image in a website or program that only accepts JPEG images.  There are some programs that will convert them but you have to install it and trust that it is not spyware or worse.

## heic2jpg

This is where this tool comes to help.  It is open source, uses .NET 6 and the Windows Imaging Component (WIC) API's to convert the images.

For more information refer to the Microsoft documentation: [Windows Imaging Component Overview](https://docs.microsoft.com/en-us/windows/win32/wic/-wic-about-windows-imaging-codec)

## Installation

Enter the following command to install this tool globally:

```dotnet tool install -g heic2jpg```

Once installed, you can open a Command prompt as Administrator (Start menu - Windows System - Command Prompt) Right-click and select "Run as Administrator"

Then simply enter the command `heic2jpg.exe`.  This will then add an option "Convert to JPG" to the Right-click menu in Windows Explorer for all .HEIC files.  
Simply browse to any .HEIC image file, right click on it, select the option "Convert to JPG" and a new JPG file with the same name will be created in the same folder.
You can even select multiple files, right click, and convert them JPG all at once. 

## HEVC Codec
You also need to install the HEVC codec from the Microsoft Store for this to work.

[Install the Microsoft HEVC codec from here](https://www.microsoft.com/en-us/p/hevc-video-extensions-from-device-manufacturer/9n4wgh0z6vhq?activetab=pivot:overviewtab)

