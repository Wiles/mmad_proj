using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace barcodeReader
{
    class BarcodeException:Exception
    {
        public BarcodeException()
        {
        }

        public BarcodeException(String message):base(message)
        {
        }

    }
}
