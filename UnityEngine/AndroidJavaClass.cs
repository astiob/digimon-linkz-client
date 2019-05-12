using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>AndroidJavaClass is the Unity representation of a generic instance of java.lang.Class.</para>
	/// </summary>
	public class AndroidJavaClass : AndroidJavaObject
	{
		/// <summary>
		///   <para>Construct an AndroidJavaClass from the class name.</para>
		/// </summary>
		/// <param name="className">Specifies the Java class name (e.g. &lt;tt&gt;java.lang.String&lt;/tt&gt;).</param>
		public AndroidJavaClass(string className)
		{
			this._AndroidJavaClass(className);
		}

		internal AndroidJavaClass(IntPtr jclass)
		{
			if (jclass == IntPtr.Zero)
			{
				throw new Exception("JNI: Init'd AndroidJavaClass with null ptr!");
			}
			this.m_jclass = AndroidJNI.NewGlobalRef(jclass);
			this.m_jobject = IntPtr.Zero;
		}

		private void _AndroidJavaClass(string className)
		{
			base.DebugPrint("Creating AndroidJavaClass from " + className);
			using (AndroidJavaObject androidJavaObject = AndroidJavaObject.FindClass(className))
			{
				this.m_jclass = AndroidJNI.NewGlobalRef(androidJavaObject.GetRawObject());
				this.m_jobject = IntPtr.Zero;
			}
		}
	}
}
