//Project: SpeechLib (http://SpeechLib.codeplex.com)
//File: ISpeechRecognitionKinect.cs
//Version: 20151117

namespace SpeechLib.Models
{
  public interface ISpeechRecognitionKinect : ISpeechRecognition
  {
    #region --- Methods ---

    void SetInputToKinectSensor();

    #endregion
  }

}
