/**
 * \file BarcodeException.cs
 * \instructor Ignac Kolenko
 * \course Multimedia Application Development
 * \assignment Milestone #3
 * \author Samuel Lewis, Hekar, Thomas
 * \brief
 *  
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace barcodeReader
{
    /// <summary>
    /// Exception in barcode reading
    /// </summary>
    class BarcodeException:Exception
    {
        public BarcodeException()
        {
        }

        /// <summary>
        /// Create a barcode exception with a message
        /// </summary>
        /// <param name="message">Exception message</param>
        public BarcodeException(String message):base(message)
        {
        }
    }
}
