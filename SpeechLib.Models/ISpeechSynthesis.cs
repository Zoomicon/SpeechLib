//Project: SpeechLib (http://SpeechLib.codeplex.com)
//File: ISpeechSynthesis.cs
//Version: 20151205

using System.Globalization;
using System.Speech.Synthesis; //do not use Microsoft.Speech.Synthesis here, throws Illegal Access Error (probably needs to have the app running as administrator, or some other issue occurs when trying to use speech synthesis with it)

namespace SpeechLib.Models
{
  public interface ISpeechSynthesis
  {

    #region --- Properties ---

    SpeechSynthesizer SpeechSynthesisEngine { get; }

    CultureInfo Culture { get; }

    #endregion

    #region --- Methods ---

    void Speak(string text);

    #endregion
  }
}
