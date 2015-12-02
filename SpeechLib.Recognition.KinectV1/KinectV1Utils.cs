//Project: SpeechLib (http://SpeechLib.codeplex.com)
//Filename: KinectV1Utils.cs
//Version: 20151202

using Microsoft.Kinect;
using System;
using System.Globalization;
using System.IO;

#if USE_MICROSOFT_SPEECH
using Microsoft.Speech.AudioFormat;
using Microsoft.Speech.Recognition;
#else
using System.Speech.AudioFormat;
using System.Speech.Recognition;
#endif

namespace SpeechLib.Recognition.KinectV1
{
  /// <summary>
  /// Kinect-related utility methods
  /// </summary>
  public static class KinectV1Utils
  {

    /// <summary>
    /// Looks through all connected Kinect sensors and tries to start one.
    /// </summary>
    /// <returns>Returns the first sensor that can be started succesfully, else null.</returns>
    public static KinectSensor StartKinectSensor()
    {
      foreach (KinectSensor sensor in KinectSensor.KinectSensors)
        if (sensor.Status == KinectStatus.Connected)
          try
          {
            sensor.Start(); // Start the sensor!
            return sensor; // return if started successfully
          }
          catch (IOException) // Some other application is streaming from the same Kinect sensor
          {
            //NOP
          }

      return null;
    }

    /// <summary>
    /// Gets the metadata for the speech recognizer (acoustic model) most suitable to
    /// process audio from Kinect device.
    /// </summary>
    /// <returns>
    /// RecognizerInfo if found, <code>null</code> otherwise.
    /// </returns>
    public static RecognizerInfo GetKinectRecognizer(CultureInfo culture)
    {
      foreach (RecognizerInfo recognizer in SpeechRecognitionEngine.InstalledRecognizers())
      {
        string value;
        recognizer.AdditionalInfo.TryGetValue("Kinect", out value);
        if ("True".Equals(value, StringComparison.OrdinalIgnoreCase) &&
             culture.Name.Equals(recognizer.Culture.Name, StringComparison.OrdinalIgnoreCase))
          return recognizer;
      }
      return null;
    }

    /// <summary>
    /// Extension method for SpeechRecognitionEngine to set audio input from Kinect sensor, with optional settings of SpeechAudioFormat.
    /// </summary>
    /// <param name="speechEngine"></param>
    /// <param name="sensor"></param>
    /// <param name="speechAudioFormat"></param>
    public static void SetInputToKinectSensor(this SpeechRecognitionEngine speechEngine, KinectSensor sensor, SpeechAudioFormatInfo speechAudioFormat = null)
    {
      if (sensor == null)
      {
        speechEngine.SetInputToDefaultAudioDevice();
        return;
      }

      if (speechAudioFormat == null)
        speechAudioFormat = new SpeechAudioFormatInfo(EncodingFormat.Pcm, 16000, 16, 1, 32000, 2, null); //default input audio format (taken from SpeechBasics-WPF C# sample of Kinect SDK 1.8)

      speechEngine.SetInputToAudioStream(sensor.AudioSource.Start(), speechAudioFormat);
    }

  }
}
