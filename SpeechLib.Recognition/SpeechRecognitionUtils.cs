//Project: SpeechLib (http://SpeechLib.codeplex.com)
//Filename: SpeechRecognitionUtils.cs
//Version: 20151202

using System.Globalization;
using System.IO;
using System.Text;

#if USE_MICROSOFT_SPEECH
using Microsoft.Speech.Recognition;
#else
using System.Speech.Recognition;
#endif

namespace SpeechLib.Recognition
{
  /// <summary>
  /// Speech-related utility methods
  /// </summary>
  public static class SpeechRecognitionUtils
  {

    /// <summary>
    /// Create a grammar from grammar definition XML (resource) file.
    /// </summary>
    /// <param name="xml">The XML data.</param>
    /// <param name="name">Optional name for returned grammar.</param>
    /// <returns>Grammar</returns>
    public static Grammar CreateGrammarFromXML(string xml, string name = "")
    {
      using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
        return new Grammar(memoryStream) { Name = name };
    }

    /// <summary>
    /// Create a grammar from a list of names.
    /// </summary>
    /// <param name="names"></param>
    /// <param name="language">Optional IETF language tag for the Grammar (default is "en")</param>
    /// <param name="name">Optional name for returned grammar.</param>
    /// <returns></returns>
    public static Grammar CreateGrammarFromNames(string[] names, string language = "en", string name = "")
    {
      var commands = new Choices(); //see https://msdn.microsoft.com/en-us/library/system.speech.recognition.choices(v=vs.110).aspx
      foreach (string s in names)
        commands.Add(new SemanticResultValue(s, s));

      var gb = new GrammarBuilder { Culture = CultureInfo.GetCultureInfoByIetfLanguageTag(language) };
      gb.Append(commands);

      return new Grammar(gb) { Name = name };
    }

  }
}
