//Project: SpeechLib (http://SpeechLib.codeplex.com)
//File: SpeechRecognition.cs
//Version: 20151208

using SpeechLib.Models;
using System;
using System.ComponentModel.Composition;
using System.IO;

#if USE_MICROSOFT_SPEECH
using Microsoft.Speech.Recognition;
#else
using System.Speech.Recognition;
using System.Threading;
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

    protected const string ACOUSTIC_MODEL_ADAPTATION = "AdaptationOn";
    protected const int PAUSE_LOOP_SLEEP = 10; //msec

    #endregion

    #region --- Fields ---

    protected SpeechRecognitionEngine speechRecognitionEngine;
    protected bool paused; //=false

    #endregion

    #region --- Initialization ---

    public SpeechRecognition()
    {
      Init();
    }

    protected void Init()
    {
      speechRecognitionEngine = CreateSpeechRecognitionEngine();

      if (speechRecognitionEngine != null)
      {
        speechRecognitionEngine.RecognizerUpdateReached += (s, e) => {
            while (paused) Thread.Sleep(PAUSE_LOOP_SLEEP);
          };

        //speechRecognitionEngine.SpeechDetected += SpeechDetected;
        //speechRecognitionEngine.SpeechHypothesized += SpeechHypothesized;
        speechRecognitionEngine.SpeechRecognized += SpeechRecognized;
        speechRecognitionEngine.SpeechRecognitionRejected += SpeechRecognitionRejected;
      }

      SetInputToInitial();
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
          Cleanup();
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

    protected virtual void Cleanup()
    {
      Stop();
      //speechRecognitionEngine.SpeechDetected -= SpeechDetected;
      speechRecognitionEngine.SpeechRecognized -= SpeechRecognized;
      //speechRecognitionEngine.SpeechHypothesized -= SpeechHypothesized;
      speechRecognitionEngine.SpeechRecognitionRejected -= SpeechRecognitionRejected;
    }

    #endregion

    #region --- Properties ---

    /// <summary>
    /// Gets the speech recognition engine.
    /// </summary>
    /// <value>
    /// The speech recognition engine.
    /// </value>
    public SpeechRecognitionEngine SpeechRecognitionEngine { get { return speechRecognitionEngine; } }

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

    public void LoadGrammar(Grammar grammar)
    {
      speechRecognitionEngine.LoadGrammar(grammar);
    }

    public void LoadGrammar(string xml, string name)
    {
      LoadGrammar(SpeechRecognitionUtils.CreateGrammarFromXML(xml, name));
    }

    public void LoadGrammar(Stream stream, string name)
    {
      LoadGrammar(new Grammar(stream) { Name = name });
    }

    public void UnloadGrammar(Grammar grammar)
    {
      speechRecognitionEngine.UnloadGrammar(grammar);
    }

    public void UnloadAllGrammars()
    {
      speechRecognitionEngine.UnloadAllGrammars();
    }

    public void SetInputToDefaultAudioDevice() //note: sets input to system's default audio device, not to the specific speech recognizer's default, use SetInput for that
    {
      speechRecognitionEngine.SetInputToDefaultAudioDevice();
    }

    public void SetInputToNone()
    {
      speechRecognitionEngine.SetInputToNull();
    }

    public virtual void SetInputToInitial() //can override this at descendants
    {
      SetInputToDefaultAudioDevice();
    }

    public void Start(bool stopOnRecognition = false)
    {
      speechRecognitionEngine.RecognizeAsync(stopOnRecognition ? RecognizeMode.Single : RecognizeMode.Multiple);
    }

    public void Stop(bool waitForCurrentRecognitionToComplete = true)
    {
      if (waitForCurrentRecognitionToComplete)
        speechRecognitionEngine.RecognizeAsyncStop();
      else
        speechRecognitionEngine.RecognizeAsyncCancel();
    }

    public void Pause()
    {
      paused = true;
      speechRecognitionEngine.RequestRecognizerUpdate();
    }

    public void Resume()
    {
      paused = false;
    }

    #endregion

    #region --- Events ---

    public event EventHandler<SpeechRecognitionEventArgs> Recognized;
    public event EventHandler NotRecognized;

    //private void SpeechDetected(object sender, SpeechDetectedEventArgs e)
    //{
    //}

    //private void SpeechHypothesized(object sender, SpeechHypothesizedEventArgs e)
    //{
    //}

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

    #endregion

  }

}
