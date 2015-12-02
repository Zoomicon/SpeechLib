//Project: SpeechLib (http://SpeechLib.codeplex.com)
//File: ISpeechSynthesis.cs
//Version: 20151202

using System.Globalization;

namespace SpeechLib.Models
{
  public interface ISpeechSynthesis
  {

    #region --- Properties ---

    CultureInfo Culture { get; }

    #endregion

    #region --- Methods ---

    void Speak(string text);

    #endregion
  }
}
