using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradesViewer.Shared
{
    [Serializable]
    public struct ExchangeInfoCarrier
    {
        public Symbol[] symbols;
    }

    [Serializable]
    public struct Symbol
    {
        public string baseAsset;
        public string quoteAsset;
    }
}
