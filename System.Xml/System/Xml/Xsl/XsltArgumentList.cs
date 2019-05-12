using System;
using System.Collections;
using System.Xml.XPath;

namespace System.Xml.Xsl
{
	/// <summary>Contains a variable number of arguments which are either XSLT parameters or extension objects.</summary>
	public class XsltArgumentList
	{
		internal Hashtable extensionObjects;

		internal Hashtable parameters;

		/// <summary>Implements a new instance of the <see cref="T:System.Xml.Xsl.XsltArgumentList" />.</summary>
		public XsltArgumentList()
		{
			this.extensionObjects = new Hashtable();
			this.parameters = new Hashtable();
		}

		/// <summary>Occurs when a message is specified in the style sheet by the xsl:message element. </summary>
		public event XsltMessageEncounteredEventHandler XsltMessageEncountered;

		/// <summary>Adds a new object to the <see cref="T:System.Xml.Xsl.XsltArgumentList" /> and associates it with the namespace URI.</summary>
		/// <param name="namespaceUri">The namespace URI to associate with the object. To use the default namespace, specify an empty string. </param>
		/// <param name="extension">The object to add to the list. </param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="namespaceUri" /> is either null or http://www.w3.org/1999/XSL/Transform The <paramref name="namespaceUri" /> already has an extension object associated with it. </exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have sufficient permissions to call this method. </exception>
		public void AddExtensionObject(string namespaceUri, object extension)
		{
			if (namespaceUri == null)
			{
				throw new ArgumentException("The namespaceUri is a null reference.");
			}
			if (namespaceUri == "http://www.w3.org/1999/XSL/Transform")
			{
				throw new ArgumentException("The namespaceUri is http://www.w3.org/1999/XSL/Transform.");
			}
			if (this.extensionObjects.Contains(namespaceUri))
			{
				throw new ArgumentException("The namespaceUri already has an extension object associated with it.");
			}
			this.extensionObjects[namespaceUri] = extension;
		}

		/// <summary>Adds a parameter to the <see cref="T:System.Xml.Xsl.XsltArgumentList" /> and associates it with the namespace qualified name.</summary>
		/// <param name="name">The name to associate with the parameter. </param>
		/// <param name="namespaceUri">The namespace URI to associate with the parameter. To use the default namespace, specify an empty string. </param>
		/// <param name="parameter">The parameter value or object to add to the list. </param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="namespaceUri" /> is either null or http://www.w3.org/1999/XSL/Transform.The <paramref name="name" /> is not a valid name according to the W3C XML specification.The <paramref name="namespaceUri" /> already has a parameter associated with it. </exception>
		public void AddParam(string name, string namespaceUri, object parameter)
		{
			if (namespaceUri == null)
			{
				throw new ArgumentException("The namespaceUri is a null reference.");
			}
			if (namespaceUri == "http://www.w3.org/1999/XSL/Transform")
			{
				throw new ArgumentException("The namespaceUri is http://www.w3.org/1999/XSL/Transform.");
			}
			if (name == null)
			{
				throw new ArgumentException("The parameter name is a null reference.");
			}
			XmlQualifiedName key = new XmlQualifiedName(name, namespaceUri);
			if (this.parameters.Contains(key))
			{
				throw new ArgumentException("The namespaceUri already has a parameter associated with it.");
			}
			parameter = this.ValidateParam(parameter);
			this.parameters[key] = parameter;
		}

		/// <summary>Removes all parameters and extension objects from the <see cref="T:System.Xml.Xsl.XsltArgumentList" />.</summary>
		public void Clear()
		{
			this.extensionObjects.Clear();
			this.parameters.Clear();
		}

		/// <summary>Gets the object associated with the given namespace.</summary>
		/// <returns>The namespace URI object or null if one was not found.</returns>
		/// <param name="namespaceUri">The namespace URI of the object. </param>
		public object GetExtensionObject(string namespaceUri)
		{
			return this.extensionObjects[namespaceUri];
		}

		/// <summary>Gets the parameter associated with the namespace qualified name.</summary>
		/// <returns>The parameter object or null if one was not found.</returns>
		/// <param name="name">The name of the parameter. <see cref="T:System.Xml.Xsl.XsltArgumentList" /> does not check to ensure the name passed is a valid local name; however, the name cannot be null. </param>
		/// <param name="namespaceUri">The namespace URI associated with the parameter. </param>
		public object GetParam(string name, string namespaceUri)
		{
			if (name == null)
			{
				throw new ArgumentException("The parameter name is a null reference.");
			}
			XmlQualifiedName key = new XmlQualifiedName(name, namespaceUri);
			return this.parameters[key];
		}

		/// <summary>Removes the object with the namespace URI from the <see cref="T:System.Xml.Xsl.XsltArgumentList" />.</summary>
		/// <returns>The object with the namespace URI or null if one was not found.</returns>
		/// <param name="namespaceUri">The namespace URI associated with the object to remove. </param>
		public object RemoveExtensionObject(string namespaceUri)
		{
			object extensionObject = this.GetExtensionObject(namespaceUri);
			this.extensionObjects.Remove(namespaceUri);
			return extensionObject;
		}

		/// <summary>Removes the parameter from the <see cref="T:System.Xml.Xsl.XsltArgumentList" />.</summary>
		/// <returns>The parameter object or null if one was not found.</returns>
		/// <param name="name">The name of the parameter to remove. <see cref="T:System.Xml.Xsl.XsltArgumentList" /> does not check to ensure the name passed is a valid local name; however, the name cannot be null. </param>
		/// <param name="namespaceUri">The namespace URI of the parameter to remove. </param>
		public object RemoveParam(string name, string namespaceUri)
		{
			XmlQualifiedName key = new XmlQualifiedName(name, namespaceUri);
			object param = this.GetParam(name, namespaceUri);
			this.parameters.Remove(key);
			return param;
		}

		private object ValidateParam(object parameter)
		{
			if (parameter is string)
			{
				return parameter;
			}
			if (parameter is bool)
			{
				return parameter;
			}
			if (parameter is double)
			{
				return parameter;
			}
			if (parameter is XPathNavigator)
			{
				return parameter;
			}
			if (parameter is XPathNodeIterator)
			{
				return parameter;
			}
			if (parameter is short)
			{
				return (double)((short)parameter);
			}
			if (parameter is ushort)
			{
				return (double)((ushort)parameter);
			}
			if (parameter is int)
			{
				return (double)((int)parameter);
			}
			if (parameter is long)
			{
				return (double)((long)parameter);
			}
			if (parameter is ulong)
			{
				return (ulong)parameter;
			}
			if (parameter is float)
			{
				return (double)((float)parameter);
			}
			if (parameter is decimal)
			{
				return (double)((decimal)parameter);
			}
			return parameter.ToString();
		}
	}
}
