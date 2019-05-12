using System;

namespace System.Xml
{
	/// <summary>Provides data for the <see cref="E:System.Xml.XmlDocument.NodeChanged" />, <see cref="E:System.Xml.XmlDocument.NodeChanging" />, <see cref="E:System.Xml.XmlDocument.NodeInserted" />, <see cref="E:System.Xml.XmlDocument.NodeInserting" />, <see cref="E:System.Xml.XmlDocument.NodeRemoved" /> and <see cref="E:System.Xml.XmlDocument.NodeRemoving" /> events.</summary>
	public class XmlNodeChangedEventArgs : EventArgs
	{
		private XmlNode _oldParent;

		private XmlNode _newParent;

		private XmlNodeChangedAction _action;

		private XmlNode _node;

		private string _oldValue;

		private string _newValue;

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.XmlNodeChangedEventArgs" /> class.</summary>
		/// <param name="node">The <see cref="T:System.Xml.XmlNode" /> that generated the event.</param>
		/// <param name="oldParent">The old parent <see cref="T:System.Xml.XmlNode" /> of the <see cref="T:System.Xml.XmlNode" /> that generated the event.</param>
		/// <param name="newParent">The new parent <see cref="T:System.Xml.XmlNode" /> of the <see cref="T:System.Xml.XmlNode" /> that generated the event.</param>
		/// <param name="oldValue">The old value of the <see cref="T:System.Xml.XmlNode" /> that generated the event.</param>
		/// <param name="newValue">The new value of the <see cref="T:System.Xml.XmlNode" /> that generated the event.</param>
		/// <param name="action">The <see cref="T:System.Xml.XmlNodeChangedAction" />.</param>
		public XmlNodeChangedEventArgs(XmlNode node, XmlNode oldParent, XmlNode newParent, string oldValue, string newValue, XmlNodeChangedAction action)
		{
			this._node = node;
			this._oldParent = oldParent;
			this._newParent = newParent;
			this._oldValue = oldValue;
			this._newValue = newValue;
			this._action = action;
		}

		/// <summary>Gets a value indicating what type of node change event is occurring.</summary>
		/// <returns>An XmlNodeChangedAction value describing the node change event.XmlNodeChangedAction Value Description Insert A node has been or will be inserted. Remove A node has been or will be removed. Change A node has been or will be changed. Note:The Action value does not differentiate between when the event occurred (before or after). You can create separate event handlers to handle both instances.</returns>
		public XmlNodeChangedAction Action
		{
			get
			{
				return this._action;
			}
		}

		/// <summary>Gets the <see cref="T:System.Xml.XmlNode" /> that is being added, removed or changed.</summary>
		/// <returns>The XmlNode that is being added, removed or changed; this property never returns null.</returns>
		public XmlNode Node
		{
			get
			{
				return this._node;
			}
		}

		/// <summary>Gets the value of the <see cref="P:System.Xml.XmlNode.ParentNode" /> before the operation began.</summary>
		/// <returns>The value of the ParentNode before the operation began. This property returns null if the node did not have a parent.Note:For attribute nodes this property returns the <see cref="P:System.Xml.XmlAttribute.OwnerElement" />.</returns>
		public XmlNode OldParent
		{
			get
			{
				return this._oldParent;
			}
		}

		/// <summary>Gets the value of the <see cref="P:System.Xml.XmlNode.ParentNode" /> after the operation completes.</summary>
		/// <returns>The value of the ParentNode after the operation completes. This property returns null if the node is being removed.Note:For attribute nodes this property returns the <see cref="P:System.Xml.XmlAttribute.OwnerElement" />.</returns>
		public XmlNode NewParent
		{
			get
			{
				return this._newParent;
			}
		}

		/// <summary>Gets the original value of the node.</summary>
		/// <returns>The original value of the node. This property returns null if the node is neither an attribute nor a text node, or if the node is being inserted.If called in a <see cref="E:System.Xml.XmlDocument.NodeChanging" /> event, OldValue returns the current value of the node that will be replaced if the change is successful. If called in a <see cref="E:System.Xml.XmlDocument.NodeChanged" /> event, OldValue returns the value of node prior to the change.</returns>
		public string OldValue
		{
			get
			{
				return (this._oldValue == null) ? this._node.Value : this._oldValue;
			}
		}

		/// <summary>Gets the new value of the node.</summary>
		/// <returns>The new value of the node. This property returns null if the node is neither an attribute nor a text node, or if the node is being removed.If called in a <see cref="E:System.Xml.XmlDocument.NodeChanging" /> event, NewValue returns the value of the node if the change is successful. If called in a <see cref="E:System.Xml.XmlDocument.NodeChanged" /> event, NewValue returns the current value of the node.</returns>
		public string NewValue
		{
			get
			{
				return (this._newValue == null) ? this._node.Value : this._newValue;
			}
		}
	}
}
