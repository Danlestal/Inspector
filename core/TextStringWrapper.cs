using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace AudioScriptInspector.Core
{
    /// <summary>
    /// A wrapper around the Racccoon TextString class. It implements the IRecognizable class so using this class is a nice way of accessing a TextString values withot having to reference any RACCOON dependency.
    /// </summary>
    class TextStringWrapper : IRecognizable
    {
        /// <summary>
        /// The text string that we are encapsulating
        /// </summary>
        private EA.Eism.Raccoon.TextUtils.TextString _textString;
        /// <summary>
        /// This parameter will set if we want to return the TextString id as case sensitive or unsensitive. Basically we set the string ToUpper.
        /// </summary>
        private bool _caseSensitive;
        /// <summary>
        /// Our wrapper constructor
        /// </summary>
        /// <param name="textString">The Texttring we are wrapping</param>
        /// <param name="caseSensitive">If the TextString ID has to be key sensitive.</param>
        public TextStringWrapper(EA.Eism.Raccoon.TextUtils.TextString textString,bool caseSensitive=false)
        {
            _textString = textString;
            _caseSensitive = caseSensitive;
        }
        /// <summary>
        /// Returns the TextString key.
        /// </summary>
        /// <returns>The TextString if</returns>
        public string GetKey()
        {
            string auxKey = Path.GetFileNameWithoutExtension(_textString.Id);
            if (!_caseSensitive)
            {
                return auxKey.ToUpper();
            }
            else
            {
                return auxKey;
            }
        }
        /// <summary>
        /// Returns a textstring custom property value.
        /// </summary>
        /// <param name="key">key of the custom property.</param>
        /// <returns>the value of the custom property</returns>
        public string GetParameter(string key)
        {
            return _textString.CustomProperties[key].ToString();
        }
        /// <summary>
        /// Returns all the TextStrings customproperties keys.
        /// </summary>
        /// <returns>An array with all the keys stored on the textstring CustomProperty</returns>
        public string[] GetParameterKeys()
        {
            return _textString.CustomProperties.Keys.ToArray();
        }
        /// <summary>
        /// Returns all the TextStrings customproperties values.
        /// </summary>
        /// <returns>An array with all the values stored on the textstring CustomProperty</returns>
        public string[] GetParameterValues()
        {
            return _textString.CustomProperties.Select(s => s.Value.ToString()).ToArray();
        }
    }
}
