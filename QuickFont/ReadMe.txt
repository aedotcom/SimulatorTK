QuickFont - Text Printing and Font Generation Library
 Posted Sunday, 31 July, 2011 - 22:51 by james_lohr

The latest release is available here : http://www.opentk.com/node/3120

QuickFont is a free C# text printing library intended for use with OpenTK in place of the current TextPrinter. As well as being faster and cleaner, it is also absent of the "game-breaking" bugs that are present in TextPrinter. 

[Features]
Loads and renders ttf font files, converting them to texture fonts
Creation of custom texture fonts
Drop shadows
Supports pixel-perfect (lock to pixel) option
Supports pixel-perfect text rendered at a size consistent with the current orthogonal projection, independent of screen resolution.
Kerning
Monospacing
Left, right, centre align
Justified text
Super-sampling during texture generation
Light-weight : written from scratch in about a week

[Requirements]
To use QuickFont in your application you will need:
OpenTk 1.0 (dll included in download)
The Mono Framework (version 2.0 or higher) or the .Net Framework (version 2.0 or higher). These come preinstalled in most modern operating systems.
A video card with OpenGL drivers.

Kind regards,
James L.

Size: 1.52 MB
md5_file hash: f6f0fab06d9ff481fb6bf1e37b5d6b05
First released: 15 August, 2012 - 19:37
Last updated: 15 August, 2012 - 19:36


=========================== 1.0.2 =========================== 
[Bug Fixes]
##When TransformToViewport is enabled for a font, but QFont.Begin/End are not called, the font will now be rendered at the correct size
##Measuring text no longer sets the color and binds the texture, nor does it set a blend function
##Reload() method added for reloading a qfont objects. This is useful when using pixel-perfect fonts which need to be regenerated when changing the screen resolution.
##QFont now correctly implements IDisposable correctly. Note: QFont objects should be explicitly exposed by the application if it required that the underlying OpenGL textures be freed during runtime.

[API Changes]
##Removed QFont.RefreshViewport(). In its place, the following have been added which add additional functionality and flexibility: 
##Added QFont.InvalidateViewport() - Invalidates the viewport cached by QFont, causing it to be read again on the next QFont.Begin()
##Added QFont.ForceViewportRefresh() - Forces QFont to refesh its cached viewport immediately
##Added QFont.PushSoftwareViewport(Viewport viewport) - Pushes a Viewport defined by the application. This is faster than InvalidateViewport() / ForceViewportRefresh() and allows you to render to an FBO with a viewport different to that of the screen. 
##Added QFont.PopSoftwareViewport()
