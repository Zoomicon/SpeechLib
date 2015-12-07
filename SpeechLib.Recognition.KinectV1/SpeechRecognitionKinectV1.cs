//Project: SpeechLib (http://SpeechLib.codeplex.com)
//File: SpeechRecognitionKinectV1.cs
//Version: 20151207

//see: http://kin-educate.blogspot.gr/2012/06/speech-recognition-for-kinect-easy-way.html

using System.Linq;
using System.ComponentModel.Composition;
using System.Globalization;
using Microsoft.Kinect;

using SpeechLib.Models;

#if USE_MICROSOFT_SPEECH
using Microsoft.Speech.Recognition;
#else
using System.Speech.Recognition;
#endif

namespace SpeechLib.Recognition.KinectV1
{
  //MEF
  [Export("SpeechLib.Recognition", typeof(ISpeechRecognition))]
  [Export("SpeechLib.Recognition.KinectV1", typeof(ISpeechRecognitionKinect))]
  [ExportMetadata("Description", "Speech Recognition using Microsoft Kinect v1 sensor")]
  [PartCreationPolicy(CreationPolicy.Shared)]
  public class SpeechRecognitionKinectV1 : SpeechRecognition, ISpeechRecognitionKinect
  {

    #region --- Fields ---

    protected KinectSensor sensor; //=null

    #endregion

    #region --- Initialization ---

    public SpeechRecognitionKinectV1() //note: this will call the base parameterless constructor, no need to add : base() at the end
    {
      //note: the base constructor will call Init, which will call SetInputToInitial virtual method, that is overriden below to do SetInputToKinectSensor
    }

    #endregion

    #region --- Cleanup ---

    protected override void Cleanup()
    {
      base.Cleanup();
      if (sensor != null)
      {
        sensor.AudioSource.Stop();
        sensor.Stop();
        sensor = null;
      }
    }

    #endregion

    #region --- Methods ---

    protected override SpeechRecognitionEngine CreateSpeechRecognitionEngine()
    {
      RecognizerInfo kinectRecognizer = KinectV1Utils.GetKinectRecognizer(CultureInfo.GetCultureInfoByIetfLanguageTag("en")); //use Kinect-based recognition engine
      return (kinectRecognizer!=null)? new SpeechRecognitionEngine(kinectRecognizer) : base.CreateSpeechRecognitionEngine(); //fallback to recognition using default audio source
    }

    public void SetInputToKinectSensor()
    {
      sensor = KinectSensor.KinectSensors.FirstOrDefault(s => s.Status == KinectStatus.Connected);
      speechRecognitionEngine.SetInputToKinectSensor(sensor);
    }
    public override void SetInputToInitial() //can override this at descendants
    {
      SetInputToKinectSensor();
    }

    #endregion
  }
}
