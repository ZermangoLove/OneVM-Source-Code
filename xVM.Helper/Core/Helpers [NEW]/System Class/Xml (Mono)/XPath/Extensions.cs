//
// Authors:
//   Atsushi Enomoto
//
// Copyright 2007 Novell (http://www.novell.com)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.XPath;
using xVM.Helper.Core.Helpers.System.Xml.Linq;

namespace xVM.Helper.Core.Helpers.System.Xml.XPath
{
	public static class Extensions
	{
		public static XPathNavigator CreateNavigator (this XNode node)
		{
			return CreateNavigator (node, new NameTable ());
		}

		public static XPathNavigator CreateNavigator (this XNode node, XmlNameTable nameTable)
		{
			return new XNodeNavigator (node, nameTable);
		}

		public static object XPathEvaluate (this XNode node, string expression)
		{
			return XPathEvaluate (node, expression, null);
		}

		public static object XPathEvaluate (this XNode node, string expression, IXmlNamespaceResolver nsResolver)
		{
			return CreateNavigator (node).Evaluate (expression, nsResolver);
		}

		public static XElement XPathSelectElement (this XNode node, string xpath)
		{
			return XPathSelectElement (node, xpath, null);
		}

		public static XElement XPathSelectElement (this XNode node, string xpath, IXmlNamespaceResolver nsResolver)
		{
			XPathNavigator nav = CreateNavigator (node).SelectSingleNode (xpath, nsResolver);
			if (nav == null)
				return null;
			return nav.UnderlyingObject as XElement;
		}

		public static IEnumerable<XElement> XPathSelectElements (this XNode node, string xpath)
		{
			return XPathSelectElements (node, xpath, null);
		}

		public static IEnumerable<XElement> XPathSelectElements (this XNode node, string xpath, IXmlNamespaceResolver nsResolver)
		{
			XPathNodeIterator iter = CreateNavigator (node).Select (xpath, nsResolver);
			foreach (XPathNavigator nav in iter){
				if (nav.UnderlyingObject is XElement)
					yield return (XElement) nav.UnderlyingObject;
			}
		}
	}
}
