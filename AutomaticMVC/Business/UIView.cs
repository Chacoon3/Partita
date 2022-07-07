using System.Collections.Generic;
using UnityEngine.UI;

namespace AutomaticMVC.Business {
    internal class UIView : View {

        readonly Dictionary<string, Text> textView = new Dictionary<string, Text>();
        readonly Dictionary<string, Image> imageView = new Dictionary<string, Image>();
        readonly Dictionary<string, Button> buttonView = new Dictionary<string, Button>();
        readonly Dictionary<string, InputField> fieldView = new Dictionary<string, InputField>();
        readonly Dictionary<string, Slider> sliderView= new Dictionary<string, Slider>();
        readonly Dictionary<string, Toggle> toggleView = new Dictionary<string, Toggle>();

        internal UIView(IEnumerable<KeyValuePair<string, Text>> texts) {
            textView = new Dictionary<string, Text>();
            foreach (var pair in texts) {
                textView.Add(pair.Key, pair.Value);
            }
        }

        public override void Dispose() {
            throw new System.NotImplementedException();
        }

        public override void OnInit(Model data) {
            throw new System.NotImplementedException();
        }

        public override void Refresh(Model data) {
            throw new System.NotImplementedException();
        }
    }
}
