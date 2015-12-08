//Project: SpeechLib (http://SpeechLib.codeplex.com)
//File: ISpeechRecognition.cs
//Version: 20151208

using System;
using System.IO;

#if USE_MICROSOFT_SPEECH
using Microsoft.Speech.Recognition;
#else
using System.Speech.Recognition;
#endif

namespace SpeechLib.Models
{

  public interface ISpeechRecognition
  {
    #region --- Properties ---

    SpeechRecognitionEngine SpeechRecognitionEngine { get; }

    bool AcousticModelAdaptation { get; set; }

    #endregion

    #region --- Methods ---

    void LoadGrammar(Grammar grammar);
    void LoadGrammar(string xml, string name);
    void LoadGrammar(Stream stream, string name);

    void UnloadGrammar(Grammar grammar);
    void UnloadAllGrammars();

    void SetInputToDefaultAudioDevice();
    void SetInputToNone();
    void SetInputToInitial(); //this can be a virtual method at some base class and overriden at descendants

    void Start(bool stopOnRecognition = false);
    void Stop(bool waitForCurrentRecognitionToComplete = true);
    void Pause();
    void Resume();

    #endregion

    #region --- Events ---

    event EventHandler<SpeechRecognitionEventArgs> Recognized;
    event EventHandler NotRecognized;

    #endregion
  }

}
