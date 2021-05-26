using System;
using System.Collections.Generic;
using System.Text;

namespace InputDrawer {

	struct Vector {

		private static Vector zero = new Vector(0f, 0f);
		public static Vector Zero { get { return zero; } }

		public float x;
		public float y;

		public Vector(float x, float y) {
			this.x = x;
			this.y = y;
		}

		public static Vector operator +(Vector v, Vector u) {
			Vector w = v;
			w.x += u.x;
			w.y += u.y;
			return w;
		}

		public static Vector operator *(Vector v, float scalar) {
			Vector r = v;
			r.x *= scalar;
			r.y *= scalar;
			return r;
		}

		public static bool operator ==(Vector v, Vector u) {
			return v.x == u.x && v.y == u.y;
		}

		public static bool operator !=(Vector v, Vector u) {
			return v.x != u.x || v.y != u.y;
		}

		public override bool Equals(object obj) {
			Vector cast = (Vector)obj;
			return this == cast;
		}

		public static void Clamp(ref Vector v, Vector upperLeftCorner, Vector bottomRightCorner) {
			//The bounds should be x:
			if (v.x > bottomRightCorner.x)
				v.x = bottomRightCorner.x;
			else if (v.x < upperLeftCorner.x)
				v.x = upperLeftCorner.x;

			if (v.y > upperLeftCorner.y)
				v.y = upperLeftCorner.y;
			else if (v.y < bottomRightCorner.y)
				v.y = bottomRightCorner.y;
		}

		public static Vector Parse(string s) {
			Vector v = Vector.Zero;
			s = s.Replace(" ", "");
			s = s.Replace("(", "");
			s = s.Replace(")", "");
			string[] data = s.Split(",");
			v.x = float.Parse(data[0]);
			v.y = float.Parse(data[1]);
			return v;
		}

		public override string ToString() {
			return $"({this.x}, {this.y})";
		}

	}

}
