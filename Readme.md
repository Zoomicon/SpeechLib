## SpeechLib
https://github.com/Zoomicon/SpeechLib


### Project Description

.NET Library providing:
* Speech Synthesis
* Speech Recognition
* Speech Recognition using Kinect V1 Sensor Microphone Array as input


### Requirements

* Microsoft Kinect Runtime 1.8 (or SDK 1.8 if you use the Kinect for Xbox 360 sensor instead of Kinect for Windows v1)
* Microsoft Speech Runtime


### Notes

The latest version has been preset to use the Windows Speech API instead of Microsoft Speech one. There is a conditional compilation switch in the source code that you can define or comment out to toggle this.

You can check out the following projects to see how a dictionary can be built. The first one shows how you can do it both declaratively (SGML) and via code:
- SpeechTurtle (http://github.com/zoomicon/speechturtle - pending move from http://speechturtle.codeplex.com)
- Hotspotizer (my fork at http://github.com/birbilis/hotspotizer)
- TrackingCam (http://trackingcam.codeplex.com - pending move from http://trackingcam.codeplex.com)

also, there is an issue with the Windows Speech API I think regarding recognition quality for commands. The Windows Speech library has less languages, but user can train it from the control panel (there is a speech item there). There is an alternative API called Microsoft Speech that is very similar (only the SGML syntax needed some small changes to support both of those). That one has its own SDK (Kinect runtime installs it) and has more generic recognition (less tuned to a specific user) with more languages. So if you get wrong recognition, maybe you should switch to the Microsoft Speech that Kinect team suggests instead.

Note that you add the conditional compilation symbol (by name) in your project build settings tab and the code will detect and use the appropriate library then. You may need to also add a reference to the Microsoft Speech library and remove the Windows Speech one

The version released on NuGet uses Windows Speech if I remember well. I plan to publish alternative packages for the other speech API .
