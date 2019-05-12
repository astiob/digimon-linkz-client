using System;
using System.Collections;

namespace Mono.Xml
{
	internal class DTDContentModelCollection
	{
		private ArrayList contentModel = new ArrayList();

		public IList Items
		{
			get
			{
				return this.contentModel;
			}
		}

		public DTDContentModel this[int i]
		{
			get
			{
				return this.contentModel[i] as DTDContentModel;
			}
		}

		public int Count
		{
			get
			{
				return this.contentModel.Count;
			}
		}

		public void Add(DTDContentModel model)
		{
			this.contentModel.Add(model);
		}
	}
}
