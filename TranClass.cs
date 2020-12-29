using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManaTranslation
{
  public class TranClass
    {
      /// <summary>
      /// From为翻译语言
      /// </summary>
        public string From { get; set; }
      /// <summary>
      /// To为翻译后的语言
      /// </summary>
        public string To { get; set; }
        public List<Trans_result> Trans_result { get; set; }
    }
}
