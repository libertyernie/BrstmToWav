BRSTM to WAV Converter 1.0
Copyright © 2015 libertyernie

https://github.com/libertyernie/BrstmToWav

This program is provided as-is without any warranty, implied or otherwise.
By using this program, the end user agrees to take full responsibility
regarding its proper and lawful use. The authors/hosts/distributors cannot be
held responsible for any damage resulting in the use of this program, nor can
they be held accountable for the manner in which it is used.

----------------------------------------

This program uses a pre-release version of BrawlLib which includes a new
feature in the WAV export code, allowing the export of certain parts of a
BRSTM file.

To add BRSTM files, use the Add button or drag-and-drop them into the list.

This program can export up to three WAV files for each BRSTM:
* From the song start to the loop start
  * The WAV filename will end with the string "(beginning)"
* From the loop start to the end
  * The WAV filename will end with the string "(loop)"
* From the song start to the end (the entire song)
  * The WAV filename will be the same as the BRSTM filename (no suffix)

The output directory defaults to the current folder, but you can change it
using the Browser button. You can also open the folder in File Explorer with
the Open button.
