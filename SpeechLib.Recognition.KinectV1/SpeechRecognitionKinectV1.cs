//Project: SpeechLib (http://SpeechLib.codeplex.com)
//File: SpeechRecognitionKinectV1.cs
//Version: 20151202

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

    public SpeechRecognitionKinectV1() //note: this will call the base parameterless constructor, no need to add : base() at the end
    {
    }

    protected override SpeechRecognitionEngine CreateSpeechRecognitionEngine()
    {
      RecognizerInfo kinectRecognizer = KinectV1Utils.GetKinectRecognizer(CultureInfo.GetCultureInfoByIetfLanguageTag("en")); //use Kinect-based recognition engine
      return (kinectRecognizer!=null)? new SpeechRecognitionEngine(kinectRecognizer) : base.CreateSpeechRecognitionEngine(); //fallback to recognition using default audio source
    }

    public void SetInputToKinectSensor()
    {
      speechRecognitionEngine.SetInputToKinectSensor(KinectSensor.KinectSensors.FirstOrDefault(s => s.Status == KinectStatus.Connected));
    }

  }
}
