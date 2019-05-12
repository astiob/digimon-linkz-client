using System;
using System.Collections;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace System
{
	[Serializable]
	internal class CurrentSystemTimeZone : TimeZone, IDeserializationCallback
	{
		private string m_standardName;

		private string m_daylightName;

		private Hashtable m_CachedDaylightChanges = new Hashtable(1);

		private long m_ticksOffset;

		[NonSerialized]
		private TimeSpan utcOffsetWithOutDLS;

		[NonSerialized]
		private TimeSpan utcOffsetWithDLS;

		private static int this_year;

		private static DaylightTime this_year_dlt;

		internal CurrentSystemTimeZone()
		{
		}

		internal CurrentSystemTimeZone(long lnow)
		{
			DateTime dateTime = new DateTime(lnow);
			long[] array;
			string[] array2;
			if (!CurrentSystemTimeZone.GetTimeZoneData(dateTime.Year, out array, out array2))
			{
				throw new NotSupportedException(Locale.GetText("Can't get timezone name."));
			}
			this.m_standardName = Locale.GetText(array2[0]);
			this.m_daylightName = Locale.GetText(array2[1]);
			this.m_ticksOffset = array[2];
			DaylightTime daylightTimeFromData = this.GetDaylightTimeFromData(array);
			this.m_CachedDaylightChanges.Add(dateTime.Year, daylightTimeFromData);
			this.OnDeserialization(daylightTimeFromData);
		}

		void IDeserializationCallback.OnDeserialization(object sender)
		{
			this.OnDeserialization(null);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetTimeZoneData(int year, out long[] data, out string[] names);

		public override string DaylightName
		{
			get
			{
				return this.m_daylightName;
			}
		}

		public override string StandardName
		{
			get
			{
				return this.m_standardName;
			}
		}

		public override DaylightTime GetDaylightChanges(int year)
		{
			if (year < 1 || year > 9999)
			{
				throw new ArgumentOutOfRangeException("year", year + Locale.GetText(" is not in a range between 1 and 9999."));
			}
			if (year == CurrentSystemTimeZone.this_year)
			{
				return CurrentSystemTimeZone.this_year_dlt;
			}
			Hashtable cachedDaylightChanges = this.m_CachedDaylightChanges;
			DaylightTime result;
			lock (cachedDaylightChanges)
			{
				DaylightTime daylightTime = (DaylightTime)this.m_CachedDaylightChanges[year];
				if (daylightTime == null)
				{
					long[] data;
					string[] array;
					if (!CurrentSystemTimeZone.GetTimeZoneData(year, out data, out array))
					{
						throw new ArgumentException(Locale.GetText("Can't get timezone data for " + year));
					}
					daylightTime = this.GetDaylightTimeFromData(data);
					this.m_CachedDaylightChanges.Add(year, daylightTime);
				}
				result = daylightTime;
			}
			return result;
		}

		public override TimeSpan GetUtcOffset(DateTime time)
		{
			if (this.IsDaylightSavingTime(time))
			{
				return this.utcOffsetWithDLS;
			}
			return this.utcOffsetWithOutDLS;
		}

		private void OnDeserialization(DaylightTime dlt)
		{
			if (dlt == null)
			{
				CurrentSystemTimeZone.this_year = DateTime.Now.Year;
				long[] data;
				string[] array;
				if (!CurrentSystemTimeZone.GetTimeZoneData(CurrentSystemTimeZone.this_year, out data, out array))
				{
					throw new ArgumentException(Locale.GetText("Can't get timezone data for " + CurrentSystemTimeZone.this_year));
				}
				dlt = this.GetDaylightTimeFromData(data);
			}
			else
			{
				CurrentSystemTimeZone.this_year = dlt.Start.Year;
			}
			this.utcOffsetWithOutDLS = new TimeSpan(this.m_ticksOffset);
			this.utcOffsetWithDLS = new TimeSpan(this.m_ticksOffset + dlt.Delta.Ticks);
			CurrentSystemTimeZone.this_year_dlt = dlt;
		}

		private DaylightTime GetDaylightTimeFromData(long[] data)
		{
			return new DaylightTime(new DateTime(data[0]), new DateTime(data[1]), new TimeSpan(data[3]));
		}

		internal enum TimeZoneData
		{
			DaylightSavingStartIdx,
			DaylightSavingEndIdx,
			UtcOffsetIdx,
			AdditionalDaylightOffsetIdx
		}

		internal enum TimeZoneNames
		{
			StandardNameIdx,
			DaylightNameIdx
		}
	}
}
