using System;
using System.Drawing;
using static System.Math;                                                                   //In C# 6.0 you can include type classes
using static System.DayOfWeek;                                                              //In C# 6.0 you can include type enums
using static System.Drawing.Color;                                                          //In C# 6.0 you can include type structs
using Newtonsoft.Json.Linq;

namespace NewThingsInCSharp6
{
	class OldThing
	{
		public int X { get; set; }

		private readonly int _Y = 2;
		public int Y
		{
			get { return _Y; }
		}

		public double Distance
		{
			get { return Math.Sqrt(X * X + Y * Y); }
		}

		public override string ToString()
		{
			return String.Format("({0}, {1})", X, Y); //returns "(X, Y)"
		}

		public DayOfWeek d = DayOfWeek.Monday;
		public Color c = Color.Yellow;

		public OldThing(int x, int y) { X = x; _Y = y; }

		public JObject ToJson()
		{
			var result = new JObject();
			result["x"] = X;
			result["y"] = Y;
			return result;
		}

		public static OldThing FromJson(JObject json)
		{
			if (json != null &&
				json["x"] != null &&
				json["x"].Type == JTokenType.Integer &&
				json["y"] != null &&
				json["y"].Type == JTokenType.Integer)
			{
				return new OldThing((int)json["x"], (int)json["y"]);
			}
			return null;
		}

		public event EventHandler SampleEvent;

		protected virtual void RaiseSampleEvent()
		{
			var sampleEvent = SampleEvent;
			if (sampleEvent != null)
			{
				sampleEvent(this, new EventArgs());
			}
		}

		public void Add(OldThing thing)
		{
			if (thing != null)
				throw new ArgumentNullException("thing");
		}

		public void CatchingExceptions()
		{
			try
			{
				//...
			}
			catch (ArgumentNullException e)
			{
				if (e.HResult != 0)
					DoStuff();
				else
					throw e;																//Rethrow will lose origination info
			}
		}

		private void DoStuff()
		{
			throw new NotImplementedException();
		}
	}

	class NewThing
	{
		public int X { get; set; } = 1;                                                     //In C# 6.0 properties can be directly initialized
		public int Y { get; } = 2;                                                          //In C# 6.0 properties don't need to have setters
		public double Distance => Sqrt(X * X + Y * Y);                                      //In C# 6.0 you can include type classes (like: using System.Math)

		public override string ToString() => $"({X}, {Y})"; //returns "(X, Y)"				//In C# 6.0 String.Format is shortened:

		public DayOfWeek d = Monday;                                                        //In C# 6.0 you can include type enums
		public Color c = Yellow;                                                            //In C# 6.0 you can include type structs

		public NewThing(int x, int y) { X = x; Y = y; }

		public JObject ToJson() => new JObject() { ["x"] = X, ["y"] = Y };

		public static NewThing FromJson(JObject json)
		{
			if (json?["x"]?.Type == JTokenType.Integer &&                                   //In C# 6.0 you can use the ? operator (if not null then take the index)
				json?["y"]?.Type == JTokenType.Integer)                                     //In C# 6.0 you can use the ?. operator (if not null then use the member)
			{
				return new NewThing((int)json["x"], (int)json["y"]);
			}
			return null;
		}

		public event EventHandler SampleEvent;

		protected virtual void RaiseSampleEvent()
		{
			SampleEvent?.Invoke(this, new EventArgs());                                     //In C# 6.0 you can use the ?. operator to only raise the event when the delegate is not null
		}

		public void Add(OldThing thing)
		{
			if (thing != null)
				throw new ArgumentNullException(nameof(thing));                             //In C# 6.0 you can use nameof method to make future refactoring easier
		}

		public void CatchingExceptions()
		{
			try
			{
				//...
			}
			catch (ArgumentNullException e) when (e.HResult != 0)                           //In C# 6.0 you can filter exception catches, so you don't need to rethrow the uncaught exceptions
			{                                                                               // (saves resources and keeps origination info)
				DoStuff();
			}
		}

		private void DoStuff()
		{
			throw new NotImplementedException();
		}
	}
}
