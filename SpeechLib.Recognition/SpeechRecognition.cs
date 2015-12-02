//Project: SpeechLib (http://SpeechLib.codeplex.com)
//File: SpeechRecognition.cs
//Version: 20151202

using SpeechLib.Models;
using System;
using System.ComponentModel.Composition;
using System.IO;

#if USE_MICROSOFT_SPEECH
using Microsoft.Speech.Recognition;
#else
using System.Speech.Recognition;
#endif

namespace SpeechLib.Recognition
{
  //MEF
  [Export("SpeechLib.Recognition", typeof(ISpeechRecognition))]
  [ExportMetadata("Description", "Speech Recognition")]
  [PartCreationPolicy(CreationPolicy.Shared)]
  public class SpeechRecognition : ISpeechRecognition, IDisposable
  {

    #region --- Constants ---

    private const string ACOUSTIC_MODEL_ADAPTATION = "AdaptationOn";

    #endregion

    #region --- Fields ---

    protected SpeechRecognitionEngine speechRecognitionEngine;

    #endregion

    #region --- Initialization ---

    public SpeechRecognition()
    {
      speechRecognitionEngine = CreateSpeechRecognitionEngine();
      if (speechRecognitionEngine != null)
      {
        speechRecognitionEngine.SpeechRecognized += SpeechRecognized;
        //speechRecognitionEngine.SpeechHypothesized += SpeechHypothesized;
        speechRecognitionEngine.SpeechRecognitionRejected += SpeechRecognitionRejected;
      }
    }

    #endregion

    #region --- Cleanup ---

    //IDisposable//

    private bool disposedValue = false; // To detect redundant calls

    protected virtual void Dispose(bool disposing)
    {
      if (!disposedValue)
      {
        if (disposing)
        {
          // TODO: dispose managed state (managed objects).
        }

        // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
        // TODO: set large fields to null.

        disposedValue = true;
      }
    }

    // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
    // ~SpeechRecognition() {
    //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
    //   Dispose(false);
    // }

    // This code added to correctly implement the disposable pattern.
    public void Dispose()
    {
      // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
      Dispose(true);
      // TODO: uncomment the following line if the finalizer is overridden above.
      // GC.SuppressFinalize(this);
    }

    #endregion

    #region --- Properties ---

    /// <summary>
    /// For long recognition sessions (a few hours or more), it may be beneficial to turn off adaptation of the acoustic model.
    /// This will prevent recognition accuracy from degrading over time.
    /// </summary>
    public bool AcousticModelAdaptation
    {
      get { return ((Int32)speechRecognitionEngine.QueryRecognizerSetting(ACOUSTIC_MODEL_ADAPTATION) != 0); }
      set { speechRecognitionEngine.UpdateRecognizerSetting(ACOUSTIC_MODEL_ADAPTATION, 0); }
    }

    #endregion

    #region --- Methods ---

    protected virtual SpeechRecognitionEngine CreateSpeechRecognitionEngine()
    {
      return new SpeechRecognitionEngine(); //use current system default recognition engine
    }

    public void LoadGrammar(string grammar, string name)
    {
      speechRecognitionEngine.LoadGrammar(SpeechRecognitionUtils.CreateGrammarFromXML(grammar, name));
    }

    public void LoadGrammar(Stream stream, string name)
    {
      speechRecognitionEngine.LoadGrammar(new Grammar(stream) { Name = name });
    }

    public void SetInputToDefaultAudioDevice()
    {
      speechRecognitionEngine.SetInputToDefaultAudioDevice();
    }

    public void Start()
    {
      speechRecognitionEngine.RecognizeAsync(RecognizeMode.Multiple); //start speech recognition (set to keep on firing speech recognition events, not just once)
    }

    #endregion

    #region --- Events ---

    public event EventHandler<SpeechRecognitionEventArgs> Recognized;
    public event EventHandler NotRecognized;

    private void SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
    {
      if (Recognized != null)
        Recognized(this, new SpeechRecognitionEventArgs(e.Result.Semantics.Value.ToString(), e.Result.Confidence));
    }

    private void SpeechRecognitionRejected(object sender, SpeechRecognitionRejectedEventArgs e)
    {
      if (NotRecognized != null)
        NotRecognized(this, null);
    }

    //private void SpeechHypothesized(object sender, SpeechHypothesizedEventArgs e)
    //{
    //  throw new NotImplementedException();
    //}

    #endregion

  }

}
