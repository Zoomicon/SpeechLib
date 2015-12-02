//Project: SpeechLib (http://SpeechLib.codeplex.com)
//File: ISpeechRecognition.cs
//Version: 20151202

using System;
using System.IO;

namespace SpeechLib.Models
{

  public interface ISpeechRecognition
  {
    #region --- Properties ---

    bool AcousticModelAdaptation { get; set; }

    #endregion

    #region --- Methods ---

    void LoadGrammar(string grammar, string name);
    void LoadGrammar(Stream stream, string name);
    void SetInputToDefaultAudioDevice();
    void Start();

    #endregion

    #region --- Events ---

    event EventHandler<SpeechRecognitionEventArgs> Recognized;
    event EventHandler NotRecognized;

    #endregion
  }

}
