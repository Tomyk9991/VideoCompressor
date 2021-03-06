### **Note:**
_In order to use this video compressor in cmd, you need to copy 
[ffmpeg.exe](https://ffmpeg.org/ffmpeg.html)
inside the root folder._

## Examples
-----

`Syntax: -[Video compressor format] [inputPath.mp4] Optional:[outputPath.mp4]`

<img src="https://i.imgur.com/ChnOPCl.png" width="500">

Supported compressor formats are:

-      dc              4500 kbit/s

If a number, instead of a video compressor format is provided, the file size of `outputfile.mp4` will be that number. The video bit rate is dynamically calculated. 

## Side effects:
-----
If no `outputPath` is provided, the file will be named `Output.mp4`. **Note** that, using the program again, before renaming the file, will cause the file to be overwritten.