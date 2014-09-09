using System;

using SQLite;

namespace NuclearPlants
{
    //[Serializable]
	public class Nuclear
	{
		public Nuclear ()
		{
		}

		[PrimaryKey]
		public int Id {get;set;}

		public string Name {get;set;}

		public double Latitude {
			get;
			set;
		}
		public double Longitude {
			get;
			set;
		}
		public int Units {
			get;
			set;
		}
		public int Capacity {
			get;
			set;
		}
		public string URL  {
			get;
			set;
		}
		public int Type  {
			get;
			set;
		}
	}
}

