//Project: SpeechLib (http://SpeechLib.codeplex.com)
//File: SpeechRecognitionEventArgs.cs
//Version: 20151202

using System;

namespace SpeechLib.Models
{

  public class SpeechRecognitionEventArgs : EventArgs
  {
    #region --- Fields ---

    public readonly string command;
    public readonly double confidence; //a value from 0 to 1 (1=most confident)

    #endregion

    #region --- Initialization ---

    public SpeechRecognitionEventArgs(string command, double confidence)
    {
      this.command = command;
      this.confidence = confidence;
    }

    #endregion
  }
}