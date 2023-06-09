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
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

using XPI = xVM.Helper.Core.Helpers.System.Xml.Linq.XProcessingInstruction;

namespace xVM.Helper.Core.Helpers.System.Xml.Linq
{
	public abstract class XNode : XObject
	{
		public static int CompareDocumentOrder (XNode n1, XNode n2)
		{
			return order_comparer.Compare (n1, n2);
		}

		public static bool DeepEquals (XNode n1, XNode n2)
		{
			return eq_comparer.Equals (n1, n2);
		}

		static XNodeEqualityComparer eq_comparer =
			new XNodeEqualityComparer ();
		static XNodeDocumentOrderComparer order_comparer =
			new XNodeDocumentOrderComparer ();

		XNode previous;
		XNode next;

		internal XNode ()
		{
		}

		public static XNodeDocumentOrderComparer DocumentOrderComparer {
			get { return order_comparer; }
		}

		public static XNodeEqualityComparer EqualityComparer {
			get { return eq_comparer; }
		}

		public XNode PreviousNode {
			get { return previous; }
			internal set { previous = value; }
		}

		public XNode NextNode {
			get { return next; }
			internal set { next = value; }
		}

		public string ToString (SaveOptions options)
		{
			StringWriter sw = new StringWriter ();
			XmlWriterSettings s = new XmlWriterSettings ();
			s.ConformanceLevel = ConformanceLevel.Auto;
			s.Indent = options != SaveOptions.DisableFormatting;
			XmlWriter xw = XmlWriter.Create (sw, s);
			WriteTo (xw);
			xw.Close ();
			return sw.ToString ();
		}

		public void AddAfterSelf (object content)
		{
			if (Parent == null)
				throw new InvalidOperationException ();
			XNode here = this;
			XNode orgNext = next;
			foreach (object o in XUtil.ExpandArray (content)) {
				if (Owner.OnAddingObject (o, true, here, false))
					continue;
				XNode n = XUtil.ToNode (o);
				n = (XNode) XUtil.GetDetachedObject (n);
				n.SetOwner (Parent);
				n.previous = here;
				here.next = n;
				n.next = orgNext;
				if (orgNext != null)
					orgNext.previous = n;
				else
					Parent.LastNode = n;
				here = n;
			}
		}

		public void AddAfterSelf (params object [] content)
		{
			if (Parent == null)
				throw new InvalidOperationException ();
			AddAfterSelf ((object) content);
		}

		public void AddBeforeSelf (object content)
		{
			if (Parent == null)
				throw new InvalidOperationException ();
			foreach (object o in XUtil.ExpandArray (content)) {
				if (Owner.OnAddingObject (o, true, previous, true))
					continue;
				XNode n = XUtil.ToNode (o);
				n = (XNode) XUtil.GetDetachedObject (n);
				n.SetOwner (Parent);
				n.previous = previous;
				n.next = this;
				if (previous != null)
					previous.next = n;
				previous = n;
				if (Parent.FirstNode == this)
					Parent.FirstNode = n;
			}
		}

		public void AddBeforeSelf (params object [] content)
		{
			if (Parent == null)
				throw new InvalidOperationException ();
			AddBeforeSelf ((object) content);
		}

		public static XNode ReadFrom (XmlReader r)
		{
			return ReadFrom (r, LoadOptions.None);
		}

		internal static XNode ReadFrom (XmlReader r, LoadOptions options)
		{
			switch (r.NodeType) {
			case XmlNodeType.Element:
				return XElement.LoadCore (r, options);
			case XmlNodeType.Whitespace:
			case XmlNodeType.SignificantWhitespace:
			case XmlNodeType.Text:
				XText t = new XText (r.Value);
				t.FillLineInfoAndBaseUri (r, options);
				r.Read ();
				return t;
			case XmlNodeType.CDATA:
				XCData c = new XCData (r.Value);
				c.FillLineInfoAndBaseUri (r, options);
				r.Read ();
				return c;
			case XmlNodeType.ProcessingInstruction:
				XPI pi = new XPI (r.Name, r.Value);
				pi.FillLineInfoAndBaseUri (r, options);
				r.Read ();
				return pi;
			case XmlNodeType.Comment:
				XComment cm = new XComment (r.Value);
				cm.FillLineInfoAndBaseUri (r, options);
				r.Read ();
				return cm;
			case XmlNodeType.DocumentType:
				XDocumentType d = new XDocumentType (r.Name,
					r.GetAttribute ("PUBLIC"),
					r.GetAttribute ("SYSTEM"),
					r.Value);
				d.FillLineInfoAndBaseUri (r, options);
				r.Read ();
				return d;
			default:
				throw new InvalidOperationException (String.Format ("Node type {0} is not supported", r.NodeType));
			}
		}

		public void Remove ()
		{
			if (Parent == null)
				throw new InvalidOperationException ("Parent is missing");

			if (Parent.FirstNode == this)
				Parent.FirstNode = next;
			if (Parent.LastNode == this)
				Parent.LastNode = previous;
			if (previous != null)
				previous.next = next;
			if (next != null)
				next.previous = previous;
			previous = null;
			next = null;
			SetOwner (null);
		}

		public override string ToString ()
		{
			return ToString (SaveOptions.None);
		}

		public abstract void WriteTo (XmlWriter w);

		public IEnumerable<XElement> Ancestors ()
		{
			for (XElement el = Parent; el != null; el = el.Parent)
				yield return el;
		}

		public IEnumerable<XElement> Ancestors (XName name)
		{
			foreach (XElement el in Ancestors ())
				if (el.Name == name)
					yield return el;
		}

		public XmlReader CreateReader ()
		{
			return new XNodeReader (this);
		}

		public IEnumerable<XElement> ElementsAfterSelf ()
		{
			foreach (XNode n in NodesAfterSelf ())
				if (n is XElement)
					yield return (XElement) n;
		}

		public IEnumerable<XElement> ElementsAfterSelf (XName name)
		{
			foreach (XElement el in ElementsAfterSelf ())
				if (el.Name == name)
					yield return el;
		}

		public IEnumerable<XElement> ElementsBeforeSelf ()
		{
			foreach (XNode n in NodesBeforeSelf ())
				if (n is XElement)
					yield return (XElement) n;
		}

		public IEnumerable<XElement> ElementsBeforeSelf (XName name)
		{
			foreach (XElement el in ElementsBeforeSelf ())
				if (el.Name == name)
					yield return el;
		}

		public bool IsAfter (XNode other)
		{
			return XNode.DocumentOrderComparer.Compare (this, other) > 0;
		}

		public bool IsBefore (XNode other)
		{
			return XNode.DocumentOrderComparer.Compare (this, other) < 0;
		}

		public IEnumerable<XNode> NodesAfterSelf ()
		{
			if (Parent == null)
				yield break;
			for (XNode n = NextNode; n != null; n = n.NextNode)
				yield return n;
		}

		public IEnumerable<XNode> NodesBeforeSelf ()
		{
			for (XNode n = Parent.FirstNode; n != this; n = n.NextNode)
				yield return n;
		}

		public void ReplaceWith (object item)
		{
			AddAfterSelf (item);
			Remove ();
		}

		public void ReplaceWith (params object [] items)
		{
			AddAfterSelf (items);
			Remove ();
		}
	}
}
