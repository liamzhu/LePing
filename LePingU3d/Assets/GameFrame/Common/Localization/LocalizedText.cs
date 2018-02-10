using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace MoleMole
{
    [RequireComponent(typeof(Text))]
    public class LocalizedText : MonoBehaviour
    {
        [SerializeField]
        private string _textID;
        public string TextID
        {
            get
            {
                return _textID;
            }
        }

        private Text _label;

        public void Start()
        {
            _label = GetComponent<Text>();
            SetupTextID(_textID);
        }

        public void SetupTextID(string textID)
        {
            _label.text = MLocalization.Instance.Get(_textID);
        }
    }
}
